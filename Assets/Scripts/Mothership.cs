using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; 
using DG.Tweening; 

public class Mothership : MonoBehaviour {

	private MainGenerator controller; 
	private List<Enemy> enemies; 

	[SerializeField]
	private int life = 20; 
	[SerializeField]
	private float aumentSpeed = 0.1f; 
	[SerializeField]
	private int lifeRound = 4; 

	private int countLife = 0; 

	private Animator anim; 
	private int roundCount = 1; 
	[SerializeField]
	private AudioSource[] audios;

	[SerializeField]
	private AudioSource interBossLevel; 
	private CircleGenerator gen;
	private Rigidbody2D rb; 
	private float speed = 0; 
	private bool roundSpeed = false; 

	[SerializeField]
	private GameObject[] motherboardNaves; 

	private float timer; 
	private float timeEachAlien = 10; 
	private List<GameObject> generatedEnemies; 


	// Use this for initialization
	void Start () {
		controller = GameObject.FindGameObjectWithTag ("GameController").GetComponent<MainGenerator> ();
		anim = this.GetComponent<Animator> (); 
		gen = GameObject.Find ("Spawner").GetComponent<CircleGenerator> (); 
		rb = this.GetComponent<Rigidbody2D> ();
		generatedEnemies = new List<GameObject> (); 
	}
	
	// Update is called once per frame
	void Update () {

		if (roundSpeed) {
			timer += Time.deltaTime; 
			if (timer > timeEachAlien) {
				generatedEnemies.Add(((GameObject)Instantiate (motherboardNaves [Random.Range (0, motherboardNaves.Length)], this.transform.position, Quaternion.identity))); 
				timer = 0; 
			}
		}
	}

	private void CleanEnemies(){
		for (int i = 0; i < generatedEnemies.Count; i++) {
			if (generatedEnemies [i] != null) {
				Destroy (generatedEnemies [i]);
			}
		}

		generatedEnemies.Clear ();
	}

	private void Die(){
		Destroy (this.gameObject); 
	}

	public void OnTriggerEnter2D(Collider2D coll) {
		if (coll.gameObject.tag == "Bullet") {

			controller.AllAliensSpeedUp (aumentSpeed); 
			life -= 1; 
			countLife++; 
			if (!roundSpeed) {
				speed += 1.5f;
				rb.velocity = new Vector2 (speed, 0); 
				roundSpeed = true; 
				timeEachAlien-=2; 
			}

			if (life <= 0) {		

				PlayerPrefs.SetInt ("win", 1); 
				
				SceneManager.LoadScene (2); 
			} else if (countLife >= lifeRound) {
				CleanEnemies ();
				roundSpeed = false; 
				audios [roundCount - 1].Stop ();
				audios [roundCount].Play (); 
				countLife = 0;
				//controller.SpawnNewRound ();
				roundCount++; 
				anim.SetInteger ("Round", roundCount);

				interBossLevel.Stop ();
				interBossLevel.Play ();
				gen.enabled = false; 
				DOVirtual.DelayedCall (2, () => {
					gen.enabled = true; 
					controller.SpawnNewRound ();
					roundSpeed = false; 


				});

				Camera.main.GetComponent<MainCamera> ().BiggerShake ();

			} else {
				Camera.main.GetComponent<MainCamera> ().CameraShake ();

			}

			if (life <= 0) {
				Die (); 
			}

			Destroy (coll.gameObject); 
		} else if (coll.gameObject.tag == "Limit") {
			rb.velocity *= -1; 
		}

	}



}

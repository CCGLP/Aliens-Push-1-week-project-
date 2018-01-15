using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; 


public class Enemy : MonoBehaviour {

    [System.Serializable]
    public struct ColorChangeFab{
        public Color color;
        public GameObject prefab; 
    }


    [SerializeField]
    private ColorChangeFab[] colorChanges; 
	[SerializeField]
	private GameObject explosion; 

	private SpriteRenderer rend; 
	private float life = 4; 
	private Rigidbody2D rb; 
	[SerializeField]
	private float stepY = 0.1f; 
	private MainGenerator controller; 
	[SerializeField]
	private float speed = 0.4f; 
	private Vector2 xLimits; 
	public int id; 
	public bool isDying; 
	private bool isDyingCharge; 

	[SerializeField]
	private Color color; 

	private int multiplier = 1;

	[SerializeField]
	private bool isMotherboard = false; 

	[SerializeField]
	private GameObject particleAparicion; 





	// Use this for initialization
	void Start () {

		((GameObject)Instantiate (particleAparicion, this.transform.position, particleAparicion.transform.rotation)).transform.SetParent(this.transform);
		if (!isMotherboard) {
			if (rb == null) {
				rb = this.GetComponent<Rigidbody2D> (); 
				rb.velocity = Vector3.right * speed;

			}
			controller = GameObject.FindGameObjectWithTag ("GameController").GetComponent<MainGenerator> (); 



		}
		else{
			if (rb == null) {
				rb = this.GetComponent<Rigidbody2D> (); 
				rb.velocity = Vector3.down * speed;
			}
			controller = GameObject.FindGameObjectWithTag ("GameController").GetComponent<MainGenerator> (); 

		}
		rend = this.GetComponent<SpriteRenderer> ();


	}

	public void SetXlimits(Vector2 limits){
		xLimits = limits; 

	}


	// Update is called once per frame
	void Update () {
		if (this.transform.position.x - this.transform.localScale.x * 0.5f < xLimits.x && rb.velocity.x < 0 && !controller.IsCalling) {
			controller.CallAliens (id); 

		} else if (this.transform.position.x +this.transform.localScale.x * 0.5f > xLimits.y && rb.velocity.x > 0 && !controller.IsCalling) {
			controller.CallAliens (id); 
		}
	}


    void ChangeColor(Color color)
    {
        GameObject instancia = null; 

        for (int i = 0; i< colorChanges.Length; i++)
        {
           if ( colorChanges[i].color == color)
            {
                instancia = colorChanges[i].prefab; 
            }
        }

        Enemy enemy = ((GameObject)Instantiate(instancia, this.transform.position, this.transform.rotation)).GetComponent<Enemy>();
		if (!isMotherboard)
        	controller.AddEnemyToList(id, enemy);
        enemy.id = id;
		enemy.speed = isMotherboard ? -rb.velocity.y : rb.velocity.x;
        enemy.stepY = stepY;
        enemy.xLimits = xLimits;
		enemy.isMotherboard = isMotherboard; 

		if (!isMotherboard)
      	  controller.CleanEnemy(this, id);


        Destroy(this.gameObject);

    }

	void OnTriggerEnter2D(Collider2D coll){
		if (coll.gameObject.tag == "Bullet" ) {


			if (coll.GetComponent<BulletColor> ().Rend.color == this.color) {
				//life -= coll.GetComponent<BulletColor> ().Damage;
				//if (life <= 0) {
				isDyingCharge = coll.GetComponent<BulletColor>().IsCharged;
				if (isDyingCharge)
					Camera.main.GetComponent<MainCamera> ().CameraShake (); 
				Die (); 
				//}
			} else {
				ChangeColor (coll.GetComponent<BulletColor> ().Rend.color); 
			}
			//coll.GetComponent<BulletColor> ().QueueDestroy (); 
			Destroy(coll.gameObject); 

		}

		if (coll.gameObject.tag == "Spawner") {
			
			SceneManager.LoadScene (2); 

		}

	/*if (coll.gameObject.tag == "Limit") {
			controller.CallAliens (); 
		}

*/
	}


	public void SpeedUp(float increment){
		if (rb == null) {
			rb = this.GetComponent<Rigidbody2D> ();
			rb.velocity = Vector2.right * speed; 

		}

		this.speed += increment; 



		if (rb.velocity.x > 0)
			rb.velocity += Vector2.right * increment;
		else
			rb.velocity -= Vector2.right * increment; 
	}

	public void ChangeDirection(){
		rb.velocity = -rb.velocity; 
		this.transform.position -= stepY * Vector3.up; 
	}

	private void Die(bool forceIsCharging = false){
		isDying = true; 
		if (!isMotherboard)
			controller.CleanEnemy (this, id); 

		if (forceIsCharging == true)
			isDyingCharge = true; 

		if (isDyingCharge && !isMotherboard) {
			RaycastHit2D hitUp = Physics2D.Raycast (this.transform.position + Vector3.up * rend.sprite.bounds.max.y * (this.transform.localScale.y +0.1f), Vector2.up, 0.3f);
		 
			RaycastHit2D hitDown = Physics2D.Raycast(this.transform.position - Vector3.up * rend.sprite.bounds.max.y * (this.transform.localScale.y + 0.1f), Vector2.down, 0.3f);
			RaycastHit2D hitLeft = Physics2D.Raycast(this.transform.position - Vector3.right * rend.sprite.bounds.max.x * (this.transform.localScale.x + 0.1f), Vector2.left, 0.3f);          



			RaycastHit2D hitRight = Physics2D.Raycast(this.transform.position + Vector3.right * rend.sprite.bounds.max.x * (this.transform.localScale.x + 0.1f), Vector2.right, 0.3f);  
			Enemy side = null; 
			if (hitUp.collider != null)
				side = hitUp.collider.GetComponent<Enemy> ();
			else
				side = null;
			if (side != null) {
				if (!side.isDying && side.color == this.color) {
					side.multiplier = multiplier + 1;
					side.Die (true);

				}
			}

			if (hitDown.collider != null)
				side = hitDown.collider.GetComponent<Enemy> ();
			else
				side = null;
			if (side != null) {
				if (!side.isDying && side.color == this.color) {
					side.multiplier = multiplier + 1;

					side.Die (true);
				}
			}

			if (hitLeft.collider != null)
				side = hitLeft.collider.GetComponent<Enemy> ();
			else
				side = null;
			if (side != null) {
				if (!side.isDying && side.color == this.color) {
					side.multiplier = multiplier + 1;

					side.Die (true);
				}
			}

			if (hitRight.collider != null)
				side = hitRight.collider.GetComponent<Enemy> ();
			else
				side = null;
			if (side != null) {
				if (!side.isDying && side.color == this.color) {
					side.multiplier = multiplier + 1;

					side.Die (true);
				}
			}

		}

		if (!isMotherboard)
			controller.AumentScore (10, multiplier); 
		//if (!isMotherboard)
		controller.SpawnText (10 * multiplier, this.transform.position, this.color); 
		Instantiate (explosion, this.transform.position, explosion.transform.rotation); 
		Destroy (this.gameObject); 

	}

	/*public void ChangeColor(Color color){
		if (rend == null) {
			rend = this.GetComponent<SpriteRenderer> (); 
		}
		rend.color = color; 
	}
    */


}

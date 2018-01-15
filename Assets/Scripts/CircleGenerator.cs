using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI; 
public class CircleGenerator : MonoBehaviour {


	[SerializeField]
	private AudioSource chargingBallAudio, dropBallNormalAudio, dropBallHardAudio, chargeAudio;

	private bool isPrefabing = false; 
	[SerializeField] 
	private GameObject circlePrefab;

	[SerializeField]
	private Image imageFill; 

	[SerializeField]
	private float maxFill = 100; 
	private float energy; 

	[SerializeField]
	private float loseFill = 10; 


	[SerializeField]
	private float loseBySecondHold = 12; 

	[SerializeField]
	private AnimationCurve recoverEnergyByTime; 

	private BulletColor prefabing; 

	[SerializeField]
	private float growRatePerSecond = 1;

	[SerializeField]
	private float damagePerSecond = 2; 

	private MainGenerator controller; 
	private float timerEnergy; 

	[SerializeField]
	private float delayTime = 0.4f; 
	private float timerDelay = 0; 

	[SerializeField]
	private float maxRadius; 

	[SerializeField]
	private float maxTimeCharge = 2; 
	private float timerCharge = 0;

	[SerializeField]
	private GameObject canon; 
	// Use this for initialization
	void Start () {
		controller = GameObject.FindGameObjectWithTag ("GameController").GetComponent<MainGenerator> (); 
		timerEnergy = 0; 
		energy = maxFill; 
	
	}
	
	// Update is called once per frame
	void Update () {

		if (Input.touchCount > 0 || Input.GetMouseButton (0)) {
			#if UNITY_ANDROID && !UNITY_EDITOR
			canon.transform.position = new Vector3 (Camera.main.ScreenToWorldPoint (Input.GetTouch (0).position).x, canon.transform.position.y, 0); 
			#endif

			#if UNITY_EDITOR
			canon.transform.position = new Vector3 (Camera.main.ScreenToWorldPoint (Input.mousePosition).x, canon.transform.position.y, 0); 

			#endif

		}

		if (Input.GetMouseButtonDown (0) || (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)) {
			OnMouseDownE ();
		}
		if (Input.GetMouseButtonUp (0) || (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Ended)) {
			OnMouseUpE ();
		}

		if (!isPrefabing) {
			timerDelay += Time.deltaTime; 
			timerEnergy += Time.deltaTime; 
			energy += recoverEnergyByTime.Evaluate (timerEnergy) * Time.deltaTime; 
			energy = Mathf.Clamp (energy, 0, maxFill); 
			//imageFill.fillAmount = energy / maxFill; 
		}
		Vector3 aux;
		if (isPrefabing) {
			timerCharge += Time.deltaTime; 
			//if (energy > 0) {
				//energy -= loseBySecondHold * Time.deltaTime; 
				//imageFill.fillAmount = energy / maxFill; 

				#if UNITY_EDITOR
				aux = Camera.main.ScreenToWorldPoint (Input.mousePosition);
				#endif
				#if UNITY_ANDROID && !UNITY_EDITOR
			aux = Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position);
				#endif
			aux = new Vector3 (aux.x, prefabing.transform.position.y, 0); 
				prefabing.transform.position = aux; 

				/*if (prefabing.transform.position.x > this.transform.position.x + this.transform.localScale.x * 0.5f ||
				prefabing.transform.position.x < transform.position.x - this.transform.localScale.x * 0.5f) {
				OnMouseUp ();
			}
			*/

			if (timerCharge > maxTimeCharge) {

				if (!chargeAudio.isPlaying)
				chargeAudio.Play ();

				prefabing.IsCharged = true; 
			}
				prefabing.UpdateDamageBySecond (damagePerSecond * Time.deltaTime);
				//prefabing.transform.localScale += Vector3.one * growRatePerSecond * Time.deltaTime; 
			//} else {

			//	OnMouseUp (); 
			//}
		}





		}






	void OnMouseDownE(){
		if (timerDelay > delayTime) {
			chargingBallAudio.Stop ();
			chargingBallAudio.Play ();
			timerCharge = 0; 
			timerDelay = 0; 
			isPrefabing = true; 
			energy -= loseFill; 
			timerEnergy = 0; 
			//imageFill.fillAmount = energy / maxFill; 
			Vector3 aux = Camera.main.ScreenToWorldPoint (Input.mousePosition);
			aux = new Vector3 (aux.x, this.transform.position.y +1f, 0); 
			prefabing = ((GameObject)Instantiate (circlePrefab, aux, circlePrefab.transform.rotation)).GetComponent<BulletColor> ();
			prefabing.ChangeColor (controller.GetNextBallColor ());
		}
	}



	void OnMouseUpE(){
		if (isPrefabing) {
			chargeAudio.Stop ();
			if (timerCharge > maxTimeCharge) {
				dropBallHardAudio.Stop ();
				dropBallHardAudio.Play ();

			} else {
				chargingBallAudio.Stop ();
				dropBallNormalAudio.Stop ();
				dropBallNormalAudio.Play ();
			}
			isPrefabing = false; 
			if (prefabing != null) 
				prefabing.DropBall (10); 
		}
	
	}

}

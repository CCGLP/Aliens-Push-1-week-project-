using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening; 
public class MainCamera : MonoBehaviour {

	private Vector3 positionInicial; 
	private bool isScreenShaking = false; 
	private float strengthX = 0.3f; 
	private float strengthY;

	[SerializeField]
	private float strengthXOriginal = 0.4f; 
	[SerializeField]
	private float strengthYOriginal = 0.2f; 

	// Use this for initialization
	void Start () {
		positionInicial = this.transform.position; 
	}

	public void CameraShake(){

		if (!isScreenShaking) {
			strengthX = strengthXOriginal; 
			strengthY = strengthYOriginal; 
			isScreenShaking = true; 
			DOVirtual.DelayedCall (1, () => {
				isScreenShaking = false; 

			});

			DOTween.To (() => strengthX, (x) => strengthX = x, 0f, 1f);
			DOTween.To (() => strengthY, (x) => strengthY = x, 0f, 1f);
		}
	}
	public void BiggerShake(){
		strengthX = strengthXOriginal*2; 
		strengthY = strengthYOriginal*2; 
		isScreenShaking = true; 
		DOVirtual.DelayedCall (1.5f, () => {
			isScreenShaking = false; 

		});

		DOTween.To (() =>strengthX, (x)=>strengthX = x, 0f, 1f );
		DOTween.To (() =>strengthY, (x)=>strengthY = x, 0f, 1f );
	}



	public void FixedUpdate(){
		if (isScreenShaking) {
			this.transform.position = positionInicial + Vector3.right * Random.Range (-strengthX, strengthX) + Vector3.up * Random.Range(-strengthY, strengthY);

		} else {
			this.transform.position = positionInicial; 
		}

	}


}

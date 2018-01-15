using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoDestroy : MonoBehaviour {

	[SerializeField]
	private float destrozaTime = 2;
		private float timer; 
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		timer += Time.deltaTime;
		if (timer > destrozaTime) {
			timer = 0; 
			Destroy (this.gameObject); 
		}
	}
}

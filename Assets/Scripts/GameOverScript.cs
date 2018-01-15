using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 
using UnityEngine.SceneManagement; 

public class GameOverScript : MonoBehaviour {

	[SerializeField]
	private Text scoreText; 

	[SerializeField]
	private Image gameOverImage; 
	[SerializeField]
	private Sprite winSprite; 

	// Use this for initialization
	void Start () {
		scoreText.text = StaticVariables.score.ToString (); 

		if (PlayerPrefs.GetInt ("win", -1) == 1) {
			gameOverImage.sprite = winSprite; 
		}
	}
	
	// Update is called once per frame
	void Update () {
	/*	if (Input.GetMouseButton (0) || Input.touchCount != 0) {
			SceneManager.LoadScene (1); 
		}
	*/

	}

	public void OnLevelReset(){
		SceneManager.LoadScene (1); 
	}

}

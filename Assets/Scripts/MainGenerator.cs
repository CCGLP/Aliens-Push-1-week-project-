using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 
using DG.Tweening; 
public class MainGenerator : MonoBehaviour {

	[SerializeField]
	private Color[] colorsArrays; 
	[SerializeField]
	private float startYSpawn = 10;   

	[SerializeField]
	private Vector2 xLimits;

    [SerializeField]
    private GameObject[] prefabsAliens; 

	[SerializeField]
	private int columnAliens = 3; 


	[SerializeField]
	private float yLimitUp = 3; 



	private float timer;
	private float timerSpawn; 

	private List<List<Enemy>> enemiesList; 

	private Color[] colors; 

	[SerializeField]
	private int alienRows = 4; 


	private bool isCalling = false; 

	private float acumulateSpeedUp = 0; 

	[SerializeField]
	private SpriteRenderer[] nextImages; 

	[SerializeField]
	private Text textScore; 


	[SerializeField]
	private GameObject textMeshPrefab; 
	public Color GetRandomColor(){
		return colorsArrays[Random.Range(0,colorsArrays.Length)];

	}

	public void AumentScore(int up, int multiplier = 1){
		StaticVariables.score += up * multiplier; 
		textScore.text = StaticVariables.score.ToString ();

	}



	// Use this for initialization
	void Awake () {
		PlayerPrefs.SetInt ("win", -1); 
		StaticVariables.score = 0; 
		timerSpawn = 0; 
		timer = 0; 
		enemiesList = new List<List<Enemy>> (); 
		colors = new Color[2]; 
		colors [0] = GetRandomColor (); 
		colors [1] = GetRandomColor (); 
		nextImages [0].color = colors [0];
		nextImages [1].color = colors [1]; 
		SpawnAliens (xLimits);

		AumentScore (0); 
		float sizeY = yLimitUp - startYSpawn; 
		 


	}

    public GameObject GetRandomPrefabAlien()
    {
        return prefabsAliens[Random.Range(0, prefabsAliens.Length)];

    }


	public int GetAliensCount(){

		int countList = 0; 

		for (int i = 0; i < enemiesList.Count; i++) {

			for (int j = 0; j < enemiesList [i].Count; j++) {
				countList++; 

			}
		}

		return countList; 

	}

	public Color GetNextBallColor(){
		Color aux = colors [0];

		colors [0] = colors [1];
		colors [1] = GetRandomColor (); 
		nextImages [0].color = colors [0];
		nextImages [1].color = colors [1]; 


		return aux; 



	}


	public void SpawnText(int score, Vector3 position, Color color){
		TextMesh text = ((GameObject)Instantiate (textMeshPrefab, position, textMeshPrefab.transform.rotation)).GetComponent<TextMesh> ();

		text.text = score.ToString ();
		text.color = color; 
		text.transform.DOMoveY (text.transform.position.y +  2, 2.5f); 

		DOTween.ToAlpha (() => text.color, x => text.color = x, 0, 2.5f).OnComplete(()=>{
			Destroy(text.gameObject); 
		}); 


	}

	public void CallAliens(int id){
		List<Enemy> enemies = enemiesList [id];
		isCalling = true; 
		for (int i = 0; i < enemies.Count; i++) {
			if (enemies[i] != null)
				enemies [i].ChangeDirection (); 
		}

		isCalling = false; 
	}



	public void CleanEnemy(Enemy enemy, int id){
		
		enemiesList[id].Remove (enemy); 

	}

	public void CleanEnemy(int id){
		//enemies.RemoveAt (id); 
	}

	public void AllAliensSpeedUp(float speedUp, bool noAccumulate = true){

		List<Enemy> enemies;  
		if (noAccumulate)
			acumulateSpeedUp += speedUp; 
		for (int j = 0; j < enemiesList.Count; j++) {
			enemies = enemiesList [j];
			for (int i = 0; i < enemies.Count; i++) {
				enemies [i].SpeedUp (speedUp); 
			}
		}

	}



    private void SetAllScalesAliens(Vector3 size)
    {
        for (int i = 0; i<prefabsAliens.Length; i++)
        {
            prefabsAliens[i].transform.localScale = size; 
        }
    }

    public void AddEnemyToList (int id, Enemy enemy)
    {
        enemiesList[id].Add(enemy);
    }

	public void SpawnNewRound(){
		CleanAllAliens ();
		SpawnAliens (xLimits);
		AllAliensSpeedUp (acumulateSpeedUp,false); 
	}

	private void CleanAllAliens(){
		
		for (int i = 0; i < enemiesList.Count; i++) {
			for (int j = 0; j < enemiesList [i].Count; j++) {
				Destroy (enemiesList [i][j].gameObject);

			}
		}

		for (int i = 0; i < enemiesList.Count; i++) {
			enemiesList.Clear (); 
		}

	}



	private void SpawnAliens(Vector2 xLimits){

		float sizeX = (xLimits.y - xLimits.x) / columnAliens; 
		float sizeY = (yLimitUp - startYSpawn) / alienRows; 

		float size = sizeX <= sizeY ? sizeX  : sizeY; 
		
		List<Enemy> enemies = new List<Enemy> ();
		SetAllScalesAliens(Vector3.one * ((size ) -0.1f)); 

		float xStart = xLimits.x + prefabsAliens[0].transform.localScale.x * 0.5f; 
		//float xStep = (Mathf.Abs (xLimits.x) + Mathf.Abs (xLimits.y)) / (prefabAlien.transform.localScale.x + 0.5f); 

		float x = xStart;
		float y = startYSpawn; 

		int count = 0; 
		for (int i = 0; i < columnAliens; i++) {
			for (int j = 0; j < alienRows; j++) {
				enemies.Add(((GameObject)Instantiate(GetRandomPrefabAlien(), new Vector3(x, y, 0), prefabsAliens[0].transform.rotation)).GetComponent<Enemy>()); 
				
				enemies [enemies.Count - 1].SetXlimits (xLimits);
				enemies [enemies.Count - 1].id = enemiesList.Count; 
				//enemies [enemies.Count - 1].name = count.ToString ();
				count++; 
				y += prefabsAliens[0].transform.localScale.y + 0.2f; 
			}
			y = startYSpawn; 
			x += prefabsAliens[0].transform.localScale.x + 0.2f; 
		}

		enemiesList.Add (enemies); 

	}


	private void SpawnNextAliens(){

		List<Enemy> enemies = new List<Enemy> (); 
		float x = xLimits.x + prefabsAliens[0].transform.localScale.x * 0.5f; 

		for (int i = 0; i < columnAliens; i++) {
			enemies.Add(((GameObject)Instantiate (GetRandomPrefabAlien(), new Vector3(x, yLimitUp, 0), prefabsAliens[0].transform.rotation)).GetComponent<Enemy>()); 

			enemies [enemies.Count - 1].SetXlimits (xLimits);
			enemies [enemies.Count - 1].id = enemiesList.Count; 
			x += prefabsAliens[0].transform.localScale.x + 0.2f; 
		}

		enemiesList.Add (enemies); 

	}

	
	// Update is called once per frame
	void Update () {
		timer += Time.deltaTime; 
		timerSpawn += Time.deltaTime; 


	}




	#if UNITY_EDITOR
	void OnDrawGizmos(){
		Gizmos.DrawLine (new Vector3 (-10, startYSpawn, 0), new Vector3 (10, startYSpawn, 0)); 

		Gizmos.DrawLine (new Vector3 (-10, yLimitUp, 0), new Vector3 (10, yLimitUp, 0)); 


		Gizmos.DrawLine (new Vector3 (xLimits.x, -10, 0), new Vector3 (xLimits.x, 10, 0)); 
		Gizmos.DrawLine (new Vector3 (xLimits.y, -10, 0), new Vector3 (xLimits.y, 10, 0)); 



	}
	#endif

	public Vector2 XLimits {
		get {
			return xLimits;
		}
	}

	public bool IsCalling {
		get {
			return isCalling;
		}
	}


		
}

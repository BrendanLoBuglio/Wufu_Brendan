using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum CursorMode{grow, recede, disabled}

public class GrowBeamCursor : MonoBehaviour {
	//Cursor state stuff:
	public CursorMode myMode = CursorMode.disabled;
	public Material growMaterial;
	public Material recedeMaterial;
	public Material rescaleMaterial;
	
	public static Transform cursorTransform;
	
	public float resizeFactor = 0.5f;
	[HideInInspector]public ParticleSystem growParticles;
	[HideInInspector]public ParticleSystem recedeParticles;
	
	public List<LifeCell> selectedLifeCells;
	public static int[] plantSpeciesRatios; //indexes line up with the "PlantCategory" enumerator (Declared in MultiLifeCell.cs
	
	void Start () 
	{
		growParticles = GameObject.FindGameObjectWithTag("GrowParticles").GetComponent<ParticleSystem>();
		recedeParticles = GameObject.FindGameObjectWithTag("RecedeParticles").GetComponent<ParticleSystem>();
		cursorTransform = transform;
		plantSpeciesRatios = new int[System.Enum.GetValues(typeof (PlantCategory)).Length];
		for (int i = 0; i< plantSpeciesRatios.Length;i++){
			plantSpeciesRatios[i]=0;
		}
	}
	
	// Update is called once per frame
	void Update () 
	{
		//Switching between grow/Recede mode
		if(Input.GetKey (KeyCode.Mouse0) || Input.GetKey (KeyCode.Space))
			myMode = CursorMode.grow;
		else if (Input.GetKey (KeyCode.Mouse1))
			myMode = CursorMode.recede;
		else
			myMode = CursorMode.disabled;
		if(myMode == CursorMode.grow){
			renderer.enabled = true;
			renderer.material = growMaterial;
			collider.enabled = true;
			recedeParticles.enableEmission = false;
			growParticles.enableEmission = true;
		}
		if(myMode == CursorMode.recede){
			renderer.enabled = true;
			renderer.material = recedeMaterial;
			collider.enabled = true;
			growParticles.enableEmission = false;
			recedeParticles.enableEmission = true;
		}
		if(myMode == CursorMode.disabled){
			recedeParticles.enableEmission = false;
			growParticles.enableEmission = false;
			renderer.enabled = false;
			collider.enabled = false;
		}
		
		//Cursor rescaling
		//NOTE: rescaling the cursor re-enables the renderer, even if the mouse is not being clicked. For that reason, it's important that the following code comes
			//AFTER the bit where the renderer is disabled if the mouse is not clicked	
		if(Input.GetKey (KeyCode.Q) || Input.GetKey (KeyCode.E)){
			renderer.enabled = true;
			renderer.materials = new Material[]{renderer.materials[0], rescaleMaterial};
		}
		else{
			renderer.materials = new Material[]{renderer.materials[0]};
		}
		Vector3 newScale = gameObject.transform.localScale;
		if(Input.GetKey (KeyCode.Q)){
			newScale.x = gameObject.transform.localScale.x - Time.deltaTime * resizeFactor;
			newScale.y = gameObject.transform.localScale.y- Time.deltaTime * resizeFactor;
			newScale.z = gameObject.transform.localScale.z - Time.deltaTime * resizeFactor;
		}
		if(Input.GetKey (KeyCode.E)){
			newScale.x = gameObject.transform.localScale.x + Time.deltaTime * resizeFactor;
			newScale.y = gameObject.transform.localScale.y + Time.deltaTime * resizeFactor;
			newScale.z = gameObject.transform.localScale.z + Time.deltaTime * resizeFactor;
		}
		gameObject.transform.localScale = newScale;
	}
	
	void OnTriggerStay(Collider other)
	{
		if(myMode != CursorMode.disabled){
			if(other.gameObject.GetComponent<LifeCell>()){
				LifeCell otherLifeCell = other.gameObject.GetComponent<LifeCell>();
				if(myMode == CursorMode.grow)
					otherLifeCell.growthLevel += Time.deltaTime * otherLifeCell.growthRate;
				else if (myMode == CursorMode.recede)
					otherLifeCell.growthLevel -= Time.deltaTime * otherLifeCell.growthRate;
					
				if(ExtraInput.framesSinceMouseWasClicked < 2 && !selectedLifeCells.Contains(otherLifeCell)){
					selectedLifeCells.Add(otherLifeCell);
				}
			}
		}
	}
	
	void LateUpdate()
	{
		if(!Input.GetKey (KeyCode.Mouse0)){
			selectedLifeCells = new List<LifeCell>();
			for(int i = 0;i<plantSpeciesRatios.Length;i++){
				plantSpeciesRatios[i] = 0;
			}
			//Clear the selected life cells list if the mouse comes up
		}
		if(ExtraInput.framesSinceMouseWasClicked ==3){
			for(int i = 0; i < selectedLifeCells.Count; i++){
				plantSpeciesRatios[(int)selectedLifeCells[i].currentCategory]++;
			}
			plantSpeciesRatios[0]=0;//All selected lifeCells with the PlantCategory "none" are disregarded
			Debug.Log ("plantSpeciesRatios is " + plantSpeciesRatios.Length + " long!");
		}
	}
	
	public static PlantCategory chooseCategoryByCursorPolarity()
	{
		if(sumOfArray (plantSpeciesRatios, 999) == 0){
			return (PlantCategory)Random.Range (1, plantSpeciesRatios.Length);
		}
		int diceRoll = Random.Range(0, sumOfArray (plantSpeciesRatios, 999));
		for(int i = 1; i < plantSpeciesRatios.Length; i++){
			if(plantSpeciesRatios[i] > 0){
				if(diceRoll >= sumOfArray(plantSpeciesRatios, i-1) && diceRoll < sumOfArray(plantSpeciesRatios, i))
					return (PlantCategory)i;
			}
		}
		//default (should only execute if I've fucked something up):
		Debug.Log ("Default return! this is fucking bad!");
		return (PlantCategory)Random.Range(1, plantSpeciesRatios.Length);
	}
	public static int sumOfArray(int[] arrayIn, int depthOfSum)
	//returns the sum of the values in an int array, but only looks at the first however many (depthOfSum) indexes
	{
		int sumOut = 0;
		for(int i = 0; i < arrayIn.Length && i <= depthOfSum; i++){
			sumOut += arrayIn[i];
		}
		return sumOut;
	}
}

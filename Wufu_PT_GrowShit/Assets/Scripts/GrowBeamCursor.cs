using UnityEngine;
using System.Collections;

public enum CursorMode{grow, recede, disabled}

public class GrowBeamCursor : MonoBehaviour {
	//Cursor state stuff:
	public CursorMode myMode = CursorMode.disabled;
	public Material growMaterial;
	public Material recedeMaterial;
	public Material rescaleMaterial;
	
	public float growFactor = 0.5f;
	[HideInInspector]public ParticleSystem growParticles;
	[HideInInspector]public ParticleSystem recedeParticles;
	void Start () 
	{
		growParticles = GameObject.FindGameObjectWithTag("GrowParticles").GetComponent<ParticleSystem>();
		recedeParticles = GameObject.FindGameObjectWithTag("RecedeParticles").GetComponent<ParticleSystem>();
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
			newScale.x = gameObject.transform.localScale.x - Time.deltaTime * growFactor;
			newScale.y = gameObject.transform.localScale.y- Time.deltaTime * growFactor;
			newScale.z = gameObject.transform.localScale.z - Time.deltaTime * growFactor;
		}
		if(Input.GetKey (KeyCode.E)){
			newScale.x = gameObject.transform.localScale.x + Time.deltaTime * growFactor;
			newScale.y = gameObject.transform.localScale.y + Time.deltaTime * growFactor;
			newScale.z = gameObject.transform.localScale.z + Time.deltaTime * growFactor;
		}
		gameObject.transform.localScale = newScale;
	}
	
	void OnTriggerStay(Collider other)
	{
		if(myMode != CursorMode.disabled){
			if(other.gameObject.GetComponent<LifeCell>()){
				if(myMode == CursorMode.grow)
					other.gameObject.GetComponent<LifeCell>().growthLevel += Time.deltaTime;
				else if (myMode == CursorMode.recede)
			   		other.gameObject.GetComponent<LifeCell>().growthLevel -= Time.deltaTime;
			}
		}
	}
}

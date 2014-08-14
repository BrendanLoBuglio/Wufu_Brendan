using UnityEngine;
using System.Collections;

public class GrowBeam : MonoBehaviour {
	[HideInInspector]public GrowBeamCursor hitSphere;
	void Start()
	{
		hitSphere = gameObject.GetComponentInChildren<GrowBeamCursor>();
	}
	
	// Update is called once per frame
	void Update () 
	{
		Ray screenRay = camera.ScreenPointToRay(new Vector3(Screen.width/2f, Screen.height/2f,0));
		RaycastHit growBeamHit;
		if(Physics.Raycast(screenRay, out growBeamHit, 20f)){
			//GameObject newSphere = (GameObject)Instantiate(hitSphere, growBeamHit.point, Quaternion.identity);
			hitSphere.transform.position = growBeamHit.point;
			hitSphere.transform.rotation = Quaternion.identity;
			//newSphere.renderer.material.color = new Color(Random.Range(0f,1f),Random.Range(0f,1f),Random.Range(0f,1f));
		}
	}
}

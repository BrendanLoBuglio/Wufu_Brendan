using UnityEngine;
using System.Collections;

public class GrowBeam : MonoBehaviour {
	[HideInInspector]public GrowBeamCursor hitSphere;
	[HideInInspector]public Transform radialGrowCenter;
	void Start()
	{
		hitSphere = gameObject.GetComponentInChildren<GrowBeamCursor>();
		radialGrowCenter = GameObject.FindGameObjectWithTag("RadialCenter").transform;
	}
	
	// Update is called once per frame
	void Update () 
	{
		//"Radial" (Monoke nature spirit-style) Growth mode:
		if(Input.GetKey (KeyCode.Space)){
			hitSphere.transform.position = radialGrowCenter.position;
			hitSphere.transform.rotation = Quaternion.identity;
		}
		//"Linear" (Beam-style) Growth mode:
		if(!Input.GetKey (KeyCode.Space)){
			Ray screenRay = camera.ScreenPointToRay(new Vector3(Screen.width/2f, Screen.height/2f,0));
			RaycastHit growBeamHit;
			if(Physics.Raycast(screenRay, out growBeamHit, 20f)){
				hitSphere.transform.position = growBeamHit.point;
				hitSphere.transform.rotation = Quaternion.identity;
			}
		}
	}
}

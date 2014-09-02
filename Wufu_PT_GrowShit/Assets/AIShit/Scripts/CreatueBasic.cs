using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum BehaviorState {sleeping, exploring}

public class CreatueBasic : MonoBehaviour 
{
	public GameObject interestPointFab;
	public float baseSpeed = 5f;
	public float energyCapacity = 15f;
	
	private InterestPoint homePoint;
	private InterestPoint interestPointTarget;
	private bool initialTargetChosen = false;
	private float energyLevel;
	
	[HideInInspector]public BehaviorState myState = BehaviorState.exploring;
	
	void Start()
	{
		energyLevel = energyCapacity;
		GameObject homePointObject = (GameObject)Instantiate(interestPointFab,transform.position,Quaternion.identity);
		homePoint = homePointObject.GetComponent<InterestPoint>();
		homePoint.myAttributes.Add (InterestPointAttribute.rest);
		homePoint.dwellingOwner = gameObject;
		StartCoroutine("ChooseInitialTarget");
	}
	IEnumerator ChooseInitialTarget()
	{
		while(!initialTargetChosen){
			if(homePoint.zone != null && !initialTargetChosen && ZoneManager.manager.worldInstantiated){
				DecideTarget();
				initialTargetChosen = true;
			}
			yield return null;
		}	
	}
	
	void Update()
	{
		if(myState == BehaviorState.exploring){
			energyLevel -= Time.deltaTime;
			//Calculate speed based on time of day:
			float speed = baseSpeed * (0.875f + (0.125f * DayNightCycle.normalizedTime));
			
			if(interestPointTarget != null){
				Vector3 inFront = transform.position + transform.forward;
				float currentAngle = Mathf.Rad2Deg * Mathf.Atan2(inFront.z-transform.position.z,inFront.x-transform.position.x);
				float angleTo = Mathf.Rad2Deg * Mathf.Atan2 (interestPointTarget.transform.position.z-transform.position.z,interestPointTarget.transform.position.x-transform.position.x);
				float angVel = Mathf.Clamp (-1f + 10f / Vector3.Distance(transform.position,interestPointTarget.transform.position),1f, 999f);
				float newAngle = Mathf.Deg2Rad * Mathf.LerpAngle(currentAngle, angleTo, Time.deltaTime * angVel);
				Vector3 newInFront = transform.position + new Vector3(Mathf.Cos (newAngle), 0, Mathf.Sin(newAngle));
				
				transform.LookAt(newInFront);
				//transform.LookAt(interestPointTarget.transform.position);
				transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, 0);
				Vector3 moveTo = transform.forward*Time.deltaTime * speed;
				if(moveTo.magnitude >= Vector3.Distance(transform.position, interestPointTarget.transform.position))
					moveTo = transform.forward * Vector3.Distance(transform.position, interestPointTarget.transform.position);
			
				transform.position += moveTo;
			}
		}
		if(myState == BehaviorState.sleeping)
		{
			energyLevel += Time.deltaTime;
			if(energyLevel >= energyCapacity && DayNightCycle.normalizedTime > 0f)
			{
				myState = BehaviorState.exploring;
				Material newMat = MaterialLibrary.library.C_Exploring;
				renderer.material = newMat;
				DecideTarget();
			}
		}
		
		energyLevel = Mathf.Clamp (energyLevel, 0, energyCapacity);
	}
	
	void DecideTarget()
	{
		List<GameObject> excludeList = new List<GameObject>();
		if(interestPointTarget != null){
			interestPointTarget.GetComponent<InterestPoint>().targetingCreature = null;
			excludeList.Add(interestPointTarget.gameObject);
		}
		
		//Choose a random, nearby target if it's the daytime.
		if(DayNightCycle.normalizedTime > 0f || energyLevel > 0f){
			WorldZone zone = homePoint.zone.GetComponent<WorldZone>();
			interestPointTarget = ZoneManager.manager.FindRandomInterestPoint((int)zone.positionInWorldGrid.x,(int)zone.positionInWorldGrid.y,1,InterestPoint.allAttributes,excludeList).GetComponent<InterestPoint>();
		}
		//Choose your home if it's a certain point in the night
		if(DayNightCycle.normalizedTime <= 0f && energyLevel <= 0f)
		{
			interestPointTarget = homePoint;
		}
	}
	
	void OnTriggerEnter(Collider other)
	{
		if(interestPointTarget != null){
			if(other.gameObject.tag == "InterestPoint" && other.gameObject == interestPointTarget.gameObject && interestPointTarget != homePoint.gameObject){
				DecideTarget();
			}
			if(other.gameObject.tag == "InterestPoint" && other.gameObject == interestPointTarget.gameObject && interestPointTarget == homePoint){
				myState = BehaviorState.sleeping;
				Material newMat = MaterialLibrary.library.C_Rest;
				renderer.material = newMat;
			}
		}
	}
}

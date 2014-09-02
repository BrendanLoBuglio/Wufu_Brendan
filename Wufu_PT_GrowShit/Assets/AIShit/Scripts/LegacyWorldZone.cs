using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LegacyWorldZone : MonoBehaviour 
{
	public GameObject interestPointFab;
	public List<GameObject> interestPoints = new List<GameObject>();
	public int numberOfInterestPoints = 7;
	public Vector2 positionInWorldGrid;
	
	
	private float zoneWidth;
	private float interestPointWidth;
	public bool isInitialized = false;
	void Start()
	{
		zoneWidth = (renderer.bounds.max.x - collider.bounds.min.x) * 2f;
		interestPointWidth = 5 * (interestPointFab.renderer.bounds.max.z - interestPointFab.collider.bounds.min.z) * 2f;
		isInitialized = true;
	}
	
	
	public void PopulateWithInterestPoints()
	{
		for(int i = 0; i < numberOfInterestPoints; i++){
			Vector3 newPosition = Vector3.one;
			float lowestDistanceFromPoints;
			bool keepGoing = true;
			int keepGoingTimeOut = 0;
			while(keepGoing && keepGoingTimeOut<10){
				lowestDistanceFromPoints = 999f;
				newPosition = new Vector3(Random.Range(transform.position.x-zoneWidth/4f,transform.position.x + zoneWidth/4f),0,Random.Range(transform.position.z-zoneWidth/4f,transform.position.z+zoneWidth/4f));
				for(int k = 0; k < interestPoints.Count; k++){
					if(Vector3.Distance(newPosition,interestPoints[k].transform.position) < lowestDistanceFromPoints){
						lowestDistanceFromPoints = Vector3.Distance(newPosition,interestPoints[k].transform.position);
						}
				}
				if(lowestDistanceFromPoints > interestPointWidth || interestPoints.Count <= 1)
					keepGoing = false;
				else
					keepGoingTimeOut++;
			}
			//Debug.Log ("KeepGoingTimeOut is " + keepGoingTimeOut);
			GameObject newInterestPoint = (GameObject)Instantiate(interestPointFab, newPosition, Quaternion.identity);
			interestPoints.Add(newInterestPoint);
			newInterestPoint.transform.parent = transform;
		}
	}
}

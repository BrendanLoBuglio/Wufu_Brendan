using UnityEngine;
using System.Collections;

public class WorldLister : MonoBehaviour 
{
	public static GameObject[] interestPoints;
	
	void Awake()
	{
		interestPoints = GameObject.FindGameObjectsWithTag("InterestPoint");
	}
	
	void Update()
	{
		interestPoints = GameObject.FindGameObjectsWithTag("InterestPoint");
	}
}

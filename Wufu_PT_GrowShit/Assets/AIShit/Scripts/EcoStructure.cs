using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public enum EcoStructureType {tree, waterHole, spring, flatRock, bush, cactus}

public class EcoStructure : MonoBehaviour 
{
	public EcoStructureType myType;
	public float sizeValue;
	public float approxWidth;
	public List<GameObject> interestPoints;
	public bool isInitialized = false;
	
	public void Awake(){
		if(!isInitialized)
			Initialize();
	}
	
	public void Initialize()
	{
		List<InterestPoint> stuff = transform.GetComponentsInChildren<InterestPoint>().ToList();
		for(int i = 0; i < stuff.Count; i++){
			interestPoints.Add(stuff[i].gameObject);
		}
		isInitialized = true;
	}
}

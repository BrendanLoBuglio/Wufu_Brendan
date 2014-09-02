using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ZoneManager : MonoBehaviour {
	public static ZoneManager manager;
	
	public GameObject zonePrefab;
	public int zoneGridColumns = 10;
	public int zoneGridRows = 10;
	public float zoneAlpha = 0.1f;
	
	public GameObject[,] zones;

	private float zoneWidth;
	
	private bool donePopulating = false;
	public bool worldInstantiated = false;
	void Start () 
	{
		manager = this;
		zoneWidth = (zonePrefab.renderer.bounds.max.x - zonePrefab.collider.bounds.min.x) * 2f;
		Instantiatezones(zoneGridColumns, zoneGridRows);
		StartCoroutine("PopulateZones");
	}
	
	void Update()
	{
		int numberOfFinishedZones = 0;
		if(donePopulating && !worldInstantiated){
			for(int x = 0; x < zoneGridColumns; x++){
				for(int y = 0; y < zoneGridRows; y++){
					if(zones[x,y].GetComponent<WorldZone>().isPopulated)
						numberOfFinishedZones++;
				}
			}
			if(numberOfFinishedZones >= zoneGridColumns * zoneGridRows)
				worldInstantiated = true;
		}
	}
	
	void Instantiatezones(int rows, int columns)
	{
		zones = new GameObject[rows,columns];
		
		Vector3 startingPoint = transform.position - new Vector3(0.5f*rows*zoneWidth,0 , 0.5f*columns*zoneWidth);
		for(int x = 0; x < columns; x++)
		{
			for(int y = 0; y < rows; y++)
			{
				Vector3 nextzonePosition = new Vector3(startingPoint.x+x*zoneWidth, startingPoint.y, startingPoint.z+y*zoneWidth);
				zones[x,y] = (GameObject)Instantiate(zonePrefab, nextzonePosition, Quaternion.identity);
				zones[x,y].renderer.material.color = new Color(Random.Range(0f,1f),Random.Range(0f,1f),Random.Range(0f,1f), zoneAlpha);
				zones[x,y].GetComponent<WorldZone>().positionInWorldGrid = new Vector2(x, y);
			}
		}
	}
	
	public GameObject FindRandomInterestPoint(int xIn, int yIn, int range, List<InterestPointAttribute> allowedTypes,List<GameObject> excluded)
	{
		//Populate a list of zones in which to search for interestPoints
		List <GameObject> zonesToSearch = new List<GameObject>();
		for(int zoneX = 0; zoneX < zoneGridColumns; zoneX++){
			for(int zoneY = 0; zoneY < zoneGridRows; zoneY++){
				int sumOfDifferences = Mathf.Abs (zoneX - xIn) + Mathf.Abs (zoneY - yIn);
				if(sumOfDifferences <= range)
					zonesToSearch.Add(zones[zoneX,zoneY]);
			}
		}
		zonesToSearch = RandomizeListOrder(zonesToSearch);
		//Choose an interestPoint among the valid zones:
		
		for(int i = 0; i < zonesToSearch.Count; i++){
			WorldZone selectedZone = zonesToSearch[i].GetComponent<WorldZone>();
			List<GameObject> interestPointsToSearch = RandomizeListOrder(selectedZone.interestPoints);
			for(int j = 0; j < interestPointsToSearch.Count; j++){
				InterestPoint selectedPoint = interestPointsToSearch[j].GetComponent<InterestPoint>();
				if(selectedPoint.targetingCreature == null && selectedPoint.dwellingOwner == null){
					bool notExcluded = true;
					for(int k = 0; k < excluded.Count; k++)
						if(selectedPoint.gameObject == excluded[k])
							notExcluded = false;
					if(notExcluded){
						selectedPoint.targetingCreature = gameObject;
						return selectedPoint.gameObject;
					}
				}
			}
		}
		Debug.Log ("DEFAULT RETURN EVERYTHING IS FUCKED!!! zonesToSearch.Count is " + zonesToSearch.Count);
		return gameObject;
	}		

	IEnumerator PopulateZones()
	{
		for(int x = 0; x < zoneGridColumns; x++){
			for(int y = 0; y < zoneGridRows; y++){
				WorldZone worldZone = zones[x,y].GetComponent<WorldZone>();
				if(worldZone.isInitialized){
					worldZone.PopulateWithEcoStructures();
				}else
					y--;
				yield return null;
			}
		}
		donePopulating = true;
	}
	
	public GameObject GetZoneAtPosition(Vector3 positionIn)
	{
		Vector3 startingPosition = transform.position - new Vector3(0.5f*zoneGridRows*zoneWidth,0, 0.5f*zoneGridColumns*zoneWidth) - new Vector3 (0.5f*zoneWidth,0,0.5f*zoneWidth);
		Vector3 differenceInPosition = positionIn - startingPosition;
		
		int xOut = (int) (differenceInPosition.x / zoneWidth);
		int zOut = (int) (differenceInPosition.z / zoneWidth);
		xOut = Mathf.Clamp (xOut, 0, zoneGridColumns - 1);
		zOut = Mathf.Clamp (zOut, 0, zoneGridRows - 1);
		return zones[xOut,zOut];
	}
	
	List<GameObject> RandomizeListOrder(List<GameObject> listIn)
	{
		List<GameObject> newList = new List<GameObject>();
		bool[] assignedIndexes = new bool[listIn.Count];
		for(int i = 0; i < assignedIndexes.Length; i++)
			assignedIndexes[i] = false;
		
		int numberOfAssigned = 0;
		
		while(numberOfAssigned < listIn.Count)
		{
			int diceRoll = Random.Range (0, listIn.Count);
			if(assignedIndexes[diceRoll] == false){
				numberOfAssigned++;
				assignedIndexes[diceRoll] = true;
				newList.Add (listIn[diceRoll]);
			}
		}
		//Debug.Log ("The list that was put in was " + listIn.Count + " and the list that came out is " + newList.Count);
		return newList;
	}
}

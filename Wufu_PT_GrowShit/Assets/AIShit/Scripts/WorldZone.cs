using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WorldZone : MonoBehaviour 
{
	public BiomeType myBiomeType;
	public List<GameObject> ecoStructurePrefabs;
	public List<GameObject> ecoStructures = new List<GameObject>();
	public List<GameObject> interestPoints = new List<GameObject>();
	public int sizeBudget = 7;
	public Vector2 positionInWorldGrid;
	private float zoneWidth;
	public bool isInitialized = false;
	public bool isPopulated = false;
	void Start()
	{
		myBiomeType = (BiomeType)Random.Range(0, (int)BiomeType.NUMBER_OF_ELEMENTS);
		//cull the ecoStructurePrefabs of all EcoStructures that aren't a part of my biome
		for(int i = 0; i < ecoStructurePrefabs.Count; i++){
			if(!ecoStructurePrefabs[i].GetComponent<EcoStructure>()){
				ecoStructurePrefabs.RemoveAt(i);
				i--;
			}
			else{
				EcoStructureType structure = ecoStructurePrefabs[i].GetComponent<EcoStructure>().myType;
				if(!Biome.getStructuresForBiome(myBiomeType).Contains(structure)){
					ecoStructurePrefabs.RemoveAt(i);
					i--;
				}
			}
		}	
		zoneWidth = (renderer.bounds.max.x - collider.bounds.min.x) * 2f;
		isInitialized = true;
	}
	public void PopulateWithEcoStructures()
	{
		float sizePointsSpent = 0f;
		while(sizePointsSpent < sizeBudget){
			GameObject newObjectFab = ecoStructurePrefabs[Random.Range (0,ecoStructurePrefabs.Count)];
			Vector3 newPosition = Vector3.one;
			float lowestDistanceFromPoints;
			float closestPointApproxWidth = 1f;
			bool keepGoing = true;
			int keepGoingTimeOut = 0;
			while(keepGoing && keepGoingTimeOut<10){
				lowestDistanceFromPoints = 999f;
				newPosition = new Vector3(Random.Range(transform.position.x-zoneWidth/4f,transform.position.x + zoneWidth/4f),0,Random.Range(transform.position.z-zoneWidth/4f,transform.position.z+zoneWidth/4f));
				for(int k = 0; k < ecoStructures.Count; k++){
					if(Vector3.Distance(newPosition,ecoStructures[k].transform.position) < lowestDistanceFromPoints){
						lowestDistanceFromPoints = Vector3.Distance(newPosition,ecoStructures[k].transform.position);
						closestPointApproxWidth = ecoStructures[k].GetComponent<EcoStructure>().approxWidth;
					}
				}
				if(lowestDistanceFromPoints > closestPointApproxWidth || ecoStructures.Count <= 0)
					keepGoing = false;
				else
					keepGoingTimeOut++;
			}
			EcoStructure newEcoStructure = ((GameObject)Instantiate(newObjectFab, newPosition, Quaternion.Euler(0f, Random.Range (0f, 360f), 0f))).GetComponent<EcoStructure>();
			sizePointsSpent += newEcoStructure.sizeValue;
			newEcoStructure.transform.parent = transform;
			ecoStructures.Add(newEcoStructure.gameObject);
			newEcoStructure.Initialize();
			interestPoints.AddRange(newEcoStructure.interestPoints);
		}
		isPopulated = true;
	}
}

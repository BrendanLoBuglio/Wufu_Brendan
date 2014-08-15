using UnityEngine;
using System.Collections;

public enum PlantCategory {none, mush, wood, flower, cactus}

public class MultiLifeCell : LifeCell {
	public GameObject emptyFab;
	public GameObject grassFab;
	public GameObject[] mushTrack = new GameObject[5];
	public GameObject[] woodTrack = new GameObject[5];
	
	public GameObject containedPlant;//ContainedPlant holds the piece of plant life currently living in this cell
	public GameObject containedGrass;//ContainedPlant holds the unit of grass currently living in this cell. This is stored separate from ContainedPlant so that the grass remains even as further growth occurs.
	
	public float oldGrowthLevel = 0;
	public int hierarchyLevel = 0;
	public int oldHierarchyLevel = 0;
	public PlantCategory currentCategory = PlantCategory.none;
	public GameObject[] currentTrack;
	
	void Start()
	{
		containedPlant = (GameObject)Instantiate(emptyFab, transform.position, Quaternion.identity);
		containedGrass = (GameObject)Instantiate(emptyFab, transform.position, Quaternion.identity);
	}
	
	void Update()
	{
		if(oldGrowthLevel != growthLevel){
			ManageHierarchy();
			ManageContainedPlants();
		}
		oldGrowthLevel = growthLevel;
		
	}
	void ManageHierarchy(){
		if(growthLevel > 0.2f){
			growthLevel = 0;
			hierarchyLevel++;
		}
		if(growthLevel < 0){
			growthLevel = 0.19f;
			hierarchyLevel--;
		}
		hierarchyLevel = Mathf.Clamp(hierarchyLevel, 0, 6);
		//Randomize the category upon reaching hierarchy 2
		if(hierarchyLevel > oldHierarchyLevel && oldHierarchyLevel == 1){
			currentCategory = (PlantCategory)Random.Range (1,3);
			if(currentCategory == PlantCategory.wood)
				currentTrack = woodTrack;
			if(currentCategory == PlantCategory.mush)
				currentTrack = mushTrack;
		}
		//Reset the category upon leaving hierarchy 2
		else if(hierarchyLevel < oldHierarchyLevel && oldHierarchyLevel == 2){
			currentCategory = PlantCategory.none;
		}
		
		oldHierarchyLevel = hierarchyLevel;
	}	
	void ManageContainedPlants(){		
		if(hierarchyLevel == 0 && !containedGrass.GetComponent<EmptyGameObject>()){
			Destroy(containedGrass);
			containedGrass = (GameObject)Instantiate(emptyFab, transform.position, Quaternion.identity);
		}
		if(hierarchyLevel == 1 && (!containedGrass.GetComponent<Grass>() || !containedPlant.GetComponent<EmptyGameObject>())){
			Destroy(containedPlant);
			containedPlant = (GameObject)Instantiate(emptyFab, transform.position, Quaternion.identity);
			Destroy(containedGrass);
			containedGrass = (GameObject)Instantiate(grassFab, transform.position, Quaternion.identity);
		}		
		if(hierarchyLevel >= 2 && containedPlant.GetComponent<PlantSpecies>().GetType() != currentTrack[hierarchyLevel-2].GetComponent<PlantSpecies>().GetType()){
			Destroy(containedPlant);
			containedPlant = (GameObject)Instantiate(currentTrack[hierarchyLevel-2], transform.position, Quaternion.identity);
		}
	}
}

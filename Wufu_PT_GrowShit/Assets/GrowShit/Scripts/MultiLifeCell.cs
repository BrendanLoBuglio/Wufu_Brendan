using UnityEngine;
using System.Collections;

public enum PlantCategory {none, mush, wood}//, flower, cactus}

public class MultiLifeCell : LifeCell {
	public bool useVariableScale = false;
	public GameObject emptyFab;
	public GameObject grassFab;
	public float[] growthTierSizes = new float[6];
	public GameObject[] mushTrack = new GameObject[5];
	public GameObject[] woodTrack = new GameObject[5];
	
	[HideInInspector]public GameObject containedPlant;//ContainedPlant holds the piece of plant life currently living in this cell
	[HideInInspector]public GameObject containedGrass;//ContainedPlant holds the unit of grass currently living in this cell. This is stored separate from ContainedPlant so that the grass remains even as further growth occurs.
	
	[HideInInspector]public float oldGrowthLevel = 0;
	[HideInInspector]public int hierarchyLevel = 0;
	[HideInInspector]public int oldHierarchyLevel = 0;
	[HideInInspector]public GameObject[] currentTrack;
	
	public GameObject bloomSphere;
	[HideInInspector]public bool hasBeenBloomChanged = false;
	
	void Start()
	{
		currentCategory = PlantCategory.none;
		growthRate = Random.Range (0.65f, 1.35f);
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
		if(hierarchyLevel < growthTierSizes.Length-1 && growthLevel > growthTierSizes[hierarchyLevel]){
			growthLevel = 0;
			hierarchyLevel++;
		}
		if(hierarchyLevel > 0 && growthLevel < 0){
			hierarchyLevel--;
			growthLevel = growthTierSizes[hierarchyLevel];
		}
		hierarchyLevel = Mathf.Clamp(hierarchyLevel, 0, 6);
		
		//Randomize the category upon reaching hierarchy 2
		if(hierarchyLevel > oldHierarchyLevel && oldHierarchyLevel == 1){
			if(PrototypeOptions.options.plantCategoryDetermination == PlantCategoryDetermination.cursorPolarity)
				currentCategory = GrowBeamCursor.chooseCategoryByCursorPolarity();
			else 
				currentCategory = (PlantCategory)Random.Range (1,3);
			SetCategory(currentCategory);
		}
		//Reset the category upon leaving hierarchy 2
		else if(hierarchyLevel < oldHierarchyLevel && oldHierarchyLevel == 2){
			currentCategory = PlantCategory.none;
			hasBeenBloomChanged = false;
		}
		
		if(PrototypeOptions.options.plantCategoryDetermination == PlantCategoryDetermination.bloomOverride){
			//Upon reaching hierarchy 3 and determining my category track, I instantiate an "override sphere," which overrides the category of level 2 plants it hits to my category
			if(hierarchyLevel > oldHierarchyLevel && oldHierarchyLevel == 2 && !hasBeenBloomChanged)
			{
				GameObject newBloomSphere = gameObject; //the "= gameObject" part is sloppy code that'll never be executed. 
														//Unity demands that newBloomSphere have a default, but it'll always be reset when the next two if statements execute
				if(PrototypeOptions.options.bloomOverrideOptions == BloomOverrideOptions.plantCentered){
					newBloomSphere = (GameObject)Instantiate(bloomSphere, containedPlant.transform.position -transform.up*1.5f, Quaternion.identity);
				}
				if(PrototypeOptions.options.bloomOverrideOptions == BloomOverrideOptions.cursorCentered){
					newBloomSphere = (GameObject)Instantiate(bloomSphere, GrowBeamCursor.cursorTransform.position, Quaternion.identity);
				}
				newBloomSphere.GetComponent<OverrideBloom>().myCategory = currentCategory;
			}
		}
		
		oldHierarchyLevel = hierarchyLevel;
	}
	
	public void SetCategory(PlantCategory categoryIn)
	{
		if(categoryIn == PlantCategory.wood)
			currentTrack = woodTrack;
		if(categoryIn == PlantCategory.mush)
			currentTrack = mushTrack;
	}
			
	public void ManageContainedPlants(){		
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
		
		
		if(containedPlant != null && containedPlant.GetComponent<PlantSpecies>()){
		   float newScale = 0.85f + 0.3f *(growthLevel/growthTierSizes[hierarchyLevel]);
		   containedPlant.transform.localScale = new Vector3(newScale,newScale,newScale);
		   float newY = 0.3f * (growthLevel/growthTierSizes[hierarchyLevel]);
		   containedPlant.transform.position = transform.position + (newY * Vector3.up);
		}
	}
}

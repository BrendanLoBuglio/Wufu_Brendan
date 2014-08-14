using UnityEngine;
using System.Collections;

public class LifeCell : MonoBehaviour {
	
	public GameObject emptyFab;
	public GameObject grassFab;
	public GameObject flowerFab;
	public GameObject bushFab;
	public GameObject saplingFab;
	public GameObject treeFab;
	
	public GameObject containedPlant;//ContainedPlant holds the piece of plant life currently living in this cell
	public GameObject containedGrass;//ContainedPlant holds the unit of grass currently living in this cell. This is stored separate from ContainedPlant so that the grass remains even as further growth occurs.
	
	public float growthLevel = 0;/*
	{
		get
		{
			return growthLevel;
		}
		set
		{
			growthLevel = value;
			ManageContainedPlant();
		}
	}*/
	private float oldGrowthLevel = 0;
	
	void Start()
	{
		containedPlant = (GameObject)Instantiate(emptyFab, transform.position, Quaternion.identity);
		containedGrass = (GameObject)Instantiate(emptyFab, transform.position, Quaternion.identity);
	}
	void Update()
	{
		growthLevel = Mathf.Clamp(growthLevel, 0f, 1f);
		if(oldGrowthLevel != growthLevel){
			ManageContainedPlants();
		}
		oldGrowthLevel = growthLevel;
	}
	
	void ManageContainedPlants () 
	{
		if(growthLevel < 0.05 && !containedGrass.GetComponent<EmptyGameObject>()){
			Destroy(containedGrass);
			containedGrass = (GameObject)Instantiate(emptyFab, transform.position, Quaternion.identity);
		}
		if(growthLevel >= 0.05 && growthLevel < 0.4 && (!containedGrass.GetComponent<Grass>() || !containedPlant.GetComponent<EmptyGameObject>())){
			Destroy(containedPlant);
			containedPlant = (GameObject)Instantiate(emptyFab, transform.position, Quaternion.identity);
			Destroy(containedGrass);
			containedGrass = (GameObject)Instantiate(grassFab, transform.position, Quaternion.identity);
		}
		if(growthLevel >= 0.4 && growthLevel < 0.6 && !containedPlant.GetComponent<Flower>()){
			Destroy(containedPlant);
			containedPlant = (GameObject)Instantiate(flowerFab, transform.position, Quaternion.identity);
		}
		if(growthLevel >= 0.6 && growthLevel < 0.8 && !containedPlant.GetComponent<Bush>()){
			Destroy(containedPlant);
			containedPlant = (GameObject)Instantiate(bushFab, transform.position, Quaternion.identity);
		}
		if(growthLevel >= 0.8 && growthLevel < 1 && !containedPlant.GetComponent<Sapling>()){
			Destroy(containedPlant);
			containedPlant = (GameObject)Instantiate(saplingFab, transform.position, Quaternion.identity);
		}
		if(growthLevel >= 1 && !containedPlant.GetComponent<Oak>()){
			Destroy(containedPlant);
			containedPlant = (GameObject)Instantiate(treeFab, transform.position, Quaternion.identity);
		}
	}
}

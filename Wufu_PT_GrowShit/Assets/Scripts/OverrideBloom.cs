using UnityEngine;
using System.Collections;

public class OverrideBloom : MonoBehaviour {
	float lifeTime = 0.5f;
	[HideInInspector]public PlantCategory myCategory;
	
	void Awake()
	{
		if(GrowBeamCursor.cursorTransform.localScale.x >2)
			transform.localScale = GrowBeamCursor.cursorTransform.localScale;
		else
			transform.localScale = new Vector3(2,2,2);
	}
	
	void Update () 
	{
		lifeTime -= Time.deltaTime;
		if(lifeTime <= 0)
			Destroy(gameObject);
	}
	
	void OnTriggerEnter(Collider other)
	{
		if(other.gameObject.GetComponent<MultiLifeCell>())
		{
			MultiLifeCell lifeCell = other.gameObject.GetComponent<MultiLifeCell>();
			if(!lifeCell.hasBeenBloomChanged && lifeCell.hierarchyLevel >= 2 && lifeCell.hierarchyLevel <= 3)
			{
				if(lifeCell.currentCategory != this.myCategory){
					lifeCell.SetCategory(this.myCategory);
					lifeCell.hasBeenBloomChanged = true;
				}
				lifeCell.ManageContainedPlants();
			}
		}
	}
}

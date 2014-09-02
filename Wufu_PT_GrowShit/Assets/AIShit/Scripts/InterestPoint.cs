using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum InterestPointAttribute {food, water, hangout, rest}

public class InterestPoint : MonoBehaviour 
{
	public static List<InterestPointAttribute> allAttributes = new List<InterestPointAttribute>() {InterestPointAttribute.food, InterestPointAttribute.water, InterestPointAttribute.hangout, InterestPointAttribute.rest, InterestPointAttribute.food};

	public GameObject dwellingOwner; //If I'm a dwelling, this is the creature gameObject who owns me. Otherwise, it's null
	public GameObject zone;
	public GameObject targetingCreature;
	public List<InterestPointAttribute> myAttributes;	
	
	void Start()
	{
		renderer.materials = new Material[6];
		Material[] mats = renderer.materials;
		mats[0] = MaterialLibrary.library.IP_Background;
		for(int i = 1; i < mats.Length; i++)
			mats[i] = MaterialLibrary.library.Empty;
		renderer.materials = mats;
	}			
		
	void Update () 
	{
		if (zone == null && ZoneManager.manager.worldInstantiated)
		{
			zone = ZoneManager.manager.GetZoneAtPosition(transform.position);		
		}
		if(dwellingOwner != null && renderer.materials[1] != MaterialLibrary.library.IP_Dwelling)
		{
			Material[] mats = renderer.materials;
			mats[1] = MaterialLibrary.library.IP_Dwelling;
			renderer.materials = mats;
		}
	}
}

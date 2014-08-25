using UnityEngine;
using System.Collections;

public enum PlantCategoryDetermination {trueRandom, bloomOverride, cursorPolarity}
public enum BloomOverrideOptions {plantCentered, cursorCentered}


public class PrototypeOptions : MonoBehaviour {
	public static PrototypeOptions options;
	
	public PlantCategoryDetermination plantCategoryDetermination = PlantCategoryDetermination.bloomOverride;
	public BloomOverrideOptions bloomOverrideOptions = BloomOverrideOptions.cursorCentered;
	
	void Awake()
	{
		options = this;
	}
}

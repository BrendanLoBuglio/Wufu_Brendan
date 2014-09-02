using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum BiomeType {forest, clearing, desert, oasis, springs, NUMBER_OF_ELEMENTS} 
//NUMBER_OF_ELEMENTS isn't a real value -- it only exists in the enum because as the last element, it's int value is equal to the number of elements

public static class Biome : object 
{
	public static List<EcoStructureType> getStructuresForBiome(BiomeType biomeIn)
	{
		if(biomeIn == BiomeType.forest)
			return new List<EcoStructureType>(){EcoStructureType.tree, EcoStructureType.bush};
		else if(biomeIn == BiomeType.clearing)
			return new List<EcoStructureType>(){EcoStructureType.tree, EcoStructureType.bush, EcoStructureType.waterHole};
		else if(biomeIn == BiomeType.desert)
			return new List<EcoStructureType>(){EcoStructureType.flatRock, EcoStructureType.cactus};
		else if(biomeIn == BiomeType.oasis)
			return new List<EcoStructureType>(){EcoStructureType.flatRock, EcoStructureType.cactus, EcoStructureType.waterHole};
		else if(biomeIn == BiomeType.springs)
			return new List<EcoStructureType>(){EcoStructureType.waterHole, EcoStructureType.spring};		
		else
			return new List<EcoStructureType>();
	}	
}

using UnityEngine;
using System.Collections;

public class MaterialLibrary : MonoBehaviour 
{
	public static MaterialLibrary library;
	public Material Empty;
	public Material IP_Background;
	public Material IP_Food;
	public Material IP_Rest;
	public Material IP_Hangout;
	public Material IP_Dwelling;
	public Material IP_Water;
	public Material C_Exploring;
	public Material C_Rest;
	
	void Awake()
	{
		library = this;
	}
}

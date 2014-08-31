using UnityEngine;
using System.Collections;

public class Sunlight : MonoBehaviour {
	Light myLight;
	void Start()
	{
		myLight = gameObject.GetComponent<Light>();
	}
	
	
	void Update () 
	{
		myLight.intensity = 0.2f + 0.2f * DayNightCycle.normalizedTime;
	}
}

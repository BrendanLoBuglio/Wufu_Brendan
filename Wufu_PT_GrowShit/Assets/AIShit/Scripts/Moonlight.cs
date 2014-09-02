using UnityEngine;
using System.Collections;

public class Moonlight : MonoBehaviour {
	Light myLight;
	void Start()
	{
		myLight = gameObject.GetComponent<Light>();
	}
	
	
	void Update () 
	{
		myLight.intensity = 0.5f - 0.5f * DayNightCycle.normalizedTime;
	}
}

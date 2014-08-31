using UnityEngine;
using System.Collections;

public class DayNightCycle : MonoBehaviour {

	public static float dayLength = 60f; //the length of each day, in seconds
	public static float worldTime = dayLength/6f;
	public static float normalizedTime; //The time of day, on a range from -1 (midnight) to 1 (noon)
	
	private Material newSkyBox;
	private Color originalColor;
	
	void Start () 
	{
		normalizedTime = Mathf.Sin((2f*Mathf.PI) * (worldTime / dayLength));
	
		originalColor = RenderSettings.skybox.GetColor("_Tint");
		newSkyBox = (Material)Instantiate(RenderSettings.skybox);
		RenderSettings.skybox = newSkyBox;
	}
	
	// Update is called once per frame
	void Update () 
	{
		normalizedTime = Mathf.Sin((2f*Mathf.PI) * (worldTime / dayLength));
	
		if(Input.GetKey (KeyCode.Space)){
			worldTime += Time.deltaTime * (dayLength / 10f);
		}
		worldTime += Time.deltaTime;
		float colorFactor = 0.25f + 0.85f * normalizedTime;
		RenderSettings.skybox.SetColor ("_Tint", new Color( originalColor.r*colorFactor,originalColor.g * colorFactor,originalColor.b * colorFactor, 1f));
	}
}

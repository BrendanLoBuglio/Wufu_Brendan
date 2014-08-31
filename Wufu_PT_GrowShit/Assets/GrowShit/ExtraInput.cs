using UnityEngine;
using System.Collections;

public class ExtraInput : MonoBehaviour {

	public static int framesSinceMouseWasClicked = 999;
	
	void Update()
	{
		framesSinceMouseWasClicked++;
		if(Input.GetKeyDown (KeyCode.Mouse0))
				framesSinceMouseWasClicked = 0;
	}
}

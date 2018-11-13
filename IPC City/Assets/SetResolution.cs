using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetResolution : MonoBehaviour 
{
    public bool desktopBuild;
	
	void Start () 
	{
        if (!desktopBuild)
        {
            Screen.SetResolution(1080, 1920, false);
        }
        Screen.autorotateToLandscapeLeft = true;
        Screen.autorotateToLandscapeRight = true;
    }
	
	
	void Update () 
	{
		
	}
}

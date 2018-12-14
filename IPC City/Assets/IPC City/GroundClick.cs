using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundClick : MonoBehaviour {

	void Start () {
		
	}
	
	
	void Update () {
		
	}

    private void OnMouseDown()
    {
        if (UI_Manager.Instance)
            UI_Manager.Instance.HideUI();
        print("Clicked on " + gameObject.name);
    }
}

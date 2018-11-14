using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtZero : MonoBehaviour 
{
	void Update () 
	{
		if(Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit))
            {
                transform.LookAt(new Vector3(hit.point.x, 0, hit.point.z));
            }
        }
	}
}

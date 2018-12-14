using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IPC_Creator : MonoBehaviour
{

    public GameObject prefab;
    public int ipcID;


    public int[] attributeBytes;

    void Start ()
    {
        //Invoke("CreateChar", 5);
	}

    [ContextMenu("Create")]
    public void CreateChar()
    {
        //GetComponent<IPC_Stats>().AssignValues();
    }
}

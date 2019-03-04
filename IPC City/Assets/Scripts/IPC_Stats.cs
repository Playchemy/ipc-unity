using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class IPC_Stats : MonoBehaviour
{
    public TextMesh overheadName;

    [Header("Inputs")]
    int input_IPCnumber;

    

    public string owner;

    public string ipc_name;
    public int ipc_Age;

    bool createdButton = false;



    [Header("Stats")]
    public int strength;
    public int dexterity;
    public int intelligence;
    public int constitution;
    public int luck;
    public int timeOfBirth;
    public int ipcID;
    public float price;

    public string race;
    public string gender;
    public string hairColor;




    void Start ()
    {

    }

    int UnixTimeStampToDateTime(double unixTimeStamp)
    {
        DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
        dtDateTime = dtDateTime.AddSeconds(unixTimeStamp).ToLocalTime();
        int i = (int)(System.DateTime.UtcNow - dtDateTime).TotalSeconds;
        int daysSince = i / 86400;
        return daysSince;
    }



    [ContextMenu("Assign values")]
    public void AssignValues()
    {
        race = GetComponent<Scribe>().race.text;
        gender = GetComponent<Scribe>().gender.text;

        ipcID = GetComponent<IpcGetService>().inputIPCID;

        ipc_Age = UnixTimeStampToDateTime(GetComponent<IpcGetService>().ipcStorage.m_timeOfBirth);
        ipc_name = GetComponent<IpcGetService>().ipcStorage.m_name;

        if(ipc_name == "")
        {
            ipc_name = gender + race + GetComponent<IpcGetService>().inputIPCID;
        }

        overheadName.text = ipc_name;
        transform.name = ipc_name + "[" + ipcID + "]";

        price = GetComponent<IpcGetService>().ipcStorage.m_price;

        if(!createdButton && FindObjectOfType<SpawnIPC>())
        {
            FindObjectOfType<SpawnIPC>().CreateButton(this);
            createdButton = true;
        }
    }

    private void OnMouseDown()
    {
        if(UI_Manager.Instance)
            UI_Manager.Instance.ClickedOn(GetComponent<IPC_Stats>());

        if (IPC_Controller.Instance)
            IPC_Controller.Instance.SelectIPC(gameObject);
    }

    void Update () {
		
	}
}

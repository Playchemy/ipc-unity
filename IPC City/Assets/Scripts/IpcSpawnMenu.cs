using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class IpcSpawnMenu : MonoBehaviour {

    public InputField inputBox;

    public SpawnIPC spawner;

    public GameObject loading;

    public GameObject panel;

    public InputField selectIpcInt;

    void Start ()
    {
		
	}
	
	void Update ()
    {

	}

    public void ResetGame()
    {
        SceneManager.LoadScene("IPCReader");
    }

    public void CreateSingleIPC()
    {
        spawner.CreateSingleIPC(int.Parse(inputBox.text));
    }

    public void CreateAllFromWallet()
    {
        spawner.CreateIpcs(int.Parse(inputBox.text));
        loading.SetActive(true);

    }

    public void CreateALL()
    {
        spawner.CreateAll();
    }

    public void SelectIPCbyID(int givenID = -1)
    {
        if (givenID == -1)
        {
            for (int i = 0; i < spawner.spawnedIpcs.Count; i++)
            {
                if (spawner.spawnedIpcs[i].ipcID == int.Parse(selectIpcInt.text))
                {
                    IPC_Controller.Instance.SelectIPC(spawner.spawnedIpcs[i].gameObject);
                    break;
                }
            }
        }

    else
        {
            for (int i = 0; i < spawner.spawnedIpcs.Count; i++)
            {
                if (spawner.spawnedIpcs[i].ipcID == givenID)
                {
                    IPC_Controller.Instance.SelectIPC(spawner.spawnedIpcs[i].gameObject);
                    break;
                }
            }
        }
    }

    public void DeleteAll()
    {
        foreach (SelectIPCButton obj in FindObjectsOfType<SelectIPCButton>())
        {
            obj.DeleteIpc();
        }

        FindObjectOfType<SpawnIPC>().spawnedIpcs.Clear();

        foreach (CharMovement obj in FindObjectsOfType<CharMovement>())
        {
            Destroy(obj.gameObject);
        }
    }

    public void DisplayNames()
    {
        foreach (IPC_Stats obj in FindObjectsOfType<IPC_Stats>())
        {
            obj.overheadName.GetComponent<MeshRenderer>().enabled = true;
        }
    }

    public void HideNames()
    {
        foreach (IPC_Stats obj in FindObjectsOfType<IPC_Stats>())
        {
            obj.overheadName.GetComponent<MeshRenderer>().enabled = false;
        }
    }



    public void ToggleMenu()
    {
        if(panel.activeSelf)
        {
            panel.SetActive(false);
        }
        else
        {
            panel.SetActive(true);
        }
    }

}

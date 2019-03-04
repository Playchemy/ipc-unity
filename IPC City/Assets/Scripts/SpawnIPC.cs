using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnIPC : MonoBehaviour {

    public GameObject prefab;
    public IpcGetService getService;

    public GameObject loading;

    public List<IPC_Stats> spawnedIpcs;


    public bool createButton;

    public GameObject ipcSelectButtonPrefab;

    public Transform buttonParent;

    void Start ()
    {
		
	}
	
	void Update ()
    {
		
	}

    [ContextMenu("CreateSingle")]
    public void CreateSingleIPC(int ipcNum)
    {
        float randX = Random.Range(280, -60);
        float randY = Random.Range(96, -234);
        Vector3 pos = new Vector3(randX, 1, randY);

        GameObject newIpc = Instantiate(prefab, pos, Quaternion.identity) as GameObject;
        //newIpc.GetComponent<CharController>().target = GameObject.Find("Target");
        newIpc.GetComponent<IpcGetService>().inputIPCID = ipcNum;
        newIpc.GetComponent<IpcGetService>().GetSingleIPC();

        spawnedIpcs.Add(newIpc.GetComponent<IPC_Stats>());

        
    }

    public void CreateButton(IPC_Stats stats)
    {
        if (createButton)
        {
            GameObject button = Instantiate(ipcSelectButtonPrefab, transform.position, Quaternion.identity, buttonParent) as GameObject;
            button.GetComponent<SelectIPCButton>().SetText(stats.name, stats.ipcID, stats.gameObject);
        }
    }




    [ContextMenu("CreateAllOwned")]
    public void CreateIpcs(int ipcNum)
    {
        StartCoroutine("CreateMultiple", ipcNum);
    }

    public void CreateAll()
    {

        // Spawning only 2000 for now, update later
        for (int i = 0; i < 2000; i++)
        { 
            CreateSingleIPC(i);
        }

        //for (int i = 0; i < DataInitializer.Instance.totalNumOfIpcs; i++)
        //{
        //    CreateSingleIPC(i);
        //}
    }

    IEnumerator CreateMultiple(int ipcNumb)
    {
        getService.inputIPCID = ipcNumb;
        //getService.GetSingleIPC();

        StartCoroutine(getService.GetOneIpc(ipcNumb));
        yield return new WaitUntil(() => getService.OneIPCLoaded == true);

        //yield return new WaitForSeconds(2f);

        getService.inputWalletID = getService.ipcStorage.m_owner;

        getService.GetOwnerTokens();
        yield return new WaitUntil(() => getService.TokensLoaded == true);


        for (int i = 0; i < getService.ownedIpcsIds.Count; i++)
        {
            CreateSingleIPC(getService.ownedIpcsIds[i]);
        }

        loading.SetActive(false);
    }
}

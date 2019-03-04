using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class DataInitializer : MonoBehaviour
{
    public static DataInitializer Instance;

    public int totalNumOfIpcs;
    public List<IPC_Data> ipcList;
    public List<Wallet> walletList;
    public List<IpcGetService> slaveList;
    public GameObject slave;
    public AdminAccounts adminData;

    private bool isGenerating = false;
    private IpcGetService ipcGetService;

    public OnDataLoadStarted onDataLoadStarted;
    public delegate void OnDataLoadStarted();

    public OnDataLoadFinished onDataLoadFinished;
    public delegate void OnDataLoadFinished();

    public OnDataSlavesInitialized onDataSlavesInitialized;
    public delegate void OnDataSlavesInitialized();

    public OnAllIpcsLoaded onAllIpcsLoaded;
    public delegate void OnAllIpcsLoaded();

    public OnUpdateDatabase onUpdateDatabase;
    public delegate void OnUpdateDatabase();

    public OnCheckDatabase onCheckDatabase;
    public delegate void OnCheckDatabase();

    private void Awake ()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        ipcGetService = GetComponent<IpcGetService>();
        StartCoroutine(DelayInitializer());

        DataPuller.Instance.onIPCsLoaded += InitiateAllWalletRetreival;
    }

    private void Update()
    {
        if(isGenerating)
        {
            if (onCheckDatabase != null)
            {
                onCheckDatabase.Invoke();
            }
        }
    }

    public IEnumerator DelayInitializer()
    {
        yield return new WaitForSeconds(1f);
        StartCoroutine(ipcGetService.GetIpcCount());
        yield return new WaitUntil(() => ipcGetService.IPCCountLoaded);
        InitializeDataRequest();
    }

    public void InitializeDataRequest()
    {
        if (onDataLoadFinished != null)
        {
            onDataLoadFinished.Invoke();
        }

        if (onAllIpcsLoaded != null)
        {
            onAllIpcsLoaded.Invoke();
        }
    }

    public IPC_Data GetIpcData(int ipcId)
    {
        return ipcList[ipcId - 1];
    }

    public void CompareListCount()
    {
        if(ipcList.Count >= totalNumOfIpcs)
        {
            Debug.Log("All IPCs Are Succesfully Retrieved! It took: " + Time.timeSinceLevelLoad);
            ipcList.Sort((x, y) => x.ipc_id.CompareTo(y.ipc_id));

            if (onAllIpcsLoaded != null)
            {
                onAllIpcsLoaded.Invoke();
            }

            Invoke("InitiateAllWalletRetreival", 0.1f);
        }
    }

    private void InitiateAllWalletRetreival()
    {
        totalNumOfIpcs = ipcGetService.ipcCount;

        List<string> addresses = new List<string>();

        for (int i = 0; i < ipcList.Count; i++)
        {
            if (!addresses.Contains(ipcList[i].ipc_owner))
            {
                addresses.Add(ipcList[i].ipc_owner);
            }
        }

        for (int i = 0; i < addresses.Count; i++)
        {
            Wallet wlt = new Wallet();
            wlt.walletAddress = addresses[i];
            wlt.ownedIpcs = new List<int>();

            walletList.Add(wlt);
        }

        for (int i = 0; i < ipcList.Count; i++)
        {
            for (int j = 0; j < walletList.Count; j++)
            {
                if (ipcList[i].ipc_owner == walletList[j].walletAddress)
                {
                    walletList[j].ownedIpcs.Add(ipcList[i].ipc_id);
                }
            }
        }

        foreach(Wallet wlt in walletList)
        {
            wlt.name = "[" + wlt.ownedIpcs.Count + "] " + wlt.walletAddress;
        }

        if (onDataLoadFinished != null)
        {
            onDataLoadFinished.Invoke();
        }

       if (FindObjectOfType<Ipc_Load_Check>())
       {
            FindObjectOfType<Ipc_Load_Check>().CheckLoaded();
       }

        isGenerating = false;
    }

    public List<int> GetOwnedIpcsByAddress(string _address)
    {
        for (int i = 0; i < walletList.Count; i++)
        {
            if (walletList[i].walletAddress == _address)
            {
                return walletList[i].ownedIpcs;
            }
        }

        return null;
    }

    private IEnumerator GetIPC(IpcGetService _slave)
    {
        yield return new WaitForSeconds(.1f);
        _slave.GetSingleIPC();
    }

    public void UpdateDatabase()
    {
        if(SystemManager.Instance.ActiveSystemLoadType == SystemManager.SystemLoadType.OnDemand)
        {
            return;
        }

        if(onUpdateDatabase != null)
        {
            onUpdateDatabase.Invoke();
        }

        ipcList.Clear();
        walletList.Clear();
        DataPuller.Instance.PullFromGoogleDatabase();
    }
}

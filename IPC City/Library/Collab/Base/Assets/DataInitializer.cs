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
        Invoke("InitializeDataRequest", 1);
    }

    public void InitializeDataRequest()
    {
        if (onDataLoadStarted != null)
        {
            onDataLoadStarted.Invoke();
        }

        if(SystemManager.Instance.ActiveSystemLoadType == SystemManager.SystemLoadType.Cache)
        {
            ipcGetService.GetIpcCount();
            totalNumOfIpcs = ipcGetService.ipcCount;

            slaveList.Clear();

            for (int i = 1; i <= totalNumOfIpcs; i++)
            {
                IpcGetService slaveunit = Instantiate(slave).GetComponent<IpcGetService>();
                slaveunit.inputIPCID = i;
                slaveList.Add(slaveunit);
            }

            StartCoroutine(LoadInChunks());

            if (onDataSlavesInitialized != null)
            {
                onDataSlavesInitialized.Invoke();
            }
        }
        else
        {
            if (onDataLoadFinished != null)
            {
                onDataLoadFinished.Invoke();
            }
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
        List<string> addresses = new List<string>();

        for (int i = 0; i < totalNumOfIpcs; i++)
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
            wlt.name = addresses[i];

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

        if (onDataLoadFinished != null)
        {
            onDataLoadFinished.Invoke();
        }

        Debug.Log("All Wallets Are Succesfully Retrieved! It took: " + Time.timeSinceLevelLoad);
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
    
    private IEnumerator LoadInChunks()
    {
        for (int i = 0; i < 100; i++)
        {
            StartCoroutine(GetIPC(slaveList[i]));
        }
        yield return new WaitUntil(() => FindObjectsOfType<SlaveLoader>().Length == totalNumOfIpcs - 100);
        for (int i = 100; i < 200; i++)
        {
            StartCoroutine(GetIPC(slaveList[i]));
        }
        yield return new WaitUntil(() => FindObjectsOfType<SlaveLoader>().Length == totalNumOfIpcs - 200);
        for (int i = 200; i < 300; i++)
        {
            StartCoroutine(GetIPC(slaveList[i]));
        }
        yield return new WaitUntil(() => FindObjectsOfType<SlaveLoader>().Length == totalNumOfIpcs - 300);
        for (int i = 300; i < 400; i++)
        {
            StartCoroutine(GetIPC(slaveList[i]));
        }
        yield return new WaitUntil(() => FindObjectsOfType<SlaveLoader>().Length == totalNumOfIpcs - 400);
        for (int i = 400; i < 500; i++)
        {
            StartCoroutine(GetIPC(slaveList[i]));
        }
        yield return new WaitUntil(() => FindObjectsOfType<SlaveLoader>().Length == totalNumOfIpcs - 500);
        for (int i = 500; i < 600; i++)
        {
            StartCoroutine(GetIPC(slaveList[i]));
        }
        yield return new WaitUntil(() => FindObjectsOfType<SlaveLoader>().Length == totalNumOfIpcs - 600);
        for (int i = 600; i < totalNumOfIpcs; i++)
        {
            StartCoroutine(GetIPC(slaveList[i]));
        }
    }

    private IEnumerator GetIPC(IpcGetService _slave)
    {
        yield return new WaitForSeconds(0.1f);
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
        InitializeDataRequest();
    }
}

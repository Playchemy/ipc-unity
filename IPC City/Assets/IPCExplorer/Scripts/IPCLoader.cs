using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class IPCLoader : MonoBehaviour
{
    public static IPCLoader Instance;

    public OnIpcLoad onIpcLoad;
    public delegate void OnIpcLoad(Button _chamber);

    public OnChamberLoadingStarted onChamberLoadingStarted;
    public delegate void OnChamberLoadingStarted(int chamberIndex);

    public OnChamberLoadingFinished onChamberLoadingFinished;
    public delegate void OnChamberLoadingFinished(int chamberIndex);

    public OnLoadingStarted onTokenLoadingStarted;
    public OnLoadingStarted onCharLoadingStarted;
    public delegate void OnLoadingStarted();

    public OnLoadingFinished onTokenLoadingFinished;
    public OnLoadingFinished onCharLoadingFinished;
    public delegate void OnLoadingFinished(string _error);

    public OnAddressError onAddressScanScreenError;
    public delegate void OnAddressError(int _errorCode);

    public List<IpcGetService> chamberIpcGetServices = new List<IpcGetService>();
    private Interpreter interpreter = null;

    public void Awake()
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
        //MenuManager.Instance.onStopAllCouroutines += OnStopAllCoroutines;
        GridManager.Instance.onStopAllCouroutines += OnStopAllCoroutines;
        SystemManager.Instance.onSystemLoadTypeSwitch += SetService;
        SetService();
    }

    private void SetService()
    {
        if (SystemManager.Instance.ActiveSystemLoadType == SystemManager.SystemLoadType.OnDemand)
        {
            interpreter = SystemManager.Instance.GetOnDemandInterpreter();
        }
        else
        {
            interpreter = SystemManager.Instance.GetCacheInterpreter();
        }
    }

    private void OnStopAllCoroutines()
    {
        Debug.Log("Stopping All Coroutines");

        //MenuManager.Instance.StopAllCoroutines();
        GridManager.Instance.StopAllCoroutines();
        GridFunctions.Instance.StopAllCoroutines();
        WalletManager.Instance.StopAllCoroutines();
        IPCLoader.Instance.StopAllCoroutines();
    }

    /// <summary>
    /// This function runs in a loop to pull each IPC data for each available chamber.
    /// Chamber IPC service runs a function to get IPC data for itself from the main IPC service.
    /// ONLY after chamber IPC service gets the IPC data its button is activated which enables user to
    /// open character sheet.
    /// </summary>
    /// <param name="IPCChamberIndex"> Which chamber data is being loading into </param>
    /// <param name="IPCIndex"> Which owned IPC is being loaded to chamber from its array</param>
    public IEnumerator LoadEachIPC(int IPCChamberIndex, int IPCIndex)
    {
        if(onChamberLoadingStarted != null)
        {
            onChamberLoadingStarted.Invoke(IPCChamberIndex);
        }

        StartCoroutine(chamberIpcGetServices[IPCChamberIndex].GetOneIpc(GridManager.Instance.ipcGetServiceMain.ownedIpcsIds[IPCIndex]));
        yield return new WaitUntil(() => chamberIpcGetServices[IPCChamberIndex].OneIPCLoaded == true);
        chamberIpcGetServices[IPCChamberIndex].inputIPCID = GridManager.Instance.ipcGetServiceMain.ownedIpcsIds[IPCIndex];
        chamberIpcGetServices[IPCChamberIndex].GetComponent<Interpreter>().Interpret();
        GridFunctions.Instance.ChangeArtAlpha(IPCChamberIndex, 1f);

        if(onIpcLoad != null)
        {
            onIpcLoad.Invoke(chamberIpcGetServices[IPCChamberIndex].GetComponent<Button>());
        }

        if (onChamberLoadingFinished != null)
        {
            onChamberLoadingFinished.Invoke(IPCChamberIndex);
        }
    }

    public IEnumerator LoadToken(string _address)
    {
        if(onTokenLoadingStarted != null)
        {
            onTokenLoadingStarted.Invoke();
        }

        GridManager.Instance.ipcGetServiceMain.inputWalletID = _address;
        StartCoroutine(GridManager.Instance.ipcGetServiceMain.GetTokensOfOwner(_address));
        yield return new WaitUntil(() => GridManager.Instance.ipcGetServiceMain.TokensLoaded == true);

        // 0x1111111111111111111111111111111111111111

        if(GridManager.Instance.ipcGetServiceMain.ownedIpcsIds == null)
        {
            if (onTokenLoadingFinished != null)
            {
                onTokenLoadingFinished.Invoke("NULL");
            }
        }
        else
        {
            if (onTokenLoadingFinished != null)
            {
                onTokenLoadingFinished.Invoke("");
            }
        }
    }

    /// <summary>
    /// Loads only one IPC to first chamber
    /// </summary>
    /// <param name="IPCID"> The ID from the chamber text </param>
    /// <returns></returns>
    public IEnumerator LoadOnlyOneIPC(int IPCID)
    {
        if (onChamberLoadingStarted != null)
        {
            onChamberLoadingStarted.Invoke(0);
        }

        StartCoroutine(GridManager.Instance.ipcGetServiceMain.GetOneIpc(IPCID));
        yield return new WaitUntil(() => GridManager.Instance.ipcGetServiceMain.OneIPCLoaded == true);
        StartCoroutine(GridManager.Instance.ipcGetServiceMain.GetTokensOfOwner(GridManager.Instance.ipcGetServiceMain.inputWalletID));
        yield return new WaitUntil(() => GridManager.Instance.ipcGetServiceMain.TokensLoaded == true);
        chamberIpcGetServices[0].ipcStorage = GridManager.Instance.ipcGetServiceMain.ipcStorage;
        chamberIpcGetServices[0].inputIPCID = IPCID;
        chamberIpcGetServices[0].GetComponent<Interpreter>().Interpret();
        for (int i = 0; i < 1; i++) { GridFunctions.Instance.ChangeArtAlpha(i, 1f); }

        if (onIpcLoad != null)
        {
            onIpcLoad.Invoke(chamberIpcGetServices[0].GetComponent<Button>());
        }

        if (onChamberLoadingFinished != null)
        {
            onChamberLoadingFinished.Invoke(0);
        }
    }

    /// <summary>
    /// Displays character sheet of the clicked chamber.
    /// </summary>
    /// <param name="ipcGetService">  </param>
    public IEnumerator LoadIPCToCharacterSheet(IpcGetService ipcGetService)
    {
        if (onCharLoadingStarted != null)
        {
            onCharLoadingStarted.Invoke();
        }

        StartCoroutine(GridManager.Instance.ipcGetServiceMain.GetOneIpc(ipcGetService.inputIPCID));
        yield return new WaitUntil(() => GridManager.Instance.ipcGetServiceMain.OneIPCLoaded == true);
        GridManager.Instance.ipcGetServiceMain.inputWalletID = GridManager.Instance.ipcGetServiceMain.ipcStorage.m_owner;
        interpreter.Interpret();

        if (onCharLoadingFinished != null)
        {
            onCharLoadingFinished.Invoke("");
        }
    }

    /// <summary>
    /// Displays character sheet of the selected id.
    /// </summary>
    public IEnumerator DirectLoadIPCToCharacterSheet(int IPCID)
    {
        if (onCharLoadingStarted != null)
        {
            onCharLoadingStarted.Invoke();
        }

        StartCoroutine(GridManager.Instance.ipcGetServiceMain.GetOneIpc(IPCID));
        yield return new WaitUntil(() => GridManager.Instance.ipcGetServiceMain.OneIPCLoaded == true);
        GridManager.Instance.ipcGetServiceMain.inputWalletID = GridManager.Instance.ipcGetServiceMain.ipcStorage.m_owner;
        interpreter.Interpret();

        if (onCharLoadingFinished != null)
        {
            onCharLoadingFinished.Invoke("");
        }
    }

    public IEnumerator LoadNextIPC()
    {
        if (onCharLoadingStarted != null)
        {
            onCharLoadingStarted.Invoke();
        }

        if(SystemManager.Instance.ActiveSystemLoadType == SystemManager.SystemLoadType.OnDemand)
        {
            StartCoroutine(GridManager.Instance.ipcGetServiceMain.GetIpcCount());
            yield return new WaitUntil(() => GridManager.Instance.ipcGetServiceMain.IPCCountLoaded == true);
        }

        if (GridManager.Instance.ipcGetServiceMain.inputIPCID < ((SystemManager.Instance.ActiveSystemLoadType == SystemManager.SystemLoadType.OnDemand) ? GridManager.Instance.ipcGetServiceMain.ipcCount : DataInitializer.Instance.ipcList.Count))
        {
            int currentIPCID = GridManager.Instance.ipcGetServiceMain.inputIPCID;
            int nextIPCID = GridManager.Instance.ipcGetServiceMain.inputIPCID + 1;
            StartCoroutine(GridManager.Instance.ipcGetServiceMain.GetOneIpc(nextIPCID));
            yield return new WaitUntil(() => GridManager.Instance.ipcGetServiceMain.OneIPCLoaded == true);
            GridManager.Instance.ipcGetServiceMain.inputWalletID = GridManager.Instance.ipcGetServiceMain.ipcStorage.m_owner;
            interpreter.Interpret();
        }

        if (onCharLoadingFinished != null)
        {
            onCharLoadingFinished.Invoke("");
        }
    }

    public IEnumerator LoadPreviousIPC()
    {
        if (onCharLoadingStarted != null)
        {
            onCharLoadingStarted.Invoke();
        }

        if (GridManager.Instance.ipcGetServiceMain.inputIPCID > 1)
        {
            int currentIPCID = GridManager.Instance.ipcGetServiceMain.inputIPCID;
            int previousIPCID = GridManager.Instance.ipcGetServiceMain.inputIPCID - 1;
            StartCoroutine(GridManager.Instance.ipcGetServiceMain.GetOneIpc(previousIPCID));
            yield return new WaitUntil(() => GridManager.Instance.ipcGetServiceMain.OneIPCLoaded == true);
            GridManager.Instance.ipcGetServiceMain.inputWalletID = GridManager.Instance.ipcGetServiceMain.ipcStorage.m_owner;
            interpreter.Interpret();
        }

        if (onCharLoadingFinished != null)
        {
            onCharLoadingFinished.Invoke("");
        }
    }
}

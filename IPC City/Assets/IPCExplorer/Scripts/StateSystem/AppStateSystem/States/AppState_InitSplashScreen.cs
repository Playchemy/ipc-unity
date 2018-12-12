using System.Collections;
using UnityEngine.UI;
using UnityEngine;

public class AppState_InitSplashScreen : State
{
    public Image splashScreenLoadingBarImage = null;
    public Text splashScreenLoadingStatusText = null;
    public Text splashScreenIpcCountText = null;
    public Text splashScreenSlaveCountLoadingText = null;

    // State Functions =========================================================================================================
    protected override void RunOnce()
    {
        base.RunOnce();

        if(SystemManager.Instance.ActiveSystemLoadType == SystemManager.SystemLoadType.OnDemand)
        {
            DataInitializer.Instance.onAllIpcsLoaded += AllIpcsLoaded;
            DataInitializer.Instance.onCheckDatabase += UpdateDatabase;
            DataInitializer.Instance.onDataLoadStarted += StartSystemLoading;
            DataInitializer.Instance.onDataLoadFinished += EndSystemLoading;
            DataInitializer.Instance.onDataSlavesInitialized += DataSlavesInitialized;
        }
        else
        {
            DataPuller.Instance.onPullInitialized += StartSystemLoading;
            DataPuller.Instance.onIPCsLoaded += EndSystemLoading;
        }
    }

    public override void EnterState()
    {
        if (Application.internetReachability == NetworkReachability.NotReachable)
        {
            splashScreenLoadingBarImage.fillAmount = 0f;
            splashScreenLoadingStatusText.text = "No Connection, Make Sure You Are Connected to Internet!";
            Invoke("TransitionFromSplashScreen", 5f);
        }
    }

    public override void UpdateState()
    {

    }

    public override void ExitState()
    {

    }
    // =========================================================================================================================

    private void StartSystemLoading()
    {
        splashScreenLoadingBarImage.fillAmount = 0f;
        splashScreenLoadingStatusText.text = "Initializing Data Stream...";
    }

    private void UpdateDatabase()
    {
        splashScreenIpcCountText.text = DataInitializer.Instance.ipcList.Count + " / " + DataInitializer.Instance.totalNumOfIpcs;
        splashScreenSlaveCountLoadingText.text = FindObjectsOfType<SlaveLoader>().Length.ToString();
        splashScreenLoadingBarImage.fillAmount = DataInitializer.Instance.ipcList.Count * (0.8f / DataInitializer.Instance.totalNumOfIpcs);
    }

    private void DataSlavesInitialized()
    {
        splashScreenLoadingStatusText.text = "Reading IPCs from Blockchain...";
    }

    private void AllIpcsLoaded()
    {
        splashScreenLoadingBarImage.fillAmount = 0.9f;
        splashScreenLoadingStatusText.text = "Retrieving All Addresses...";
    }

    private void EndSystemLoading()
    {
        splashScreenLoadingBarImage.fillAmount = 1f;
        splashScreenLoadingStatusText.text = "IPC Explorer Succesfully Loaded...";
        Invoke("TransitionFromSplashScreen", 1f);
    }

    private void TransitionFromSplashScreen()
    {
        AppStateManager.Instance.SetState(AppStateManager.AppState.InitMainMenu);
    }
}

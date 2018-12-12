using UnityEngine;

public class AppStateManager : MonoBehaviour
{
    public static AppStateManager Instance = null;

    public AppState ActiveAppState = AppState.Initialize;
    public enum AppState { Initialize, InitSplashScreen,
                           InitMainMenu, InitSettings, InitScanQR,
                           InitAbout,
                           InitGrid,
                           InitCharacterSheet,
                           InitViewInAR }

    public AppState PreviousAppState = AppState.Initialize;

    private bool RunUpdate = false;

    public OnEnterState onEnterState;
    public OnUpdateState onUpdateState;
    public OnExitState onExitState;

    public delegate void OnEnterState();
    public delegate void OnUpdateState();
    public delegate void OnExitState();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        SetFirstState();
    }

    private void SetFirstState()
    {
        ActiveAppState = AppState.Initialize;
        SetPreviousState();
        EnterState();
    }

    public void SetState(AppState _newState)
    {
        ExitState();
        SetPreviousState();
        ActiveAppState = _newState;
        EnterState();
    }

    public void SetState(string _newStateName)
    {
        ExitState();
        SetPreviousState();
        ActiveAppState = (AppState)System.Enum.Parse(typeof(AppState), _newStateName);
        EnterState();
    }

    private void SetPreviousState()
    {
        if(ActiveAppState == AppState.Initialize || ActiveAppState == AppState.InitMainMenu || ActiveAppState == AppState.InitGrid)
        {
            PreviousAppState = ActiveAppState;
        }
    }

    private void EnterState()
    {
        if (onEnterState != null)
        {
            onEnterState.Invoke();
        }

        RunUpdate = true;
    }

    private void UpdateState()
    {
        if (RunUpdate == false)
        {
            return;
        }

        if (onUpdateState != null)
        {
            onUpdateState.Invoke();
        }
    }

    private void ExitState()
    {
        RunUpdate = false;

        if (onExitState != null)
        {
            onExitState.Invoke();
        }
    }

    private void Update()
    {
        UpdateState();
    }
}

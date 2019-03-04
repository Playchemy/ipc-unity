using UnityEngine;

public abstract class State : MonoBehaviour
{
    public AppStateManager.AppState ChosenAppState = AppStateManager.AppState.Initialize;

    public GameObject StateScreen = null;

    private bool RanOnce = false;

    public abstract void EnterState();
    public abstract void UpdateState();
    public abstract void ExitState();

    private void Start()
    {
        SubscribeToState();
    }

    private void SubscribeToState()
    {
        AppStateManager.Instance.onEnterState  += CanEnterState;
        AppStateManager.Instance.onUpdateState += CanUpdateState;
        AppStateManager.Instance.onExitState   += CanExitState;
    }

    protected void GoBackToPreviousState()
    {
        AppStateManager.Instance.SetState(AppStateManager.Instance.PreviousAppState);
    }

    private void CanEnterState()
    {
        if(IsActive())
        {
            if (RanOnce == false) RunOnce();
            if (StateScreen) StateScreen.SetActive(true);
            EnterState();
        }
        else
        {
            if(StateScreen) StateScreen.SetActive(false);
        }
    }

    private void CanUpdateState()
    {
        if(IsActive())
        {
            UpdateState();
        }
    }

    private void CanExitState()
    {
        if(IsActive())
        {
            ExitState();
        }
    }

    protected virtual void RunOnce()
    {
        RanOnce = true;
    }

    private bool IsActive()
    {
        if (AppStateManager.Instance.ActiveAppState == ChosenAppState)
        {
            return true;
        }

        return false;
    }
}

using System.Collections;
using UnityEngine;

public class AppState_Initialize : State
{
    public AppStateManager.AppState StartFrom = AppStateManager.AppState.Initialize;

    // State Functions =========================================================================================================
    protected override void RunOnce()
    {
        base.RunOnce();

        Screen.orientation = ScreenOrientation.Portrait;
        Screen.SetResolution(1080, 1920, true);
    }

    public override void EnterState()
    {
        WalletManager.Instance.GetWallet();
        SetStartFrom();
    }

    public override void UpdateState()
    {

    }

    public override void ExitState()
    {

    }
    // =========================================================================================================================

    private void SetStartFrom()
    {
        AppStateManager.Instance.SetState(StartFrom);
    }
}

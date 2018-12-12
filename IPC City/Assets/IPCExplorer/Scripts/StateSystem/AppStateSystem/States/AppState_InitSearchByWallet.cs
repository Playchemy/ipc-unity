using System.Collections;
using UnityEngine.UI;
using UnityEngine;

public class AppState_InitSearchByWallet : State
{
    // State Functions =========================================================================================================
    public override void EnterState()
    {

    }

    public override void UpdateState()
    {

    }

    public override void ExitState()
    {

    }
    // =========================================================================================================================

    public void SetInputfield(InputField _addressInputfield)
    {
        LoadWallet(_addressInputfield.text);
    }

    public void SetText(Text _addressText)
    {
        LoadWallet(_addressText.text);
    }

    private void LoadWallet(string _address)
    {
        StartCoroutine(IPCLoader.Instance.LoadToken(_address));
        WalletManager.Instance.PushWallet(_address);
        AppStateManager.Instance.SetState(AppStateManager.AppState.InitGrid);
    }
}

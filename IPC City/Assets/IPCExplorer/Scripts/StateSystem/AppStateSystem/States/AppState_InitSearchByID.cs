using System.Collections;
using UnityEngine.UI;
using UnityEngine;

public class AppState_InitSearchByID : State
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

    public void SetID(InputField _IPCIDInputfield)
    {
        StartCoroutine(IPCLoader.Instance.DirectLoadIPCToCharacterSheet(int.Parse(_IPCIDInputfield.text)));
        AppStateManager.Instance.SetState(AppStateManager.AppState.InitCharacterSheet);
    }
}

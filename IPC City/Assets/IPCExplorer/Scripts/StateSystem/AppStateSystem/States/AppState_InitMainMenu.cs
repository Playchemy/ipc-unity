using System.Collections;
using UnityEngine.UI;
using UnityEngine;

public class AppState_InitMainMenu : State
{
    // State Functions =========================================================================================================
    public override void EnterState()
    {
        VuforiaQRReader.Instance.StopQRReader();
        WalletManager.Instance.GetWallet();

        if (AppStateCallbackHandler.Instance.onStopAllCouroutines != null)
        {
            AppStateCallbackHandler.Instance.onStopAllCouroutines.Invoke();
        }
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
        AppStateHelpers.SetID(_IPCIDInputfield);
    }

    public void SetInputfield(InputField _addressInputfield)
    {
        AppStateHelpers.SetInputfield(_addressInputfield);
    }

    public void SetText(Text _addressText)
    {
        AppStateHelpers.SetText(_addressText);
    }

    public void PasteWallet(InputField _addressInputfield)
    {
        if (GUIUtility.systemCopyBuffer.Length == 42 && GUIUtility.systemCopyBuffer.Substring(0, 2) == "0x")
        {
            _addressInputfield.text = GUIUtility.systemCopyBuffer;
        }
        else
        {
            Debug.Log("This is not an address!");
        }
    }
}

using System.Collections;
using UnityEngine.UI;
using UnityEngine;

public class AppState_InitSettings : State
{
    public Button updateDatabaseButton = null;
    public Text currentMethodText = null;

    // State Functions =========================================================================================================
    protected override void RunOnce()
    {
        base.RunOnce();

        SystemManager.Instance.onSystemLoadTypeSwitch += SetSystemLoadText;
        DataInitializer.Instance.onUpdateDatabase += UpdateDatabase;
    }

    public override void EnterState()
    {
        SetSystemLoadText();
    }

    public override void UpdateState()
    {

    }

    public override void ExitState()
    {

    }
    // =========================================================================================================================

    private void SetSystemLoadText()
    {
        if (SystemManager.Instance.ActiveSystemLoadType == SystemManager.SystemLoadType.OnDemand)
        {
            currentMethodText.text = "Current Method: Always Update";
        }
        else
        {
            currentMethodText.text = "Current Method: Pre-Load";
        }
    }

    private void UpdateDatabase()
    {
        StartCoroutine(WaitForFileClose());
        AppStateManager.Instance.SetState(AppStateManager.AppState.InitSplashScreen);
    }

    private IEnumerator WaitForFileClose()
    {
        updateDatabaseButton.interactable = false;
        yield return new WaitForSeconds(20f);
        updateDatabaseButton.interactable = true;
    }
}

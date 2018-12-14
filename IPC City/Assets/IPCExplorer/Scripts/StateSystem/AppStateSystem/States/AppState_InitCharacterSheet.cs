using System.Collections;
using UnityEngine.UI;
using UnityEngine;

public class AppState_InitCharacterSheet : State
{
    public GameObject attributesSubMenu = null;
    public GameObject dnaSubMenu = null;
    public GameObject characterScreenLoading = null;

    public Text subMenuText = null;

    public Text currentSubrace = null;
    public Image currentSubraceSymbol = null;

    public SubraceSymbolData symbolData;

    // State Functions =========================================================================================================
    protected override void RunOnce()
    {
        base.RunOnce();

        AppStateInputHandler.Instance.onLoadNext += LoadNextIPC;
        AppStateInputHandler.Instance.onLoadPrevious += LoadPreviousIPC;

        IPCLoader.Instance.onCharLoadingStarted += OpenCharacterLoading;
        IPCLoader.Instance.onCharLoadingFinished += CloseCharacterLoading;
    }

    public override void EnterState()
    {
        OpenAttributesSubMenu();
        SetSubraceSymbol();
    }

    public override void UpdateState()
    {

    }

    public override void ExitState()
    {

    }
    // =========================================================================================================================

    public void SetText(UnityEngine.UI.Text _addressText)
    {
        AppStateHelpers.SetText(_addressText);
    }

    public void OpenAttributesSubMenu()
    {
        attributesSubMenu.SetActive(true);
        dnaSubMenu.SetActive(false);

        subMenuText.text = "DNA";
    }

    public void OpenSubMenu()
    {
        if(attributesSubMenu.activeInHierarchy)
        {
            attributesSubMenu.SetActive(false);
            dnaSubMenu.SetActive(true);

            subMenuText.text = "Attributes";
        }
        else
        {
            attributesSubMenu.SetActive(true);
            dnaSubMenu.SetActive(false);

            subMenuText.text = "DNA";
        }
    }

    public void LoadNextIPC()
    {
        if(AppStateManager.Instance.ActiveAppState == AppStateManager.AppState.InitCharacterSheet) StartCoroutine(IPCLoader.Instance.LoadNextIPC());
    }

    public void LoadPreviousIPC()
    {
        if(AppStateManager.Instance.ActiveAppState == AppStateManager.AppState.InitCharacterSheet) StartCoroutine(IPCLoader.Instance.LoadPreviousIPC());
    }

    private void SetSubraceSymbol()
    {
        currentSubraceSymbol.sprite = symbolData.GetSubraceSymbol(currentSubrace.text);
    }

    // Loading Character
    private void OpenCharacterLoading()
    {
        characterScreenLoading.SetActive(true);
    }

    private void CloseCharacterLoading(string _error)
    {
        characterScreenLoading.SetActive(false);
        SetSubraceSymbol();
    }
}

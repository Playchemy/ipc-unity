using System.Collections;
using UnityEngine.UI;
using UnityEngine;

public class AppState_InitGrid : State
{
    public Text pageNumberText = null;
    public Text walletAddressText = null;
    public Button changePageNextButton = null;
    public Button changePagePreviousButton = null;
    public GameObject tokenScreenLoading = null;

    // State Functions =========================================================================================================
    protected override void RunOnce()
    {
        base.RunOnce();

        AppStateInputHandler.Instance.onLoadNext += GoToNextPage;
        AppStateInputHandler.Instance.onLoadPrevious += GoToPreviousPage;

        IPCLoader.Instance.onTokenLoadingFinished += InitiateGrid;
        IPCLoader.Instance.onIpcLoad += UpdateChamberInteractivity;
        GridManager.Instance.onPageButtonUpdate += UpdatePageButtons;
        GridManager.Instance.onPageNumberUpdate += UpdatePageNumber;
        GridFunctions.Instance.onCreateChamber += OpenCharacterSheetFromChamber;
        GridFunctions.Instance.CheckIfCanChangePage();

        // Loading Callbacks
        IPCLoader.Instance.onChamberLoadingStarted += OpenChamberLoading;
        IPCLoader.Instance.onChamberLoadingFinished += CloseChamberLoading;
        IPCLoader.Instance.onTokenLoadingStarted += OpenTokenLoading;
        IPCLoader.Instance.onTokenLoadingFinished += CloseTokenLoading;

        AppStateHelpers.onGridInitialized += SetWalletAddressText;
    }

    public override void EnterState()
    {
        CloseAllLoadingInGrid();

        if(AppStateManager.Instance.PreviousAppState == AppStateManager.AppState.InitMainMenu)
        {
            GridFunctions.Instance.DisableGrid(); // ???
        }
    }

    public override void UpdateState()
    {

    }

    public override void ExitState()
    {

    }
    // =========================================================================================================================

    private void CloseAllLoadingInGrid()
    {
        for (int i = 0; i < GridFunctions.Instance.chamberCount; i++)
        {
            GridManager.Instance.ipcGrid.transform.GetChild(i).Find("LoadingPanel").gameObject.SetActive(false);
        }
    }

    private void InitiateGrid(string _error)
    {
        if(_error == "NULL")
        {
            AppStateManager.Instance.SetState(AppStateManager.AppState.InitMainMenu);
            return;
        }

        GridManager.Instance.InitiateGridFromFirstPage();
    }

    private void UpdateChamberInteractivity(Button _chamber)
    {
        _chamber.interactable = true;
    }

    private void UpdatePageNumber(int _aPage)
    {
        pageNumberText.text = "- " + _aPage.ToString() + " -";
    }

    private void UpdatePageButtons(bool _leftButton, bool _rightButton)
    {
        changePagePreviousButton.gameObject.SetActive(_leftButton);
        changePageNextButton.gameObject.SetActive(_rightButton);
    }

    public void OpenCharacterSheetFromChamber(GameObject _clickedChamber)
    {
        StartCoroutine(IPCLoader.Instance.LoadIPCToCharacterSheet(_clickedChamber.GetComponent<IpcGetService>()));
        AppStateManager.Instance.SetState(AppStateManager.AppState.InitCharacterSheet);
    }

    public void GoToNextPage()
    {
        if (!changePageNextButton.gameObject.activeInHierarchy)
            return;

        if (AppStateManager.Instance.ActiveAppState == AppStateManager.AppState.InitGrid)
        {
            CloseAllLoadingInGrid();
            GridManager.Instance.ChangePage(true);
        }
    }

    public void GoToPreviousPage()
    {
        if (!changePagePreviousButton.gameObject.activeInHierarchy)
            return;

        if (AppStateManager.Instance.ActiveAppState == AppStateManager.AppState.InitGrid)
        {
            CloseAllLoadingInGrid();
            GridManager.Instance.ChangePage(false);
        }
    }

    private void SetWalletAddressText(string _address)
    {
        walletAddressText.text = _address;
    }

    // Grid Loading
    private void OpenChamberLoading(int chamberIndex)
    {
        GridManager.Instance.ipcGrid.transform.GetChild(chamberIndex).Find("LoadingPanel").gameObject.SetActive(true);
    }

    private void CloseChamberLoading(int chamberIndex)
    {
        GridManager.Instance.ipcGrid.transform.GetChild(chamberIndex).Find("LoadingPanel").gameObject.SetActive(false);
    }

    private void OpenTokenLoading()
    {
        tokenScreenLoading.SetActive(true);
    }

    private void CloseTokenLoading(string _error)
    {
        tokenScreenLoading.SetActive(false);
    }
}

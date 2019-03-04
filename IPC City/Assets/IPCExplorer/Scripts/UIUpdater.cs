using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UIUpdater : MonoBehaviour
{
    public static UIUpdater Instance;

    public void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }

    [Header("Pre-App Scene")]
    public GameObject splashScreen = null;

    [Header("Menus")]
    public GameObject settingMenuScreen = null;
    public GameObject aboutMenuScreen = null;
    public GameObject mainMenuScreen = null;
    public GameObject scanMenuScreen = null;
    public GameObject gridMenuScreen = null;
    public GameObject charMenuScreen = null;

    [Header("SubMenus")]
    public GameObject attributesSubMenu = null;
    public GameObject dnaSubMenu = null;
    public GameObject characterScreenLoading = null;
    public GameObject tokenScreenLoading = null;

    [Header("UI Variables")]
    public GameObject background = null;
    public InputField idInputField = null;
    public Button homeButton = null;
    public Button scanButton = null;
    public Button closeCharMenuButton = null;
    public Button changePageLeftButton = null;
    public Button changePageRightButton = null;
    public Button updateDatabaseButton = null;
    public Button turnOffARButton = null;
    public Image dataLoadingFill = null;
    public Text versionText = null;
    public Text dataLoadingText = null;
    public Text ipcLoadingText = null;
    public Text slaceCountLoadingText = null;
    public Text pageNumberText = null;
    public Text currentMethodText = null;
    public Text walletAddressText = null;
    public Text addressScanScreenErrorText = null;

    [Header("Settings Variables")]
    public Toggle animationToggle = null;

    private void Start()
    {
        SetVersion();
        RunSplashScreen();
        SystemManager.Instance.onSystemLoadTypeSwitch += SwitchSystemLoadType;
        DataInitializer.Instance.onDataLoadStarted += OpenSystemLoading;
        DataInitializer.Instance.onDataLoadFinished += CloseSystemLoading;
        DataInitializer.Instance.onDataSlavesInitialized += DataSlavesInitialized;
        DataInitializer.Instance.onAllIpcsLoaded += AllIpcsLoaded;
        DataInitializer.Instance.onUpdateDatabase += UpdateDatabase;
        DataInitializer.Instance.onCheckDatabase += CheckDatabase;
        GridManager.Instance.onPageButtonUpdate += UpdatePageButtons;
        GridManager.Instance.onPageNumberUpdate += UpdatePageNumber;
        GridManager.Instance.onLoadingNewPage += CloseAllLoadingInGrid;
        GridFunctions.Instance.onGridGenerationFinished += CheckAlwaysAnimation;
        IPCLoader.Instance.onIpcLoad += UpdateChamberInteractivity;
        IPCLoader.Instance.onCharLoadingStarted += OpenCharacterLoading;
        IPCLoader.Instance.onCharLoadingFinished += CloseCharacterLoading;
        IPCLoader.Instance.onChamberLoadingStarted += OpenChamberLoading;
        IPCLoader.Instance.onChamberLoadingFinished += CloseChamberLoading;
        IPCLoader.Instance.onTokenLoadingStarted += OpenTokenLoading;
        IPCLoader.Instance.onTokenLoadingFinished += CloseTokenLoading;
        MenuManager.Instance.onStartUp += StartUp;
        MenuManager.Instance.onAttributesOpen += OpenAttributesMenu;
        MenuManager.Instance.onCharMenuClose += CloseCharMenu;
        MenuManager.Instance.onSettingsMenuOpen += OpenSettingsMenu;
        MenuManager.Instance.onAboutMenuOpen += OpenAboutMenu;
        MenuManager.Instance.onMainMenuOpen += OpenMainMenu;
        MenuManager.Instance.onScanMenuOpen += OpenScanMenu;
        MenuManager.Instance.onGridMenuOpen += OpenGridMenu;
        MenuManager.Instance.onCharMenuOpen += OpenCharMenu;
        MenuManager.Instance.onDNAOpen += OpenDNAMenu;
        MenuManager.Instance.onTurnOnAR += TurnOnAR;
        MenuManager.Instance.onTurnOffAR += TurnOffAR;
        QRReader.Instance.onSetWalletAddress += SetWalletAddress;
        VuforiaQRReader.Instance.onSetWalletAddress += SetWalletAddress;

        // ERROR CALLBACKS
        IPCLoader.Instance.onAddressScanScreenError += ScanScreenError;
        QRReader.Instance.onAddressScanScreenError += ScanScreenError;
        VuforiaQRReader.Instance.onAddressScanScreenError += ScanScreenError;
    }

    private void SetVersion()
    {
        versionText.text = "v" + Application.version;
    }

    private void CheckAlwaysAnimation()
    {
        animationToggle.onValueChanged.AddListener((bool value) => ToggleAnimationOption(animationToggle.isOn));

        if (PlayerPrefs.HasKey("AlwaysAnimate") && PlayerPrefs.GetInt("AlwaysAnimate") == 1)
        {
            animationToggle.isOn = true;
            animationToggle.isOn = true;
            ToggleAnimationOption(true);
        }
        else if (PlayerPrefs.HasKey("AlwaysAnimate") && PlayerPrefs.GetInt("AlwaysAnimate") == 0)
        {
            animationToggle.isOn = false;
            animationToggle.isOn = false;
            ToggleAnimationOption(false);
        }
        else
        {
            PlayerPrefs.SetInt("AlwaysAnimate", 1);
            ToggleAnimationOption(true);
            animationToggle.isOn = true;
        }
    }

    private void ToggleAnimationOption(bool isOn)
    {
        if(isOn)
        {
            for (int i = 0; i < IPCLoader.Instance.chamberIpcGetServices.Count; i++)
            {
                IPCLoader.Instance.chamberIpcGetServices[i].GetComponent<SpriteManagerV2>().addToAnimator = true;
                PlayerPrefs.SetInt("AlwaysAnimate", 1);
            }
        }
        else
        {
            for (int i = 0; i < IPCLoader.Instance.chamberIpcGetServices.Count; i++)
            {
                IPCLoader.Instance.chamberIpcGetServices[i].GetComponent<SpriteManagerV2>().addToAnimator = false;
                PlayerPrefs.SetInt("AlwaysAnimate", 0);
            }
        }

        PlayerPrefs.Save();
    }

    private void RunSplashScreen()
    {
        splashScreen.SetActive(true);

        if (SystemManager.Instance.ActiveSystemLoadType == SystemManager.SystemLoadType.OnDemand)
        {
            dataLoadingFill.transform.parent.GetComponent<Image>().CrossFadeAlpha(0f, 0.1f, true);
            dataLoadingFill.transform.GetComponent<Image>().CrossFadeAlpha(0f, 0.1f, true);
            dataLoadingText.CrossFadeAlpha(0f, 0.1f, true);
            ipcLoadingText.CrossFadeAlpha(0f, 0.1f, true);
        }
        else
        {
            dataLoadingFill.transform.parent.GetComponent<Image>().CrossFadeAlpha(1f, 0.1f, true);
            dataLoadingFill.transform.GetComponent<Image>().CrossFadeAlpha(1f, 0.1f, true);
            dataLoadingText.CrossFadeAlpha(1f, 0.1f, true);
            ipcLoadingText.CrossFadeAlpha(1f, 0.1f, true);
        }
    }

    private void SetWalletAddress(string _address)
    {
        walletAddressText.text = _address;
    }

    // ERROR FUNCTIONS ================================================================================================================
    private void ScanScreenError(int _errorCode)
    {
        if(_errorCode == 0)
        {
            addressScanScreenErrorText.text = "";
        }
        else if (_errorCode == 1)
        {
            addressScanScreenErrorText.text = "ERROR: This is not a valid address!";
        }
        else if(_errorCode == 2)
        {
            addressScanScreenErrorText.text = "ERROR: This wallet doesn't have any IPCs!";
        }
    }
    // ================================================================================================================================

    // LOADING FUNCTIONS ==============================================================================================================
    private void SwitchSystemLoadType()
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

    private void CheckDatabase()
    {
        ipcLoadingText.text = DataInitializer.Instance.ipcList.Count + " / " + DataInitializer.Instance.totalNumOfIpcs;
        slaceCountLoadingText.text = FindObjectsOfType<SlaveLoader>().Length.ToString();
        dataLoadingFill.fillAmount = DataInitializer.Instance.ipcList.Count * (0.8f / DataInitializer.Instance.totalNumOfIpcs);
        //result = (data - actual_min)*((desired_max - desired_min)/(actual_max - actual_min)) + desired_min; Normalize formula
    }

    private void UpdateDatabase()
    {
        RunSplashScreen();
        MenuManager.Instance.OpenMainMenu();
        StartCoroutine(WaitForFileClose());
    }

    private IEnumerator WaitForFileClose()
    {
        updateDatabaseButton.interactable = false;
        yield return new WaitForSeconds(15f);
        updateDatabaseButton.interactable = true;
    }

    private void OpenSystemLoading()
    {
        dataLoadingFill.fillAmount = 0f;
        dataLoadingText.text = "Initializing Data Stream...";
    }

    private void DataSlavesInitialized()
    {
        dataLoadingText.text = "Reading IPCs from Blockchain...";
    }

    private void AllIpcsLoaded()
    {
        dataLoadingFill.fillAmount = 0.9f;
        dataLoadingText.text = "Retrieving All Addresses...";
    }

    private void CloseSystemLoading()
    {
        dataLoadingFill.fillAmount = 1f;
        dataLoadingText.text = "IPC Explorer Succesfully Loaded...";
        //splashScreen.transform.GetChild(0).GetComponent<SplashScreen>().ExitSplashScreen();
    }

    private void CloseAllLoadingInGrid()
    {
        for (int i = 0; i < GridFunctions.Instance.chamberCount; i++)
        {
            GridManager.Instance.ipcGrid.transform.GetChild(i).Find("LoadingPanel").gameObject.SetActive(false);
        }
    }

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

    private void OpenCharacterLoading()
    {
        characterScreenLoading.SetActive(true);
    }

    private void CloseCharacterLoading(string _error)
    {
        characterScreenLoading.SetActive(false);
    }
    // ================================================================================================================================

    // GRID UPDATE ====================================================================================================================
    private void UpdatePageNumber(int _aPage)
    {
        pageNumberText.text = "- " + _aPage.ToString() + " -";
    }

    private void UpdatePageButtons(bool _leftButton, bool _rightButton)
    {
        changePageLeftButton.gameObject.SetActive(_leftButton);
        changePageRightButton.gameObject.SetActive(_rightButton);
    }

    private void UpdateChamberInteractivity(Button _chamber)
    {
        _chamber.interactable = true;
    }
    // ================================================================================================================================

    // MENU CALLBACKS =================================================================================================================
    private void StartUp()
    {
        Invoke("OpenMainMenu", 0.5f);
    }

    private void OpenMainMenu()
    {
        settingMenuScreen.SetActive(false);
        aboutMenuScreen.SetActive(false);
        mainMenuScreen.SetActive(true);
        scanMenuScreen.SetActive(false);
        gridMenuScreen.SetActive(false);
        charMenuScreen.SetActive(false);

        walletAddressText.text = "";
        scanButton.gameObject.SetActive(false);
        //homeButton.gameObject.SetActive(false);
    }

    private void OpenSettingsMenu()
    {
        settingMenuScreen.SetActive(true);
        aboutMenuScreen.SetActive(false);
        mainMenuScreen.SetActive(false);
        scanMenuScreen.SetActive(false);
        gridMenuScreen.SetActive(false);
        charMenuScreen.SetActive(false);

        if (SystemManager.Instance.ActiveSystemLoadType == SystemManager.SystemLoadType.OnDemand)
        {
            currentMethodText.text = "Current Method: Always Update";
        }
        else
        {
            currentMethodText.text = "Current Method: Pre-Load";
        }
    }

    private void OpenAboutMenu()
    {
        settingMenuScreen.SetActive(false);
        aboutMenuScreen.SetActive(true);
        mainMenuScreen.SetActive(false);
        scanMenuScreen.SetActive(false);
        gridMenuScreen.SetActive(false);
        charMenuScreen.SetActive(false);
    }

    private void OpenScanMenu()
    {
        CloseAllLoadingInGrid();
        settingMenuScreen.SetActive(false);
        aboutMenuScreen.SetActive(false);
        mainMenuScreen.SetActive(false);
        scanMenuScreen.SetActive(true);
        gridMenuScreen.SetActive(false);
        charMenuScreen.SetActive(false);

        scanButton.gameObject.SetActive(true);
        homeButton.gameObject.SetActive(true);
        scanButton.interactable = true;
        pageNumberText.enabled = false;
        UpdatePageButtons(false, false);

        addressScanScreenErrorText.text = "";
    }

    private void OpenGridMenu()
    {
        CloseAllLoadingInGrid();
        settingMenuScreen.SetActive(false);
        aboutMenuScreen.SetActive(false);
        mainMenuScreen.SetActive(false);
        scanMenuScreen.SetActive(false);
        gridMenuScreen.SetActive(true);
        charMenuScreen.SetActive(false);

        QRReader.Instance.cameraCanvas.enabled = false;
        homeButton.gameObject.SetActive(true);
        scanButton.gameObject.SetActive(true);
        pageNumberText.enabled = true;
        UpdatePageButtons(false, false);
        Invoke("DelayInputFieldEmpty", 0.5f);
    }

    private void DelayInputFieldEmpty()
    {
        idInputField.text = "";
    }

    private void OpenCharMenu()
    {
        CloseAllLoadingInGrid();
        settingMenuScreen.SetActive(false);
        aboutMenuScreen.SetActive(false);
        mainMenuScreen.SetActive(false);
        scanMenuScreen.SetActive(false);
        gridMenuScreen.SetActive(false);
        charMenuScreen.SetActive(true);

        closeCharMenuButton.gameObject.SetActive(true);
        scanButton.interactable = false;
        pageNumberText.enabled = false;
        UpdatePageButtons(false, false);
    }

    private void CloseCharMenu()
    {
        CloseAllLoadingInGrid();
        settingMenuScreen.SetActive(false);
        aboutMenuScreen.SetActive(false);
        mainMenuScreen.SetActive(false);
        scanMenuScreen.SetActive(false);
        gridMenuScreen.SetActive(true);
        charMenuScreen.SetActive(false);

        closeCharMenuButton.gameObject.SetActive(false);
        scanButton.interactable = true;
    }

    private void OpenAttributesMenu()
    {
        attributesSubMenu.SetActive(true);
        dnaSubMenu.SetActive(false);
    }

    private void OpenDNAMenu()
    {
        attributesSubMenu.SetActive(false);
        dnaSubMenu.SetActive(true);
    }

    private void TurnOnAR()
    {
        CloseAllLoadingInGrid();
        settingMenuScreen.SetActive(false);
        aboutMenuScreen.SetActive(false);
        mainMenuScreen.SetActive(false);
        scanMenuScreen.SetActive(false);
        gridMenuScreen.SetActive(false);
        charMenuScreen.SetActive(false);

        closeCharMenuButton.gameObject.SetActive(true);
        scanButton.interactable = false;
        pageNumberText.enabled = false;
        UpdatePageButtons(false, false);
        background.SetActive(false);
        turnOffARButton.gameObject.SetActive(true);
    }

    private void TurnOffAR()
    {
        OpenCharMenu();
        background.SetActive(true);
        turnOffARButton.gameObject.SetActive(false);
    }
    // =================================================================================================================================
}

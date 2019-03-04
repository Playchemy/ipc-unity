using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Events;

public class UIOrganizer : MonoBehaviour
{
    [Header("System Events")]
    public UnityEvent onSceneStart;
    public UnityEvent onInitGrid;
    public UnityEvent onBackToMenu;

    [Header("System Variables")]
    public GameObject ipcGrid                        = null;
    public GameObject characterWindow                = null;

    // UI Variables ----------------------------------------------------------------
    public GameObject menuPanel                      = null;
    public GameObject attributesMenu                 = null;
    public GameObject dnaMenu                        = null;
    public Button pageRightButton                    = null;
    public Button pageLeftButton                     = null;
    public Button characterWindowButton              = null;
    public Button scanButton                         = null;
    public Button backButton                         = null;
    public Text pageNumberText                       = null;
    // -----------------------------------------------------------------------------

    private int pageNumber = 1;
    private List<IpcGetService> chamberIpcGetServices = new List<IpcGetService>();
    private List<Button> chamberButtons               = new List<Button>();
    private QRReader qrReader                         = null;
    private IpcGetService ipcGetServiceMain           = null;
    private Interpreter interpreter                   = null;

    // IPCID Search Variables -------------------------------------------------------
    public InputField ipcIDInputField                 = null;
    private bool searchedID                           = false;
    private int searchID                              = 0;
    // -----------------------------------------------------------------------------

    private void Start()
    {
        Screen.SetResolution(1080, 1920, true);

        qrReader = FindObjectOfType<QRReader>();
        interpreter = GetComponent<Interpreter>();
        SystemManager.Instance.onSystemLoadTypeSwitch += SetService;
        SetService();

        if(onSceneStart != null)
        {
            onSceneStart.Invoke();
        }

        for (int i = 0; i < ipcGrid.transform.childCount; i++)
        {
            chamberIpcGetServices.Add(ipcGrid.transform.GetChild(i).GetComponent<IpcGetService>());
            chamberButtons.Add(ipcGrid.transform.GetChild(i).GetComponent<Button>());
        }
    }

    private void SetService()
    {
        if (SystemManager.Instance.ActiveSystemLoadType == SystemManager.SystemLoadType.OnDemand)
        {
            ipcGetServiceMain = SystemManager.Instance.GetOnDemandService();
        }
        else
        {
            ipcGetServiceMain = SystemManager.Instance.GetCacheService();
        }
    }

    public void InitiateOrganizationFromFirstPage()
    {
        StopAllCoroutines();

        if(onInitGrid != null)
        {
            onInitGrid.Invoke();
        }

        pageNumber = 1;
        pageNumberText.text = "- " + pageNumber.ToString() + " -";

        if (ipcGetServiceMain.ownedIpcsIds.Count > 1) ipcGrid.SetActive(true);
        RunGrid();
    }

    private void InitiateOrganizationFromSamePage()
    {
        StopAllCoroutines();

        if (onInitGrid != null)
        {
            onInitGrid.Invoke();
        }

        if (ipcGetServiceMain.ownedIpcsIds.Count > 1) ipcGrid.SetActive(true);
        RunGrid();

        pageNumberText.text = "- " + pageNumber.ToString() + " -";
    }

    public void ChangePage(bool goToNext)
    {
        StopAllCoroutines();

        if (goToNext) pageNumber++; else pageNumber--;
        RunGrid();

        pageNumberText.text = "- " + pageNumber.ToString() + " -";
    }

    public void BackToMenu()
    {
        if(onBackToMenu != null)
        {
            onBackToMenu.Invoke();
        }

        menuPanel.SetActive(true);
        scanButton.gameObject.SetActive(false);
        backButton.gameObject.SetActive(false);
        DisableGrid();
    }

    public void OpenCharacterWindow(GameObject clickedObject)
    {
        DisableGrid();
        OpenAttributesMenu();
        StartCoroutine(LoadIPCToCharacterSheet(clickedObject.GetComponent<IpcGetService>()));
    }

    public void CloseCharacterWindow()
    {
        if (searchedID == false)
        {
            InitiateOrganizationFromSamePage();
        }
        else
        {
            // Creates temporary text to feed it into debug function
            GameObject tempGameObject = new GameObject();
            tempGameObject.AddComponent<Text>();
            tempGameObject.GetComponent<Text>().text = searchID.ToString();
            IPCIDConfirm(tempGameObject.GetComponent<Text>());
            Destroy(tempGameObject);
        }

        scanButton.interactable = true;
        characterWindow.SetActive(false);
        characterWindowButton.gameObject.SetActive(false);
        OpenAttributesMenu();
    }

    public void OpenAttributesMenu()
    {
        attributesMenu.SetActive(true);
        dnaMenu.SetActive(false);
    }

    public void OpenDNAMenu()
    {
        attributesMenu.SetActive(false);
        dnaMenu.SetActive(true);
    }

    // IPC LOADERS ========================================================================================================
    /// <summary>
    /// This function runs in a loop to pull each IPC data for each available chamber.
    /// Chamber IPC service runs a function to get IPC data for itself from the main IPC service.
    /// ONLY after chamber IPC service gets the IPC data its button is activated which enables user to
    /// open character sheet.
    /// </summary>
    /// <param name="IPCChamberIndex"> Which chamber data is being loading into </param>
    /// <param name="IPCIndex"> Which owned IPC is being loaded to chamber from its array</param>
    private IEnumerator LoadEachIPC(int IPCChamberIndex, int IPCIndex)
    {
        StartCoroutine(chamberIpcGetServices[IPCChamberIndex].GetOneIpc(ipcGetServiceMain.ownedIpcsIds[IPCIndex]));
        yield return new WaitUntil(() => chamberIpcGetServices[IPCChamberIndex].OneIPCLoaded == true);
        chamberButtons[IPCChamberIndex].interactable = true;
        chamberIpcGetServices[IPCChamberIndex].inputIPCID = ipcGetServiceMain.ownedIpcsIds[IPCIndex];
        chamberIpcGetServices[IPCChamberIndex].GetComponent<Interpreter>().Interpret();
        ChangeArtAlpha(IPCChamberIndex, 1f);
        scanButton.interactable = true;
    }

    private IEnumerator LoadToken(string address)
    {
        ipcGetServiceMain.inputWalletID = address;
        StartCoroutine(ipcGetServiceMain.GetTokensOfOwner(address));
        yield return new WaitUntil(() => ipcGetServiceMain.TokensLoaded == true);
        InitiateOrganizationFromFirstPage();
    }

    /// <summary>
    /// Loads only one IPC to first chamber
    /// </summary>
    /// <param name="IPCID"> The ID from the chamber text </param>
    /// <returns></returns>
    private IEnumerator LoadOnlyOneIPC(int IPCID)
    {
        StartCoroutine(chamberIpcGetServices[0].GetOneIpc(IPCID));
        chamberButtons[0].interactable = false;
        yield return new WaitUntil(() => chamberIpcGetServices[0].OneIPCLoaded == true);
        chamberButtons[0].interactable = true;
        chamberIpcGetServices[0].inputIPCID = IPCID;
        scanButton.interactable = true;
        chamberIpcGetServices[0].GetComponent<Interpreter>().Interpret();
        ChangeArtAlpha(0, 1f);

        searchedID = true;
        searchID = IPCID;
    }

    /// <summary>
    /// Displays character sheet of the clicked chamber.
    /// </summary>
    /// <param name="ipcGetService">  </param>
    private IEnumerator LoadIPCToCharacterSheet(IpcGetService ipcGetService)
    {
        characterWindow.SetActive(true);
        characterWindowButton.gameObject.SetActive(true);
        StartCoroutine(ipcGetServiceMain.GetOneIpc(ipcGetService.inputIPCID));
        yield return new WaitUntil(() => ipcGetServiceMain.OneIPCLoaded == true);
        ipcGetServiceMain.inputWalletID = ipcGetServiceMain.ipcStorage.m_owner;
        interpreter.Interpret();
    }
    // ==================================================================================================================

    // GRID COMMANDS ====================================================================================================
    private void RunGrid()
    {
        DisableGrid();
        EnableCardTexts(ipcGetServiceMain);
        EnableCardArts(ipcGetServiceMain);
        for (int i = 0; i < 9; i++) { ChangeArtAlpha(i, 0f); }
        EnableCardIPCs();
        CheckIfCanChangePage();
    }

    private void EnableCardTexts(IpcGetService ipcGetService)
    {
        Debug.Log("Card Text(s) Enabled");

        if (ipcGetService.ownedIpcsIds.Count > pageNumber * 9)
        {
            // Activate every grid chamber
            for (int i = 0; i < 9; i++)
            {
                Color temp = ipcGrid.transform.GetChild(i).GetComponent<Image>().color;
                temp.a = 1f;
                ipcGrid.transform.GetChild(i).GetComponent<Image>().color = temp;

                for (int j = 0; j < ipcGrid.transform.GetChild(i).childCount; j++)
                {
                    // Name, Price and ID
                    if (j < 3)
                    {
                        ipcGrid.transform.GetChild(i).GetChild(j).GetComponent<Text>().enabled = true;
                    }
                }
            }
        }
        else
        {
            for (int i = 0; i < 9 - ((pageNumber * 9) - ipcGetService.ownedIpcsIds.Count); i++)
            {
                if (pageNumber * 9 > ipcGetService.ownedIpcsIds.Count)
                {
                    if (i == 9 - (pageNumber * 9 - ipcGetService.ownedIpcsIds.Count))
                    {
                        break;
                    }
                }

                Color temp = ipcGrid.transform.GetChild(i).GetComponent<Image>().color;
                temp.a = 1f;
                ipcGrid.transform.GetChild(i).GetComponent<Image>().color = temp;

                for (int j = 0; j < ipcGrid.transform.GetChild(i).childCount; j++)
                {
                    // Name, Price and ID
                    if (j < 3)
                    {
                        ipcGrid.transform.GetChild(i).GetChild(j).GetComponent<Text>().enabled = true;
                    }
                }
            }
        }
    }

    private void EnableCardText()
    {
        Debug.Log("Card Text Enabled");

        Color temp = ipcGrid.transform.GetChild(0).GetComponent<Image>().color;
        temp.a = 1f;
        ipcGrid.transform.GetChild(0).GetComponent<Image>().color = temp;

        for (int j = 0; j < ipcGrid.transform.GetChild(0).childCount; j++)
        {
            // Name, Price and ID
            if (j < 3)
            {
                ipcGrid.transform.GetChild(0).GetChild(j).GetComponent<Text>().enabled = true;
            }
        }
    }

    private void EnableCardArts(IpcGetService ipcGetService)
    {
        Debug.Log("Card Art(s) Enabled");

        if (ipcGetService.ownedIpcsIds.Count > pageNumber * 9)
        {
            // Activate every grid chamber
            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < ipcGrid.transform.GetChild(i).childCount; j++)
                {
                    ipcGrid.transform.GetChild(i).GetChild(3).GetComponent<Image>().enabled = true;

                    for (int k = 0; k < ipcGrid.transform.GetChild(i).GetChild(3).childCount; k++)
                    {
                        ipcGrid.transform.GetChild(i).GetChild(3).GetChild(k).GetComponent<Image>().enabled = true;
                    }
                }
            }
        }
        else
        {
            for (int i = 0; i < 9 - ((pageNumber * 9) - ipcGetService.ownedIpcsIds.Count); i++)
            {
                if (pageNumber * 9 > ipcGetService.ownedIpcsIds.Count)
                {
                    if (i == 9 - (pageNumber * 9 - ipcGetService.ownedIpcsIds.Count))
                    {
                        break;
                    }
                }

                ipcGrid.transform.GetChild(i).GetChild(3).GetComponent<Image>().enabled = true;

                for (int j = 0; j < ipcGrid.transform.GetChild(i).GetChild(3).childCount; j++)
                {
                    ipcGrid.transform.GetChild(i).GetChild(3).GetChild(j).GetComponent<Image>().enabled = true;
                }
            }
        }
    }

    private void EnableCardArt()
    {
        Debug.Log("Card Art Enabled");

        ipcGrid.transform.GetChild(0).GetChild(3).GetComponent<Image>().enabled = true;

        for (int k = 0; k < ipcGrid.transform.GetChild(0).GetChild(3).childCount; k++)
        {
            ipcGrid.transform.GetChild(0).GetChild(3).GetChild(k).GetComponent<Image>().enabled = true;
        }
    }

    private void ChangeArtAlpha(int childIndex, float newAlpha)
    {
        Color temp = ipcGrid.transform.GetChild(childIndex).GetChild(3).GetComponent<Image>().color;
        temp.a = newAlpha;
        ipcGrid.transform.GetChild(childIndex).GetChild(3).GetComponent<Image>().color = temp;

        for (int j = 0; j < ipcGrid.transform.GetChild(childIndex).GetChild(3).childCount; j++)
        {
            Color temp2 = ipcGrid.transform.GetChild(childIndex).GetChild(3).GetChild(j).GetComponent<Image>().color;
            temp2.a = newAlpha;
            ipcGrid.transform.GetChild(childIndex).GetChild(3).GetChild(j).GetComponent<Image>().color = temp2;
        }
    }

    private void EnableCardIPCs()
    {
        Debug.Log("Card IPC(s) Enabled");

        if (ipcGetServiceMain.ownedIpcsIds.Count > pageNumber * 9)
        {
            for (int i = (pageNumber * 9) - 9; i < pageNumber * 9; i++)
            {
                int childIndex = ((i - ((pageNumber * 9) - 9)) % (pageNumber * 9));
                StartCoroutine(LoadEachIPC(childIndex, i));
            }
        }
        else
        {
            for (int i = (pageNumber * 9) - 9; i < ipcGetServiceMain.ownedIpcsIds.Count; i++)
            {
                int childIndex = ((i - ((pageNumber * 9) - 9)) % ipcGetServiceMain.ownedIpcsIds.Count);
                StartCoroutine(LoadEachIPC(childIndex, i));
            }
        }
    }

    private void EnableCardIPC(int IPCID)
    {
        Debug.Log("Card IPC Enabled");

        StartCoroutine(LoadOnlyOneIPC(IPCID));
    }

    private void DisableGrid()
    {
        // Disable buttons
        pageLeftButton.gameObject.SetActive(false);
        pageRightButton.gameObject.SetActive(false);

        for (int i = 0; i < 9; i++)
        {
            ipcGrid.transform.GetChild(i).GetComponent<Button>().interactable = false;
        }

        // Deactivate every grid chamber
        for (int i = 0; i < 9; i++)
        {
            Color temp = ipcGrid.transform.GetChild(i).GetComponent<Image>().color;
            temp.a = 0f;
            ipcGrid.transform.GetChild(i).GetComponent<Image>().color = temp;

            for (int j = 0; j < ipcGrid.transform.GetChild(i).childCount; j++)
            {
                // Name, Price and ID
                if(j < 3)
                {
                    ipcGrid.transform.GetChild(i).GetChild(j).GetComponent<Text>().enabled = false;
                    ipcGrid.transform.GetChild(i).GetChild(j).GetComponent<Text>().text = "";
                }
                else if(j == 3) // IPC body art
                {
                    ipcGrid.transform.GetChild(i).GetChild(j).GetComponent<Image>().enabled = false;

                    for (int k = 0; k < ipcGrid.transform.GetChild(i).GetChild(j).childCount; k++)
                    {
                        ipcGrid.transform.GetChild(i).GetChild(j).GetChild(k).GetComponent<Image>().enabled = false;
                    }
                }
            }
        }
    }

    private void CheckIfCanChangePage()
    {
        Debug.Log("Checking Page(s)");

        if (pageNumber == 1 && ipcGetServiceMain.ownedIpcsIds.Count > pageNumber * 9)
        {
            pageLeftButton.gameObject.SetActive(false);
            pageRightButton.gameObject.SetActive(true);
        }
        else if (pageNumber == 1 && ipcGetServiceMain.ownedIpcsIds.Count <= pageNumber * 9)
        {
            pageLeftButton.gameObject.SetActive(false);
            pageRightButton.gameObject.SetActive(false);
        }
        else if (pageNumber != 1 && ipcGetServiceMain.ownedIpcsIds.Count > pageNumber * 9)
        {
            pageLeftButton.gameObject.SetActive(true);
            pageRightButton.gameObject.SetActive(true);
        }
        else if (pageNumber != 1 && ipcGetServiceMain.ownedIpcsIds.Count <= pageNumber * 9)
        {
            pageLeftButton.gameObject.SetActive(true);
            pageRightButton.gameObject.SetActive(false);
        }
    }

    public void IPCIDConfirm(Text IPCIDText)
    {
        menuPanel.SetActive(false);
        backButton.gameObject.SetActive(true);
        StopAllCoroutines();
        qrReader.cameraCanvas.enabled = false;
        ipcGrid.SetActive(true);
        pageNumber = 1;
        pageNumberText.text = "- " + pageNumber.ToString() + " -";
        EnableCardText();
        EnableCardArt();
        for (int i = 0; i < 9; i++) { ChangeArtAlpha(i, 0f); }
        StartCoroutine(LoadOnlyOneIPC(int.Parse(IPCIDText.text)));
        ipcIDInputField.text = "";
        pageLeftButton.gameObject.SetActive(false);
        pageRightButton.gameObject.SetActive(false);
    }

    public void WalletConfirm(Text IPCIDText)
    {
        StartCoroutine(LoadToken(IPCIDText.text));
        scanButton.gameObject.SetActive(true);
    }

    public void PasteWallet(InputField IPCIDText)
    {
        IPCIDText.text = GUIUtility.systemCopyBuffer;
        StartCoroutine(LoadToken(IPCIDText.text));
    }
}
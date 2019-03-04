using System.Collections.Generic;
using System.Collections;
using UnityEngine.UI;
using UnityEngine;
using Vuforia;

public class MenuManager : MonoBehaviour
{
    public static MenuManager Instance;

    public OnStartUp onStartUp;
    public delegate void OnStartUp();

    public OnMainMenuOpen onMainMenuOpen;
    public delegate void OnMainMenuOpen();

    public OnSettingsMenuOpen onSettingsMenuOpen;
    public delegate void OnSettingsMenuOpen();

    public OnAboutMenuOpen onAboutMenuOpen;
    public delegate void OnAboutMenuOpen();

    public OnScanMenuOpen onScanMenuOpen;
    public delegate void OnScanMenuOpen();

    public OnGridMenuOpen onGridMenuOpen;
    public delegate void OnGridMenuOpen();

    public OnCharMenuOpen onCharMenuOpen;
    public delegate void OnCharMenuOpen();

    public OnCharMenuClose onCharMenuClose;
    public delegate void OnCharMenuClose();

    public OnAttributesOpen onAttributesOpen;
    public delegate void OnAttributesOpen();

    public OnDNAOpen onDNAOpen;
    public delegate void OnDNAOpen();

    public OnStopAllCouroutines onStopAllCouroutines;
    public delegate void OnStopAllCouroutines();

    public OnTurnOnAR onTurnOnAR;
    public delegate void OnTurnOnAR();

    public OnTurnOffAR onTurnOffAR;
    public delegate void OnTurnOffAR();

    public bool canGoToNextPage = false;
    public bool canGoToPreviousPage = false;
    public int singleIPCSearchID = 0;

    // AR
    public List<UnityEngine.UI.Image> characterImages;
    public List<SpriteRenderer> arSprites;
    public SpriteHandler_UI characterSpritehandler = null;
    public SpriteHandler_UI arSpritehandler = null;
    public VuforiaBehaviour vuforiaBehaviour = null;

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

    private void Start()
    {
        Screen.orientation = ScreenOrientation.Portrait;    
        Screen.SetResolution(1080, 1920, true);
        dragDistance = Screen.height * 15 / 100;
        StartUp();
    }

    public void OpenWebsite()
    {
        Application.OpenURL("https://www.immortalplayercharacters.com/");
    }

    private void StartUp()
    {
        if(onStartUp != null)
        {
            onStartUp.Invoke();
        }

        //ServiceInitializer.Instance.InitializeSystemLoad();
        GridFunctions.Instance.DisableGrid();
        WalletManager.Instance.GetWallet();
    }

    public void OpenMainMenu()
    {
        if(onMainMenuOpen != null)
        {
            onMainMenuOpen.Invoke();
        }

        if (onStopAllCouroutines != null)
        {
            onStopAllCouroutines.Invoke();
        }

        VuforiaQRReader.Instance.StopQRReader();
        GridFunctions.Instance.DisableGrid();
        WalletManager.Instance.GetWallet();
        canGoToNextPage = false;
        canGoToPreviousPage = false;
        singleIPCSearchID = 0;
    }

    public void OpenSettingsMenu()
    {
        if (onSettingsMenuOpen != null)
        {
            onSettingsMenuOpen.Invoke();
        }
    }

    public void OpenAboutMenu()
    {
        if(onAboutMenuOpen != null)
        {
            onAboutMenuOpen.Invoke();
        }
    }

    public void OpenScanMenu()
    {
        if (onScanMenuOpen != null)
        {
            onScanMenuOpen.Invoke();
        }

        if(onStopAllCouroutines != null)
        {
            onStopAllCouroutines.Invoke();
        }

        VuforiaQRReader.Instance.StartQRReader();
        //QRReader.Instance.StartScanner();
        GridFunctions.Instance.DisableGrid();
        singleIPCSearchID = 0;
    }

    public void OpenGridMenu(bool _loadSamePage)
    {
        if (onGridMenuOpen != null)
        {
            onGridMenuOpen.Invoke();
        }

        if (_loadSamePage)
        {
            GridManager.Instance.InitiateGridFromSamePage();
        }
        else
        {
            GridManager.Instance.InitiateGridFromFirstPage();
        }
    }

    public void OpenCharacterWindow(GameObject clickedObject)
    {
        if (onCharMenuOpen != null)
        {
            onCharMenuOpen.Invoke();
        }

        if (onStopAllCouroutines != null)
        {
            onStopAllCouroutines.Invoke();
        }

        OpenAttributesMenu();
        GridFunctions.Instance.DisableGrid();
        StartCoroutine(IPCLoader.Instance.LoadIPCToCharacterSheet(clickedObject.GetComponent<IpcGetService>()));
    }

    public void CloseCharacterWindow()
    {
        if (onCharMenuClose != null)
        {
            onCharMenuClose.Invoke();
        }

        OpenAttributesMenu();

        if(singleIPCSearchID == 0)
        {
            OpenGridMenu(true);
        }
        else
        {
            IPCIDConfirm(singleIPCSearchID);
        }
    }

    public void IPCIDConfirm(Text IPCIDText)
    {
        if (onGridMenuOpen != null)
        {
            onGridMenuOpen.Invoke();
        }

        GridManager.Instance.SetPageNumber(1);
        GridFunctions.Instance.DisableGrid();
        GridFunctions.Instance.EnableCardText();
        GridFunctions.Instance.EnableCardArt();
        for (int i = 0; i < 1; i++) { GridFunctions.Instance.ChangeArtAlpha(i, 0f); }
        StartCoroutine(IPCLoader.Instance.LoadOnlyOneIPC(int.Parse(IPCIDText.text)));
        singleIPCSearchID = int.Parse(IPCIDText.text);
    }

    public void IPCIDConfirm(int IPCID)
    {
        if (onGridMenuOpen != null)
        {
            onGridMenuOpen.Invoke();
        }

        GridManager.Instance.SetPageNumber(1);
        GridFunctions.Instance.DisableGrid();
        GridFunctions.Instance.EnableCardText();
        GridFunctions.Instance.EnableCardArt();
        for (int i = 0; i < 1; i++) { GridFunctions.Instance.ChangeArtAlpha(i, 0f); }
        StartCoroutine(IPCLoader.Instance.LoadOnlyOneIPC(IPCID));
    }

    public void WalletConfirm(InputField IPCIDText)
    {
        if (IPCIDText.text.Length != 42 || IPCIDText.text.Substring(0, 2) != "0x")
        {
            return;
        }

        if (onGridMenuOpen != null)
        {
            onGridMenuOpen.Invoke();
        }

        StartCoroutine(IPCLoader.Instance.LoadToken(IPCIDText.text));
        singleIPCSearchID = 0;
    }

    public void WalletLoadFromRecents(Text IPCIDText)
    {
        if (onGridMenuOpen != null)
        {
            onGridMenuOpen.Invoke();
        }

        StartCoroutine(IPCLoader.Instance.LoadToken(IPCIDText.text));
        singleIPCSearchID = 0;
    }

    public void PasteWallet(InputField IPCIDText)
    {
        if (GUIUtility.systemCopyBuffer.Length == 42 && GUIUtility.systemCopyBuffer.Substring(0, 2) == "0x")
        {
            IPCIDText.text = GUIUtility.systemCopyBuffer;
        }
        else
        {
            Debug.Log("This is not an address!");
        }
    }

    public void OpenWalletFromCharScreen()
    {
        if (onGridMenuOpen != null)
        {
            onGridMenuOpen.Invoke();
        }

        StartCoroutine(IPCLoader.Instance.LoadToken(GridManager.Instance.ipcGetServiceMain.inputWalletID));
        GUIUtility.systemCopyBuffer = GridManager.Instance.ipcGetServiceMain.inputWalletID;
    }

    public void OpenAttributesMenu()
    {
        if (onAttributesOpen != null)
        {
            onAttributesOpen.Invoke();
        }
    }

    public void OpenDNAMenu()
    {
        if (onDNAOpen != null)
        {
            onDNAOpen.Invoke();
        }
    }

    public void LoadNextIPC()
    {
        StartCoroutine(IPCLoader.Instance.LoadNextIPC());
    }

    public void LoadPreviousIPC()
    {
        StartCoroutine(IPCLoader.Instance.LoadPreviousIPC());
    }

    public void LoadNextPage()
    {
        GridManager.Instance.ChangePage(true);
    }

    public void LoadPreviousPage()
    {
        GridManager.Instance.ChangePage(false);
    }

    public void TurnOnAR()
    {
        vuforiaBehaviour.enabled = true;
        VuforiaRuntime.Instance.InitVuforia();
        VuforiaRenderer.Instance.Pause(false);

        for (int i = 0; i < characterImages.Count; i++)
        {
            arSprites[i].sprite = characterImages[i].sprite;
            arSprites[i].color = characterImages[i].color;
        }

        arSpritehandler.transform.parent.gameObject.SetActive(true);
        arSpritehandler.skinData = characterSpritehandler.skinData;
        arSpritehandler.clothesData = characterSpritehandler.clothesData;
        arSpritehandler.hairData = characterSpritehandler.hairData;
        arSpritehandler.accessoryData = characterSpritehandler.accessoryData;

        if (onTurnOnAR != null)
        {
            onTurnOnAR.Invoke();
        }
    }

    public void TurnOffAR()
    {
        //VuforiaRuntime.Instance.Deinit();
        VuforiaRenderer.Instance.Pause(true);
        //vuforiaBehaviour.enabled = false;
        arSpritehandler.transform.parent.gameObject.SetActive(false);

        if (onTurnOffAR != null)
        {
            onTurnOffAR.Invoke();
        }
    }

    // INPUT CHECK ==============================================================================================================
    private Vector3 firstTouchPosition;
    private Vector3 lastTouchPosition;
    private float dragDistance;

    private void Update()
    {
        if (Input.touchCount == 1)
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began)
            {
                firstTouchPosition = touch.position;
                lastTouchPosition = touch.position;
            }
            else if (touch.phase == TouchPhase.Moved)
            {
                lastTouchPosition = touch.position;
            }
            else if (touch.phase == TouchPhase.Ended)
            {
                lastTouchPosition = touch.position;

                if (Mathf.Abs(lastTouchPosition.x - firstTouchPosition.x) > dragDistance || Mathf.Abs(lastTouchPosition.y - firstTouchPosition.y) > dragDistance)
                {
                    if (Mathf.Abs(lastTouchPosition.x - firstTouchPosition.x) > Mathf.Abs(lastTouchPosition.y - firstTouchPosition.y))
                    {
                        if (lastTouchPosition.x > firstTouchPosition.x)
                        {
                            Debug.Log("Right Swipe");
                            if(canGoToPreviousPage)
                            {
                                LoadPreviousPage();
                            }
                            LoadPreviousIPC();
                        }
                        else
                        {
                            Debug.Log("Left Swipe");
                            if(canGoToNextPage)
                            {
                                LoadNextPage();
                            }
                            LoadNextIPC();
                        }
                    }
                    else
                    {
                        if (lastTouchPosition.y > firstTouchPosition.y)
                        {
                            Debug.Log("Up Swipe");
                        }
                        else
                        {
                            Debug.Log("Down Swipe");
                        }
                    }
                }
                else
                {
                    Debug.Log("Tap");
                }
            }
        }
    }
    // ==============================================================================================================================
}
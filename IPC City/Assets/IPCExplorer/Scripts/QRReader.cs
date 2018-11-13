using BarcodeScanner;
using BarcodeScanner.Scanner;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class QRReader : MonoBehaviour
{
    public static QRReader Instance;

    public RawImage cameraCanvas;

    private float RestartTime;
    private IScanner BarcodeScanner;
    private IpcGetService ipcGetService = null;

    public OnScanEnable onSetWalletAddress;
    public delegate void OnScanEnable(string _address);

    public OnAddressError onAddressScanScreenError;
    public delegate void OnAddressError(int _errorCode);

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

        SystemManager.Instance.onSystemLoadTypeSwitch += SetService;
        SetService();
    }

    private void SetService()
    {
        if (SystemManager.Instance.ActiveSystemLoadType == SystemManager.SystemLoadType.OnDemand)
        {
            ipcGetService = SystemManager.Instance.GetOnDemandService();
        }
        else
        {
            ipcGetService = SystemManager.Instance.GetCacheService();
        }
    }

    private void Start()
    {
        StartCoroutine(CheckPermisions());
    }

    public void StopBarcode()
    {
        BarcodeScanner.Stop();
    }

    private IEnumerator CheckPermisions()
    {
        yield return Application.RequestUserAuthorization(UserAuthorization.WebCam);
        yield return new WaitForSeconds(1);
        if(Application.HasUserAuthorization(UserAuthorization.WebCam))
        {
            BarcodeScanner = new Scanner();
            BarcodeScanner.Camera.Play();

            BarcodeScanner.OnReady += (sender, arg) => {
            cameraCanvas.transform.localScale = BarcodeScanner.Camera.GetScale();
            cameraCanvas.texture = BarcodeScanner.Camera.Texture;

            var rect = cameraCanvas.GetComponent<RectTransform>();
            var newHeight = rect.sizeDelta.x * BarcodeScanner.Camera.Height / BarcodeScanner.Camera.Width;

            RestartTime = Time.realtimeSinceStartup;
            };
        }
    }

    private void RunScanner()
    {
        BarcodeScanner.Scan((barCodeType, barCodeValue) => 
        {
            RestartTime += Time.realtimeSinceStartup + 1f;

            if (onSetWalletAddress != null)
            {
                onSetWalletAddress.Invoke("Wallet Address: " + barCodeValue);
            }

            if (barCodeValue.Length == 42 && barCodeValue.Substring(0, 2) == "0x")
            {
                if (MenuManager.Instance.onGridMenuOpen != null)
                {
                    MenuManager.Instance.onGridMenuOpen.Invoke();
                }

                if (onAddressScanScreenError != null)
                {
                    onAddressScanScreenError.Invoke(0);
                }

                StartCoroutine(IPCLoader.Instance.LoadToken(barCodeValue));
            }
            else
            {
                if (onAddressScanScreenError != null)
                {
                    onAddressScanScreenError.Invoke(1);
                }
            }

            if (onSetWalletAddress != null)
            {
                onSetWalletAddress.Invoke("Wallet Address: (None Found)");
            }
        });
    }

    public void StartScanner()
    {
        cameraCanvas.enabled = true;
        ClickStart();

        if(onSetWalletAddress != null)
        {
            onSetWalletAddress.Invoke("Wallet Address: (None Found)");
        }

        if (onAddressScanScreenError != null)
        {
            onAddressScanScreenError.Invoke(0);
        }
    }

    /// <summary>
    /// The Update method from unity need to be propagated
    /// </summary>
    private void Update()
    {
        if (BarcodeScanner != null)
        {
            BarcodeScanner.Update();
        }

        // Check if the Scanner need to be started or restarted
        if (RestartTime != 0 && RestartTime < Time.realtimeSinceStartup)
        {
            StartScanner();
            RestartTime = 0;
        }
    }

    #region UI Buttons

    public void ClickBack()
    {
        // Try to stop the camera before loading another scene
        StartCoroutine(StopCamera(() => {
            SceneManager.LoadScene("Boot");
        }));
    }

    public void ClickStart()
    {
        BarcodeScanner.Scan((barCodeType, barCodeValue) =>
        {
            BarcodeScanner.Stop();

            RestartTime += Time.realtimeSinceStartup + 1f;

            if (onSetWalletAddress != null)
            {
                onSetWalletAddress.Invoke("Wallet Address: " + barCodeValue);
            }

            if (barCodeValue.Length == 42 && barCodeValue.Substring(0, 2) == "0x")
            {
                if (MenuManager.Instance.onGridMenuOpen != null)
                {
                    MenuManager.Instance.onGridMenuOpen.Invoke();
                }

                if (onAddressScanScreenError != null)
                {
                    onAddressScanScreenError.Invoke(0);
                }

                StartCoroutine(IPCLoader.Instance.LoadToken(barCodeValue));
            }
            else
            {
                if (onAddressScanScreenError != null)
                {
                    onAddressScanScreenError.Invoke(1);
                }
            }

            if (onSetWalletAddress != null)
            {
                onSetWalletAddress.Invoke("Wallet Address: (None Found)");
            }
        });
    }

    /// <summary>
    /// This coroutine is used because of a bug with unity (http://forum.unity3d.com/threads/closing-scene-with-active-webcamtexture-crashes-on-android-solved.363566/)
    /// Trying to stop the camera in OnDestroy provoke random crash on Android
    /// </summary>
    /// <param name="callback"></param>
    /// <returns></returns>
    public IEnumerator StopCamera(Action callback)
    {
        // Stop Scanning
        cameraCanvas = null;
        BarcodeScanner.Destroy();
        BarcodeScanner = null;

        // Wait a bit
        yield return new WaitForSeconds(0.1f);

        callback.Invoke();
    }

    #endregion

    public void ManualStartScanner(string _address)
    {
        if (onSetWalletAddress != null)
        {
            onSetWalletAddress.Invoke("Wallet Address: " + _address);
        }

        if (_address.Length == 42 && _address.Substring(0, 2) == "0x")
        {
            if (MenuManager.Instance.onGridMenuOpen != null)
            {
                MenuManager.Instance.onGridMenuOpen.Invoke();
            }

            if (onAddressScanScreenError != null)
            {
                onAddressScanScreenError.Invoke(0);
            }

            StartCoroutine(IPCLoader.Instance.LoadToken(_address));
        }
        else
        {
            if (onAddressScanScreenError != null)
            {
                onAddressScanScreenError.Invoke(1);
            }
        }

        if (onSetWalletAddress != null)
        {
            onSetWalletAddress.Invoke("Wallet Address: (None Found)");
        }
    }
}

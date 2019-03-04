using System.Collections.Generic;
using System.Collections;
using ZXing.QrCode;
using ZXing.Common;
using UnityEngine;
using Vuforia;
using System;
using ZXing;

public class VuforiaQRReader : MonoBehaviour
{
    public static VuforiaQRReader Instance;

    public GameObject background = null;
    public Image.PIXEL_FORMAT pixelFormat = Image.PIXEL_FORMAT.RGBA8888;

    private bool cameraInitialized;
    private BarcodeReader barCodeReader;
    private IpcGetService ipcGetService = null;
    private VuforiaBehaviour vuforiaBehaviour = null;

    private bool isFormatSet = false;
    private Image.PIXEL_FORMAT tempFormat = Image.PIXEL_FORMAT.RGBA8888;

    public OnAddressError onAddressScanScreenError;
    public OnScanEnable onSetWalletAddress;

    public delegate void OnAddressError(int _errorCode);
    public delegate void OnScanEnable(string _address);

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
        SystemManager.Instance.onSystemLoadTypeSwitch += SetService;
        SetService();
        barCodeReader = new BarcodeReader();
        vuforiaBehaviour = GetComponent<VuforiaBehaviour>();
        //StartCoroutine(InitializeCamera());
    }

    private IEnumerator InitializeCamera()
    {
        yield return new WaitForSeconds(1.25f);

        var isFrameFormatSet = CameraDevice.Instance.SetFrameFormat(Image.PIXEL_FORMAT.RGB888, true);
        Debug.Log(String.Format("FormatSet : {0}", isFrameFormatSet));

        var isAutoFocus = CameraDevice.Instance.SetFocusMode(CameraDevice.FocusMode.FOCUS_MODE_CONTINUOUSAUTO);
        if (!isAutoFocus)
        {
            CameraDevice.Instance.SetFocusMode(CameraDevice.FocusMode.FOCUS_MODE_NORMAL);
        }
        Debug.Log(String.Format("AutoFocus : {0}", isAutoFocus));
    }

    private void Update()
    {
        if (cameraInitialized)
        {
            if(!isFormatSet)
            {
                tempFormat = pixelFormat;
                CameraDevice.Instance.SetFrameFormat(tempFormat, false);
                if (CameraDevice.Instance.SetFrameFormat(tempFormat, true))
                {
                    isFormatSet = true;
                }
                else
                {
                    tempFormat = Image.PIXEL_FORMAT.UNKNOWN_FORMAT;
                }
            }

            try
            {
                var cameraFeed = CameraDevice.Instance.GetCameraImage(pixelFormat);
                if (cameraFeed == null)
                {
                    return;
                }
                var data = barCodeReader.Decode(cameraFeed.Pixels, cameraFeed.BufferWidth, cameraFeed.BufferHeight, RGBLuminanceSource.BitmapFormat.RGB24);
                if (data != null)
                {
                    if (data.Text.Length == 42 && data.Text.Substring(0, 2) == "0x")
                    {
                        if (onSetWalletAddress != null)
                        {
                            onSetWalletAddress.Invoke(data.Text);
                        }

                        if (onAddressScanScreenError != null)
                        {
                            onAddressScanScreenError.Invoke(0);
                        }
                    }
                    else
                    {
                        if (onAddressScanScreenError != null)
                        {
                            onAddressScanScreenError.Invoke(1);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Debug.LogError("VUFORIA QR ERROR: " + e.Message);
            }
        }
    }

    public void Confirm()
    {
        if (MenuManager.Instance.onGridMenuOpen != null)
        {
            MenuManager.Instance.onGridMenuOpen.Invoke();
        }

        StartCoroutine(IPCLoader.Instance.LoadToken(UIUpdater.Instance.walletAddressText.text));
    }

    public void StartQRReader()
    {
        var isAutoFocus = CameraDevice.Instance.SetFocusMode(CameraDevice.FocusMode.FOCUS_MODE_CONTINUOUSAUTO);
        if (!isAutoFocus)
        {
            CameraDevice.Instance.SetFocusMode(CameraDevice.FocusMode.FOCUS_MODE_NORMAL);
        }

        background.SetActive(false);
        vuforiaBehaviour.enabled = true;
        cameraInitialized = true;
        VuforiaRenderer.Instance.Pause(false);

        if (onSetWalletAddress != null)
        {
            onSetWalletAddress.Invoke("Wallet Address: (None Found)");
        }

        if (onAddressScanScreenError != null)
        {
            onAddressScanScreenError.Invoke(0);
        }
    }

    public void StopQRReader()
    {
        background.SetActive(true);
        cameraInitialized = false;
        VuforiaRenderer.Instance.Pause(true);
    }
}

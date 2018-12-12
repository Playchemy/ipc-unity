using System.Collections;
using UnityEngine.UI;
using UnityEngine;

public class AppState_InitScanQR : State
{
    public Vuforia.Image.PIXEL_FORMAT pixelFormat = Vuforia.Image.PIXEL_FORMAT.RGB888;
    public Text walletAddressText = null;
    public Text errorText = null;

    // State Functions =========================================================================================================
    protected override void RunOnce()
    {
        base.RunOnce();

        VuforiaQRReader.Instance.onAddressScanScreenError += ScanScreenError;
        VuforiaQRReader.Instance.onSetWalletAddress += SetWalletAddress;
    }

    public override void EnterState()
    {
        errorText.text = "";
        VuforiaQRReader.Instance.pixelFormat = pixelFormat;
        VuforiaQRReader.Instance.StartQRReader();
    }

    public override void UpdateState()
    {

    }

    public override void ExitState()
    {
        VuforiaQRReader.Instance.StopQRReader();
    }
    // =========================================================================================================================

    public void SetText(Text _addressText)
    {
        AppStateHelpers.SetText(_addressText);
    }

    private void SetWalletAddress(string _address)
    {
        walletAddressText.text = _address;
    }

    private void ScanScreenError(int _errorCode)
    {
        if (_errorCode == 0)
        {
            errorText.text = "";
        }
        else if (_errorCode == 1)
        {
            errorText.text = "ERROR: This is not a valid address!";
        }
        else if (_errorCode == 2)
        {
            errorText.text = "ERROR: This wallet doesn't have any IPCs!";
        }
    }
}

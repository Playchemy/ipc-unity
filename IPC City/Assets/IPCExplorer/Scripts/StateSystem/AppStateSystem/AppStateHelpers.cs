using System.Collections;
using UnityEngine.UI;
using UnityEngine;

public static class AppStateHelpers
{
    public static OnGridInitialized onGridInitialized;
    public delegate void OnGridInitialized(string _address);

    public static void SetID(InputField _IPCIDInputfield)
    {
        if(int.Parse(_IPCIDInputfield.text) < 1)
        {
            return;
        }

        AppStateManager.Instance.StartCoroutine(IPCLoader.Instance.DirectLoadIPCToCharacterSheet(int.Parse(_IPCIDInputfield.text)));
        AppStateManager.Instance.SetState(AppStateManager.AppState.InitCharacterSheet);
    }

    public static void SetInputfield(InputField _addressInputfield)
    {
        if (_addressInputfield.text.Length != 42)
        {
            return;
        }

        if (!_addressInputfield.text.Contains("0x"))
        {
            return;
        }

        LoadWallet(_addressInputfield.text);
    }

    public static void SetText(Text _addressText)
    {
        if(_addressText.text.Length != 42)
        {
            return;
        }

        if(!_addressText.text.Contains("0x"))
        {
            return;
        }

        LoadWallet(_addressText.text);
    }

    private static void LoadWallet(string _address)
    {
        AppStateManager.Instance.StartCoroutine(IPCLoader.Instance.LoadToken(_address));
        AppStateManager.Instance.SetState(AppStateManager.AppState.InitGrid);
        WalletManager.Instance.PushWallet(_address);

        if (onGridInitialized != null)
        {
            onGridInitialized.Invoke(_address);
        }
    }
}

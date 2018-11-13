using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IpcGetServiceTest : MonoBehaviour
{
    public bool DEBUG = true;
    private IpcGetService ipcGetService = null;
    private UIOrganizer uiOrganizer = null;

    private void Start()
    {
        ipcGetService = GetComponent<IpcGetService>();
        uiOrganizer = GetComponent<UIOrganizer>();
    }

    public void RunWalletEd()
    {
        ipcGetService.inputWalletID = "0x9e0cad443a4c3cf4ddce1a16d67cba5eef14bc8f";
        StartCoroutine(RunWallet());
    }

    public void RunWalletSherman()
    {
        ipcGetService.inputWalletID = "0x2fb80d6bb63f13eed2c1d5a01e2da1c0fe61082d";
        StartCoroutine(RunWallet());
    }

    public void RunWalletOmer()
    {
        ipcGetService.inputWalletID = "0x4383e10a9fd27fc6332a91e5398484cb7bc4da89";
        StartCoroutine(RunWallet());
    }

    public void RunWalletArmaan()
    {
        ipcGetService.inputWalletID = "0xd1433fF08c561B70f2B02a7b0EC75Fe9Eab95C86";
        StartCoroutine(RunWallet());
    }

    private IEnumerator RunWallet()
    {
        ipcGetService.GetOwnerTokens();
        yield return new WaitUntil(() => ipcGetService.TokensLoaded);
        //uiOrganizer.EnableScan();
        //uiOrganizer.InitiateOrganizationFromFirstPage();
        //MenuManager.Instance.OpenGridMenu(false);
    }
}

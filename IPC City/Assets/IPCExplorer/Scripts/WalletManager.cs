using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class WalletManager : MonoBehaviour
{
    public static WalletManager Instance;

    public List<Text> recentWalletList;

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
        IPCLoader.Instance.onTokenLoadingFinished += SaveWallets;
    }

    public void GetWallet()
    {
        if (PlayerPrefs.HasKey("Wallet1"))
        {
            recentWalletList[0].text = PlayerPrefs.GetString("Wallet1");
        }
        if (PlayerPrefs.HasKey("Wallet2"))
        {
            recentWalletList[1].text = PlayerPrefs.GetString("Wallet2");
        }
        if (PlayerPrefs.HasKey("Wallet3"))
        {
            recentWalletList[2].text = PlayerPrefs.GetString("Wallet3");
        }
        if (PlayerPrefs.HasKey("Wallet4"))
        {
            recentWalletList[3].text = PlayerPrefs.GetString("Wallet4");
        }

        SetButtonInteractivity();
    }

    private void SetButtonInteractivity()
    {
        if (string.IsNullOrEmpty(recentWalletList[0].text))
        {
            recentWalletList[0].transform.GetComponentInParent<Button>().interactable = false;
        }
        else
        {
            if(recentWalletList[0].transform.GetComponentInParent<Button>())
            {
                recentWalletList[0].transform.GetComponentInParent<Button>().interactable = true;
            }
        }

        if (string.IsNullOrEmpty(recentWalletList[1].text))
        {
            recentWalletList[1].transform.GetComponentInParent<Button>().interactable = false;
        }
        else
        {
            if (recentWalletList[1].transform.GetComponentInParent<Button>())
            {
                recentWalletList[1].transform.GetComponentInParent<Button>().interactable = true;
            }
        }

        if (string.IsNullOrEmpty(recentWalletList[2].text))
        {
            recentWalletList[2].transform.GetComponentInParent<Button>().interactable = false;
        }
        else
        {
            if (recentWalletList[2].transform.GetComponentInParent<Button>())
            {
                recentWalletList[2].transform.GetComponentInParent<Button>().interactable = true;
            }
        }

        if (string.IsNullOrEmpty(recentWalletList[3].text))
        {
            recentWalletList[3].transform.GetComponentInParent<Button>().interactable = false;
        }
        else
        {
            if (recentWalletList[3].transform.GetComponentInParent<Button>())
            {
                recentWalletList[3].transform.GetComponentInParent<Button>().interactable = true;
            }
        }
    }

    public void PushWallet(string _newWallet)
    {
        if(recentWalletList[0].text == _newWallet || recentWalletList[1].text == _newWallet || recentWalletList[2].text == _newWallet || recentWalletList[3].text == _newWallet)
        {
            return;
        }
        else
        {
            for (int i = 3; i >= 1; i--)
            {
                recentWalletList[i].text = recentWalletList[i - 1].text;
            }

            recentWalletList[0].text = _newWallet;
        }
    }

    private void SaveWallets(string _error)
    {
        PlayerPrefs.SetString("Wallet1", recentWalletList[0].text);
        PlayerPrefs.SetString("Wallet2", recentWalletList[1].text);
        PlayerPrefs.SetString("Wallet3", recentWalletList[2].text);
        PlayerPrefs.SetString("Wallet4", recentWalletList[3].text);
        PlayerPrefs.Save();
    }
}

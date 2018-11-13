using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class UI_Manager : MonoBehaviour {


    IPC_Stats ipc;

    public Text nameText;
    public Text ageText;
    public Text repText;
    public Text priceText;

    [Header("Stats")]
    public Text strText;
    public Text dexText;
    public Text intText;
    public Text conText;
    public Text luckText;

    public GameObject stats;
    public Text buttonText;

    private static UI_Manager _instance;

    public static UI_Manager Instance
    {
        get
        {
            return _instance;
        }
    }

    public void ShowStats()
    {
        if(stats.activeSelf)
        {
            stats.SetActive(false);
            buttonText.text = "Show stats >";
        }
        else
        {
            stats.SetActive(true);
            buttonText.text = "< Hide stats";
        }
    }

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }
    }

    private void Update()
    {
        if(ipc)
        transform.position = new Vector3(ipc.transform.position.x, 1, ipc.transform.position.z + 6.5f);
    }

    public void UpdateUI()
    {
        nameText.text = ipc.ipc_name.ToString();
        ageText.text = ipc.ipc_Age + " days old.";
        repText.text = "IPC ID: " + ipc.ipcID.ToString();
        priceText.text = "$" + ipc.price.ToString();

        if (!strText)
            return;

        strText.text = "Str: " + ipc.strength.ToString();
        dexText.text = "Dex: " + ipc.dexterity.ToString();
        intText.text = "Int: " + ipc.intelligence.ToString();
        conText.text = "Con: " + ipc.constitution.ToString();
        luckText.text = "Luck: " + ipc.luck.ToString();
    }

    public void HideUI()
    {
        ipc = null;
        transform.position = new Vector3(5000, 5000, 5000);
    }

    public void ClickedOn(IPC_Stats stats)
    {
        ipc = stats;
        UpdateUI();
    }
}

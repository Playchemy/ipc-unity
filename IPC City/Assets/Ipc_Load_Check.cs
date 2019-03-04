using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Ipc_Load_Check : MonoBehaviour 
{
    public GameObject loadCheck;
    DataInitializer initializer;
    public GameObject splashScreen;
    public Image dataLoadingFill;
    public Text dataLoadingText;


    void Start () 
	{
        initializer = GetComponent<DataInitializer>();
        //initializer.onDataLoadFinished += CheckLoaded;
        //loadCheck.SetActive(true);
        RunSplashScreen();
    }

    public void CheckLoaded()
    {
        //RunSplashScreen();
        //FindObjectOfType<IpcSpawnMenu>().CreateALL();
        splashScreen.SetActive(false);
        loadCheck.SetActive(false);
    }



    private void RunSplashScreen()
    {
        splashScreen.SetActive(true);

        /*
        if (SystemManager.Instance.ActiveSystemLoadType == SystemManager.SystemLoadType.OnDemand)
        {
            dataLoadingFill.transform.parent.GetComponent<Image>().CrossFadeAlpha(0f, 0.1f, true);
            dataLoadingFill.transform.GetComponent<Image>().CrossFadeAlpha(0f, 0.1f, true);
            dataLoadingText.CrossFadeAlpha(0f, 0.1f, true);
            //ipcLoadingText.CrossFadeAlpha(0f, 0.1f, true);
        }
        else
        {
            dataLoadingFill.transform.parent.GetComponent<Image>().CrossFadeAlpha(1f, 0.1f, true);
            dataLoadingFill.transform.GetComponent<Image>().CrossFadeAlpha(1f, 0.1f, true);
            dataLoadingText.CrossFadeAlpha(1f, 0.1f, true);
            //ipcLoadingText.CrossFadeAlpha(1f, 0.1f, true);
        }
        */
    }
}

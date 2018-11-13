using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    public static GridManager Instance;

    public GameObject ipcGrid = null;

    public int pageNumber = 1;
    public IpcGetService ipcGetServiceMain = null;

    public OnLoadingNewPage onLoadingNewPage;
    public delegate void OnLoadingNewPage();

    public OnPageNumberUpdate onPageNumberUpdate;
    public delegate void OnPageNumberUpdate(int _aPage);

    public OnPageButtonUpdate onPageButtonUpdate;
    public delegate void OnPageButtonUpdate(bool _leftButton, bool _rightButton);

    public OnStopAllCouroutines onStopAllCouroutines;
    public delegate void OnStopAllCouroutines();

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else if(Instance != this)
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        SystemManager.Instance.onSystemLoadTypeSwitch += SetService;
        SetService();
    }

    private void SetService()
    {
        if(SystemManager.Instance.ActiveSystemLoadType == SystemManager.SystemLoadType.OnDemand)
        {
            ipcGetServiceMain = SystemManager.Instance.GetOnDemandService();
        }
        else
        {
            ipcGetServiceMain = SystemManager.Instance.GetCacheService();
        }
    }

    public void InitiateGridFromFirstPage()
    {
        if(OwnsIPC())
        {
            if (onStopAllCouroutines != null)
            {
                onStopAllCouroutines.Invoke();
            }

            SetPageNumber(1);
            RunGrid();
        }
    }

    public void InitiateGridFromSamePage()
    {
        if (OwnsIPC())
        {
            if (onStopAllCouroutines != null)
            {
                onStopAllCouroutines.Invoke();
            }

            SetPageNumber(pageNumber);
            RunGrid();
        }
    }

    private void RunGrid()
    {
        QRReader.Instance.StopBarcode();
        GridFunctions.Instance.DisableGrid();
        GridFunctions.Instance.EnableCardTexts(ipcGetServiceMain);
        GridFunctions.Instance.EnableCardArts(ipcGetServiceMain);
        for (int i = 0; i < GridFunctions.Instance.chamberCount; i++) { GridFunctions.Instance.ChangeArtAlpha(i, 0f); }
        GridFunctions.Instance.EnableIPCCards();
        GridFunctions.Instance.CheckIfCanChangePage();
    }

    public void ChangePage(bool goToNext)
    {
        if(UIUpdater.Instance.gridMenuScreen.activeInHierarchy)
        {
            if (onLoadingNewPage != null)
            {
                onLoadingNewPage.Invoke();
            }

            if (onStopAllCouroutines != null)
            {
                onStopAllCouroutines.Invoke();
            }

            if (goToNext)
            {
                pageNumber++;
            }
            else
            {
                pageNumber--;
            }

            RunGrid();
            SetPageNumber(pageNumber);
        }
    }

    public void SetPageNumber(int _aPage)
    {
        pageNumber = _aPage;

        if(onPageNumberUpdate != null)
        {
            onPageNumberUpdate.Invoke(_aPage);
        }
    }

    private bool OwnsIPC()
    {
        if (ipcGetServiceMain.ownedIpcsIds.Count > 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}

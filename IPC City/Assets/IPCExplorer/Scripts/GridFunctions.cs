using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GridFunctions : MonoBehaviour
{
    public static GridFunctions Instance;

    public int chamberCount = 9;
    public GameObject chamberOnDemandPrefab = null;
    public GameObject chamberCachePrefab = null;

    public OnCreateChamber onCreateChamber;
    public delegate void OnCreateChamber(GameObject _chamber);

    public OnGridGenerationFinished onGridGenerationFinished;
    public delegate void OnGridGenerationFinished();

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

    public void Start()
    {
        GenerateGrid(chamberCount);
        SystemManager.Instance.onSystemLoadTypeSwitch += RegenerateGrid;
    }

    private void RegenerateGrid()
    {
        GenerateGrid(chamberCount);
    }

    private void GenerateGrid(int _newChamberCount)
    {
        foreach (Transform chamber in GridManager.Instance.ipcGrid.transform)
        {
            Destroy(chamber.gameObject);
        }

        IPCLoader.Instance.chamberIpcGetServices.Clear();
        for (int i = 0; i < _newChamberCount; i++)
        {
            if(SystemManager.Instance.ActiveSystemLoadType == SystemManager.SystemLoadType.OnDemand)
            {
                IpcGetService ipcGetService = Instantiate(chamberOnDemandPrefab, GridManager.Instance.ipcGrid.transform).GetComponent<IpcGetService>();

                IPCLoader.Instance.chamberIpcGetServices.Add(ipcGetService);
                ipcGetService.GetComponent<Button>().onClick.AddListener(delegate { if(onCreateChamber != null) onCreateChamber.Invoke(ipcGetService.gameObject); });
            }
            else
            {
                IpcGetService ipcGetService = Instantiate(chamberCachePrefab, GridManager.Instance.ipcGrid.transform).GetComponent<IpcGetService>();

                IPCLoader.Instance.chamberIpcGetServices.Add(ipcGetService);
                ipcGetService.GetComponent<Button>().onClick.AddListener(delegate { if (onCreateChamber != null) onCreateChamber.Invoke(ipcGetService.gameObject); });
            }
        }

        ScaleChambers();
        DisableGrid();

        if (onGridGenerationFinished != null)
        {
            onGridGenerationFinished.Invoke();
        }
    }

    private void OnClickOpenCharWindow(GameObject _clickedObject)
    {
        MenuManager.Instance.OpenCharacterWindow(_clickedObject);
    }

    private void ScaleChambers()
    {
        GridLayoutGroup grid = GridManager.Instance.ipcGrid.GetComponent<GridLayoutGroup>();

        if(chamberCount == 4)
        {
            grid.padding.left = 146;
            grid.padding.right = 0;
            grid.padding.top = 97;
            grid.padding.bottom = 0;
            grid.cellSize = new Vector2(394.2f, 682.67f);
            grid.spacing = new Vector2(24.6f, -112.2f);
        }
        else if(chamberCount == 9)
        {
            grid.padding.left = 59;
            grid.padding.right = 0;
            grid.padding.top = 47;
            grid.padding.bottom = 0;
            grid.cellSize = new Vector2(268.42f, 376.6f);
            grid.spacing = new Vector2(24.6f, 24.55f);
        }
    }

    public void EnableIPCCards()
    {
        Debug.Log("Card IPC(s) Enabled");

        if (GridManager.Instance.ipcGetServiceMain.ownedIpcsIds.Count > GridManager.Instance.pageNumber * chamberCount)
        {
            for (int i = (GridManager.Instance.pageNumber * chamberCount) - chamberCount; i < GridManager.Instance.pageNumber * chamberCount; i++)
            {
                int childIndex = ((i - ((GridManager.Instance.pageNumber * chamberCount) - chamberCount)) % (GridManager.Instance.pageNumber * chamberCount));
                StartCoroutine(IPCLoader.Instance.LoadEachIPC(childIndex, i));
            }
        }
        else
        {
            for (int i = (GridManager.Instance.pageNumber * chamberCount) - chamberCount; i < GridManager.Instance.ipcGetServiceMain.ownedIpcsIds.Count; i++)
            {
                int childIndex = ((i - ((GridManager.Instance.pageNumber * chamberCount) - chamberCount)) % GridManager.Instance.ipcGetServiceMain.ownedIpcsIds.Count);
                StartCoroutine(IPCLoader.Instance.LoadEachIPC(childIndex, i));
            }
        }
    }

    private void EnableCardIPC(int IPCID)
    {
        Debug.Log("Card IPC Enabled");
        StartCoroutine(IPCLoader.Instance.LoadOnlyOneIPC(IPCID));
    }

    public void DisableGrid()
    {
        for (int i = 0; i < chamberCount; i++)
        {
            GridManager.Instance.ipcGrid.transform.GetChild(i).GetComponent<Button>().interactable = false;
        }

        // Deactivate every grid chamber
        for (int i = 0; i < chamberCount; i++)
        {
            Color temp = GridManager.Instance.ipcGrid.transform.GetChild(i).GetComponent<Image>().color;
            temp.a = 0f;
            GridManager.Instance.ipcGrid.transform.GetChild(i).GetComponent<Image>().color = temp;

            for (int j = 0; j < GridManager.Instance.ipcGrid.transform.GetChild(i).childCount; j++)
            {
                // Name, Price and ID
                if (j < 3)
                {
                    GridManager.Instance.ipcGrid.transform.GetChild(i).GetChild(j).GetComponent<Text>().enabled = false;
                    GridManager.Instance.ipcGrid.transform.GetChild(i).GetChild(j).GetComponent<Text>().text = "";
                }
                else if (j == 3) // IPC body art
                {
                    GridManager.Instance.ipcGrid.transform.GetChild(i).GetChild(j).GetComponent<Image>().enabled = false;

                    for (int k = 0; k < GridManager.Instance.ipcGrid.transform.GetChild(i).GetChild(j).childCount; k++)
                    {
                        GridManager.Instance.ipcGrid.transform.GetChild(i).GetChild(j).GetChild(k).GetComponent<Image>().enabled = false;
                    }
                }
            }
        }

        if (GridManager.Instance.onPageButtonUpdate != null)
        {
            GridManager.Instance.onPageButtonUpdate.Invoke(false, false);
        }
    }

    public void EnableCardTexts(IpcGetService ipcGetService)
    {
        Debug.Log("Card Text(s) Enabled");

        if (ipcGetService.ownedIpcsIds.Count > GridManager.Instance.pageNumber * chamberCount)
        {
            // Activate every grid chamber
            for (int i = 0; i < chamberCount; i++)
            {
                Color temp = GridManager.Instance.ipcGrid.transform.GetChild(i).GetComponent<Image>().color;
                temp.a = 1f;
                GridManager.Instance.ipcGrid.transform.GetChild(i).GetComponent<Image>().color = temp;

                for (int j = 0; j < GridManager.Instance.ipcGrid.transform.GetChild(i).childCount; j++)
                {
                    // Name, Price and ID
                    if (j < 3)
                    {
                        GridManager.Instance.ipcGrid.transform.GetChild(i).GetChild(j).GetComponent<Text>().enabled = true;
                    }
                }
            }
        }
        else
        {
            for (int i = 0; i < chamberCount - ((GridManager.Instance.pageNumber * chamberCount) - ipcGetService.ownedIpcsIds.Count); i++)
            {
                if (GridManager.Instance.pageNumber * chamberCount > ipcGetService.ownedIpcsIds.Count)
                {
                    if (i == chamberCount - (GridManager.Instance.pageNumber * chamberCount - ipcGetService.ownedIpcsIds.Count))
                    {
                        break;
                    }
                }

                Color temp = GridManager.Instance.ipcGrid.transform.GetChild(i).GetComponent<Image>().color;
                temp.a = 1f;
                GridManager.Instance.ipcGrid.transform.GetChild(i).GetComponent<Image>().color = temp;

                for (int j = 0; j < GridManager.Instance.ipcGrid.transform.GetChild(i).childCount; j++)
                {
                    // Name, Price and ID
                    if (j < 3)
                    {
                        GridManager.Instance.ipcGrid.transform.GetChild(i).GetChild(j).GetComponent<Text>().enabled = true;
                    }
                }
            }
        }
    }

    public void EnableCardText()
    {
        Debug.Log("Card Text Enabled");

        Color temp = GridManager.Instance.ipcGrid.transform.GetChild(0).GetComponent<Image>().color;
        temp.a = 1f;
        GridManager.Instance.ipcGrid.transform.GetChild(0).GetComponent<Image>().color = temp;

        for (int j = 0; j < GridManager.Instance.ipcGrid.transform.GetChild(0).childCount; j++)
        {
            // Name, Price and ID
            if (j < 3)
            {
                GridManager.Instance.ipcGrid.transform.GetChild(0).GetChild(j).GetComponent<Text>().enabled = true;
            }
        }
    }

    public void EnableCardArts(IpcGetService ipcGetService)
    {
        Debug.Log("Card Art(s) Enabled");

        if (ipcGetService.ownedIpcsIds.Count > GridManager.Instance.pageNumber * chamberCount)
        {
            // Activate every grid chamber
            for (int i = 0; i < chamberCount; i++)
            {
                for (int j = 0; j < GridManager.Instance.ipcGrid.transform.GetChild(i).childCount; j++)
                {
                    GridManager.Instance.ipcGrid.transform.GetChild(i).GetChild(3).GetComponent<Image>().enabled = true;

                    for (int k = 0; k < GridManager.Instance.ipcGrid.transform.GetChild(i).GetChild(3).childCount; k++)
                    {
                        GridManager.Instance.ipcGrid.transform.GetChild(i).GetChild(3).GetChild(k).GetComponent<Image>().enabled = true;
                    }
                }
            }
        }
        else
        {
            for (int i = 0; i < chamberCount - ((GridManager.Instance.pageNumber * chamberCount) - ipcGetService.ownedIpcsIds.Count); i++)
            {
                if (GridManager.Instance.pageNumber * chamberCount > ipcGetService.ownedIpcsIds.Count)
                {
                    if (i == chamberCount - (GridManager.Instance.pageNumber * chamberCount - ipcGetService.ownedIpcsIds.Count))
                    {
                        break;
                    }
                }

                GridManager.Instance.ipcGrid.transform.GetChild(i).GetChild(3).GetComponent<Image>().enabled = true;

                for (int j = 0; j < GridManager.Instance.ipcGrid.transform.GetChild(i).GetChild(3).childCount; j++)
                {
                    GridManager.Instance.ipcGrid.transform.GetChild(i).GetChild(3).GetChild(j).GetComponent<Image>().enabled = true;
                }
            }
        }
    }

    public void EnableCardArt()
    {
        Debug.Log("Card Art Enabled");

        GridManager.Instance.ipcGrid.transform.GetChild(0).GetChild(3).GetComponent<Image>().enabled = true;

        for (int k = 0; k < GridManager.Instance.ipcGrid.transform.GetChild(0).GetChild(3).childCount; k++)
        {
            GridManager.Instance.ipcGrid.transform.GetChild(0).GetChild(3).GetChild(k).GetComponent<Image>().enabled = true;
        }
    }

    public void ChangeArtAlpha(int childIndex, float newAlpha)
    {
        Color temp = GridManager.Instance.ipcGrid.transform.GetChild(childIndex).GetChild(3).GetComponent<Image>().color;
        temp.a = newAlpha;
        GridManager.Instance.ipcGrid.transform.GetChild(childIndex).GetChild(3).GetComponent<Image>().color = temp;

        for (int j = 0; j < GridManager.Instance.ipcGrid.transform.GetChild(childIndex).GetChild(3).childCount; j++)
        {
            Color temp2 = GridManager.Instance.ipcGrid.transform.GetChild(childIndex).GetChild(3).GetChild(j).GetComponent<Image>().color;
            temp2.a = newAlpha;
            GridManager.Instance.ipcGrid.transform.GetChild(childIndex).GetChild(3).GetChild(j).GetComponent<Image>().color = temp2;
        }
    }

    public void CheckIfCanChangePage()
    {
        Debug.Log("Checking Page(s)");

        if (GridManager.Instance.pageNumber == 1 && GridManager.Instance.ipcGetServiceMain.ownedIpcsIds.Count > GridManager.Instance.pageNumber * chamberCount)
        {
            if(GridManager.Instance.onPageButtonUpdate != null)
            {
                GridManager.Instance.onPageButtonUpdate.Invoke(false, true);
            }
        }
        else if (GridManager.Instance.pageNumber == 1 && GridManager.Instance.ipcGetServiceMain.ownedIpcsIds.Count <= GridManager.Instance.pageNumber * chamberCount)
        {
            if (GridManager.Instance.onPageButtonUpdate != null)
            {
                GridManager.Instance.onPageButtonUpdate.Invoke(false, false);
            }
        }
        else if (GridManager.Instance.pageNumber != 1 && GridManager.Instance.ipcGetServiceMain.ownedIpcsIds.Count > GridManager.Instance.pageNumber * chamberCount)
        {
            if (GridManager.Instance.onPageButtonUpdate != null)
            {
                GridManager.Instance.onPageButtonUpdate.Invoke(true, true);
            }
        }
        else if (GridManager.Instance.pageNumber != 1 && GridManager.Instance.ipcGetServiceMain.ownedIpcsIds.Count <= GridManager.Instance.pageNumber * chamberCount)
        {
            if (GridManager.Instance.onPageButtonUpdate != null)
            {
                GridManager.Instance.onPageButtonUpdate.Invoke(true, false);
            }
        }
    }
}

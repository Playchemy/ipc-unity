using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System.IO;
using System;

using UnityEngine.UI;

[RequireComponent(typeof(IpcGetService))]
public class DataPuller : MonoBehaviour
{
    public static DataPuller Instance;

    public LoadMethod loadMethod;
    public enum LoadMethod { LoadFromGoogleSheets, LoadLocal }

    [Header("Google Sheet Info")]
    public string GoogleSheetID = "1AcC4wjb1fLd8lpw3SzU2nQfC05IlWcoNlsq8TqgZZi0";
    public string NotInterpretedGoogleSheetTabID = "2126091068";
    public string adminAccountsGoogleSheetTabID = "273410213";

    [Header("File Names")]
    public string NotInterpretedCSV_FileName = "NotInterpretedCSV";
    public string NotInterpretedCSV_Server_FileName = "ServerNotInterpretedCSV";
    public string AdminAccountsCSV_Server_FileName = "ServerAdminAccountsCSV";

    public GameObject serviceSlavePrefab = null;

    // Callbacks ---------------------------------------------------
    public OnPushInitialized onPushInitialized;
    public delegate void OnPushInitialized();

    public OnPullInitialized onPullInitialized;
    public delegate void OnPullInitialized();

    public OnSlaveLoaded onSlaveLoaded;
    public delegate void OnSlaveLoaded();

    public OnIPCsLoaded onIPCsLoaded;
    public delegate void OnIPCsLoaded();
    // -------------------------------------------------------------

    private string NotInterpretedCSV_FilePath_Complete = "";
    private string NotInterpretedCSV_Server_FilePath_Complete = "";
    private string AdminAccountCSV_Server_FilePath_Complete = "";

    private IpcGetService ipcGetService = null;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        ipcGetService = GetComponent<IpcGetService>();
    }

    private void Start()
    {
        SetPaths();
        if(SystemManager.Instance.disablePreLoad == false) Invoke("InitializeDatabaseLoad", 1f);
    }

    public void PullFromGoogleDatabase()
    {
        DataInitializer.Instance.ipcList.Clear();
        loadMethod = LoadMethod.LoadFromGoogleSheets;
        InitializeDatabaseLoad();

        if (onPullInitialized != null)
        {
            onPullInitialized.Invoke();
        }
    }

    public void PullLocally()
    {
        DataInitializer.Instance.ipcList.Clear();
        loadMethod = LoadMethod.LoadLocal;
        InitializeDatabaseLoad();
    }

    public void SlaveLoaded()
    {
        if (onSlaveLoaded != null)
        {
            onSlaveLoaded.Invoke();
        }
    }

    private void SetPaths()
    {
#if UNITY_EDITOR
        NotInterpretedCSV_FilePath_Complete = Application.dataPath + "/Resources/" + NotInterpretedCSV_FileName + ".csv";
        NotInterpretedCSV_Server_FilePath_Complete = Application.dataPath + "/Resources/" + NotInterpretedCSV_Server_FileName + ".csv";
        AdminAccountCSV_Server_FilePath_Complete = Application.dataPath + "/Resources/" + AdminAccountsCSV_Server_FileName + ".csv";

#elif UNITY_ANDROID
        NotInterpretedCSV_FilePath_Complete = Application.persistentDataPath + "/" + NotInterpretedCSV_FileName + ".csv";
        NotInterpretedCSV_Server_FilePath_Complete = Application.persistentDataPath + "/" + NotInterpretedCSV_Server_FileName + ".csv";
        AdminAccountCSV_Server_FilePath_Complete = Application.persistentDataPath + "/" + AdminAccountsCSV_Server_FileName + ".csv";
#elif UNITY_STANDALONE_WIN
        NotInterpretedCSV_FilePath_Complete = Application.persistentDataPath + NotInterpretedCSV_FileName + ".csv";
        NotInterpretedCSV_Server_FilePath_Complete = Application.persistentDataPath + NotInterpretedCSV_Server_FileName + ".csv";
        AdminAccountCSV_Server_FilePath_Complete = Application.persistentDataPath + AdminAccountsCSV_Server_FileName + ".csv";
#elif UNITY_STANDALONE_OSX
        NotInterpretedCSV_FilePath_Complete = Application.persistentDataPath + NotInterpretedCSV_FileName + ".csv";
        NotInterpretedCSV_Server_FilePath_Complete = Application.persistentDataPath + NotInterpretedCSV_Server_FileName + ".csv";
        AdminAccountCSV_Server_FilePath_Complete = Application.persistentDataPath + AdminAccountsCSV_Server_FileName + ".csv";
#endif
    }

    private void InitializeDatabaseLoad()
    {
        if (loadMethod == LoadMethod.LoadFromGoogleSheets)
        {
            DownloadGoogleSheet();
        }
        else if (loadMethod == LoadMethod.LoadLocal)
        {
            LoadFromNotInterpreterCSV();
        }
    }

    private void DownloadGoogleSheet()
    {
        StartCoroutine(DownloadCSVCoroutine(NotInterpretedGoogleSheetTabID, NotInterpretedCSV_Server_FileName, NotInterpretedCSV_Server_FilePath_Complete, true));
        StartCoroutine(DownloadCSVCoroutine(adminAccountsGoogleSheetTabID, AdminAccountsCSV_Server_FileName, AdminAccountCSV_Server_FilePath_Complete, false));
    }

    private void LoadFromNotInterpreterCSV()
    {
        if (!File.Exists(NotInterpretedCSV_FilePath_Complete))
        {
            Debug.Log(NotInterpretedCSV_FileName + ".csv File Doesn't Exist");
            return;
        }

        StreamReader reader = new StreamReader(NotInterpretedCSV_FilePath_Complete);
        string csvText = reader.ReadToEnd();
        string[,] grid = SplitCsvGrid(csvText);

        for (int i = 1; i < grid.GetLength(1) - 2; i++)
        {
            IPC_Data ipcInfo = new IPC_Data();

            ipcInfo.name = grid[0, i];
            ipcInfo.ipc_id = int.Parse(grid[0, i]);
            ipcInfo.ipc_name = grid[1, i];
            ipcInfo.ipc_owner = grid[2, i];
            ipcInfo.ipc_timeOfBirth = uint.Parse(grid[3, i]);
            ipcInfo.ipc_xp = uint.Parse(grid[4, i]);
            ipcInfo.ipc_dna = grid[5, i];
            ipcInfo.ipc_attributes = grid[6, i];
            ipcInfo.ipc_price = uint.Parse(grid[7, i]);

            if (ipcInfo.ipc_name == "\"")
            {
                ipcInfo.ipc_name = "";
            }
            if (ipcInfo.ipc_name == " ")
            {
                ipcInfo.ipc_name = "";
            }

            DataInitializer.Instance.ipcList.Add(ipcInfo);
        }

        if (onIPCsLoaded != null)
        {
            onIPCsLoaded.Invoke();
        }

        Debug.Log("Succesfully Loaded Local Copy: " + NotInterpretedCSV_FileName);
    }

    private void LoadNotInterpretedFromGoogleSheets(string _path)
    {
        StreamReader reader = new StreamReader(_path);
        string csvText = reader.ReadToEnd();
        string[,] grid = SplitCsvGrid(csvText);

        for (int i = 1; i < grid.GetLength(1) - 2; i++)
        {
            int shouldBreak = 0;
            if (!int.TryParse(grid[0, i], out shouldBreak))
            {
                break;
            }

            IPC_Data ipcInfo = new IPC_Data();

            ipcInfo.name = grid[0, i];
            ipcInfo.ipc_id = int.Parse(grid[0, i]);
            ipcInfo.ipc_name = grid[1, i];
            ipcInfo.ipc_owner = grid[2, i];
            ipcInfo.ipc_timeOfBirth = uint.Parse(grid[3, i]);
            ipcInfo.ipc_xp = uint.Parse(grid[4, i]);
            ipcInfo.ipc_dna = grid[5, i];
            ipcInfo.ipc_attributes = grid[6, i];
            ipcInfo.ipc_price = uint.Parse(grid[7, i]);

            if (ipcInfo.ipc_name == "\"")
            {
                ipcInfo.ipc_name = "";
            }
            if (ipcInfo.ipc_name == " ")
            {
                ipcInfo.ipc_name = "";
            }

            DataInitializer.Instance.ipcList.Add(ipcInfo);
        }

        if (onIPCsLoaded != null)
        {
            onIPCsLoaded.Invoke();
        }

        Debug.Log("Succesfully Loaded Google Sheet Copy: " + NotInterpretedCSV_Server_FileName);
    }

    private void LoadAdminAccountsFromGoogleSheets(string _path)
    {
        StreamReader reader = new StreamReader(_path);
        string csvText = reader.ReadToEnd();
        string[,] grid = SplitCsvGrid(csvText);

        DataInitializer.Instance.adminData.adminAccounts.Clear();

        for (int i = 1; i < grid.GetLength(1) - 1; i++)
        {
            DataInitializer.Instance.adminData.AddEntry(grid[0, i], int.Parse(grid[1, i]));
        }

        //if (onIPCsLoaded != null)
        //{
        //    onIPCsLoaded.Invoke();
        //}

        Debug.Log("Succesfully Loaded Google Sheet Copy: " + AdminAccountsCSV_Server_FileName);
    }

    private void SaveNotInterpretedToCSV()
    {
        string csvText = "ID,Name,Owner Address, Time of Birth,XP,DNA,Attributes,Price\n";

        for (int i = 0; i < DataInitializer.Instance.ipcList.Count; i++)
        {
            if (string.IsNullOrEmpty(DataInitializer.Instance.ipcList[i].ipc_name))
            {
                DataInitializer.Instance.ipcList[i].ipc_name = " ";
            }

            csvText += DataInitializer.Instance.ipcList[i].ipc_id + "," + "\"" + DataInitializer.Instance.ipcList[i].ipc_name + "\"" + "," + DataInitializer.Instance.ipcList[i].ipc_owner + "," + DataInitializer.Instance.ipcList[i].ipc_timeOfBirth + "," + DataInitializer.Instance.ipcList[i].ipc_xp + "," + DataInitializer.Instance.ipcList[i].ipc_dna + "," + DataInitializer.Instance.ipcList[i].ipc_attributes + "," + DataInitializer.Instance.ipcList[i].ipc_price + "\n";
        }

        File.WriteAllText(NotInterpretedCSV_FilePath_Complete, csvText);

        Debug.Log("Succesfully Saved: " + NotInterpretedCSV_FileName);
    }

    private IEnumerator DownloadCSVCoroutine(string _tabID, string _fileName, string _filePath, bool _loadNotInterpreted)
    {
        string url = "https://docs.google.com/spreadsheets/d/" + GoogleSheetID + "/export?format=csv";

        if (!string.IsNullOrEmpty(_tabID))
        {
            url += "&gid=" + _tabID;
        }

        WWWForm form = new WWWForm();
        WWW download = new WWW(url, form);

        yield return download;

        if (!string.IsNullOrEmpty(download.error))
        {
            Debug.Log("Error Downloading: " + download.error);
        }
        else
        {
            if (!string.IsNullOrEmpty(_fileName))
            {
                Debug.Log("Download Complete: " + _fileName);

                File.CreateText(_filePath).Dispose();
                
                using (TextWriter writer = new StreamWriter(_filePath, false))
                {
                    writer.Write(download.text);
                    writer.Close();
                }

                //File.WriteAllText(_filePath, download.text);

                if (_loadNotInterpreted)
                {
                    LoadNotInterpretedFromGoogleSheets(_filePath);
                }
                else
                {
                    LoadAdminAccountsFromGoogleSheets(_filePath);
                }
            }
            else
            {
                throw new Exception("Asset Name is Null");
            }
        }
    }

    private string[,] SplitCsvGrid(string csvText)
    {
        string[] lines = csvText.Split("\n"[0]);

        int width = 0;
        for (int i = 0; i < lines.Length; i++)
        {
            string[] row = SplitCsvLine(lines[i]);
            width = Mathf.Max(width, row.Length);
        }

        string[,] outputGrid = new string[width + 1, lines.Length + 1];
        for (int y = 0; y < lines.Length; y++)
        {
            string[] row = SplitCsvLine(lines[y]);
            for (int x = 0; x < row.Length; x++)
            {
                outputGrid[x, y] = row[x];
                outputGrid[x, y] = outputGrid[x, y].Replace("\"\"", "\"");
            }
        }

        return outputGrid;
    }

    private string[] SplitCsvLine(string line)
    {
        return (from System.Text.RegularExpressions.Match m in System.Text.RegularExpressions.Regex.Matches(line,
        @"(((?<x>(?=[,\r\n]+))|""(?<x>([^""]|"""")+)""|(?<x>[^,\r\n]+)),?)",
        System.Text.RegularExpressions.RegexOptions.ExplicitCapture)
                select m.Groups[1].Value).ToArray();
    }

    // FILTERS ================================================================================================================================================================================================================

    [Serializable]
    public class NotInterpretedData
    {
        public int id;
        public string name;
        public string owner;
        public uint timeOfBirth;
        public uint xp;
        public string dna;
        public string attributes;
        public uint price;
    }

    [Serializable]
    public class InterpretedData
    {
        public int ipcId = 0;
        public string ipcName = null;
        public string timeOfBirth = null;
        public string xp = null;
        public string explosive = null;
        public string sustained = null;
        public string tolerance = null;
        public string strength = null;
        public string speed = null;
        public string precision = null;
        public string reaction = null;
        public string dexterity = null;
        public string memory = null;
        public string calculus = null;
        public string simulation = null;
        public string intelligence = null;
        public string healing = null;
        public string fortitude = null;
        public string growth = null;
        public string constitution = null;
        public string luck = null;
        public string race = null;
        public string gender = null;
        public string height = null;
        public string handedness = null;
        public string price = null;
        public string publicAddress = null;

        public InterpretedData(int _ipcId, string _ipcName, string _timeOfBirth, string _xp, string _explosive, string _sustained, string _tolerance, string _strength, string _speed, string _precision, string _reaction,
                               string _dexterity, string _memory, string _calculus, string _simulation, string _intelligence, string _healing, string _fortitude, string _growth, string _constitution, string _luck, string _race,
                               string _gender, string _height, string _handedness, string _price, string _publicAddress)
        {
            ipcId = _ipcId;
            ipcName = _ipcName;
            timeOfBirth = _timeOfBirth;
            xp = _xp;
            explosive = _explosive;
            sustained = _sustained;
            tolerance = _tolerance;
            strength = _strength;
            speed = _speed;
            precision = _precision;
            reaction = _reaction;
            dexterity = _dexterity;
            memory = _memory;
            calculus = _calculus;
            simulation = _simulation;
            intelligence = _intelligence;
            healing = _healing;
            fortitude = _fortitude;
            growth = _growth;
            constitution = _constitution;
            luck = _luck;
            race = _race;
            gender = _gender;
            height = _height;
            handedness = _handedness;
            price = _price;
            publicAddress = _publicAddress;
        }
    }
}

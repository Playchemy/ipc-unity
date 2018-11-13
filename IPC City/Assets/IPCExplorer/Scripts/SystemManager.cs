using UnityEngine;
using System.Collections;

public class SystemManager : MonoBehaviour
{
    public static SystemManager Instance;

    public bool ignorePlayerPrefs;
    public SystemLoadType ActiveSystemLoadType;
    public enum SystemLoadType { OnDemand, Cache }
    public GameObject onDemandSystem = null;
    public GameObject cacheSystem = null;

    public OnSystemLoadStart onSystemLoadStart;
    public delegate void OnSystemLoadStart();

    public OnSystemLoadTypeSwitch onSystemLoadTypeSwitch;
    public delegate void OnSystemLoadTypeSwitch();

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

        SetStartUpdateMethod();
    }

    private void SetStartUpdateMethod()
    {
        if (ignorePlayerPrefs)
            return;

        if (PlayerPrefs.HasKey("UpdateMethod"))
        {
            if(PlayerPrefs.GetInt("UpdateMethod") == 0)
            {
                ActiveSystemLoadType = SystemLoadType.OnDemand;
            }
            else if(PlayerPrefs.GetInt("UpdateMethod") == 1)
            {
                ActiveSystemLoadType = SystemLoadType.Cache;
            }
        }
        else
        {
            PlayerPrefs.SetInt("UpdateMethod", 0);
        }

        if (onSystemLoadTypeSwitch != null)
        {
            onSystemLoadTypeSwitch.Invoke();
        }
    }

    public void SwitchSystemLoadType()
    {
        if (ActiveSystemLoadType == SystemLoadType.OnDemand)
        {
            ActiveSystemLoadType = SystemLoadType.Cache;
            if(DataInitializer.Instance.ipcList.Count == 0)
            {
                DataInitializer.Instance.UpdateDatabase();
            }
            PlayerPrefs.SetInt("UpdateMethod", 1);
        }
        else
        {
            ActiveSystemLoadType = SystemLoadType.OnDemand;
            PlayerPrefs.SetInt("UpdateMethod", 0);
        }

        if (onSystemLoadTypeSwitch != null)
        {
            onSystemLoadTypeSwitch.Invoke();
        }
    }

    public IpcGetService GetOnDemandService()
    {
        return onDemandSystem.GetComponent<IpcGetService>();
    }

    public IpcGetServiceFromCache GetCacheService()
    {
        return cacheSystem.GetComponent<IpcGetServiceFromCache>();
    }

    public Interpreter GetOnDemandInterpreter()
    {
        return onDemandSystem.GetComponent<Interpreter>();
    }

    public InterpretFromCache GetCacheInterpreter()
    {
        return cacheSystem.GetComponent<InterpretFromCache>();
    }
}

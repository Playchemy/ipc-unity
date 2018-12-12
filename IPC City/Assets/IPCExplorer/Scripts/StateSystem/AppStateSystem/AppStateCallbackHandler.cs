using UnityEngine;
using System.Collections;

public class AppStateCallbackHandler : MonoBehaviour
{
    public static AppStateCallbackHandler Instance;

    public OnStopAllCouroutines onStopAllCouroutines;

    public delegate void OnStopAllCouroutines();

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
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class IPC_Controller : MonoBehaviour 
{
    public GameObject selectedIPC;
    public Animator selectedAnim;
    public NavMeshAgent selectedAgent;
    public static IPC_Controller Instance;
    public LayerMask mask;
    public GameObject waypoint;

    public GameObject selectedIPC_Circle;

    public EventSystem _eventSystem;

    public Toggle followCameraToggle;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    public void UnselectIPC()
    {
        selectedIPC = null;
        selectedAnim = null;
        selectedAgent = null;
        selectedIPC_Circle.SetActive(false);
    }

    void OnIPCSelected(GameObject chosen)
    {
        if(selectedIPC)
        {
            selectedIPC.GetComponent<CharMovement>().manualControl = false;
            selectedIPC.GetComponent<CharMovement>().FindNewTarget();
        }

        selectedIPC = chosen;

        selectedIPC_Circle.SetActive(true);
        


        selectedIPC.GetComponent<CharMovement>().manualControl = true;

        selectedAnim = selectedIPC.GetComponent<Animator>();
        selectedAgent = selectedIPC.GetComponent<NavMeshAgent>();
        //selectedAgent.isStopped = true;
        selectedAgent.ResetPath();
    }


    public void SelectIPC(GameObject chosenIPC)
    {
        OnIPCSelected(chosenIPC);
        chosenIPC.GetComponent<CharMovement>().EnableIPC();
    }

    void MoveToDestination () 
	{
        Vector3 targetLoc;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;


        if (Physics.Raycast(ray, out hit, Mathf.Infinity, mask))
        {
            targetLoc = hit.point;
            waypoint.transform.position = hit.point;

            Vector3 direc = targetLoc - Camera.main.transform.position;
            Debug.DrawRay(Camera.main.transform.position, direc, Color.blue, 5f);

            selectedIPC.GetComponent<CharMovement>().GoToTarget(hit.point);
        }
    }


    void Update()
    {
        if(selectedIPC)
        {
            if(Input.GetMouseButton(0))
            {
                MoveToDestination();
            }

            selectedIPC_Circle.transform.position = selectedIPC.transform.position + new Vector3(0, 0, -1.1f);

            if(followCameraToggle)
            {
                if(followCameraToggle.isOn)
                {
                    Camera.main.transform.position = new Vector3(selectedIPC.transform.position.x, Camera.main.transform.position.y, selectedIPC.transform.position.z);
                }
            }

        }
    }
}

using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.EventSystems;
using TMPro;

public class ManualCharacterControllerManager : MonoBehaviour
{
    [Header("Manual Control")]
    public bool manualControl = false;
    public int stepDistance = 1;
    public float comfortZoneVerticalSwipe = 50; // the vertical swipe will have to be inside a 50 pixels horizontal boundary
    public float comfortZoneHorizontalSwipe = 50; // the horizontal swipe will have to be inside a 50 pixels vertical boundary

    [Header("Debug UI")]
    public Slider stepDistanceSlider = null;
    public Slider comfortZoneVerticalSwipeSlider = null;
    public Slider comfortZoneHorizontalSwipeSlider = null;

    private bool isSwipe = false;
    private Vector2 pointerDownPosition;
    private Vector2 pointerUpPosition;
    private ManualCharacterController closest;
    private ManualCharacterController[] mcc;

    private float minSwipeDistance = 14f; // the swipe distance will have to be longer than this for it to be considered a swipe
    private float startTime;
    private float maxSwipeTime = 2f;
    private Vector2 startPos;

    private void Start()
    {
        if(manualControl)
        {
            CharController[] charController = FindObjectsOfType<CharController>();

            foreach (CharController cc in charController)
            {
                cc.gameObject.AddComponent<ManualCharacterController>().SetNewTarget(cc.transform.localPosition);
                Destroy(cc);
            }
        }
    }

    private void Update()
    {
        if (Input.touchCount > 0)
        {
            if(Input.touchCount == 3)
            {
                manualControl = !manualControl;
            }

            if (!manualControl)
                return;

            Touch touch = Input.touches[0];

            switch (touch.phase)
            {
                case TouchPhase.Began:
                    FindClosestIPC();
                    startPos = touch.position;
                    startTime = Time.time;
                    break;

                case TouchPhase.Ended:
                    float swipeTime = Time.time - startTime;
                    float swipeDist = (touch.position - startPos).magnitude;

                    if ((Mathf.Abs(touch.position.x - startPos.x)) < comfortZoneVerticalSwipe && (swipeTime < maxSwipeTime) && (swipeDist > minSwipeDistance) && Mathf.Sign(touch.position.y - startPos.y) > 0)
                    {
                        GiveTargetToIPC(2);
                    }
                    else if ((Mathf.Abs(touch.position.x - startPos.x)) < comfortZoneVerticalSwipe && (swipeTime < maxSwipeTime) && (swipeDist > minSwipeDistance) && Mathf.Sign(touch.position.y - startPos.y) < 0)
                    {
                        GiveTargetToIPC(3);
                    }
                    else if ((Mathf.Abs(touch.position.y - startPos.y)) < comfortZoneHorizontalSwipe && (swipeTime < maxSwipeTime) && (swipeDist > minSwipeDistance) && Mathf.Sign(touch.position.x - startPos.x) < 0)
                    {
                        GiveTargetToIPC(0);
                    }
                    else if ((Mathf.Abs(touch.position.y - startPos.y)) < comfortZoneHorizontalSwipe && (swipeTime < maxSwipeTime) && (swipeDist > minSwipeDistance) && Mathf.Sign(touch.position.x - startPos.x) > 0)
                    {
                        GiveTargetToIPC(1);
                    }
                    break;
            }
        }

        if (!manualControl)
            return;

#if (UNITY_EDITOR || UNITY_STANDALONE)

        Vector2 mousePosition = Input.mousePosition;

            if (Input.GetMouseButtonDown(0))
            {
                FindClosestIPC();

                startPos = mousePosition;
                startTime = Time.time;
            }

            if (Input.GetMouseButtonUp(0))
            {
                float swipeTime = Time.time - startTime;
                float swipeDist = (mousePosition - startPos).magnitude;

                if ((Mathf.Abs(mousePosition.x - startPos.x)) < comfortZoneVerticalSwipe && (swipeTime < maxSwipeTime) && (swipeDist > minSwipeDistance) && Mathf.Sign(mousePosition.y - startPos.y) > 0)
                {
                    GiveTargetToIPC(2);
                }
                else if ((Mathf.Abs(mousePosition.x - startPos.x)) < comfortZoneVerticalSwipe && (swipeTime < maxSwipeTime) && (swipeDist > minSwipeDistance) && Mathf.Sign(mousePosition.y - startPos.y) < 0)
                {
                    GiveTargetToIPC(3);
                }
                else if ((Mathf.Abs(mousePosition.y - startPos.y)) < comfortZoneHorizontalSwipe && (swipeTime < maxSwipeTime) && (swipeDist > minSwipeDistance) && Mathf.Sign(mousePosition.x - startPos.x) < 0)
                {
                    GiveTargetToIPC(0);
                }
                else if ((Mathf.Abs(mousePosition.y - startPos.y)) < comfortZoneHorizontalSwipe && (swipeTime < maxSwipeTime) && (swipeDist > minSwipeDistance) && Mathf.Sign(mousePosition.x - startPos.x) > 0)
                {
                    GiveTargetToIPC(1);
                }
            }
#endif

            // Update Debug UI
            stepDistance = (int)stepDistanceSlider.value;
            stepDistanceSlider.transform.parent.Find("Value").GetComponent<TextMeshProUGUI>().text = stepDistanceSlider.value.ToString();
            comfortZoneVerticalSwipeSlider.transform.parent.Find("Value").GetComponent<TextMeshProUGUI>().text = comfortZoneVerticalSwipeSlider.value.ToString();
            comfortZoneHorizontalSwipeSlider.transform.parent.Find("Value").GetComponent<TextMeshProUGUI>().text = comfortZoneHorizontalSwipeSlider.value.ToString();
            comfortZoneVerticalSwipe = comfortZoneVerticalSwipeSlider.value;
            comfortZoneHorizontalSwipe = comfortZoneHorizontalSwipeSlider.value;
    }

    private void FindClosestIPC()
    {
        pointerDownPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        closest = GetClosestIPC(FindObjectsOfType<ManualCharacterController>(), pointerDownPosition);
        closest.onManualControl = true;
    }

    private void GiveTargetToIPC(int direction)
    {
        if (closest.onManualControl)
        {
            // Left drag
            if (direction == 0)
            {
                closest.SetNewTarget(new Vector2(closest.transform.position.x - stepDistance, closest.transform.position.y));
            }
            else if (direction == 1) // Right drag
            {
                closest.SetNewTarget(new Vector2(closest.transform.position.x + stepDistance, closest.transform.position.y));
            }
            else if (direction == 2) // Upward drag
            {
                closest.SetNewTarget(new Vector2(closest.transform.position.x, closest.transform.position.y + stepDistance));
            }
            else if (direction == 3) // Downward drag
            {
                closest.SetNewTarget(new Vector2(closest.transform.position.x, closest.transform.position.y - stepDistance));
            }
            else
            {
                Debug.Log("...");
            }
        }
    }

    private ManualCharacterController GetClosestIPC(ManualCharacterController[] ipcs, Vector2 mousePosition)
    {
        ManualCharacterController closestIPC = null;
        float closestDistanceSqr = Mathf.Infinity;
        foreach (ManualCharacterController potentialTarget in ipcs)
        {
            Vector2 directionToTarget = (Vector2)potentialTarget.transform.position - mousePosition;
            float dSqrToTarget = directionToTarget.sqrMagnitude;
            if (dSqrToTarget < closestDistanceSqr)
            {
                closestDistanceSqr = dSqrToTarget;
                closestIPC = potentialTarget;
            }
        }

        return closestIPC;
    }
}

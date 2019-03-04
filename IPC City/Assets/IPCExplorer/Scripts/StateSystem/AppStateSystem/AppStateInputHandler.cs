using UnityEngine;
using System.Collections;

public class AppStateInputHandler : MonoBehaviour
{
    public static AppStateInputHandler Instance;

    public OnLoadNext onLoadNext;
    public OnLoadPrevious onLoadPrevious;

    public delegate void OnLoadNext();
    public delegate void OnLoadPrevious();

    private float dragDistance;
    private Vector3 firstTouchPosition;
    private Vector3 lastTouchPosition;

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
        dragDistance = Screen.height * 15 / 100;
    }

    private void Update()
    {
        if (Input.touchCount == 1)
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began)
            {
                firstTouchPosition = touch.position;
                lastTouchPosition = touch.position;
            }
            else if (touch.phase == TouchPhase.Moved)
            {
                lastTouchPosition = touch.position;
            }
            else if (touch.phase == TouchPhase.Ended)
            {
                lastTouchPosition = touch.position;

                if (Mathf.Abs(lastTouchPosition.x - firstTouchPosition.x) > dragDistance || Mathf.Abs(lastTouchPosition.y - firstTouchPosition.y) > dragDistance)
                {
                    if (Mathf.Abs(lastTouchPosition.x - firstTouchPosition.x) > Mathf.Abs(lastTouchPosition.y - firstTouchPosition.y))
                    {
                        if (lastTouchPosition.x > firstTouchPosition.x)
                        {
                            if (onLoadPrevious != null)
                            {
                                onLoadPrevious.Invoke();
                            }
                        }
                        else
                        {
                            if (onLoadNext != null)
                            {
                                onLoadNext.Invoke();
                            }
                        }
                    }
                }
            }
        }
    }
}

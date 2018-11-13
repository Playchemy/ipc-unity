using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class SceneSwapper : MonoBehaviour
{
    public GameObject menu = null;

    public Vector2 startPos;

    private static SceneSwapper sceneSwapperInstance;

    private void Awake()
    {
        DontDestroyOnLoad(this);

        if (sceneSwapperInstance == null)
        {
            sceneSwapperInstance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);

        if(sceneName == "Scene1")
        {
            Screen.orientation = ScreenOrientation.LandscapeLeft;
        }
        else if(sceneName == "IPCReader")
        {
            Screen.orientation = ScreenOrientation.Portrait;
        }
        else if (sceneName == "Scene1")
        {
            Screen.orientation = ScreenOrientation.Portrait;
        }

        ResetPosition();
    }

    private Vector3 fp;   //First touch position
    private Vector3 lp;   //Last touch position
    private float dragDistance;  //minimum distance for a swipe to be registered

    private void Start()
    {
        dragDistance = Screen.height * 15 / 100; //dragDistance is 15% height of the screen
        startPos = menu.transform.position;
    }

    private void Update()
    {
        if (Input.touchCount == 1) // user is touching the screen with a single touch
        {
            Touch touch = Input.GetTouch(0); // get the touch
            if (touch.phase == TouchPhase.Began) //check for the first touch
            {
                fp = touch.position;
                lp = touch.position;
            }
            else if (touch.phase == TouchPhase.Moved) // update the last position based on where they moved
            {
                lp = touch.position;
            }
            else if (touch.phase == TouchPhase.Ended) //check if the finger is removed from the screen
            {
                lp = touch.position;  //last touch position. Ommitted if you use list

                //Check if drag distance is greater than 20% of the screen height
                if (Mathf.Abs(lp.x - fp.x) > dragDistance || Mathf.Abs(lp.y - fp.y) > dragDistance)
                {//It's a drag
                 //check if the drag is vertical or horizontal
                    if (Mathf.Abs(lp.x - fp.x) > Mathf.Abs(lp.y - fp.y))
                    {   //If the horizontal movement is greater than the vertical movement...
                        if ((lp.x > fp.x))  //If the movement was to the right)
                        {   //Right swipe
                            Debug.Log("Right Swipe");
                        }
                        else
                        {   //Left swipe
                            Debug.Log("Left Swipe");
                        }
                    }
                    else
                    {   //the vertical movement is greater than the horizontal movement
                        if (lp.y > fp.y)  //If the movement was up
                        {   //Up swipe
                            Debug.Log("Up Swipe");
                            ResetPosition();
                        }
                        else
                        {   //Down swipe
                            Debug.Log("Down Swipe");
                            Move();
                        }
                    }
                }
                else
                {   //It's a tap as the drag distance is less than 20% of the screen height
                    Debug.Log("Tap");
                }
            }
        }
    }

    private void Move()
    {
        menu.transform.DOMoveY(1700, 1f);
    }

    public void ResetPosition()
    {
        menu.transform.DOMoveY(startPos.y, 1f);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{

    public Transform target;
    public float smoothTime = 0.3f;

    private Vector3 velocity = Vector3.zero;

    void Update()
    {
        Vector3 goalPos = target.position;
        goalPos.z = -50;

        //goalPos.y = transform.position.y;
        //transform.position = Vector3.SmoothDamp(transform.position, goalPos, ref velocity, smoothTime);

        transform.Translate(Input.GetAxis("Horizontal") * 20 * Time.deltaTime, Input.GetAxis("Vertical") * 20 * Time.deltaTime, 0);

        if(transform.position.x > 500)
        {
            transform.position = new Vector3(29.6f, transform.position.y, -50);
        }
        if (transform.position.x < -500)
        {
            transform.position = new Vector3(-29.6f, transform.position.y, -50);
        }
        if (transform.position.y > 500)
        {
            transform.position = new Vector3(transform.position.x, 13.8f, -50);
        }
        if (transform.position.y < -500)
        {
            transform.position = new Vector3(transform.position.x, -13.8f, -50);
        }

        Camera.main.orthographicSize -= Input.mouseScrollDelta.y;

        if(Camera.main.orthographicSize < 3)
        {
            Camera.main.orthographicSize = 3;
        }

        if (Camera.main.orthographicSize < 3)
        {
            Camera.main.orthographicSize = 3;
        }
        if (Camera.main.orthographicSize > 80)
        {
            Camera.main.orthographicSize = 80;
        }

    }
}

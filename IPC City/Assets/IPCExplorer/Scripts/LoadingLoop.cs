using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class LoadingLoop : MonoBehaviour
{
    private RectTransform loadingImage = null;
    private float rotateSpeed = 500f;

    private void Start()
    {
        loadingImage = GetComponent<RectTransform>();
    }

    private void OnEnable()
    {
        if(loadingImage)
        {
            loadingImage.rotation = new Quaternion(0f, 0f, 0f, 0f);
        }
    }

    private void Update()
    {
        loadingImage.Rotate(0f, 0f, rotateSpeed * Time.deltaTime);
    }
}

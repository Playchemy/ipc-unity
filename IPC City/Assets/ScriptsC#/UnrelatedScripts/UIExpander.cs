using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIExpander : MonoBehaviour
{
    public RectTransform leftPanel = null;
    public RectTransform rightPanel = null;

    public float speed = 10f;
    private bool expanded = false;

    private float leftDiff = 0f;
    private float rightDiff = 0f;

    private void Start()
    {
        RectTransform thisTransform = GetComponent<RectTransform>();

        leftDiff = leftPanel.offsetMin.x - leftPanel.offsetMax.x;
        leftPanel.offsetMin = new Vector2(thisTransform.offsetMin.x, thisTransform.offsetMin.y);
        leftPanel.offsetMax = new Vector2(thisTransform.offsetMax.x - leftDiff, thisTransform.offsetMax.y);

        rightDiff = rightPanel.offsetMin.x - rightPanel.offsetMax.x;
        rightPanel.offsetMin = new Vector2(thisTransform.offsetMax.x + rightDiff, thisTransform.offsetMax.y);
        rightPanel.offsetMax = new Vector2(thisTransform.offsetMin.x, thisTransform.offsetMin.y);
    }

    public void Expand()
    {
        if(expanded == false)
        {
            expand = true;

            GetComponent<UnityEngine.UI.Button>().interactable = false;
            expanded = true;
        }
        else
        {
            collapse = true;

            GetComponent<UnityEngine.UI.Button>().interactable = false;
            expanded = false;
        }
    }

    private bool expand = false;
    private bool collapse = false;

    private float waitLockTimer = 0f;

    private void Update()
    {
        if(expand)
        {
            RectTransform thisTransform = GetComponent<RectTransform>();

            leftPanel.offsetMin = Vector2.Lerp(leftPanel.offsetMin, new Vector2(thisTransform.offsetMin.x + leftDiff, thisTransform.offsetMin.y), Time.deltaTime * speed);
            leftPanel.offsetMax = Vector2.Lerp(leftPanel.offsetMax, thisTransform.offsetMin, Time.deltaTime * speed);

            rightPanel.offsetMin = Vector2.Lerp(rightPanel.offsetMin, thisTransform.offsetMax, Time.deltaTime * speed);
            rightPanel.offsetMax = Vector2.Lerp(rightPanel.offsetMax, new Vector2(thisTransform.offsetMax.x - rightDiff, thisTransform.offsetMax.y), Time.deltaTime * speed);
        }

        if (collapse)
        {
            RectTransform thisTransform = GetComponent<RectTransform>();

            leftPanel.offsetMin = Vector2.Lerp(leftPanel.offsetMin, new Vector2(thisTransform.offsetMin.x, thisTransform.offsetMin.y), Time.deltaTime * speed);
            leftPanel.offsetMax = Vector2.Lerp(leftPanel.offsetMax, new Vector2(thisTransform.offsetMax.x - leftDiff, thisTransform.offsetMax.y), Time.deltaTime * speed);

            rightPanel.offsetMin = Vector2.Lerp(rightPanel.offsetMin, new Vector2(thisTransform.offsetMax.x + rightDiff, thisTransform.offsetMax.y), Time.deltaTime * speed);
            rightPanel.offsetMax = Vector2.Lerp(rightPanel.offsetMax, new Vector2(thisTransform.offsetMin.x, thisTransform.offsetMin.y), Time.deltaTime * speed);
        }

        if(GetComponent<UnityEngine.UI.Button>().interactable == false)
        {
            waitLockTimer += Time.deltaTime;

            if(waitLockTimer >= 1f)
            {
                expand = false;
                collapse = false;

                GetComponent<UnityEngine.UI.Button>().interactable = true;
                waitLockTimer = 0f;
            }
        }
    }
}

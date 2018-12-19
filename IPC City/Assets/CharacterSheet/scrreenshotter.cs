using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scrreenshotter : MonoBehaviour
{


    public Vector2 UpperLeft, LowerRight, tempUL = Vector2.zero, tempLR = Vector2.zero;

    private Rect LassoRect;
    bool Lassoing = false;


    private void OnGUI()
    {
        // Left mouse button is pressed down. Record Upper left corner.
        if (Event.current.type == EventType.MouseDown && Event.current.button == 0)
        {
            Lassoing = true;
            tempUL = Event.current.mousePosition;
        }

        // Mouse is dragged while the left mouse button is held down. Continuously update lower right corner
        if (Event.current.type == EventType.MouseDrag && Event.current.button == 0 && Lassoing)
        {
            // Swap variable coordinates where necessary, to account for "negative-area" rectangles. This occurs if the user
            // left-clicks somewhere, then drags left instead of right
            tempLR = Event.current.mousePosition;

            UpperLeft = new Vector2(Mathf.Min(tempUL.x, tempLR.x), Mathf.Min(tempUL.y, tempLR.y));
            LowerRight = new Vector2(Mathf.Max(tempUL.x, tempLR.x), Mathf.Max(tempUL.y, tempLR.y));

            LassoRect = new Rect(UpperLeft.x, UpperLeft.y, LowerRight.x - UpperLeft.x, LowerRight.y - UpperLeft.y);
        }
    }
 

    private void Start()
    {
            LassoRect = new Rect(UpperLeft.x, UpperLeft.y, LowerRight.x - UpperLeft.x, LowerRight.y - UpperLeft.y);
    }

    [ContextMenu("CopPositions")]
    void CopyLocations()
    {
        GetComponent<Screenshot>().UpperLeft = UpperLeft;
        GetComponent<Screenshot>().LowerRight = LowerRight;
    }

    public void CaptureScreen()
    {
            StartCoroutine(Screenshott());
    }

    IEnumerator Screenshott()
    {
        yield return new WaitForEndOfFrame();
        int superSamplingFactor = 1;
        Texture2D snapShot = new Texture2D((int)LassoRect.width * superSamplingFactor, (int)LassoRect.height * superSamplingFactor, TextureFormat.RGB24, false);
        //RenderTexture snapShotRT = new RenderTexture(Screen.width * superSamplingFactor, Screen.height * superSamplingFactor, 24, RenderTextureFormat.ARGB32); // We're gonna render the entire screen into this
        //RenderTexture.active = snapShotRT;
        //Camera.main.targetTexture = snapShotRT;
        //Camera.main.Render();
        Rect lassoRectSS = new Rect(LassoRect.xMin * superSamplingFactor, LassoRect.yMin * superSamplingFactor, LassoRect.width * superSamplingFactor, LassoRect.height * superSamplingFactor);
        snapShot.ReadPixels(lassoRectSS, 0, 0);
        snapShot.Apply();

        var bytes = snapShot.EncodeToPNG();
        Destroy(snapShot);

        System.IO.File.WriteAllBytes(Application.dataPath + "CharacterSheet_IPC_NEW" + ".png", bytes);
            Debug.Log("Captured");

        //RenderTexture.active = null;
        //Camera.main.targetTexture = null;
    }
}

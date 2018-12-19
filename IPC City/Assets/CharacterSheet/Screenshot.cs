using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Screenshot : MonoBehaviour {

    // Use this for initialization
    public UnityEvent hideEvent;
    public UnityEvent unhideEvent;

    public Vector2 UpperLeft, LowerRight;
    private Rect LassoRect;

int resWidth = Screen.width;
int resHeight = Screen.height;

    public int superSamplingFactor;
    private void Start()
    {
        LassoRect = new Rect(UpperLeft.x, UpperLeft.y, LowerRight.x - UpperLeft.x, LowerRight.y - UpperLeft.y);

        //resWidth -= (resWidth - (int)LowerRight.x); 
        //resWidth -= 1;

        //resWidth = (int)LassoRect.width;
        //resHeight = (int)LassoRect.height;

        //LassoRect.width = resWidth;
        //LassoRect.height = resHeight;

        //print(LassoRect.width);
        //print(LassoRect.height);


        //int wSub = resWidth - (int)LassoRect.width;
        //resWidth -= wSub;

        //int hSub = resHeight - (int)LassoRect.height;
        //resHeight -= hSub;

        //print(resWidth);
        //print(resHeight);
    }

    public void TakeScreenshot ()
    {
        StartCoroutine("ScreenshotSequence");

    }

    /*
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
*/
    void Update ()
    {

    }

    void CapScreen()
    {

            //int resWidthN = (int)LassoRect.width * superSamplingFactor;
            //int resHeightN = (int)LassoRect.height * superSamplingFactor;
        int resWidthN = resWidth * superSamplingFactor;
        int resHeightN = resHeight * superSamplingFactor;

        RenderTexture rt = new RenderTexture(resWidthN, resHeightN, 24);
            Camera.main.targetTexture = rt;

            TextureFormat tFormat;
            //if (isTransparent)
            //    tFormat = TextureFormat.ARGB32;
            //else
                tFormat = TextureFormat.RGB24;


            Texture2D screenShot = new Texture2D(resWidthN, resHeightN, tFormat, false);
            Camera.main.Render();
            RenderTexture.active = rt;
            screenShot.ReadPixels(new Rect(0, 0, resWidthN, resHeightN), 0, 0);
            //screenShot.ReadPixels(new Rect(LassoRect.xMin * superSamplingFactor, LassoRect.yMin * superSamplingFactor, LassoRect.width * superSamplingFactor, LassoRect.height * superSamplingFactor), 0, 0);
            Camera.main.targetTexture = null;
            RenderTexture.active = null;
            byte[] bytes = screenShot.EncodeToPNG();

        string filename = "CharacterSheet_IPC_" + GetComponent<CharacterSheetGenerator>().dataToUse.ipc_id + ".png";
        //string filename = "file" + ".png";

        System.IO.File.WriteAllBytes(filename, bytes);
            Debug.Log(string.Format("Took screenshot to: {0}", filename));
            Application.OpenURL(filename);

    }

    IEnumerator ScreenshotSequence()
    {
        hideEvent.Invoke();

        yield return new WaitForSeconds(0.1f);
        yield return new WaitForEndOfFrame();

        CapScreen();

        yield return new WaitForSeconds(0.1f);
        unhideEvent.Invoke();

        //        yield return new WaitForEndOfFrame();
        //        int superSamplingFactor = 1;
        //        Texture2D snapShot = new Texture2D((int)LassoRect.width * superSamplingFactor, (int)LassoRect.height * superSamplingFactor, TextureFormat.RGB24, false);

        //        Rect lassoRectSS = new Rect(LassoRect.xMin * superSamplingFactor, LassoRect.yMin * superSamplingFactor, LassoRect.width * superSamplingFactor, LassoRect.height * superSamplingFactor);
        //        snapShot.ReadPixels(lassoRectSS, 0, 0);
        //        snapShot.Apply();

        //        var bytes = snapShot.EncodeToPNG();

        //        Destroy(snapShot);

        //#if UNITY_EDITOR
        //        System.IO.File.WriteAllBytes("CharacterSheet_IPC_" + GetComponent<CharacterSheetGenerator>().dataToUse.ipc_id + ".png", bytes);
        //        Debug.Log("Captured");

        //        yield return new WaitForSeconds(0.2f);

        //        unhideEvent.Invoke();

        //#elif UNITY_STANDALONE_OSX
        //        string path = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Desktop);
        //        System.IO.File.WriteAllBytes(path + "/CharacterSheet_IPC_" + GetComponent<CharacterSheetGenerator>().dataToUse.ipc_id + ".png", bytes);

        //        yield return new WaitForSeconds(0.2f);

        //        unhideEvent.Invoke();
        //        yield break;
        //#else

        //        System.IO.File.WriteAllBytes("CharacterSheet_IPC_" + GetComponent<CharacterSheetGenerator>().dataToUse.ipc_id + ".png", bytes);
        //        Debug.Log("Captured");

        //        yield return new WaitForSeconds(0.2f); 

        //        unhideEvent.Invoke();
        //#endif




    }


}

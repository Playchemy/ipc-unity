using UnityEngine;
using Vuforia;

public class ImageFormat : MonoBehaviour
{
    private Rect mCurrentFormatRect;
    private Rect mDebugRect;
    private Rect mImageInfoRect;

    private string mDebugMsg = "";
    private string mImageInfo = "";
    private Image.PIXEL_FORMAT mFormat = Image.PIXEL_FORMAT.GRAYSCALE;

    private bool mRegisteredFormat = false;
    private bool mQCARInitialized = false;

    void Start()
    {
        VuforiaBehaviour qcar = (VuforiaBehaviour)FindObjectOfType(typeof(VuforiaBehaviour));
        if (qcar)
        {
            //qcar.RegisterTrackerEventHandler(this);
        }
        else
        {
            Debug.LogError("Could not find QCARBehaviour (i.e. ARCamera) in the scene");
        }

        int w = Screen.width;
        int h = Screen.height;
        mCurrentFormatRect = new Rect(w / 2 - 200, h / 2 + 130, 350, 30);
        mImageInfoRect = new Rect(w / 2 - 200, h / 2 + 160, 350, 30);
        mDebugRect = new Rect(w / 2 - 200, h / 2 + 190, 350, 30);
    }

    // This is called when QCAR has been initialized
    public void OnInitialized()
    {
        mQCARInitialized = true;
        mRegisteredFormat = false;
    }

    // Implementing OnTrackablesUpdated() 
    // of ITrackerEventHandler interface
    public void OnTrackablesUpdated()
    {
    }

    // This is called whenever the app gets paused
    void OnApplicationPaused(bool paused)
    {
        if (paused)
        {
            // invalidate registered format if app has been paused
            mRegisteredFormat = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Skip if QCAR has not been initialized yet
        if (mQCARInitialized)
        {
            if (!mRegisteredFormat)
            {
                //first time update or first resume after pause
                //see OnApplicationPaused() above
                mFormat = Image.PIXEL_FORMAT.RGB888;// Choose the Frame Format you want here
                CameraDevice.Instance.SetFrameFormat(mFormat, false);
                if (CameraDevice.Instance.SetFrameFormat(mFormat, true))
                {
                    mDebugMsg = mFormat.ToString() + " successfully set.";
                    mRegisteredFormat = true;
                }
                else
                {
                    mDebugMsg = "Failed to set RGB888.";
                    mFormat = Image.PIXEL_FORMAT.UNKNOWN_FORMAT;
                }
            }

            Image img = CameraDevice.Instance.GetCameraImage(mFormat);
            if (img != null)
            {
                mImageInfo = img.Width + " x " + img.Height + " " + "Pixels: " + img.Pixels[0] + ", " + img.Pixels[1] + ", " + img.Pixels[2] + " ...";
            }
            else
            {
                mImageInfo = "Can't get Image for " + mFormat.ToString();
            }
        }
    }

    void OnGUI()
    {
        //GUI.skin.box.fontSize = 18;
        //GUI.Box(mCurrentFormatRect, "Current Format: " + mFormat.ToString());
        //GUI.Box(mImageInfoRect, "Image: " + mImageInfo);
        //GUI.Box(mDebugRect, "Messages: " + mDebugMsg);
    }
}
using UnityEngine;
using System.Collections;

public class BuildProperties : MonoBehaviour
{
    public Texture2D[] defaultIcons;
    public string[] scenePaths = new[] { "Assets/IPCExplorer/Scenes/Main.unity" };
    public string locationPathName = "Builds/IPCExplorer_Demo.apk";
    public string applicationIdentifier = "com.Playchemy.IPCExplorer";
    public string productName = "IPCExplorer";
    public string version = "1.0";

    public DefaultInterfaceOrientation defaultInterfaceOrientation;
    public BuildTarget buildTarget;

    public enum DefaultInterfaceOrientation { AutoRotation, LandscapeLeft, LandscapeRight, Portrait, PortraitUpsideDown };
    public enum BuildTarget {PC, MAC, Android, iOS };
}

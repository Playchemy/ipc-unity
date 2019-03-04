using UnityEditor;
using UnityEngine;
using System.Collections;

public class Builder : MonoBehaviour
{
    [MenuItem("Build/Build")]
    public static void Build()
    {
        BuildPlayerOptions buildPlayerOptions = new BuildPlayerOptions();

        buildPlayerOptions.scenes = FindObjectOfType<BuildProperties>().scenePaths;
        buildPlayerOptions.locationPathName = FindObjectOfType<BuildProperties>().locationPathName;
        PlayerSettings.applicationIdentifier = FindObjectOfType<BuildProperties>().applicationIdentifier;
        PlayerSettings.productName = FindObjectOfType<BuildProperties>().productName;
        PlayerSettings.bundleVersion = FindObjectOfType<BuildProperties>().version;
        PlayerSettings.SetIconsForTargetGroup(BuildTargetGroup.Unknown, FindObjectOfType<BuildProperties>().defaultIcons);

        switch (FindObjectOfType<BuildProperties>().defaultInterfaceOrientation)
        {
            case BuildProperties.DefaultInterfaceOrientation.AutoRotation:
                PlayerSettings.defaultInterfaceOrientation = UIOrientation.AutoRotation;
                break;
            case BuildProperties.DefaultInterfaceOrientation.LandscapeLeft:
                PlayerSettings.defaultInterfaceOrientation = UIOrientation.LandscapeLeft;
                break;
            case BuildProperties.DefaultInterfaceOrientation.LandscapeRight:
                PlayerSettings.defaultInterfaceOrientation = UIOrientation.LandscapeRight;
                break;
            case BuildProperties.DefaultInterfaceOrientation.Portrait:
                PlayerSettings.defaultInterfaceOrientation = UIOrientation.Portrait;
                break;
            case BuildProperties.DefaultInterfaceOrientation.PortraitUpsideDown:
                PlayerSettings.defaultInterfaceOrientation = UIOrientation.PortraitUpsideDown;
                break;
        }

        switch (FindObjectOfType<BuildProperties>().buildTarget)
        {
            case BuildProperties.BuildTarget.Android:
                buildPlayerOptions.target = BuildTarget.Android;
                break;
            case BuildProperties.BuildTarget.iOS:
                buildPlayerOptions.target = BuildTarget.iOS;
                break;
            case BuildProperties.BuildTarget.MAC:
                buildPlayerOptions.target = BuildTarget.StandaloneOSX;
                break;
            case BuildProperties.BuildTarget.PC:
                buildPlayerOptions.target = BuildTarget.StandaloneWindows64;
                break;
        }
        buildPlayerOptions.options = BuildOptions.None;
        BuildPipeline.BuildPlayer(buildPlayerOptions);
    }

    [MenuItem("Build/Build AutoRun")]
    public static void Build_AutoRun()
    {
        BuildPlayerOptions buildPlayerOptions = new BuildPlayerOptions();

        buildPlayerOptions.scenes = FindObjectOfType<BuildProperties>().scenePaths;
        buildPlayerOptions.locationPathName = FindObjectOfType<BuildProperties>().locationPathName;
        PlayerSettings.applicationIdentifier = FindObjectOfType<BuildProperties>().applicationIdentifier;
        PlayerSettings.productName = FindObjectOfType<BuildProperties>().productName;
        PlayerSettings.bundleVersion = FindObjectOfType<BuildProperties>().version;
        PlayerSettings.SetIconsForTargetGroup(BuildTargetGroup.Unknown, FindObjectOfType<BuildProperties>().defaultIcons);

        switch (FindObjectOfType<BuildProperties>().defaultInterfaceOrientation)
        {
            case BuildProperties.DefaultInterfaceOrientation.AutoRotation:
                PlayerSettings.defaultInterfaceOrientation = UIOrientation.AutoRotation;
                break;
            case BuildProperties.DefaultInterfaceOrientation.LandscapeLeft:
                PlayerSettings.defaultInterfaceOrientation = UIOrientation.LandscapeLeft;
                break;
            case BuildProperties.DefaultInterfaceOrientation.LandscapeRight:
                PlayerSettings.defaultInterfaceOrientation = UIOrientation.LandscapeRight;
                break;
            case BuildProperties.DefaultInterfaceOrientation.Portrait:
                PlayerSettings.defaultInterfaceOrientation = UIOrientation.Portrait;
                break;
            case BuildProperties.DefaultInterfaceOrientation.PortraitUpsideDown:
                PlayerSettings.defaultInterfaceOrientation = UIOrientation.PortraitUpsideDown;
                break;
        }

        switch (FindObjectOfType<BuildProperties>().buildTarget)
        {
            case BuildProperties.BuildTarget.Android:
                buildPlayerOptions.target = BuildTarget.Android;
                break;
            case BuildProperties.BuildTarget.iOS:
                buildPlayerOptions.target = BuildTarget.iOS;
                break;
            case BuildProperties.BuildTarget.MAC:
                buildPlayerOptions.target = BuildTarget.StandaloneOSX;
                break;
            case BuildProperties.BuildTarget.PC:
                buildPlayerOptions.target = BuildTarget.StandaloneWindows64;
                break;
        }
        buildPlayerOptions.options = BuildOptions.AutoRunPlayer;
        BuildPipeline.BuildPlayer(buildPlayerOptions);
    }
}

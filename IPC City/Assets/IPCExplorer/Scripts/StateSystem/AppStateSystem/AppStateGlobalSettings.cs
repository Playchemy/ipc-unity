using System.Collections;
using UnityEngine.UI;
using UnityEngine;

public class AppStateGlobalSettings : MonoBehaviour
{
    public Text versionText = null;
    public Toggle chamberAnimationToggle = null;

    private void Start()
    {
        Setting_AlwaysAnimation();
        GridFunctions.Instance.onGridGenerationFinished += Setting_AlwaysAnimation;
    }

    // Version
    private void SetVersion()
    {
        versionText.text = "v" + Application.version;
    }

    // Chamber Animations
    private void Setting_AlwaysAnimation()
    {
        chamberAnimationToggle.onValueChanged.AddListener((bool value) => ToggleAnimationOption(chamberAnimationToggle.isOn));

        if (PlayerPrefs.HasKey("AlwaysAnimate") && PlayerPrefs.GetInt("AlwaysAnimate") == 1)
        {
            chamberAnimationToggle.isOn = true;
            chamberAnimationToggle.isOn = true;
            ToggleAnimationOption(true);
        }
        else if (PlayerPrefs.HasKey("AlwaysAnimate") && PlayerPrefs.GetInt("AlwaysAnimate") == 0)
        {
            chamberAnimationToggle.isOn = false;
            chamberAnimationToggle.isOn = false;
            ToggleAnimationOption(false);
        }
        else
        {
            PlayerPrefs.SetInt("AlwaysAnimate", 1);
            ToggleAnimationOption(true);
            chamberAnimationToggle.isOn = true;
        }
    }

    private void ToggleAnimationOption(bool isOn)
    {
        if (isOn)
        {
            for (int i = 0; i < IPCLoader.Instance.chamberIpcGetServices.Count; i++)
            {
                IPCLoader.Instance.chamberIpcGetServices[i].GetComponent<SpriteManagerV2>().addToAnimator = true;
                PlayerPrefs.SetInt("AlwaysAnimate", 1);
            }
        }
        else
        {
            for (int i = 0; i < IPCLoader.Instance.chamberIpcGetServices.Count; i++)
            {
                IPCLoader.Instance.chamberIpcGetServices[i].GetComponent<SpriteManagerV2>().addToAnimator = false;
                PlayerPrefs.SetInt("AlwaysAnimate", 0);
            }
        }

        PlayerPrefs.Save();
    }
}

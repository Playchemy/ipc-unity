using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class SplashScreen : MonoBehaviour
{
    public float fadeOutSpeed = 1f;

    public void ExitSplashScreen()
    {
        FadeOut();
        Invoke("DeactivateSplashScreen", fadeOutSpeed);
    }

    private void FadeOut()
    {
        transform.GetChild(0).GetComponent<Image>().CrossFadeAlpha(0f, fadeOutSpeed, true);
        transform.GetChild(1).GetComponent<Image>().CrossFadeAlpha(0f, fadeOutSpeed, true);
        transform.GetChild(2).GetComponent<Image>().CrossFadeAlpha(0f, fadeOutSpeed, true);
        transform.GetChild(2).GetChild(0).GetComponent<Image>().CrossFadeAlpha(0f, fadeOutSpeed, true);
        transform.GetChild(2).GetChild(1).GetComponent<Text>().CrossFadeAlpha(0f, fadeOutSpeed, true);
    }

    private void DeactivateSplashScreen()
    {
        transform.parent.gameObject.SetActive(false);
    }
}

using System.Collections;
using UnityEngine.UI;
using UnityEngine;

public class ColorThemeSwapper : MonoBehaviour
{
    public Material defaultThemeMaterial = null;
    public Material darkThemeMaterial = null;

    private void Startasd()
    {
        Text[] texts = FindObjectsOfType<Text>();
        Image[] images = FindObjectsOfType<Image>();

        foreach (Text text in texts)
        {
            text.material = darkThemeMaterial;
        }

        foreach (Image image in images)
        {
            image.material = darkThemeMaterial;
        }
    }
}

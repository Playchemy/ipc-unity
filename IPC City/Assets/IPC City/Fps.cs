using UnityEngine;
using System.Collections;
using UnityEngine.UI;
 
public class Fps : MonoBehaviour
{
	float deltaTime = 0.0f;
    public Text fpsText;
 
	void Update()
	{
		deltaTime += (Time.unscaledDeltaTime - deltaTime) * 0.1f;
        Write();
	}
 
	void Write()
    { 
		float msec = deltaTime * 1000.0f;
		float fps = 1.0f / deltaTime;
		string text = string.Format("{0:0.0} ms ({1:0.} fps)", msec, fps);
        fpsText.text = text;
	}
}

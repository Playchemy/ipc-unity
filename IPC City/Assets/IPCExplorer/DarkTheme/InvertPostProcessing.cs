using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode, ImageEffectAllowedInSceneView]
public class InvertPostProcessing : MonoBehaviour {

	[HideInInspector]
	Material Invert;
	public Shader InvertShader;

	void OnRenderImage (RenderTexture source, RenderTexture destination) {
		
		if (Invert == null) {
			Invert = new Material(InvertShader);
			Invert.hideFlags = HideFlags.HideAndDontSave;
		}

		Graphics.Blit(source, destination, Invert);
	}
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class ObjectScript : MonoBehaviour {

	public RenderTexture paintingTexture;

	void Start () {
		paintingTexture = new RenderTexture(1024, 1024, 24);
		paintingTexture.enableRandomWrite = true;
		paintingTexture.Create();

		GetComponent<Renderer>().sharedMaterial.SetTexture("_PaintingTex", paintingTexture);
	}
}

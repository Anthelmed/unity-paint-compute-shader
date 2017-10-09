using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class ObjectScript : MonoBehaviour {

	public Color decalColor;
	public RenderTexture decalTexture;

	void Start () {
		decalTexture = new RenderTexture(1024, 1024, 24);
		decalTexture.enableRandomWrite = true;
		decalTexture.Create();

		GetComponent<Renderer>().sharedMaterial.SetTexture("_DecalTex", decalTexture);
	}
}

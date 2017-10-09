using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrailScript : MonoBehaviour 
{

	public ComputeShader computeShader;
	public float radius = 10;

	private Camera mainCamera;

	private int kernel;

	private Ray ray;
	RaycastHit hit;

	new Renderer renderer;
	MeshCollider meshCollider;

	Texture2D mainTexture;
	Vector2 pixelUV;

	ObjectScript objectScript;

	void Start () 
	{
		mainCamera = Camera.main;
		kernel = computeShader.FindKernel("CSMain");
	}

	void Update() {
        if (!Input.GetMouseButton(0))
            return;
        
        if (!Physics.Raycast(mainCamera.ScreenPointToRay(Input.mousePosition), out hit))
            return;
        
        renderer = hit.transform.GetComponent<Renderer>();
        meshCollider = hit.collider as MeshCollider;
        if (renderer == null || renderer.sharedMaterial == null || renderer.sharedMaterial.mainTexture == null || meshCollider == null)
            return;
        
        mainTexture = renderer.material.mainTexture as Texture2D;
        pixelUV = hit.textureCoord;
        pixelUV.x *= mainTexture.width;
        pixelUV.y *= mainTexture.height;

		objectScript = hit.transform.GetComponent<ObjectScript>();

		Compute(objectScript.decalTexture, objectScript.decalColor, pixelUV);
    }

	void Compute(RenderTexture texture, Color color, Vector2 uv) {
		computeShader.SetTexture(kernel, "Texture", texture);
		computeShader.SetVector("Color", color);
		computeShader.SetVector("UV", new Vector2((int)uv.x, (int)uv.y));
		computeShader.SetFloat("Radius", radius);

		computeShader.Dispatch(kernel, 1024/8, 1024/8, 1);
	}
}

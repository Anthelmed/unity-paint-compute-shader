using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrushScript : MonoBehaviour 
{
	public static BrushScript instance;

	public ComputeShader computeShader;
	public Color color;
	public int layerMask = 1 << 8;

	private int kernel;

	new Renderer renderer;
	MeshCollider meshCollider;

	Texture2D mainTexture;
	Vector2 pixelUV;

	ObjectScript objectScript;

	void Awake () 
	{
		if (instance == null) {
			instance = this;
		}
	}

	void Start () 
	{
		kernel = computeShader.FindKernel("CSMain");
	}

	void Update () 
	{
		if (Input.GetButton("Fire2")) 
		{
			color = Random.ColorHSV(0f, 1f, 1f, 1f, 0.5f, 1f);
			ParticleLauncherScript.instance.ParticleColor = color;
			SplatterParticlesScript.instance.ParticleColor = color;
		}
	}

	public void Paint(RaycastHit hit, Color color, float radius) {
		renderer = hit.transform.GetComponent<Renderer>();
        meshCollider = hit.collider as MeshCollider;
        if (renderer == null || renderer.sharedMaterial == null || renderer.sharedMaterial.mainTexture == null || meshCollider == null)
            return;
        
        mainTexture = renderer.material.mainTexture as Texture2D;
        pixelUV = hit.textureCoord;
        pixelUV.x *= mainTexture.width;
        pixelUV.y *= mainTexture.height;

		objectScript = hit.transform.GetComponent<ObjectScript>();

		Compute(objectScript.paintingTexture, color, pixelUV, radius);
	}

	void Compute(RenderTexture texture, Color color, Vector2 uv, float radius) {
		computeShader.SetTexture(kernel, "Texture", texture);
		computeShader.SetVector("Color", color);
		computeShader.SetVector("UV", new Vector2((int)uv.x, (int)uv.y));
		computeShader.SetFloat("Radius", radius);

		computeShader.Dispatch(kernel, 1024/8, 1024/8, 1);
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplatterParticlesScript : MonoBehaviour 
{
	public static SplatterParticlesScript instance;

	public ParticleSystem splatterParticles;
	private ParticleSystem.MainModule particleSystemMain;

	private Color particleColor = Color.white;
	public Color ParticleColor {
		get {
			return particleColor;
		}
		set {
			particleColor = value;
			particleSystemMain.startColor = value; 
		}
	}

	private float particleSize = 0.5f;
	public float ParticleSize {
		get {
			return particleSize;
		}
		set {
			particleSize = value;
			particleSystemMain.startSize = value; 
		}
	}

	private Ray ray;
	private RaycastHit hit;
	private List<ParticleCollisionEvent> particleCollisionEvents;
	
	void Awake () 
	{
		if (instance == null) {
			instance = this;
		}
	}

	void Start() 
	{
		particleSystemMain = splatterParticles.main;
		ParticleColor = particleColor;
		ParticleSize = particleSize;

		particleCollisionEvents = new List<ParticleCollisionEvent> ();
	}

	void OnParticleCollision(GameObject other) 
	{	
		
		ParticlePhysicsExtensions.GetCollisionEvents (splatterParticles, other, particleCollisionEvents);

		for (int i = 0; i < particleCollisionEvents.Count; i++) {
			if (!Physics.Raycast(particleCollisionEvents[i].intersection, 
			-particleCollisionEvents[i].normal, 
			out hit, 
			Mathf.Infinity, 
			BrushScript.instance.layerMask))
            continue;

			BrushScript.instance.Paint(hit, ParticleColor, ParticleSize * 20);
		}
	}
}

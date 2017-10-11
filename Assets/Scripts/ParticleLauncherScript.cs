using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleLauncherScript : MonoBehaviour 
{
	public static ParticleLauncherScript instance;

	public ParticleSystem particleLauncher;
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

	private float particleSize = 1f;
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
	private new Rigidbody rigidbody;
	private List<ParticleCollisionEvent> particleCollisionEvents;

	void Awake () 
	{
		if (instance == null) {
			instance = this;
		}
	}

	void Start() 
	{
		particleSystemMain = particleLauncher.main;
		ParticleColor = particleColor;
		ParticleSize = particleSize;

		particleCollisionEvents = new List<ParticleCollisionEvent> ();
	}

	void OnParticleCollision(GameObject other) 
	{	
		
		ParticlePhysicsExtensions.GetCollisionEvents (particleLauncher, other, particleCollisionEvents);

		for (int i = 0; i < particleCollisionEvents.Count; i++) {
			if (!Physics.Raycast(particleCollisionEvents[i].intersection, 
			-particleCollisionEvents[i].normal, 
			out hit, 
			Mathf.Infinity, 
			BrushScript.instance.layerMask))
            continue;

			rigidbody = other.transform.parent.GetComponent<Rigidbody>();
			if (rigidbody) {
				rigidbody.AddForce(particleCollisionEvents[i].velocity);
			}

			BrushScript.instance.Paint(hit, ParticleColor, ParticleSize * 20);
			EmitAtLocation(particleCollisionEvents[i]);
		}
	}

	void EmitAtLocation(ParticleCollisionEvent particleCollisionEvent) {
		splatterParticles.transform.position = particleCollisionEvent.intersection;
		splatterParticles.transform.rotation = Quaternion.LookRotation(particleCollisionEvent.normal);
		splatterParticles.Emit(1);
	}

	void Update () 
	{
		if (Input.GetButton("Fire1")) 
		{
			particleLauncher.Emit(1);
		}
	}
}

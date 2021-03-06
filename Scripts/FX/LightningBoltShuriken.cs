/*
	This script is placed in public domain. The author takes no responsibility for any possible harm.
	Contributed by Jonathan Czeck
*/
using UnityEngine;
using System.Collections;

public class LightningBoltShuriken : MonoBehaviour
{
	public Transform target;
	public int zigs = 100;
	public float speed = 1f;
	public float scale = 1f;
	public Light startLight;
	public Light endLight;
	
	Perlin noise;
	float oneOverZigs;
	
	private ParticleSystem.Particle[] particles; 
	
	void Start()
	{
		oneOverZigs = 1f / (float)zigs;

		particles = new ParticleSystem.Particle[zigs];

		GetComponent<ParticleSystem>().Emit(zigs);
	}
	
	void Update ()
	{
		
		if (noise == null)
			noise = new Perlin();
			
		float timex = Time.time * speed * 0.1365143f;
		float timey = Time.time * speed * 1.21688f;
		float timez = Time.time * speed * 2.5564f;


		particles = new ParticleSystem.Particle[GetComponent<ParticleSystem>().particleCount];
		int total = GetComponent<ParticleSystem>().GetParticles(particles);
		for (int i=0; i < particles.Length; i++)
		{
			if(null == target) return;
			Vector3 position = Vector3.Lerp(transform.position, target.position, oneOverZigs * (float)i);
			Vector3 offset = new Vector3(noise.Noise(timex + position.x, timex + position.y, timex + position.z),
										noise.Noise(timey + position.x, timey + position.y, timey + position.z),
										noise.Noise(timez + position.x, timez + position.y, timez + position.z));
			position += (offset * scale * ((float)i * oneOverZigs));


			particles[i].position = position;
			particles[i].color = Color.white;
//			particles[i].energy = 1f;
		}

		GetComponent<ParticleSystem>().SetParticles(particles, particles.Length);
//		Debug.Log("parsyscount:"+GetComponent<ParticleSystem>().particleCount);
//		Debug.Log("setting particles. nth:"+particles[Random.Range(0,99)].position);
		
		if (GetComponent<ParticleSystem>().particleCount >= 2)
		{
			if (startLight)
				startLight.transform.position = particles[0].position;
			if (endLight)
				endLight.transform.position = particles[particles.Length - 1].position;
		}
	}	
}
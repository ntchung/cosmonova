using UnityEngine;
using System.Collections;

public class Thruster : MonoBehaviour 
{
	private bool isActive = false;	

	private ParticleSystem cacheParticleSystem;

	// Use this for initialization
	void Start () 
	{
		cacheParticleSystem = particleSystem;
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (isActive) 
		{
			if (cacheParticleSystem != null) 
			{	
				cacheParticleSystem.enableEmission = true;
			}		
		} 
		else 
		{
			if (cacheParticleSystem != null) 
			{				
				cacheParticleSystem.enableEmission = false;				
			}
		}
	}

	public void StartThruster() 
	{
		isActive = true; 
	}
	
	public void StopThruster() 
	{
		isActive = false; 
	}
}

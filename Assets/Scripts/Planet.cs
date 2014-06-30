// Moon C# Script (version: 1.02)
using UnityEngine;
using System.Collections;

public class Planet : MonoBehaviour 
{
	public float orbitSpeed = 0.0f;
	public float rotationSpeed = 0.0f;	
	
	private Transform _cacheTransform;
	private Transform _cacheMeshTransform;
	
	void Start () 
	{
		_cacheTransform = transform;
		_cacheMeshTransform = transform.Find("Body");
	}
	
	void Update () 
	{		
		if (_cacheTransform != null) 
		{
			_cacheTransform.Rotate(Vector3.up * orbitSpeed * Time.deltaTime);
		}
		
		if (_cacheMeshTransform != null) 
		{
			_cacheMeshTransform.Rotate(Vector3.up * rotationSpeed * Time.deltaTime);
		}
	}
}

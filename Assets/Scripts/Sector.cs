﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Sector : MonoBehaviour 
{
	public Material[] planetMaterials;
	public Material[] planetRingMaterials;
	public Material[] starMaterials;

	public GameObject starPrefab;
	public GameObject planetPrefab;
	public GameObject ringPlanetPrefab;

	private GameObject star;
	private List<GameObject> planets;

	void Start() 
	{
		star = GameObject.Instantiate(starPrefab) as GameObject;

		Transform starBody = star.transform.Find("Body");
		starBody.renderer.sharedMaterial = starMaterials[UnityEngine.Random.Range(0, starMaterials.Length)];

		List<int> unusedPlanetMaterialIds = new List<int>();
		for (int i = 0; i < planetMaterials.Length; i++) unusedPlanetMaterialIds.Add(i);

		planets = new List<GameObject>();

		int numPlanets = 8;
		numPlanets = Mathf.Min (numPlanets, planetMaterials.Length);

		for (int i = 0; i < numPlanets; i++)
		{
			bool hasRing = (UnityEngine.Random.Range(0, 4) == 0);
			GameObject planet = GameObject.Instantiate(hasRing ? ringPlanetPrefab : planetPrefab) as GameObject;

			float radius = UnityEngine.Random.Range(i + 1.0f, i + 1.5f) * 300.0f;
			float angle = UnityEngine.Random.Range(0.0f, Mathf.PI * 2.0f);

			planet.transform.parent = star.transform;

			Transform planetBody = planet.transform.Find("Body");
			int pos = UnityEngine.Random.Range(0, unusedPlanetMaterialIds.Count);
			planetBody.renderer.sharedMaterial = planetMaterials[unusedPlanetMaterialIds[pos]];
			unusedPlanetMaterialIds.RemoveAt(pos);

			if (hasRing)
			{
				Transform planetRing = planetBody.Find("Ring");
				planetRing.renderer.sharedMaterial = planetRingMaterials[UnityEngine.Random.Range(0, planetRingMaterials.Length)];
			}

			planetBody.localPosition = new Vector3(Mathf.Cos(angle) * radius, 0.0f, Mathf.Sin(angle) * radius);

			Planet p = planet.GetComponent<Planet>();
			p.orbitSpeed = 360.0f * UnityEngine.Random.Range(1.0f / (i * 4.0f + 2.5f), 1.0f / (i * 4.0f + 5.0f)) * (UnityEngine.Random.Range(0, 2) == 0 ? 1.0f : -1.0f);
			p.rotationSpeed = UnityEngine.Random.Range(-180.0f, 180.0f);
		}
	}
	
	void Update()
	{
	
	}
}

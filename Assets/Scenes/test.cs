using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class test : MonoBehaviour 
{
	CRSpline spline;

	float t;

	// Use this for initialization
	void Start () 
	{
		Vector3[] drawPath = iTweenPath.GetPath("test path");

		spline = new CRSpline(drawPath);
		t = 0.0f;


		/*
		iTween.MoveTo(gameObject, 
			iTween.Hash(
				"path", iTweenPath.GetPath("test path"), 
				"looptype", "loop",
				"time", 10, 
				"movetopath", false
			));
		*/
	}
	
	// Update is called once per frame
	void Update () 
	{
		gameObject.transform.position = spline.Interp(t);
		t += Time.deltaTime * 8.0f;
	}
}

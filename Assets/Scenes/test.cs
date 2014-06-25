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
		Vector3 vel;
		gameObject.transform.position = spline.Interp(t, out vel);

		//t += Time.deltaTime * (velocity / vel.magnitude);
		t += Time.deltaTime * 20.0f;

		Vector3 front = vel.normalized;
		Vector3 up = new Vector3(0.0f, 1.0f, 0.0f);
		Vector3 right = Vector3.Cross(front, up);

		up = Vector3.Cross(right, front);

		gameObject.transform.rotation =  Quaternion.LookRotation(front, up);

	}
}

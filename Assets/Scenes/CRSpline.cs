using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class CRSpline 
{
	private Vector3[] pts;
	private int numPoints;

	private float[] t;
	private Vector3[] m;

	private bool isLoop;

	public CRSpline(Vector3[] _pts) 
	{
		numPoints = _pts.Length;

		pts = new Vector3[numPoints + 2];
		Array.Copy(_pts, 0, pts, 1, numPoints);

		isLoop = false;
		
		if (pts[1] == pts[numPoints])
		{
			isLoop = true;

			pts[0] = pts[numPoints - 1];
			pts[numPoints + 1] = pts[2];
		}
		else
		{
			pts[0] = pts[1] * 2.0f - pts[2];
			pts[numPoints + 1] = pts[numPoints] * 2.0f - pts[numPoints - 1];
		}
		
		t = new float[numPoints + 2];
		
		t[1] = 0.0f;
		for (int i = 2; i <= numPoints + 1; i++)
		{
			//t[i] = i - 1; //uniform
			//t[i] = t[i - 1] + Vector3.Distance(pts[i - 1], pts[i]); //chordal 
			t[i] = t[i - 1] + Mathf.Sqrt(Vector3.Distance(pts[i - 1], pts[i])); //centripetal
		}

		t[0] = -t[2];

 		m = new Vector3[numPoints];
		for (int i = 1; i <= numPoints; i++)
		{
			m[i - 1] = (pts[i] - pts[i - 1]) / (t[i] - t[i - 1]) - 
					(pts[i + 1] - pts[i - 1]) / (t[i + 1] - t[i - 1]) +
					(pts[i + 1] - pts[i]) / (t[i + 1] - t[i]);
		}
	}

	public Vector3 Interp(float curT, out Vector3 vel) 
	{
		if (!isLoop)
		{
			if (curT > t[numPoints]) curT = t[numPoints];
		}
		else 
		{
			curT %= t[numPoints];
		}

		int section = 1;
		float dt = 1.0f;

		for (; section <= numPoints; section++)
		{
			if (curT <= t[section + 1])
			{
				dt = t[section + 1] - t[section];
				curT = (curT - t[section]) / dt;
				break;
			}
		}

		//Debug.Log(curT + " " + section);

		Vector3 a = pts[section];
		Vector3 b = m[section - 1] * dt;
		Vector3 c = pts[section + 1];
		Vector3 d = m[section] * dt;

		Vector3 p = 
			(2.0f * curT * curT * curT - 3.0f * curT * curT + 1.0f) * a + 
			(curT * curT * curT - 2.0f * curT * curT + curT) * b + 
			(-2.0f * curT * curT * curT + 3.0f * curT * curT) * c + 
			(curT * curT * curT - curT * curT) * d;

		vel = 
			(6.0f * curT * curT - 6.0f * curT) * a + 
			(3.0f * curT * curT - 4.0f * curT + 1.0f) * b + 
			(-6.0f * curT * curT + 6.0f * curT) * c + 
			(3.0f * curT * curT - 2.0f * curT) * d;

		return p;
	}	
}	
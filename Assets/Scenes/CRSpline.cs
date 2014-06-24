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
			pts[0] = pts[1] + (pts[1] - pts[2]);
			pts[numPoints + 1] = pts[numPoints] + (pts[numPoints] - pts[numPoints - 1]);
		}
		
		t = new float[numPoints + 2];
		
		t[1] = 0.0f;
		for (int i = 2; i <= numPoints; i++)
		{
			//t[i] = i - 1; //uniform
			//t[i] = t[i - 1] + Vector3.Distance(pts[i - 1], pts[i]); //chordal 
			t[i] = t[i - 1] + Mathf.Sqrt(Vector3.Distance(pts[i - 1], pts[i])); //centripetal
		}

		t[0] = t[1] - Mathf.Sqrt(Vector3.Distance(pts[0], pts[1]));
		t[numPoints + 1] = t[numPoints] + Mathf.Sqrt(Vector3.Distance(pts[numPoints], pts[numPoints + 1]));

		m = new Vector3[numPoints];
		for (int i = 1; i <= numPoints; i++)
			m[i - 1] = (pts[i + 1] - pts[i - 1]) / (t[i + 1] - t[i - 1]);
	}

	public Vector3 Interp(float curT) 
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

		for (; section <= numPoints; section++)
		{
			if (curT <= t[section + 1])
			{
				curT = (curT - t[section]) / (t[section + 1] - t[section]);
				break;
			}
		}

		Debug.Log(curT + " " + section);

		Vector3 a = pts[section];
		Vector3 b = m[section - 1];
		Vector3 c = pts[section + 1];
		Vector3 d = m[section];

		Vector3 p = 
			(2.0f * curT * curT * curT - 3.0f * curT * curT + 1.0f) * a + 
			(curT * curT * curT - 2.0f * curT * curT + curT) * b + 
			(-2.0f * curT * curT * curT + 3.0f * curT * curT) * c + 
			(curT * curT * curT - curT * curT) * d;

		return p;
	}	
}	
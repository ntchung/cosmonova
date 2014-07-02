﻿using UnityEngine;
using System.Collections;

public class Utils {

	public static int UI_LAYER_MASK = 1 << LayerMask.NameToLayer( "UI" );
	
	public static float shortestAngle(float start, float end)
	{
		return ((((end - start) % 360.0f) + 540.0f) % 360.0f) - 180.0f;
	}	
	
	public static Quaternion rotationByEulerZ(float z)
	{
		float rollOver2 = z * (Mathf.Deg2Rad * 0.5f);
		return new Quaternion(0.0f, 0.0f, Mathf.Sin(rollOver2), Mathf.Cos(rollOver2));
	}
	
	public static float snapRotationBy90(float z)
	{
		return Mathf.Round(z / 90.0f) * 90.0f;
	}
	
	public static bool odd(int v)
	{
		return (v & 1) != 0;
	}
	
	public static bool even(int v)
	{
		return (v & 1) == 0;
	}

	public static bool Possible50()
	{
		return (Random.Range(0, 100) & 1) == 0;
	}

	public static IEnumerator WaitForRealSeconds(float time)
	{
		float start = Time.realtimeSinceStartup;
		while (Time.realtimeSinceStartup < start + time)
		{
			yield return null;
		}
	}
	
	public static bool IsTouchOnGUI(Vector3 pos)
	{
		// This grabs the camera attached to the NGUI UI_Root object.
		Camera nguiCam = UICamera.mainCamera;
		
		if( nguiCam != null )
		{	
			// pos is the Vector3 representing the screen position of the input
			Ray inputRay = nguiCam.ScreenPointToRay( pos );    
			RaycastHit hit;
			
			return Physics.Raycast( inputRay.origin, inputRay.direction, out hit, Mathf.Infinity, UI_LAYER_MASK );			
		}
		
		return false;
	}
	
	public static bool IsPtInRect(float x, float y, float bx1, float by1, float bx2, float by2)
	{	
		return !(x > bx2 || y < by1 || x < bx1 || y > by2);
	}
	
	public static bool TestRectsHit(float ax1, float ay1, float ax2, float ay2, float bx1, float by1, float bx2, float by2)
	{	
		return !(ax1 > bx2 || ay1 > by2 || ax2 < bx1 || ay2 < by1);
	}
	
	public static int Max(int a, int b)
	{
		return (a > b) ? a : b;
	}
	
	public static int Min(int a, int b)
	{
		return (a < b) ? a : b;
	}
	
	public static AudioClip PickSoundRandomly(AudioClip[] clips)
	{
		if( clips == null || clips.Length < 1 )
		{
			return null;
		}
	
		int i = Random.Range(0, clips.Length);
		if( i >= clips.Length )
		{
			i = 0;
		}		
		return clips[i];
	}
	
	public static void PlaySoundRandomly(AudioSource audio, AudioClip[] clips)
	{
		if( audio != null && clips != null && clips.Length > 0 )
		{
			audio.PlayOneShot(PickSoundRandomly(clips));
		}
	}
}

using UnityEditor;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class EffectScaler : EditorWindow {

	[MenuItem("Tools/Scale Effects")]
	private static void ScaleEffects()  {
		EditorWindow.GetWindow<EffectScaler>();
	}
	
	void OnGUI()
	{
		GUI.Label(new Rect( 10, 10, 50, 25 ), "Ratio");
		scaleTextValue = GUI.TextField(new Rect( 50, 10, 50, 25 ), scaleTextValue);
		
		float scale = 1.0f;
		float.TryParse(scaleTextValue, out scale);
		
		if( GUI.Button(new Rect(10, 40, 70, 30), "Scale PS") )
		{
			foreach( Object obj in Selection.objects )
			{
				GameObject gameObj = obj as GameObject;
				if( gameObj != null )
				{
					ScalePS(gameObj, scale);
				}				
			}
		}
		
		if( GUI.Button(new Rect(80, 40, 70, 30), "Serz log") )
		{
			foreach( Object obj in Selection.objects )
			{
				GameObject gameObj = obj as GameObject;
				if( gameObj.particleSystem != null )
				{
					SerializedObject so = new SerializedObject(gameObj.particleSystem);
					SerializedProperty it = so.GetIterator();
					while (it.Next(true))
						Debug.Log (it.propertyPath);
						
					break;
				}
				
				LineRenderer lineRenderer = gameObj.GetComponent<LineRenderer>();
				if( lineRenderer != null )
				{
					SerializedObject so = new SerializedObject(lineRenderer);
					SerializedProperty it = so.GetIterator();
					while (it.Next(true))
						Debug.Log (it.propertyPath);
					
					break;
				}
			}
		}
	}
	
	public static void ScalePS(GameObject gameObj, float scale)
	{
		ParticleSystem ps = gameObj.particleSystem;
		if( ps != null )
		{
			ScalePSComponent(ps, scale);
		}
		
		Collider collider = gameObj.collider;
		if( collider != null )
		{
			if( collider is SphereCollider )
			{
				SphereCollider sphr = collider as SphereCollider;
				sphr.radius *= scale;
			}
			else if( collider is BoxCollider )
			{
				BoxCollider box = collider as BoxCollider;
				box.size *= scale;
			}
		}		
		
		LineRenderer lineRenderer = gameObj.GetComponent<LineRenderer>();
		if( lineRenderer != null )
		{
			ScaleLineRenderer(lineRenderer, scale);	
		}
		
		SFE_LaserEffect other = gameObj.GetComponent<SFE_LaserEffect>();
		if( other != null )
		{
			other.laserSize *= scale;
		}
		
		if( gameObj.GetComponent<MeshRenderer>() != null )
		{
			gameObj.transform.localScale *= scale;
		}
	
		foreach( Transform child in gameObj.transform )
		{
			ScalePSTransform(child.transform, scale);
			ScalePS(child.gameObject, scale);				
		}
	}
	
	private static void ScalePSTransform(Transform trans, float scale)
	{
		trans.localPosition = trans.localPosition * scale;
	}
	
	private static void ScaleLineRenderer(LineRenderer lr, float scale)
	{
		SerializedObject so = new SerializedObject(lr);
		
		so.FindProperty("m_Parameters.startWidth").floatValue *= scale;
		so.FindProperty("m_Parameters.endWidth").floatValue *= scale;
		
		so.ApplyModifiedProperties();
	}
	
	private static void ScalePSComponent(ParticleSystem ps, float scale)
	{
		ps.startSpeed *= scale;
		ps.startSize *= scale;
				
		SerializedObject so = new SerializedObject(ps);
		
		so.FindProperty("ShapeModule.boxX").floatValue *= scale;
		so.FindProperty("ShapeModule.boxY").floatValue *= scale;
		so.FindProperty("ShapeModule.boxZ").floatValue *= scale;
		so.FindProperty("ShapeModule.radius").floatValue *= scale;
		
		so.FindProperty("VelocityModule.x.scalar").floatValue *= scale;
		so.FindProperty("VelocityModule.y.scalar").floatValue *= scale;
		so.FindProperty("VelocityModule.z.scalar").floatValue *= scale;
		
		so.FindProperty("ForceModule.x.scalar").floatValue *= scale;
		so.FindProperty("ForceModule.y.scalar").floatValue *= scale;
		so.FindProperty("ForceModule.z.scalar").floatValue *= scale;
		
		so.FindProperty("ClampVelocityModule.x.scalar").floatValue *= scale;
		so.FindProperty("ClampVelocityModule.y.scalar").floatValue *= scale;
		so.FindProperty("ClampVelocityModule.z.scalar").floatValue *= scale;
		so.FindProperty("ClampVelocityModule.magnitude.scalar").floatValue *= scale;
		
		so.FindProperty("SizeModule.curve.scalar").floatValue *= scale;
		
		so.ApplyModifiedProperties();
	}
	
	private string scaleTextValue = "1.0";
}

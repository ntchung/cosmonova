using UnityEngine;
using UnityEditor;
using System.Collections;

public class Dummy : MonoBehaviour {

	void OnDrawGizmos()
	{
		Gizmos.color = Color.yellow;		
		Gizmos.DrawWireSphere(transform.position, 0.5f);
	}		
	
	void OnDrawGizmosSelected()
	{
		Gizmos.color = Color.yellow;		
		Gizmos.DrawSphere(transform.position, 0.5f);
	}
}

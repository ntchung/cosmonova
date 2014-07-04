using UnityEngine;
using System.Collections;

public class ShipTurretPoint : MonoBehaviour {

	public enum EDockPosition
	{
		PINPOINT = 0,
		TOP,
		BOTTOM,
		FORE,
		REAR,
		LEFT_SIDE,
		RIGHT_SIDE,
	}

	public EDockPosition DockPosition;
	public int Power;

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

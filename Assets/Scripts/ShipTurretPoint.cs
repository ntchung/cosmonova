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

	public GameObject bullet;

	public void Rotate(Quaternion quaternion)
	{

	}

	public void Shoot(int layer)
	{
		GameObject go = GameObject.Instantiate(bullet, transform.position, transform.rotation) as GameObject;
		go.layer = layer;
	}

	public void Shoot(Vector3 dir, int layer)
	{
		GameObject go = GameObject.Instantiate(bullet, transform.position, Quaternion.LookRotation(dir) * transform.localRotation) as GameObject;
		go.layer = layer;
	}

	void Update()
	{
	}

	void OnDrawGizmos()
	{
		//Gizmos.color = Color.yellow;		
		//Gizmos.DrawWireSphere(transform.position, 0.5f);
	}		
	
	void OnDrawGizmosSelected()
	{
		Gizmos.color = Color.yellow;		
		Gizmos.DrawSphere(transform.position, 0.5f);
	}
}

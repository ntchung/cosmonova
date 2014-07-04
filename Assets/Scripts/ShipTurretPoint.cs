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

	private float cooldown = 0.0f;

	public void Rotate(Quaternion quaternion)
	{

	}

	public void Shoot()
	{
		if (cooldown > 0.0f) return;

		GameObject go = GameObject.Instantiate(bullet, transform.position, transform.rotation) as GameObject;
		//go.transform.localScale = transform.localScale;

		cooldown = 0.2f;
	}

	void Update()
	{
		cooldown -= Time.deltaTime;
	}

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

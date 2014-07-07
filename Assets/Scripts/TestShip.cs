using UnityEngine;
using System.Collections;

public class TestShip : MonoBehaviour
{
	public int side;

	public Vector3 pos;

	public Vector3 forward;
	public Vector3 up;
	public Vector3 right;

	public float speed;

	public Vector3 size;

	// Use this for initialization
	void Start ()
	{
		Init();
	}

	protected void Init()
	{
		pos = transform.position;
		
		forward = transform.rotation * Vector3.forward;
		up = Vector3.up;
		right = Vector3.Cross(up, forward);
		
		speed = 0.0f;
	}

	// Update is called once per frame
	void Update ()
	{

	}

	public Vector3 GetRandomPos()
	{
		Vector3 res = pos;
		res += forward * Random.Range(-size.z / 2.0f, size.z / 2.0f);
		res += up * Random.Range(-size.y / 2.0f, size.y / 2.0f);
		res += right * Random.Range(-size.x / 2.0f, size.x / 2.0f);

		return res;
	}
}


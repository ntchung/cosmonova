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

	public float hp;
	public bool isDead;

	public GameObject explosion;

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

		isDead = false;
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

	void OnCollisionEnter(Collision collision) 
	{
		SFE_BulletController bulletController = collision.gameObject.GetComponent<SFE_BulletController>();
		if (bulletController != null)
		{
			hp -= bulletController.damage;
		}

		SFE_LaserController laserController = collision.gameObject.GetComponent<SFE_LaserController>();
		if (laserController != null)
		{
			hp -= laserController.damage;
		}

		if (hp <= 0.0f)
		{
			isDead = true;

			Instantiate(explosion, transform.position, transform.rotation);
			Destroy(gameObject);
		}
	}
}


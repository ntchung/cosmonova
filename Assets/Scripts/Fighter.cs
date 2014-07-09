using UnityEngine;
using System.Collections;

public class Fighter : TestShip
{
	float gunCooldown;
	bool canShoot;

	TestShip targetShip;
	TestShip guardShip;

	public ShipTurretPoint turret;

	float targetCooldown;

	// Use this for initialization
	void Start ()
	{
		Init();

		if (side == 0) guardShip = GameObject.Find("BattleCruiser").GetComponent<TestShip>();
		else guardShip = null;

		targetCooldown = 0.0f;
		FindTarget();

		gunCooldown = 0.0f;
		canShoot = true;
	}

	void FindTarget()
	{
		if (side == 1)
		{
			if (Random.Range(0, 2) == 0)
			{
				targetShip = GameObject.Find("BattleCruiser").GetComponent<TestShip>();
				return;
			}
		}

		GameObject root = GameObject.Find(side == 0 ? "Enemy" : "Me");
		int numTargets = root.transform.childCount;

		targetShip = null;
		float d = float.MaxValue;

		for (int i = 0; i < numTargets; i++)
		{
			TestShip curShip = root.transform.GetChild(i).GetComponent<TestShip>();

			Vector3 u = (side == 0 ? curShip.pos - guardShip.pos : curShip.pos - pos);
			u.y = 0.0f;

			float l = u.magnitude;
			if (l < d)
			{
				d = l;
				targetShip = curShip;
			}
		}

		if (targetShip == null) targetShip = guardShip;

		targetCooldown = Random.Range(8.0f, 10.0f);
	}

	// Update is called once per frame
	void Update ()
	{
		targetCooldown -= Time.deltaTime;
		if (targetCooldown <= 0.0f || (targetShip != null && targetShip.isDead))
		{
			FindTarget();
		}

		gunCooldown -= Time.deltaTime;
		if (gunCooldown <= 0.0f)
		{
			canShoot = true;
			gunCooldown = 1.0f;
		}
		else canShoot = false;

		Vector3 targetPos = targetShip.pos;
		Vector3 dist = targetPos - pos;

		if (canShoot && (targetShip != guardShip))
		{
			if (dist.magnitude < 300.0f)
			{
				Vector3 shootDir = Vector3.zero;

				Fighter fighter = targetShip as Fighter;
				if (fighter != null)
				{
					shootDir = targetPos + targetShip.forward * targetShip.speed * 1.0f - turret.transform.position;
				}
				else 
				{
					Vector3 target = targetShip.GetRandomPos();
					shootDir = target + targetShip.forward * targetShip.speed * 1.0f - turret.transform.position;
				}

				shootDir.Normalize();

				//if (Vector3.Dot(forward, shootDir) > 0.707f)
				if (Vector3.Dot(forward, shootDir) > 0.5f)
				{
					gunCooldown = 1.2f;
					turret.Shoot(shootDir, gameObject.layer);
					if( audio != null && Global.audioCount < 10 && Vector3.Distance(Camera.main.transform.localPosition, this.transform.localPosition) < 1000.0f )
					{
						++Global.audioCount;
						audio.Play();
					}
				}
			}
		}

		Vector3 futurePos = Vector3.zero;

		bool hasTarget = false;

		if (guardShip != null)
		{
			Vector3 guardPos = guardShip.pos;
			guardPos.y = pos.y;

			if ((guardPos - pos).magnitude > 300.0f)
			{
				futurePos = guardPos;
				hasTarget = true;
			}
		}

		if (!hasTarget)
		{
			targetPos.y = pos.y;
			dist.y = 0.0f;

			//float T = dist.magnitude / maxVel;
			//futurePos = targetPos + targetShip.forward * targetShip.speed * T;

			futurePos = targetPos;
		}

		Vector3 newForward = (futurePos - pos).normalized;
		forward = Vector3.RotateTowards(forward, newForward, turnSpeed * Time.deltaTime, 0.0F);
		right = Vector3.Cross(up, forward);

		speed = maxVel;

		pos += forward * speed * Time.deltaTime; 

		transform.position = pos;
		transform.rotation = Quaternion.LookRotation(forward);
	}
}


using UnityEngine;
using System.Collections;

public class Fighter : TestShip
{
	const float turnSpeed = Mathf.PI / 4.0f;
	const float maxVel = 50.0f;

	float gunCooldown;
	bool canShoot;

	TestShip targetShip;
	TestShip guardShip;

	public ShipTurretPoint turret;

	// Use this for initialization
	void Start ()
	{
		Init();

		GameObject root = GameObject.Find(side == 0 ? "Enemy" : "Me");
		int numTargets = root.transform.childCount;
		int targetIndex = Random.Range(0, numTargets);
		targetShip = root.transform.GetChild(targetIndex).GetComponent<TestShip>();

		if (side == 0) guardShip = GameObject.Find("BattleCruiser").GetComponent<TestShip>();
		else
		{
			targetShip = GameObject.Find("BattleCruiser").GetComponent<TestShip>();
			guardShip = null;
		}

		gunCooldown = 0.0f;
		canShoot = true;
	}

	// Update is called once per frame
	void Update ()
	{
		gunCooldown -= Time.deltaTime;
		if (gunCooldown <= 0.0f)
		{
			canShoot = true;
			gunCooldown = 0.0f;
		}
		else canShoot = false;

		Vector3 targetPos = targetShip.pos;
		Vector3 dist = targetPos - pos;

		if (canShoot)
		{
			if (dist.magnitude < 300.0f)
			{
				Vector3 target = targetShip.GetRandomPos();
				Vector3 shootDir = targetPos - turret.transform.position;

				Fighter fighter = targetShip as Fighter;
				if (fighter != null)
				{
					shootDir = targetPos + targetShip.forward * targetShip.speed * 1.0f - fighter.turret.transform.position;
				}

				shootDir.Normalize();

				//if (Vector3.Dot(forward, shootDir) > 0.707f)
				if (Vector3.Dot(forward, shootDir) > 0.0f)
				{
					gunCooldown = 0.2f;
					turret.Shoot(shootDir, gameObject.layer);
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


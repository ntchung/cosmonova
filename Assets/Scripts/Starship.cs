using UnityEngine;
using System.Collections;

public class Starship : MonoBehaviour 
{
	private enum State
	{
		TRAVELLING = 0,
		STOPPING,
		DOCKING,
		UNDOCKING,
		LANDING,
		ORBITING
	};

	private State state;

	private const float maxSpeed = 500.0f;
	private const float maxForce = 100.0f;

	public Thruster[] thrusters;
	public ShipTurretPoint[] barrels;

	public Camera camera;
	public GameObject turret;

	private Vector3 forward;
	private float speed;

	private Vector3 position;

	private int targetPlanetId;
	private int landingPlanetId;
	private float landingTime;

	private Sector sector;

	// Use this for initialization
	void Start () 
	{
		sector = Sector.Instance;

		state = State.TRAVELLING;
		targetPlanetId = UnityEngine.Random.Range(0, 8);

		forward = Vector3.right;
		speed = 0.0f;
	}

	// Update is called once per frame
	void Update () 
	{
		position = transform.position;

		bool thrusterOn = false;

		targetPlanetId = sector.selectedPlanet - 1;

		if (state == State.TRAVELLING)
		{
			Vector3 target = Vector3.zero;

			if (targetPlanetId >= 0)
			{
				target = sector.GetPlanetPosition(targetPlanetId);
			}

			Vector3 desired = target - position;
			desired.y = 0.0f;

			float d = desired.magnitude;

			desired.Normalize();

			if (targetPlanetId >= 0.0f)
			{
				float planetRadius = sector.GetPlanetRadius(targetPlanetId);

				if (d < planetRadius * 0.5f)
				{
					landingPlanetId = targetPlanetId;
					state = State.DOCKING;
				} 
				else if (d < planetRadius * 2.0f)
				{
					float m = d / (2.0f * planetRadius) * maxSpeed;
					desired *= m;
				}
				else desired *= maxSpeed;
			}
			else 
			{
				desired *= maxSpeed;

				if (d < 400.0f)
				{
					state = State.ORBITING;
				}
			}

			Vector3 steer = desired - forward * speed;
			steer = Vector3.ClampMagnitude(steer, maxForce);

			Vector3 velocity = forward * speed + steer * Time.deltaTime;
			speed = velocity.magnitude;
			forward = velocity / speed;
			position += velocity * Time.deltaTime;

			thrusterOn = true;

			if (sector.stopping && state == State.TRAVELLING)
			{
				state = State.STOPPING;
			}
		}
		else if (state == State.ORBITING)
		{
			Vector3 target = -position;
			target.y = 0.0f;

			target.Normalize();
			Vector3 tangent = target;
			tangent = new Vector3(-tangent.z, 0.0f, tangent.x);

			target *= 400.0f;

			target.y = position.y;

			if (Vector3.Cross(forward, position).y > 0.0f) tangent = -tangent;

			speed = maxSpeed * 0.01f;
			forward = tangent;
			position += speed * forward * Time.deltaTime;
			
			thrusterOn = true;

			if (targetPlanetId >= 0.0f)
			{
				state = State.TRAVELLING;
			}
		}
		else if (state == State.STOPPING)
		{
			speed = 0.0f;
			thrusterOn = false;

			if (!sector.stopping)
			{
				state = State.TRAVELLING;
			}
		}
		else if (state == State.DOCKING)
		{
			Vector3 target = sector.GetPlanetPosition(landingPlanetId);

			float planetRadius = sector.GetPlanetRadius(landingPlanetId);
			target.y += planetRadius;

			Vector3 desired = target - position;

			float maxD = maxSpeed * 0.2f * Time.deltaTime;
			float d = desired.magnitude;

			if (d < maxD)
			{
				landingTime = 0.0f;
				state = State.LANDING;
			}
			else d = maxD;

			desired.Normalize();
			desired *= d;

			speed = 0.0f;
			position += desired;

			thrusterOn = true;

			if (targetPlanetId != landingPlanetId)
			{
				state = State.UNDOCKING;
			}
		}
		else if (state == State.UNDOCKING)
		{
			Vector3 target = sector.GetPlanetPosition(landingPlanetId);
			
			float planetRadius = sector.GetPlanetRadius(landingPlanetId);
			target.y += 200.0f;
			
			Vector3 desired = target - position;
			
			float maxD = maxSpeed * 0.2f * Time.deltaTime;
			float d = desired.magnitude;
			
			if (d < maxD)
			{
				state = State.TRAVELLING;
			}
			else d = maxD;

			desired.Normalize();
			desired *= d;
			
			speed = 0.0f;
			position += desired;
			
			thrusterOn = true;

			if (targetPlanetId == landingPlanetId)
			{
				state = State.DOCKING;
			}
		}
		else if (state == State.LANDING)
		{
			landingTime += Time.deltaTime;

			speed = 0.0f;
			thrusterOn = false;

			if (targetPlanetId != landingPlanetId)
			{
				state = State.UNDOCKING;
			}
		}

		if (thrusterOn)
		{
			foreach (Thruster thruster in thrusters) 
			{
				thruster.StartThruster();
			}
		}
		else 
		{		
			foreach (Thruster thruster in thrusters) 
			{
				thruster.StopThruster();
			}
		}

		transform.position = position;
		transform.rotation = Quaternion.LookRotation(forward);

		UpdateShoot();
	}

	private void UpdateShoot()
	{
		Vector3 targetPoint = GetTargetPoint();
		Vector3 forward = (targetPoint - turret.transform.position).normalized;

		Quaternion quaternion = Quaternion.LookRotation(forward, Vector3.up);
		turret.transform.rotation = quaternion;

		if (Input.GetMouseButton(0))
		{
			Shoot();
		}
	}

	private void Shoot()
	{
		foreach (ShipTurretPoint barrel in barrels)
		{
			barrel.Shoot();
		}
	}

	public Vector3 GetTargetPoint()
	{
		return camera.ScreenToWorldPoint(new Vector3(camera.pixelWidth / 2.0f, camera.pixelHeight / 2.0f, camera.farClipPlane));
	}
}

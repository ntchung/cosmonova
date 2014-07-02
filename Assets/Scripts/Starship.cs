using UnityEngine;
using System.Collections;

public class Starship : MonoBehaviour 
{
	private enum State
	{
		TRAVELLING = 0,
		TRAVELLING_PAUSE,
		DOCKING,
		UNDOCKING,
		LANDING,
		ORBITING
	};

	private State state;

	private const float maxSpeed = 500.0f;
	private const float maxForce = 100.0f;

	public Thruster[] thrusters;

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

		if (state == State.TRAVELLING)
		{
			Vector3 target = sector.GetPlanetPosition(targetPlanetId);

			Vector3 desired = target - position;
			desired.y = 0.0f;

			float d = desired.magnitude;

			desired.Normalize();

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

			Vector3 steer = desired - forward * speed;
			steer = Vector3.ClampMagnitude(steer, maxForce);

			Vector3 velocity = forward * speed + steer * Time.deltaTime;
			speed = velocity.magnitude;
			forward = velocity / speed;
			position += velocity * Time.deltaTime;

			thrusterOn = true;
		}
		else if (state == State.DOCKING)
		{
			Vector3 target = sector.GetPlanetPosition(targetPlanetId);

			float planetRadius = sector.GetPlanetRadius(targetPlanetId);
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
		}
		else if (state == State.LANDING)
		{
			landingTime += Time.deltaTime;
			if (landingTime > 5.0f)
			{
				state = State.UNDOCKING;

				for (;;)
				{
					int nextTargetPlanetId = UnityEngine.Random.Range(0, 8);
					if (nextTargetPlanetId != targetPlanetId)
					{
						targetPlanetId = nextTargetPlanetId;
						break;
					}
				}
			}

			thrusterOn = false;
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
	}
}

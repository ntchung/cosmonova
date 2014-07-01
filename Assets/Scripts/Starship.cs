using UnityEngine;
using System.Collections;

public class Starship : MonoBehaviour 
{
	private const float maxMoveSpeed = 1000.0f;
	private const float moveAccel = 200.0f;

	private const float turnMinRate = Mathf.PI / 10.0f;
	private const float turnMaxRate = Mathf.PI / 5.0f;

	private const float turnAccel = Mathf.PI / 10.0f;

	public Thruster[] thrusters;

	private Vector3 forward;

	private float moveSpeed;
	private float targetMoveSpeed;

	private float turnSpeed;
	private float targetTurnSpeed;

	// Use this for initialization
	void Start () 
	{
		forward = Vector3.right;
		moveSpeed = targetMoveSpeed = 0.0f;

		turnSpeed = targetTurnSpeed = 0.0f;
	}
	
	// Update is called once per frame
	void Update () 
	{
		float moveDir = Input.GetAxis("Vertical");

		if (moveDir > 0.0f)
		{
			foreach (Thruster thruster in thrusters) 
			{
				thruster.StartThruster();
			}
			
			targetMoveSpeed = maxMoveSpeed;
		}
		else 
		{		
			foreach (Thruster thruster in thrusters) 
			{
				thruster.StopThruster();
			}

			targetMoveSpeed = 0.0f;
		}

		float turnDir = Input.GetAxis("Horizontal");

		if (turnDir != 0.0f)
		{
			targetTurnSpeed = Mathf.Sign(turnDir) * Mathf.Lerp(turnMinRate, turnMaxRate, (moveSpeed / maxMoveSpeed));
		}
		else 
		{
			targetTurnSpeed = 0.0f;
		}

		float diffTurn = targetTurnSpeed - turnSpeed;
		float maxDiffTurn = turnAccel * Time.deltaTime;
		if (diffTurn > maxDiffTurn) diffTurn = maxDiffTurn;
		else if (diffTurn < -maxDiffTurn) diffTurn = -maxDiffTurn;

		turnSpeed += diffTurn;

		forward = Quaternion.AngleAxis(Mathf.Rad2Deg * turnSpeed * Time.deltaTime, Vector3.up) * forward;

		float diffSpeed = targetMoveSpeed - moveSpeed;
		float maxDiffSpeed = moveAccel * Time.deltaTime;
		if (diffSpeed > maxDiffSpeed) diffSpeed = maxDiffSpeed;
		else if (diffSpeed < -maxDiffSpeed) diffSpeed = -maxDiffSpeed;

		moveSpeed += diffSpeed;

		Vector3 pos = transform.position;
		pos += forward * moveSpeed * Time.deltaTime;
		transform.position = pos;
		transform.rotation = Quaternion.LookRotation(forward);
	}
}

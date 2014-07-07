using UnityEngine;

public struct Matrix
{
	public Vector3 rvec;
	public Vector3 uvec;
	public Vector3 fvec;

	public void SetZero()
	{
		rvec = Vector3.zero;
		uvec = Vector3.zero;
		fvec = Vector3.zero;
	}

	public void SetIdentity()
	{
		rvec = Vector3.right;
		uvec = Vector3.up;
		fvec = Vector3.forward;
	}

	public void Copy(ref Matrix other)
	{
		rvec = other.rvec;
		uvec = other.uvec;
		fvec = other.fvec;
	}
}


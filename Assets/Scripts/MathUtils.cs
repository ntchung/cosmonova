using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public static class MathUtils
{
	public static float frand()
	{
		return UnityEngine.Random.Range(0.0f, 1.0f);
	}

	public static void assert(bool condition)
	{
		if (!condition) Debug.LogError("Failed!");
	}

	public static Vector3 vm_vec_rotate(ref Vector3 src, ref Matrix m)
	{
		return new Vector3(Vector3.Dot(src, m.rvec), Vector3.Dot(src, m.uvec), Vector3.Dot(src, m.fvec));
	}

	public static Vector3 vm_vec_unrotate(ref Vector3 src, ref Matrix m)
	{
		return new Vector3(
			src.x * m.rvec.x + src.y * m.uvec.x + src.z * m.fvec.x,
			src.x * m.rvec.y + src.y * m.uvec.y + src.z * m.fvec.y,
			src.x * m.rvec.z + src.y * m.uvec.z + src.z * m.fvec.z
			);
	}

	public static float vm_vec_copy_normalize(out Vector3 dest, ref Vector3 src)
	{
		float m = src.magnitude;

		if (m <= 0.0f) 
		{
			Debug.LogWarning("Null vec3d in vec3d normalize");

			dest = Vector3.right;		
			return 1.0f;
		}
		
		float im = 1.0f / m;
		dest = src * im;

		return m;
	}

	public static float vm_vec_normalize(ref Vector3 v)
	{
		float t = vm_vec_copy_normalize(out v, ref v);
		return t;
	}

	public static void vm_orthogonalize_matrix(ref Matrix src)
	{
		Matrix m = new Matrix();
		
		if (vm_vec_copy_normalize(out m.fvec, ref src.fvec) == 0.0f) 
		{
			Debug.LogError("forward vec should not be zero-length");
		}
		
		float umag = src.uvec.magnitude;
		float rmag = src.rvec.magnitude;

		if (umag <= 0.0f) 
		{  
			if (rmag <= 0.0f) 
			{
				if (m.fvec.x == 0.0f && m.fvec.z == 0.0f && m.fvec.y != 0.0f)
				{
					m.uvec = Vector3.forward;
				}
				else
				{
					m.uvec = Vector3.up;
				}
			} 
			else 
			{  
				m.uvec = Vector3.Cross(m.fvec, src.rvec);

				if (vm_vec_normalize(ref m.uvec) == 0.0f)
				{
					Debug.LogError("Bad vector!");
				}
			}
		} 
		else 
		{  
			vm_vec_copy_normalize(out m.uvec, ref src.uvec);
		}
		
		m.rvec = Vector3.Cross(m.uvec, m.fvec);

		if (vm_vec_normalize(ref m.rvec) == 0.0f)
		{
			Debug.LogError("Bad vector!");
		}
		
		m.uvec = Vector3.Cross(m.fvec, m.rvec);
		src.Copy(ref m);
	}

	public static float vm_vec_dot3(float x, float y, float z, ref Vector3 v)
	{
		return x * v.x + y * v.y + z * v.z;
	}

	public static Matrix sincos_2_matrix(float sinp, float cosp, float sinb, float cosb, float sinh, float cosh)
	{
		float sbsh = sinb * sinh;
		float cbch = cosb * cosh;
		float cbsh = cosb * sinh;
		float sbch = sinb * cosh;

		Matrix m = new Matrix();

		m.rvec = new Vector3(cbch + sinp * sbsh, sinb * cosp, sinp * sbch - cbsh);
		m.uvec = new Vector3(sinp * cbsh - sbch, cosb * cosp, sbsh + sinp * cbch);
		m.fvec = new Vector3(sinh * cosp, -sinp, cosh * cosp);

		return m;
	}

	public static Matrix vm_angles_2_matrix(ref Angles a)
	{
		float sinp = Mathf.Sin(a.pitch);
		float cosp = Mathf.Cos(a.pitch);

		float sinb = Mathf.Sin(a.bank);
		float cosb = Mathf.Cos(a.bank);

		float sinh = Mathf.Sin(a.heading);
		float cosh = Mathf.Cos(a.heading);
		
		return sincos_2_matrix(sinp, cosp, sinb, cosb, sinh, cosh);
	}

	public static Matrix vm_matrix_x_matrix(ref Matrix src0, ref Matrix src1)
	{
		Matrix m = new Matrix();

		m.rvec = new Vector3(
			vm_vec_dot3(src0.rvec.x, src0.uvec.x, src0.fvec.x, ref src1.rvec),
			vm_vec_dot3(src0.rvec.y, src0.uvec.y, src0.fvec.y, ref src1.rvec),
			vm_vec_dot3(src0.rvec.z, src0.uvec.z, src0.fvec.z, ref src1.rvec)
			);

		m.uvec = new Vector3(
			vm_vec_dot3(src0.rvec.x, src0.uvec.x, src0.fvec.x, ref src1.uvec),
			vm_vec_dot3(src0.rvec.y, src0.uvec.y, src0.fvec.y, ref src1.uvec),
			vm_vec_dot3(src0.rvec.z, src0.uvec.z, src0.fvec.z, ref src1.uvec)
			);

		m.fvec = new Vector3(
			vm_vec_dot3(src0.rvec.x, src0.uvec.x, src0.fvec.x, ref src1.fvec),
			vm_vec_dot3(src0.rvec.y, src0.uvec.y, src0.fvec.y, ref src1.fvec),
			vm_vec_dot3(src0.rvec.z, src0.uvec.z, src0.fvec.z, ref src1.fvec)
			);

		return m;
	}

	public static bool IsVelNullSqSafe(ref Vector3 v)
	{
		return Mathf.Abs(v.x) < 1e-16f && Mathf.Abs(v.y) < 1e-16f && Mathf.Abs(v.z) < 1e-16f;
	}

	public static void VecAdd(out Vector3 dest, ref Vector3 src0, ref Vector3 src1)
	{
		dest = src0 + src1;
	}

	public static void VecSub(out Vector3 dest, ref Vector3 src0, ref Vector3 src1)
	{
		dest = src0 - src1;
	}

	public static void VecAdd2(ref Vector3 dest, ref Vector3 src)
	{
		dest += src;
	}
	
	public static void VecSub2(ref Vector3 dest, ref Vector3 src)
	{
		dest -= src;
	}

	public static void VecScale(ref Vector3 dest, float s)
	{
		dest *= s;
	}

	public static void VecCopyScale(out Vector3 dest, ref Vector3 src, float s)
	{
		dest = src * s;
	}

	public static void VecScaleAdd(out Vector3 dest, ref Vector3 src1, ref Vector3 src2, float k)
	{
		dest = src1 + src2 * k;
	}

	public static void VecScaleSub(out Vector3 dest, ref Vector3 src1, ref Vector3 src2, float k)
	{
		dest = src1 - src2 * k;
	}

	public static void VecScaleAdd2(ref Vector3 dest, ref Vector3 src, float k)
	{
		dest += src * k;
	} 

	public static void VecScaleSub2(ref Vector3 dest, ref Vector3 src, float k)
	{
		dest -= src * k;
	} 

	public static void VecScale2(ref Vector3 dest, float n, float d)
	{
		d = 1.0f / d;
		dest *= n * d;
	}

	public static float VecDotProd(ref Vector3 v0, ref Vector3 v1)
	{
		return Vector3.Dot(v0, v1);
	}

	public static float VecDot3(float x, float y, float z, ref Vector3 v)
	{
		return x * v.x + y * v.y + z * v.z;
	}

	public static float VecMag(ref Vector3 v)
	{
		return v.magnitude;
	}
		
	public static float VecMagSquared(ref Vector3 v)
	{
		return v.sqrMagnitude;
	}

	public static float VecDistSquared(ref Vector3 v0, ref Vector3 v1)
	{
		return (v0 - v1).sqrMagnitude;
	}
	
	public static float VecDist(ref Vector3 v0, ref Vector3 v1)
	{
		return (v0 - v1).magnitude;
	}
	
	public static float VecMagQuick(ref Vector3 v)
	{
		float a = Mathf.Abs(v.x);
		float b = Mathf.Abs(v.y);
		float c = Mathf.Abs(v.z);

		if (a < b) 
		{
			float temp=a; 
			a=b; 
			b=temp;
		}
		
		if (b < c) 
		{
			float temp=b; 
			b=c; 
			c=temp;
			
			if (a < b) 
			{
				float temp2=a; 
				a=b; 
				b=temp2;
			}
		}
		
		float bc = (b * 0.25f) + (c * 0.125f);
		return a + bc + (bc * 0.5f);
	}
	
	public static float VecDistQuick(ref Vector3 v0, ref Vector3 v1)
	{
		Vector3 t;
		VecSub(out t, ref v0, ref v1);

		return VecMagQuick(ref t);
	}
		
	public static float VecCopyNormalize(out Vector3 dest, ref Vector3 src)
	{
		float m = VecMag(ref src);
		
		if (m <= 0.0f) 
		{
			Debug.LogWarning("Null Vector3 in Vector3 normalize.\n");
			
			dest = Vector3.right;
			return 1.0f;
		}
		
		float im = 1.0f / m;
		
		dest = src * im;
		return m;
	}
	
	public static float VecNormalize(ref Vector3 v)
	{
		float t = VecCopyNormalize(out v, ref v);
		return t;
	}
	
	public static float VecNormalizeSafe(ref Vector3 v)
	{
		float m = VecMag(ref v);
		
		if (m <= 0.0f) 
		{
			v = Vector3.right;
			return 1.0f;
		}
		
		float im = 1.0f / m;
		
		v *= im;
		return m;
	}
	
	public static float VecIMag(ref Vector3 v)
	{
		return 1.0f / v.magnitude;
	}
	
	public static float VecCopyNormalizeQuick(ref Vector3 dest, ref Vector3 src)
	{
		float im = VecIMag(ref src);
		assert(im > 0.0f);
		
		dest = src * im;
		return 1.0f / im;
	}
	
	public static float VecNormalizeQuick(ref Vector3 src)
	{
		float im = VecIMag(ref src);
		assert(im > 0.0f);
		
		src *= im;
		return 1.0f/im;
	}
	
	public static float VecCopyNormalizeQuickMag(ref Vector3 dest, ref Vector3 src)
	{
		float m = VecMagQuick(ref src);
		assert(m > 0.0f);
		
		float im = 1.0f / m;
		dest = src * im;		
		
		return m;
	}
	
	public static float VecNormalizeQuickMag(ref Vector3 v)
	{
		float m = VecMagQuick(ref v);
		assert(m > 0.0f);
		
		v *= m;
		return m;
	}

	public static void VecRandVecQuick(out Vector3 rvec)
	{
		rvec = new Vector3((frand() - 0.5f) * 2.0f, (frand() - 0.5f) * 2.0f, (frand() - 0.5f) * 2.0f);
		if (IsVelNullSqSafe(ref rvec)) rvec.x = 1.0f;
		
		VecNormalizeQuick(ref rvec);
	}
}
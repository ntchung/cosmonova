Shader "Custom/ShipTeamColorVeryFar" {
Properties
	{
		_TeamColor ("Team base Color (RGB)", Color) = (1.0, 0.0, 0.0, 1.0)
	}
	
	SubShader
	{
		Tags
		{
			"RenderType" = "Opaque"
			"Queue" = "Geometry"
			"LightMode" = "Vertex"
			"ForceNoShadowCasting" = "True"
			"IgnoreProjector" = "True"
		}
		
		Cull Back
		Lighting Off
		Fog { Mode Off }
		ZWrite Off
		ZTest On
		ColorMask RGB

		Pass
		{
			CGPROGRAM
				#pragma vertex vert
				#pragma fragment frag
				
				#include "UnityCG.cginc"
	
				struct appdata_t
				{
					float4 vertex : POSITION;
				};
	
				struct v2f
				{
					float4 vertex : SV_POSITION;
				};
	
				fixed4 _TeamColor;
				
				v2f vert (appdata_t v)
				{
					v2f o;
					o.vertex = mul(UNITY_MATRIX_MVP, v.vertex);						
					return o;
				}
				
				fixed4 frag (v2f i) : COLOR
				{
					return _TeamColor;
				}
			ENDCG
		}
	}
}

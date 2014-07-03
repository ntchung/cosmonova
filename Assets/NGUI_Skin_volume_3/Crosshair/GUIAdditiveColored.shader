﻿Shader "Custom/GUIAdditiveColored" {
	Properties
	{
		_MainTex ("Base (RGB)", 2D) = "white" {}
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
		
		Cull Off
		Lighting Off
		Fog { Mode Off }
		ZWrite Off
		ZTest Off
		Blend One One
		ColorMask RGB

		Pass
		{
			CGPROGRAM
				#pragma vertex vert
				#pragma fragment frag
				
				#include "UnityCG.cginc"
	
				struct appdata_t
				{
					fixed4 vertex : POSITION;
					fixed2 texcoord : TEXCOORD0;
					fixed4 color : Color;
				};
	
				struct v2f
				{
					fixed4 vertex : SV_POSITION;
					fixed2 texcoord : TEXCOORD0;
					fixed4 color : Color;
				};
	
				sampler2D _MainTex;
				
				v2f vert (appdata_t v)
				{
					v2f o;
					o.vertex = mul(UNITY_MATRIX_MVP, v.vertex);
					o.texcoord = v.texcoord;						
					o.color = v.color;
					return o;
				}
				
				fixed4 frag (v2f i) : COLOR
				{
					fixed4 col = tex2D(_MainTex, i.texcoord) * i.color;
					return col;
				}
			ENDCG
		}
	}
}

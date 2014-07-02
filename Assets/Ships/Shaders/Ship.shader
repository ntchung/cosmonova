Shader "Custom/Ship" {
	Properties
	{
		_MainTex ("Base (RGB)", 2D) = "white" {}
		_LightDirection ("Light direction", Vector) = (1.0, 1.0, -1.0, 1.0)
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
		ZWrite On
		ZTest On

		Pass
		{
			CGPROGRAM
				#pragma vertex vert
				#pragma fragment frag
				
				#include "UnityCG.cginc"
	
				struct appdata_t
				{
					float4 vertex : POSITION;
					half2 texcoord : TEXCOORD0;
					fixed3 normal : NORMAL;
				};
	
				struct v2f
				{
					float4 vertex : SV_POSITION;
					half2 texcoord : TEXCOORD0;
					fixed3 normal : TEXCOORD1;
				};
	
				sampler2D _MainTex;
				fixed4 _LightDirection;
				
				v2f vert (appdata_t v)
				{
					v2f o;
					o.vertex = mul(UNITY_MATRIX_MVP, v.vertex);
					o.texcoord = v.texcoord;	
					o.normal = v.normal;									
					return o;
				}
				
				fixed4 frag (v2f i) : COLOR
				{
					fixed3 n =  i.normal; 
					
					//Angle to the light
					fixed d = max( dot (n, _LightDirection), 0.0);
                	fixed lv = min( d  + 0.5, 1.0 ); 
                	
					fixed4 col = tex2D(_MainTex, i.texcoord) * lv;
					return col;
				}
			ENDCG
		}
	}
}

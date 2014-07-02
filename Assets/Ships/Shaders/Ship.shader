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
					fixed3 color : COLOR;
					half2 texcoord : TEXCOORD0;
					fixed3 specColor : TEXCOORD1;
				};
	
				sampler2D _MainTex;
				fixed4 _LightDirection;
				
				v2f vert (appdata_t v)
				{
					v2f o;
					o.vertex = mul(UNITY_MATRIX_MVP, v.vertex);
					o.texcoord = v.texcoord;	
					
					// Vertex lighting
				    fixed lightIntensity = max(dot(v.normal, _LightDirection), 0.25);
				    
				    // Specular
				    float4x4 modelMatrix = _Object2World;
				    fixed4 camPos = fixed4(_WorldSpaceCameraPos, 1.0);
				    fixed4 localPos = mul(modelMatrix, v.vertex);
				    fixed3 viewDirection = normalize(camPos - localPos);
               
					fixed3 r = reflect(-_LightDirection, v.normal);
					fixed dt = max(0.0, dot(r, viewDirection));
					fixed shine = pow(dt, 2.0);			
               
               		// Color
               		o.color = saturate(fixed3(1.0, 1.0, 1.0) * lightIntensity);
               		o.specColor = fixed3(0.25, 0.25, 0.25) * shine;																		
					return o;
				}
				
				fixed4 frag (v2f i) : COLOR
				{
					fixed4 col = tex2D(_MainTex, i.texcoord);
					col.rgb = col.rgb * i.color + i.specColor;
					return col;
				}
			ENDCG
		}
	}
}

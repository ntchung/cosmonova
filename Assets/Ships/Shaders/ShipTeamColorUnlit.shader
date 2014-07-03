Shader "Custom/ShipTeamColorUnlit" {
	Properties
	{
		_MainTex ("Base (RGB)", 2D) = "white" {}
		_GlowTex ("Glow (RGB)", 2D) = "white" {}
		_TeamColor ("Team base Color (RGB)", Color) = (1.0, 0.0, 0.0, 1.0)
		_StripeColor ("Stripe Color (RGB)", Color) = (1.0, 1.0, 0.0, 1.0)
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
					half2 texcoord : TEXCOORD0;
				};
	
				struct v2f
				{
					float4 vertex : SV_POSITION;
					half2 texcoord : TEXCOORD0;		
				};
	
				sampler2D _MainTex;
				sampler2D _GlowTex;
				fixed4 _TeamColor;
				fixed4 _StripeColor;
				
				v2f vert (appdata_t v)
				{
					v2f o;
					o.vertex = mul(UNITY_MATRIX_MVP, v.vertex);
					o.texcoord = v.texcoord;	
					return o;
				}
				
				fixed3 frag (v2f i) : COLOR
				{
					fixed4 diff = tex2D(_MainTex, i.texcoord);					
					fixed4 glow = tex2D(_GlowTex, i.texcoord);
					
					fixed4 base = diff + 0.5;
					fixed4 teamBaseColour = base * _TeamColor;
					fixed4 teamStripeColour = base * _StripeColor;
					
					base = diff - 0.5;
					teamBaseColour = teamBaseColour + base;
					teamStripeColour = teamStripeColour + base;
					
					base.rgb = lerp(teamBaseColour, diff, diff.a);
					base.rgb = lerp(teamStripeColour, base, glow.a);
					
					// Output
					return base.rgb;
				}
			ENDCG
		}
	}
}

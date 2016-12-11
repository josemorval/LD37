// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'

Shader "LineShader"
{

	SubShader
	{

		Tags {"Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent"}
		LOD 100
		
		ZWrite Off
		Blend SrcAlpha OneMinusSrcAlpha 

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag

			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				float4 posWorld: TEXCOORD1;
				float4 vertex : SV_POSITION;
			};
			
			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.posWorld = mul(unity_ObjectToWorld,v.vertex);
				o.uv = v.uv;
				return o;
			}
			
			float4 frag (v2f i) : SV_Target
			{
				float f = i.uv.x-0.5;
				float f2 = exp(-1.0*f*f)*(0.8+0.3*cos(200.0*i.uv.y-1.0*_Time.y));
				f2 = 1.0;
				float4 f3 = 0.9*f2;// + 0.5*lerp(float4(0.1,0.1,1.0,1.0),float4(0.0,0.0,0.0,0.0),exp(-20.0*f*f));
				return f3*smoothstep(0.0,0.2,i.uv.y)*(1.0-smoothstep(0.9,1.0,i.uv.y));
			}
			ENDCG
		}
	}
}

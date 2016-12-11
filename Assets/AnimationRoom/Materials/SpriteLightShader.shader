// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'

Shader "SpriteLightShader"
{
	Properties
	{
		[PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}
		_Color ("Tint", Color) = (1,1,1,1)
		[MaterialToggle] PixelSnap ("Pixel snap", Float) = 0
	}

	SubShader
	{
		Tags
		{ 
			"Queue"="Transparent" 
			"IgnoreProjector"="True" 
			"RenderType"="Transparent" 
			"PreviewType"="Plane"
			"CanUseSpriteAtlas"="True"
		}

		Cull Off
		Lighting Off
		ZWrite Off
		Blend One OneMinusSrcAlpha

		Pass
		{
		CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma target 2.0
			#pragma multi_compile _ PIXELSNAP_ON
			#pragma multi_compile _ ETC1_EXTERNAL_ALPHA
			#include "UnityCG.cginc"
			
			struct appdata_t
			{
				float4 vertex   : POSITION;
				float4 color    : COLOR;
				float2 texcoord : TEXCOORD0;
			};

			struct v2f
			{
				float4 posWorld : TEXCOORD1;
				float4 vertex   : SV_POSITION;
				fixed4 color    : COLOR;
				float2 texcoord  : TEXCOORD0;
			};
			
			fixed4 _Color;
			float _Switch;

			v2f vert(appdata_t IN)
			{
				v2f OUT;
				OUT.vertex = UnityObjectToClipPos(IN.vertex);
				OUT.texcoord = IN.texcoord;
				OUT.color = IN.color * _Color;
				#ifdef PIXELSNAP_ON
				OUT.vertex = UnityPixelSnap (OUT.vertex);
				#endif

				OUT.posWorld = mul(unity_ObjectToWorld,IN.vertex);

				return OUT;
			}

			sampler2D _MainTex;
			sampler2D _AlphaTex;

			fixed4 SampleSpriteTexture (float2 uv)
			{
				fixed4 color = tex2D (_MainTex, uv);

#if ETC1_EXTERNAL_ALPHA
				// get the color from an external texture (usecase: Alpha support for ETC1 on android)
				color.a = tex2D (_AlphaTex, uv).r;
#endif //ETC1_EXTERNAL_ALPHA

				return color;
			}

			fixed4 frag(v2f IN) : SV_Target
			{
				fixed4 c = SampleSpriteTexture (IN.texcoord) * IN.color;

				float2 center = float2(1.7,4.0);
				float2 dir = normalize(IN.posWorld-center);
				float angle = smoothstep(0.91,0.93,-dir.y);

				float dis = distance(IN.posWorld.xy,center);
				float cdis = 1.0-smoothstep(-1.5,-1.1,IN.posWorld.y-center.y);
				dis = 1.0-smoothstep(0.0,4.0,dis);
				dis*=2.0*(0.9+0.2*sin(dot(IN.posWorld.xy,float2(2.0,10.0))+5.0*_Time.y));


				float2 centerSquare = float2(-.9,1.0);
				float square = distance(IN.posWorld.xy,centerSquare);
				square = 1.0-smoothstep(0.0,7.0,square);
		
				c.rgb *= _Switch*(angle*dis*cdis+0.5*square);
				c.rgb *= c.a;
				return c;
			}
		ENDCG
		}
	}
}

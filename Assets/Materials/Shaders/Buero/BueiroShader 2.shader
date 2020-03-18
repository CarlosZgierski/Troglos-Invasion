Shader "lucas/BueiroShader 1"
{
	Properties
	{
		__Color2 ("Main Color", Color) = (0.5,0.5,0.5,1)
		_MainTex ("Texture", 2D) = "white" {}
		_Color ("Sempre visivel",Color) = (0,0,0,0)
		_OutlineColor("Outline color", Color) = (0,0,0,1)
		_OutlineWidth("Outline width", Range(1.0,5.0)) = 1.01
	}

		CGINCLUDE
	#include "UnityCG.cginc"

	struct appdata
	{
		float4 vertex : POSITION;
		float3 normal : NORMAL;
	};

	struct v2f
	{
		float4 pos : POSITION;
		float3 normal : NROMAL;
	};

	float _OutlineWidth;
	float4 _OutlineColor;

	v2f vert(appdata v)
	{
		v.vertex.xyz *= _OutlineWidth;

		v2f o;
		o.pos = UnityObjectToClipPos(v.vertex);
		return o;
	}

	ENDCG

	SubShader
	{
		Tags { "Queue"="Transparent" }
		LOD 100

				Pass
		{
			Cull Off
			ZWrite Off
			ZTest Always

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag			
			#include "UnityCG.cginc"

			struct appdata2
			{
				float4 vertex : POSITION;
			};

			struct v2f2
			{
				float4 vertex : SV_POSITION;
			};

			float4 _Color;

			v2f2 vert (appdata2 v)
			{
				v2f2 o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				return o;
			}
			
			fixed4 frag (v2f2 i) : SV_Target
			{
				return _Color;
			}

			ENDCG
		}

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag			
			#include "UnityCG.cginc"

			struct appdata3
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct v2f3
			{
				float2 uv : TEXCOORD0;
				float4 vertex : SV_POSITION;
			};

			sampler2D _MainTex;
			float4 _MainTex_ST;
			
			v2f3 vert (appdata3 v)
			{
				v2f3 o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				return o;
			}
			
			fixed4 frag (v2f3 i) : SV_Target
			{
				// sample the texture
				fixed4 col = tex2D(_MainTex, i.uv);
				return col;
			}
			ENDCG
		}

		Pass
		{
			Zwrite off

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag

			half4 frag(v2f i) : Color
			{
			return _OutlineColor;
			}

			ENDCG
		}
		
		Pass
		{
			Zwrite On

			Material
			{
				Diffuse[__Color2]
				Ambient[__Color2]
			}

			Lighting On

			SetTexture[_MainTex]
			{
				ConstantColor[__Color2]
			}

			SetTexture[_MainTex]
			{
				Combine previous * primary DOUBLE
			}

		}


	}
}

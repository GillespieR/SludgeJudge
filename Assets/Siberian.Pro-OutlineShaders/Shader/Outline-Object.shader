Shader "Siberian.Pro/Outline-Object" 
{
	Properties	{
		_MainTex("Main Texture (RBG)", 2D) = "white" {}
		_Color("Color", Color) = (1,1,1,1)

		_OutlineTex("Outline Texture", 2D) = "white" {}
		_OutlineColor("Outline Color", Color) = (1,1,1,1)
		_OutlineWidth("Outline Width", float) = 1.1
		
		_datX("Outline Offset X", float) = 0
		_datY("Outline Offset Y", float) = 0
		_datZ("Outline Offset Z", float) = 0
		
		_Angle("Outline Rotation Y", float) = 0

	}

	SubShader {
		Tags { "Queue" = "Transparent" }
		
		Pass {
			Name "OUTLINE"
			ZWrite Off
			Cull Off
			CGPROGRAM

			#pragma vertex vert
			#pragma fragment frag

			#include "UnityCG.cginc"

			struct appdata {
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct v2f {
				float4 pos : SV_POSITION;
				float2 uv : TEXCOORD0;
			};


			float _OutlineWidth;
			float4 _OutlineColor;
			sampler2D _OutlineTex;
			float _datX;
			float _datY;
			float _datZ;
			float _Angle;

			float4 RotateAroundYInDegrees(float4 vertex, float degrees)	{
				float alpha = degrees * UNITY_PI / 180.0;
				float sina, cosa;
				sincos(alpha, sina, cosa);
				float2x2 m = float2x2(cosa, -sina, sina, cosa);
				return float4(mul(m, vertex.xz), vertex.yw).xzyw;
			}

			v2f vert(appdata IN) {
				v2f OUT;

				IN.vertex.xyz = IN.vertex.xyz * _OutlineWidth + float3(_datX, _datY, _datZ);			
				IN.vertex = RotateAroundYInDegrees(IN.vertex, _Angle);

				OUT.pos = UnityObjectToClipPos(IN.vertex);
				OUT.uv = IN.uv;

				return OUT;
			}			

			fixed4 frag(v2f IN) : SV_Target {
				float4 texColor = tex2D(_OutlineTex, IN.uv);
				return texColor * _OutlineColor;
			}

			ENDCG
		}

		Pass {
			Name "OBJECT"
			CGPROGRAM

			#pragma vertex vert
			#pragma fragment frag

			#include "UnityCG.cginc"
			
			struct appdata {
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct v2f {
				float4 pos : SV_POSITION;
				float2 uv : TEXCOORD0;
			};

			float4 _Color;
			sampler2D _MainTex;
			
			v2f vert(appdata IN) {
				v2f OUT;

				OUT.pos = UnityObjectToClipPos(IN.vertex);
				OUT.uv = IN.uv;

				return OUT;
			}

			fixed4 frag(v2f IN) : SV_Target {
				float4 texColor = tex2D(_MainTex, IN.uv);
				return texColor * _Color;
			}

			ENDCG
		}
	}
}

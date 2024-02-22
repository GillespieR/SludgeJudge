Shader "Siberian.Pro/Outline-Vertex-Unlit"
{
	Properties	{
	_MainTex("Texture", 2D) = "white" {}
	_Color("Color", Color) = (1, 1, 1, 1)
	_OutlineExtrusion("Outline Extrusion", Range(0.00,1.0)) = 0.03
	_OutlineColor("Outline Color", Color) = (0, 0, 0, 1)
	}

	SubShader	{		
		Pass {
			Name "OBJECT"

			Stencil	{
				Ref 4
				Comp always
				Pass replace
				ZFail keep
			}

			CGPROGRAM

			#pragma vertex vert
			#pragma fragment frag
			#include "UnityCG.cginc"
	
			sampler2D _MainTex;
			float4 _Color;

			struct appdata	{
				float4 pos : POSITION;
				float3 uv  : TEXCOORD0;
			};
		
			struct v2f	{
				float4 pos : SV_POSITION;
				float3 uv  : TEXCOORD0;
			};

			v2f vert(appdata IN) {
				v2f OUT;
				OUT.pos = UnityObjectToClipPos(IN.pos);		
				OUT.uv = IN.uv;
				return OUT;
			}

			float4 frag(v2f IN) : COLOR	{
				float4 texColor = tex2D(_MainTex, IN.uv);
				return texColor * _Color;
			}

			ENDCG
		}
	
		
		Pass {
			Name "OUTLINE"
			Cull OFF
			ZWrite OFF
			ZTest ON

			Stencil {
				Ref 4
				Comp notequal
				Fail keep
				Pass replace
			}

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag

			uniform float4 _OutlineColor;
			uniform float  _OutlineSize;
			uniform float  _OutlineExtrusion;
			sampler2D _MainTex;

			struct appdata {
				float4 vertex : POSITION;
				float3 normal : NORMAL;
				float3 uv     : TEXCOORD0;
				float4 color  : TEXCOORD1;
			};

			struct v2f {
				float4 pos   : SV_POSITION;
				float3 uv    : TEXCOORD0;
				float4 color : TEXCOORD1;
			};

			v2f vert(appdata IN) {
				v2f OUT;

				float4 newPos = IN.vertex;
				
				float3 normal = normalize(IN.normal);
				newPos += float4(normal, 0.0) * _OutlineExtrusion;				
				OUT.pos = UnityObjectToClipPos(newPos);

				return OUT;
			}

			float4 frag(v2f IN) : COLOR {
				IN.color = tex2Dlod(_MainTex, float4(IN.uv.xy, 0, 0));
				IN.color *= _OutlineColor;
				return IN.color;
			}

			ENDCG
		}
	}
}
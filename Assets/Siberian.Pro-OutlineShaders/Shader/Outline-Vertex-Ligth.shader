Shader "Siberian.Pro/Outline-Vertex-Ligth"
{
	Properties	{
	_MainTex("Texture", 2D) = "white" {}
	_RampTex("Ramp", 2D) = "white" {}
	_Color("Color", Color) = (1, 1, 1, 1)
	_OutlineExtrusion("Outline Extrusion", float) = 0.03
	_OutlineColor("Outline Color", Color) = (0, 0, 0, 1)
	}

	SubShader	{		
		Pass {
			Name "COLORLIGHT"
			Tags { "LightMode" = "ForwardBase" 	}
			
			Stencil	{
				Ref 4
				Comp always
				Pass replace
				ZFail keep
			}

			CGPROGRAM

			#pragma vertex vert
			#pragma fragment frag
			#pragma multi_compile_fwdbase 
			#include "AutoLight.cginc"
			#include "UnityCG.cginc"


			sampler2D _MainTex;
			sampler2D _RampTex;
			float4 _Color;
			float4 _LightColor0;

			struct appdata {
				float4 vertex : POSITION;
				float3 normal : NORMAL;
				float3 texCoord : TEXCOORD0;
			};
		
			struct v2f	{
				float4 pos : SV_POSITION;
				float3 normal : NORMAL;
				float3 texCoord : TEXCOORD0;
				LIGHTING_COORDS(1,2) 
			};

			v2f vert(appdata IN) {
				v2f OUT;

				OUT.pos = UnityObjectToClipPos(IN.vertex);
				float4 normal4 = float4(IN.normal, 0.0);
				OUT.normal = normalize(mul(normal4, unity_WorldToObject).xyz);

				OUT.texCoord = IN.texCoord;

				TRANSFER_VERTEX_TO_FRAGMENT(OUT);
				return OUT;
			}

			float4 frag(v2f IN) : COLOR	{
		
				float3 lightDir = normalize(_WorldSpaceLightPos0.xyz);
				float ramp = clamp(dot(IN.normal, lightDir), 0, 1.0);
				float3 lighting = tex2D(_RampTex, float2(ramp, 0.5)).rgb;

				float4 albedo = tex2D(_MainTex, IN.texCoord.xy);
	
				float attenuation = LIGHT_ATTENUATION(IN);
				float3 rgb = albedo.rgb * _LightColor0.rgb * lighting * _Color.rgb * attenuation;
				return float4(rgb, 1.0);
			}

			ENDCG
		}
				
		Pass {
			Name "SHADOW"
			Tags { "LightMode" = "ShadowCaster" }

			CGPROGRAM

			#pragma vertex vert
			#pragma fragment frag
			#pragma multi_compile_shadowcaster
			#include "UnityCG.cginc"

			struct v2f {
				V2F_SHADOW_CASTER;
			};

			v2f vert(appdata_base v) {
				v2f OUT;
				TRANSFER_SHADOW_CASTER_NORMALOFFSET(OUT)
				return OUT;
			}

			float4 frag(v2f IN) : SV_Target {
				SHADOW_CASTER_FRAGMENT(IN)
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
			uniform float _OutlineSize;
			uniform float _OutlineExtrusion;
			sampler2D _MainTex;

			struct appdata {
				float4 pos : POSITION;
				float3 normal : NORMAL;
				float3 uv : TEXCOORD0;
				float4 color : TEXCOORD1;
			};

			struct v2f {
				float4 pos   : SV_POSITION;
				float4 color : TEXCOORD0;
				float3 uv    : TEXCOORD1;
			};

			v2f vert(appdata IN) {
				v2f OUT;

				float4 newPos = IN.pos;
				
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
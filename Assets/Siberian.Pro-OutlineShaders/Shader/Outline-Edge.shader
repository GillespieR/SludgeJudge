Shader "Siberian.Pro/Outline-Edge"
{
	Properties {
		_MainTex("Main Texture (RBG)", 2D) = "white" {}
		_Color("Main Color", Color) = (1,0,0,1)
		_Thickness("Line Width", float) = 4
		_LineColor("Line Color", Color) = (1,0,0,1)
		_LineSmoothing("Line Smoothing", Range(0.001,2)) = 0.5
		
		[Toggle(SHOULD_RENDER_MESH)]
		_Visibility("Render Mesh", float) = 0
	}
		SubShader {
			Tags{ "Queue" = "Geometry" "IgnoreProjector" = "True" "RenderType" = "Transparent" }
			Blend SrcAlpha OneMinusSrcAlpha
			Cull Back
			
			Pass {
				Name "OBJECT"
				Stencil {
					Ref 1
					Comp always
					Pass replace
				}
			
				CGPROGRAM
			
				#pragma vertex vert
				#pragma fragment frag
				#pragma multi_compile_fog
				#pragma shader_feature SHOULD_RENDER_MESH

				#include "UnityCG.cginc"

				sampler2D _MainTex;
				float4 _Color;

				struct v2g {
					float4  pos : SV_POSITION;
					float2  uv : TEXCOORD0;
					float3 viewT : TANGENT;
					float3 normals : NORMAL;
				};

				struct g2f {
					float4  pos : SV_POSITION;
					float2  uv : TEXCOORD0;
					float3  viewT : TANGENT;
					float3  normals : NORMAL;
				};

				v2g vert(appdata_base v) {
					v2g OUT;
					OUT.pos = UnityObjectToClipPos(v.vertex);
					OUT.uv = v.texcoord;
					OUT.normals = v.normal;
					OUT.viewT = ObjSpaceViewDir(v.vertex);

					return OUT;
				}

				half4 frag(g2f IN) : COLOR {
					#ifdef SHOULD_RENDER_MESH
						float4 texColor = tex2D(_MainTex, IN.uv);
						return texColor * _Color;
					#else
						return 0;
					#endif
				}
				ENDCG
			}
			
			Pass {
				Name "OUTLINE"
				Stencil{
					Ref 0
					Comp equal
				}
				
				CGPROGRAM

				#pragma target 4.0
				#pragma vertex vert
				#pragma geometry geom
				#pragma fragment frag

				#include "UnityCG.cginc"


				half4 _LineColor;
				float _LineSmoothing;
				float _Thickness;

				struct v2g {
					float4 pos : SV_POSITION;
					float2 uv : TEXCOORD0;
					float3 viewT : TANGENT;
					float3 normals : NORMAL;
				};

				struct g2f {
					float4 pos : SV_POSITION;
					float2 uv : TEXCOORD0;
					float3 viewT : TANGENT;
					float3 normals : NORMAL;
				};

				v2g vert(appdata_base v) {
					v2g OUT;
					OUT.pos = UnityObjectToClipPos(v.vertex);

					OUT.uv = v.texcoord;
					OUT.normals = v.normal;
					OUT.viewT = ObjSpaceViewDir(v.vertex);

					return OUT;
				}

				void geom2(v2g start, v2g end, inout TriangleStream<g2f> triStream) {
					float thisWidth = _Thickness / 100;
					float4 parallel = (end.pos - start.pos) *_LineSmoothing;;
					normalize(parallel);
					parallel *= thisWidth;

					float4 perpendicular = float4(parallel.y,-parallel.x, 0, 0);
					perpendicular = normalize(perpendicular) * thisWidth;
					float4 v1 = start.pos - parallel;
					float4 v2 = end.pos + parallel;
					g2f OUT;
					OUT.pos = v1 - perpendicular;
					OUT.uv = start.uv;
					OUT.viewT = start.viewT;
					OUT.normals = start.normals;
					triStream.Append(OUT);

					OUT.pos = v1 + perpendicular;
					triStream.Append(OUT);

					OUT.pos = v2 - perpendicular;
					OUT.uv = end.uv;
					OUT.viewT = end.viewT;
					OUT.normals = end.normals;
					triStream.Append(OUT);

					OUT.pos = v2 + perpendicular;
					OUT.uv = end.uv;
					OUT.viewT = end.viewT;
					OUT.normals = end.normals;
					triStream.Append(OUT);
				}

				[maxvertexcount(12)]
				void geom(triangle v2g IN[3], inout TriangleStream<g2f> triStream) {
					geom2(IN[0],IN[1],triStream);
					geom2(IN[1],IN[2],triStream);
					geom2(IN[2],IN[0],triStream);
				}

				half4 frag(g2f IN) : COLOR {
					//_LineColor.a = 1;
					return _LineColor;
				}

				ENDCG
			}
		}

		FallBack "Diffuse"
}
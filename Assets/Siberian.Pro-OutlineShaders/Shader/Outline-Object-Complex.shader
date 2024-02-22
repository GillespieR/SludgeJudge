Shader "Siberian.Pro/Outline-Object-Complex" 
{
	Properties {
		_MainTex("Texture", 2D) = "white"  {}
		_Color("Color", Color) = (1,1,1,1)

		_OutlineWidth("Outline Width", Range(1.0,10.0)) = 1.1
		_OutlineColor("Outline Color", Color) = (1,1,1,1)
		_OutlineTex("Outline Texture", 2D) = "white" { }

		_DistortionScale("Distortion Scale", Range(-5.0, 5.0)) = 1.0
		_DistortionIntensity("Distortion Intensity", Range(0,128)) = 10
		
		_DistortionMap("Distortion Normal Map", 2D) = "distor" { }

		[Toggle(X_BLUR_ENABLED)]
		_EnableBlurX("Enable X-pass blur", float) = 1

		[Toggle(Y_BLUR_ENABLED)]
		_EnableBlurY("Enable Y-pass blur", float) = 0

		_BlurRadius("Blur Radius", Range(0.0,20.0)) = 1
		_BlurIntensity("Blur Intensity", Range(0.0,1.0)) = 0.01
	}

	SubShader {

		Tags { "Queue" = "Transparent" }

		GrabPass { } 
		
		//Pass 1 -> Distorts all behind object with outline.
		Pass {
			Name "OUTLINEDISTORT"
			ZWrite Off
			CGPROGRAM

			#pragma vertex vert
			#pragma fragment frag
				
			#include "UnityCG.cginc"

			struct appdata	{
				float4 vertex : POSITION;
				float2 texcoord : TEXCOORD0;
			};

			struct v2f
			{
				float4 vertex : SV_POSITION;
				float4 uvgrab : TEXCOORD0;
				float2 uvbump : TEXCOORD1;
				float uvmain : TEXCOORD2;
			};

			float _DistortionIntensity;
			float4 _DistortionMap_ST;
			float4 _OutlineTex_ST;
			float _OutlineWidth;
			fixed4 _OutlineColor;
			sampler2D _GrabTexture;
			float4 _GrabTexture_TexelSize;
			sampler2D _DistortionMap;
			sampler2D _OutlineTex;
			
			float _DistortionScale;

			v2f vert(appdata IN) {
				IN.vertex.xyz *= _OutlineWidth;
				v2f OUT;
				OUT.vertex = UnityObjectToClipPos(IN.vertex);

				#if UNITY_UV_STARTS_AT_TOP
						float scale = -_DistortionScale;
				#else
						float scale = _DistortionScale;
				#endif

				OUT.uvgrab.xy = (float2(OUT.vertex.x, OUT.vertex.y * scale) + OUT.vertex.w) * 0.5;
				OUT.uvgrab.zw = OUT.vertex.zw;

				OUT.uvbump = TRANSFORM_TEX(IN.texcoord, _DistortionMap);
				OUT.uvmain = TRANSFORM_TEX(IN.texcoord, _OutlineTex);
				return OUT;
			}

			half4 frag(v2f IN) : COLOR {
				half2 bump = UnpackNormal(tex2D(_DistortionMap, IN.uvbump)).rg;
				float2 offset = bump * _DistortionIntensity * _GrabTexture_TexelSize.xy;
				IN.uvgrab.xy = offset * IN.uvgrab.z + IN.uvgrab.xy;

				half4 col = tex2Dproj(_GrabTexture, UNITY_PROJ_COORD(IN.uvgrab));
				half4 tint = tex2D(_OutlineTex, IN.uvmain) * _OutlineColor;

				return col * tint;
			}

			ENDCG
		}

		GrabPass { }
		
		//Pass 2 -> Blurs distortion on X-axis and otline width +0.1 
		Pass {
			Name "OUTLINEHORIZONTALBLUR"
			ZWrite Off
			CGPROGRAM			
			

			#pragma vertex vert
			#pragma fragment frag
			#pragma shader_feature X_BLUR_ENABLED

			#include "UnityCG.cginc"

			struct v2f {
				float4 vertex : SV_POSITION;
				float4 uvgrab : TEXCOORD0;
			};


			float _BlurRadius;
			float _BlurIntensity;
			sampler2D _GrabTexture;
			float4 _GrabTexture_TexelSize;
			float _OutlineWidth;


			v2f vert(appdata_base IN) {
				v2f OUT;
				#ifdef X_BLUR_ENABLED
					IN.vertex.xyz *= _OutlineWidth + 0.1;				
					OUT.vertex = UnityObjectToClipPos(IN.vertex);

					#if UNITY_UV_STARTS_AT_TOP
						float scale = -1.0;
					#else
						float scale = 1.0;
					#endif

					OUT.uvgrab.xy = (float2(OUT.vertex.x, OUT.vertex.y * scale) + OUT.vertex.w) * 0.5;
					OUT.uvgrab.zw = OUT.vertex.zw;
				#else	
					OUT.vertex = UnityObjectToClipPos(IN.vertex);
					OUT.uvgrab = IN.vertex;
				#endif

				return OUT;
			}

			half4 frag(v2f IN) : COLOR {
				half4 texcol = tex2Dproj(_GrabTexture, UNITY_PROJ_COORD(IN.uvgrab));
				#ifdef X_BLUR_ENABLED
					half4 texsum = half4(0, 0, 0, 0);
					#define GRABPIXEL(weight, kernelx) tex2Dproj(_GrabTexture, UNITY_PROJ_COORD(float4(IN.uvgrab.x + _GrabTexture_TexelSize.x * kernelx * _BlurRadius, IN.uvgrab.y,  IN.uvgrab.z,  IN.uvgrab.w))) * weight

					texsum += GRABPIXEL(0.05, -4.0);
					texsum += GRABPIXEL(0.09, -3.0);
					texsum += GRABPIXEL(0.12, -2.0);
					texsum += GRABPIXEL(0.15, -1.0);
					texsum += GRABPIXEL(0.18, 0.0);
					texsum += GRABPIXEL(0.15, 1.0);
					texsum += GRABPIXEL(0.12, 2.0);
					texsum += GRABPIXEL(0.09, 3.0);
					texsum += GRABPIXEL(0.05, 4.0);

					texcol = lerp(texcol, texsum, _BlurIntensity);
				#endif
				return texcol;
			}

			
			ENDCG
		}

		GrabPass { }

		//Pass 3 -  Blurs distortion on Y-axis and otline width +0.1 
		Pass {
			Name "OUTLINEVERTICALBLUR"
			ZWrite Off
			CGPROGRAM
			
			#pragma vertex vert
			#pragma fragment frag
			#pragma shader_feature Y_BLUR_ENABLED

			#include "UnityCG.cginc"

			struct v2f {
				float4 vertex : SV_POSITION;
				float4 uvgrab : TEXCOORD0;
			};

			float _BlurRadius;
			float _BlurIntensity;
			sampler2D _GrabTexture;
			float4 _GrabTexture_TexelSize;
			float _OutlineWidth;

			v2f vert(appdata_base IN) {
				v2f OUT;
				
				#ifdef Y_BLUR_ENABLED
					IN.vertex.xyz *= _OutlineWidth + 0.1;
					OUT.vertex = UnityObjectToClipPos(IN.vertex);

					#if UNITY_UV_STARTS_AT_TOP
						float scale = -1.0;
					#else
						float scale = 1.0;
					#endif

					OUT.uvgrab.xy = (float2(OUT.vertex.x, OUT.vertex.y * scale) + OUT.vertex.w) * 0.5;
					OUT.uvgrab.zw = OUT.vertex.zw;
				#else
					OUT.vertex = UnityObjectToClipPos(IN.vertex);
					OUT.uvgrab = IN.vertex;
				#endif

				return OUT;
			}

			half4 frag(v2f IN) : COLOR {
				half4 texcol = tex2Dproj(_GrabTexture, UNITY_PROJ_COORD(IN.uvgrab));
				#ifdef Y_BLUR_ENABLED
				half4 texsum = half4(0, 0, 0, 0);

				#define GRABPIXEL(weight, kernely) tex2Dproj(_GrabTexture, UNITY_PROJ_COORD(float4(IN.uvgrab.x, IN.uvgrab.y + _GrabTexture_TexelSize.y * kernely * _BlurRadius,  IN.uvgrab.z,  IN.uvgrab.w))) * weight

				texsum += GRABPIXEL(0.05, -4.0);
				texsum += GRABPIXEL(0.09, -3.0);
				texsum += GRABPIXEL(0.12, -2.0);
				texsum += GRABPIXEL(0.15, -1.0);
				texsum += GRABPIXEL(0.18, 0.0);
				texsum += GRABPIXEL(0.15, 1.0);
				texsum += GRABPIXEL(0.12, 2.0);
				texsum += GRABPIXEL(0.09, 3.0);
				texsum += GRABPIXEL(0.05, 4.0);

				texcol = lerp(texcol, texsum, _BlurIntensity);
				#endif
				return texcol;
			}

			ENDCG
		}

		//Pass 4 -  Render object itself
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

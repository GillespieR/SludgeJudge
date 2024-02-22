// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Custom/HoleShader" {
Properties {
    _holeColor ("Color", Color) = (1,1,1,1)
    _glowColor ("Glow Color", Color) = (1,1,1,1)
    _glowSize ("Glow Size", Range (0.0,5)) = 1.0
    _glowOpacity ("Glow Opacity", Range(0,1)) = 1
}
SubShader {
Tags { "LightMode" = "ForwardBase" }
    Pass {
        CGPROGRAM
        #pragma vertex vert 
        #pragma fragment frag
        uniform float4 _holeColor;
        uniform float4 _glowColor;

        struct appData {
            float4 normal : NORMAL;
            float4 vertex : POSITION;
        };

        struct v2f {
            float4 pos : SV_POSITION;
            float4 col : COLOR;
        };

        v2f vert (appData v) {
            v2f o;
            o.pos = UnityObjectToClipPos(v.vertex);
            return o;   
        }

        float4 frag(v2f i) : COLOR {
            return _holeColor;
        }
        ENDCG
    }
    Tags { "LightMode" = "ForwardBase" "Queue" = "Transparent" "RenderType" = "Transparent"}
    Pass {
        ZWrite Off
        //Blend One OneMinusSrcColor
        Blend SrcAlpha OneMinusSrcAlpha
        CGPROGRAM
// Upgrade NOTE: excluded shader from DX11; has structs without semantics (struct v2f members viewDirection,normalDirection)
#pragma exclude_renderers d3d11
// Upgrade NOTE: excluded shader from DX11; has structs without semantics (struct v2f members viewDirection,normalDirection)
//#pragma exclude_renderers d3d11
        #pragma vertex vert 
        #pragma fragment frag
        uniform float4 _holeColor;
        uniform float4 _glowColor;
        uniform float _strength;
        uniform float _glowSize;
        uniform float _glowOpacity;

        struct appData {
            float4 normal : NORMAL;
            float4 vertex : POSITION;
        };

        struct v2f {
            float4 pos : SV_POSITION;
            float4 col : COLOR;
            float3 viewDirection;
            float3 normalDirection;
        };

        v2f vert (appData v) {
            v2f o;
            float4 pos = v.vertex + (v.normal * _glowSize);
            o.pos = UnityObjectToClipPos(pos);
            return o;   
        }

        float4 frag(v2f i) : COLOR {
            return float4(_glowColor.rgb, _glowOpacity);
        }
        ENDCG
    } 
}
Fallback "Diffuse"
}
Shader "Custom/Line"
{
    Properties
    {
        _Color ("Main Color", Color) = (1,1,1,1)
        _CenterUVX ("CenterUVX", Range(0.0, 1.0)) = 0.5
        _StartWidthHalf ("StartWidthHalf", Range(0.0, 0.5)) = 0.1
        _FadeRange ("FadeRange", Range(0.01, 1.0)) = 0.4
    }

    SubShader
    {
        Tags
        {
            "RenderType"="Transparent" "Queue"="Transparent"
        }
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
                float4 vertex : SV_POSITION;
            };

            fixed4 _Color;
            float _CenterUVX;
            float _StartWidthHalf;
            float _FadeRange;

            v2f vert(appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                fixed4 col = _Color;

                float dist = abs(i.uv.x - _CenterUVX);

                float startDist = _StartWidthHalf;

                float endDist = _StartWidthHalf + _FadeRange;

                float alphaFade = 1 - smoothstep(startDist, endDist, dist);

                col.a *= alphaFade;

                return col;
            }
            ENDCG
        }
    }
}
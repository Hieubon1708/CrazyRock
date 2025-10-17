Shader "Custom/TrajectoryPrediction"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1)
        _StartRadius ("StartRadius", Range(0.01, 1.0)) = 0.25 
        _FadeRange ("FadeRange)", Range(0.01, 1.0)) = 0.5
        _FadeCenterUV ("FadeCenterUV", Vector) = (0.5,0.5,0,0) 
    }

    SubShader
    {
        Tags { "RenderType"="Transparent" "Queue"="Transparent" }
        LOD 100
        
        Cull Off
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
            float _StartRadius;
            float _FadeRange;
            float2 _FadeCenterUV; 

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 col = _Color; 

                float dist = distance(i.uv, _FadeCenterUV);
                
                float startDist = _StartRadius;
                float endDist = _StartRadius + _FadeRange;

                float alphaFade = 1.0 - smoothstep(startDist, endDist, dist);

                col.a = alphaFade;

                return col;
            }
            ENDCG
        }
    }
}
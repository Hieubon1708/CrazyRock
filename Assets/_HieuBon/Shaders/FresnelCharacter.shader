Shader "Custom/FresnelSurfaceSolidTextured"
{
    Properties
    {
        _Color ("Tint Albedo (RGB)", Color) = (0.8705882, 0.5607843, 0.5882353, 1)
        _MainTex ("Albedo Texture (RGB)", 2D) = "white" {}
        [KeywordEnum(None, X2, X4)] _TilingAlbedo ("Tiling", Float) = 1 // Giả định Tiling là 1x1

        _EmissionColor ("Tint Emission (RGB)", Color) = (0.3018868, 0.3018868, 0.3018868, 1)
        _EmissionMap ("Emission Texture (RGB)", 2D) = "black" {}

        _FresnelColor ("Tint Fresnel (RGB)", Color) = (0.8153532, 0.240566, 1, 1)
        _FresnelTex ("Fresnel Texture (RGB)", 2D) = "white" {}
        _FresnelScale ("Fresnel Scale", Range(0, 5)) = 2.0
        _FresnelPower ("Fresnel Power", Range(0.1, 10)) = 5.0

        _Smoothness ("Smooth", Range(0, 1)) = 0.5
        _Metallic ("Metal", Range(0, 1)) = 0.0
        [Toggle] _Grayscale ("Grayscale Toggle", Float) = 0
    }

    SubShader
    {
        Tags { "RenderType" = "Opaque" }
        LOD 200

        CGPROGRAM
        #pragma surface surf Standard fullforwardshadows

        fixed4 _Color;
        sampler2D _MainTex;
        fixed4 _EmissionColor;
        sampler2D _EmissionMap;

        fixed4 _FresnelColor;
        sampler2D _FresnelTex;
        half _FresnelScale;
        half _FresnelPower;

        half _Smoothness;
        half _Metallic;
        half _Grayscale;


        struct Input
        {
            float2 uv_MainTex;
            float2 uv_EmissionMap;
            float2 uv_FresnelTex;
            float3 viewDir; 
        };

        void surf (Input IN, inout SurfaceOutputStandard o)
        {
            fixed4 albedoTex = tex2D (_MainTex, IN.uv_MainTex);
            fixed4 finalAlbedo = albedoTex * _Color;

            if (_Grayscale > 0.5) {
                float gray = dot(finalAlbedo.rgb, float3(0.299, 0.587, 0.114));
                finalAlbedo.rgb = gray;
            }

            o.Albedo = finalAlbedo.rgb;


            o.Metallic = _Metallic;
            o.Smoothness = _Smoothness;


            fixed4 emissionTex = tex2D (_EmissionMap, IN.uv_EmissionMap);
            fixed3 emission = emissionTex.rgb * _EmissionColor.rgb;


            float rim = 1.0 - saturate(dot (IN.viewDir, o.Normal));

            rim = pow (rim, _FresnelPower) * _FresnelScale;
            rim = saturate(rim); 

            fixed4 fresnelTex = tex2D(_FresnelTex, IN.uv_FresnelTex);
            fixed3 fresnelColor = fresnelTex.rgb * _FresnelColor.rgb;

            o.Emission = emission + (fresnelColor * rim);
        }
        ENDCG
    }

    FallBack "Standard"
}
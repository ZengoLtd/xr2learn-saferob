Shader "ZenLab/Standard Colormap Blend"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1)
        _Colormap ("Colormap Texture (RGB)", 2D) = "white" {}
        _ColormapOpacity ("Colormap opacity", Range(0,1)) = 1        
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
        _BumpMap ("Normals", 2D) = "bump" {}
        _NormalStrength ("Normal strength", Range(0,2)) = 1
        _BumpMap2 ("Detail Normals", 2D) = "bump" {}
        _NormalStrength2 ("Detail Normal strength", Range(0,2)) = 1
        _MetallicMap ("Metallic map", 2D) = "white" {}
        _Metallic ("Metallic strength", Range(0,2)) = 1
        _RoughnessMap ("Roughness map", 2D) = "black" {}
        _Glossiness ("Roughness strength", Range(0,2)) = 1

    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200

        CGPROGRAM
        #pragma surface surf Standard fullforwardshadows
        #pragma target 3.0

        sampler2D _MainTex, _BumpMap, _BumpMap2, _RoughnessMap, _MetallicMap, _Colormap;
        half _Glossiness, _Metallic, _NormalStrength, _NormalStrength2, _ColormapOpacity;
        fixed4 _Color;

        struct Input
        {
            float2 uv_MainTex, uv_BumpMap2, uv_Colormap;
        };

        UNITY_INSTANCING_BUFFER_START(Props)
        UNITY_INSTANCING_BUFFER_END(Props)

        void surf (Input IN, inout SurfaceOutputStandard o)
        {
            // Albedo comes from a texture tinted by color
            fixed4 c = tex2D (_MainTex, IN.uv_MainTex);
            fixed4 cc = tex2D (_Colormap, IN.uv_Colormap);
            half3 normals1 =  lerp(half3(0,0,1),UnpackNormal (tex2D (_BumpMap, IN.uv_MainTex)), _NormalStrength);
            half3 normals2 =  lerp(half3(0,0,1),UnpackNormal (tex2D (_BumpMap2, IN.uv_BumpMap2)), _NormalStrength2);
            o.Normal = normalize(normals1 + normals2);
            o.Albedo = c.rgb * _Color.rgb * 2 * lerp(0.5f, cc.rgb, _ColormapOpacity);
            // Metallic and smoothness come from slider variables
            o.Metallic = lerp(0,tex2D (_MetallicMap, IN.uv_MainTex), _Metallic);
            o.Smoothness = lerp(0,1-tex2D (_RoughnessMap, IN.uv_MainTex), _Glossiness);
            o.Alpha = c.a;
        }
        ENDCG
    }
    FallBack "Diffuse"
}

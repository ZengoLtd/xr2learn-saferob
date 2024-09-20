Shader "ZenLab/Mix Logo shader"
{
    Properties
    {
        _Color("Color", Color) = (1,1,1,1)
        _MainTex("Albedo (RGB)", 2D) = "white" {}
        _BumpMap("Normals", 2D) = "bump" {}
        _NormalStrength("Normal strength", Range(0,2)) = 1
        _BumpMap2("Detail Normals", 2D) = "bump" {}
        _NormalStrength2("Detail Normal strength", Range(0,2)) = 1

        _MetallicMap("Metallic map", 2D) = "white" {}
        _Metallic("Metallic strength", Range(0,2)) = 0.5
        _RoughnessMap("Roughness map", 2D) = "black" {}
        _Glossiness("Roughness strength", Range(0,2)) = 0.5

         _Logo1("Logo 1 (RGBA)", 2D) = "black" {}
         _Logo1opacity("Logo 1 strength", Range(0,1)) = 1

         _Logo2("Logo 2 (RGBA)", 2D) = "black" {}
         _Logo2opacity("Logo 2 strength", Range(0,1)) = 1

         _Alphamask("Alpha Mask (RGBA)", 2D) = "white" {}
         _Acolor("Color", Color) = (1,1,1,1)
         _Acolorstrenght("Color strength", Range(0,2)) = 1




    }
        SubShader
        {
            Tags { "RenderType" = "Opaque" }
            LOD 200

            CGPROGRAM
            // Physically based Standard lighting model, and enable shadows on all light types
            #pragma surface surf Standard fullforwardshadows

            // Use shader model 3.0 target, to get nicer looking lighting
            #pragma target 3.0


            sampler2D _MainTex, _BumpMap, _BumpMap2, _RoughnessMap, _MetallicMap, _Logo1, _Logo2, _Alphamask;

            struct Input
            {
                float2 uv_MainTex, uv_BumpMap2;
                float2 uv2_Logo1;
                float2 uv3_Logo2;
            };

            half _Glossiness, _Metallic, _NormalStrength, _NormalStrength2, _Logo1opacity, _Logo2opacity, _Acolorstrenght;
            fixed4 _Color, _Acolor;

            // Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
            // See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
            // #pragma instancing_options assumeuniformscaling
            UNITY_INSTANCING_BUFFER_START(Props)
                // put more per-instance properties here
            UNITY_INSTANCING_BUFFER_END(Props)

            void surf(Input IN, inout SurfaceOutputStandard o)
            {
                // Albedo comes from a texture tinted by color
                fixed4 maintex = tex2D(_MainTex, IN.uv_MainTex);
                fixed4 alphatint = tex2D(_Alphamask, IN.uv_MainTex) * _Acolor;
                fixed4 logo1 = tex2D(_Logo1, IN.uv2_Logo1);
                fixed4 logo2 = tex2D(_Logo2, IN.uv3_Logo2);
                half3 normals1 = lerp(half3(0,0,1),UnpackNormal(tex2D(_BumpMap, IN.uv_MainTex)), _NormalStrength);
                half3 normals2 = lerp(half3(0,0,1),UnpackNormal(tex2D(_BumpMap2, IN.uv_BumpMap2)), _NormalStrength2);
                o.Normal = normalize(normals1 + normals2);
                o.Albedo = lerp(maintex.rgb, alphatint.rgb, alphatint.a * _Acolorstrenght) * _Color;
                o.Albedo = lerp(o.Albedo, logo1.rgb, logo1.a * _Logo1opacity);
                o.Albedo = lerp(o.Albedo, logo2.rgb, logo2.a * _Logo2opacity);
                // Metallic and smoothness come from slider variables
                o.Metallic = lerp(0,tex2D(_MetallicMap, IN.uv_MainTex), _Metallic);
                o.Smoothness = lerp(0,1 - tex2D(_RoughnessMap, IN.uv_MainTex), _Glossiness);

                o.Alpha = alphatint.r;
            }
            ENDCG
        }
            FallBack "Diffuse"
}

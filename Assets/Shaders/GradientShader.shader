Shader "Custom/GradientShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}  // Add this line
        _TopColor ("Top Color", Color) = (0.73, 0.15, 0.88, 1)  // Top color, fully opaque
        _BottomColor ("Bottom Color", Color) = (0.73, 0.15, 0.88, 0) // Bottom color, fully transparent
    }
    SubShader
    {
        Tags { "RenderType"="Transparent" "Queue"="Transparent" }
        LOD 100

        Pass
        {
            Blend SrcAlpha OneMinusSrcAlpha  // Enable transparency
            ZWrite Off  // Disable depth writing for proper transparency rendering
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata_t
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            fixed4 _TopColor;
            fixed4 _BottomColor;
            sampler2D _MainTex;  // Add this line
            v2f vert(appdata_t v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                // Interpolate between the top and bottom colors based on the y-coordinate
                return lerp(_BottomColor, _TopColor, i.uv.y);
            }
            ENDCG 
        }
    }
}

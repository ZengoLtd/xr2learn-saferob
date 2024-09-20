// Unity built-in shader source. Copyright (c) 2016 Unity Technologies. MIT license (see license.txt)

// Unlit alpha-blended shader.
// - no lighting
// - no lightmap support
// - no per-material color

Shader "ZenLab/Unlit Transparent Offset" {
Properties {
    _MainTex ("Base (RGB) Trans (A)", 2D) = "white" {}
    _FadeInCameraDistance ("Fade In Camera Distance", Float) = 1
    _FadeOutCameraDistance ("Fade Out Camera Distance", Float) = 1.1   
}

SubShader {
    Tags {"Queue"="Overlay" "IgnoreProjector"="True" "RenderType"="Transparent" "ForceNoShadowCasting"="True"}
    LOD 100

    ZWrite Off
    Blend SrcAlpha OneMinusSrcAlpha
    //Offset -200,0   
    ZTest Always
    cull off

    Pass {
        CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma target 2.0

            #include "UnityCG.cginc"

            struct appdata_t {
                float4 vertex : POSITION;
                float2 texcoord : TEXCOORD0;
                UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            struct v2f {
                float4 vertex : SV_POSITION;
                float2 texcoord : TEXCOORD0;
				float4 worldPos : TEXCOORD2;                
                UNITY_VERTEX_OUTPUT_STEREO
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
			float _FadeInCameraDistance;
			float _FadeOutCameraDistance;

			float3 getCameraPosition() 
			{
				#ifdef USING_STEREO_MATRICES
					return lerp(unity_StereoWorldSpaceCameraPos[0], unity_StereoWorldSpaceCameraPos[1], 0.5);
				#endif
				return _WorldSpaceCameraPos;
			}

            v2f vert (appdata_t v)
            {
                v2f o;
                UNITY_SETUP_INSTANCE_ID(v);
                UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.texcoord = TRANSFORM_TEX(v.texcoord, _MainTex);
				o.worldPos = mul(unity_ObjectToWorld, float4(v.vertex.xyz, 1));                
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float3 cameraPos = getCameraPosition();
                fixed4 col = tex2D(_MainTex, i.texcoord);
				float distanceToCamera = distance(cameraPos.xyz, i.worldPos.xyz);
				float cameraDistanceFade = smoothstep(_FadeOutCameraDistance, _FadeInCameraDistance, distanceToCamera);
                col.a *= cameraDistanceFade;
                return col;
            }
        ENDCG
    }
}

}

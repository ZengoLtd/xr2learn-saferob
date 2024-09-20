Shader "ZenLab/Opaque Unlit Basic" {
	Properties {
		_Color ("Main Color", Color) = (1,1,1,1)
		_MainTex ("Base (RGB)", 2D) = "white" {}
		_VCPower ("Vertex color intensity", float) = 1.0
	}
	
	SubShader {
		
		LOD 50
		Tags { "RenderType"="Opaque" }
				
		CGPROGRAM
		#pragma surface surf Unlit vertex:vert  noambient novertexlights nolightmap noforwardadd nodirlightmap 

		inline fixed4 LightingUnlit (SurfaceOutput s, fixed3 lightDir, fixed atten) {
			return fixed4(s.Albedo, s.Alpha);
		}
		half4 LightingUnlit_SingleLightmap (SurfaceOutput s, fixed4 color){
			half3 lm = DecodeLightmap (color);
			return fixed4(lm*0.5, 0);
		}

		struct Input {
			float2 uv_MainTex;
			half4 color;
		};
      
	  	sampler2D _MainTex;
		half4 _Color;
		half _VCPower;
		void vert (inout appdata_full v, out Input o) { 
			UNITY_INITIALIZE_OUTPUT(Input,o);
			o.color = lerp(half4(1,1,1,1),v.color,_VCPower) * _Color;
		}
		
		void surf (Input IN, inout SurfaceOutput o) {
			half4 c = tex2D (_MainTex, IN.uv_MainTex);
			o.Albedo = c.rgb * IN.color.rgb * 2.0;
			o.Alpha = c.a;
		}
		
		ENDCG
		
	}

}

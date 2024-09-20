Shader "ZenLab/Alphablend Unlit Basic" {
	Properties {
		_Color ("Main Color", Color) = (1,1,1,1)
		_MainTex ("Base (RGB)", 2D) = "white" {}
		_VCPower ("Vertex color intensity", float) = 1.0
	}
		
	SubShader {
		
		LOD 70
		Tags {"Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent"}
		zwrite off
		blend srcalpha oneminussrcalpha
		
		CGPROGRAM
		#pragma surface surf Unlit vertex:vert alpha:blend noambient novertexlights nolightmap noforwardadd nodirlightmap  // finalcolor:EnhanceColor
	
		inline fixed4 LightingUnlit (SurfaceOutput s, fixed3 lightDir, fixed atten) {
			return fixed4(s.Albedo, s.Alpha);
		}
		half4 LightingUnlit_SingleLightmap (SurfaceOutput s, fixed4 color){
			half3 lm = DecodeLightmap (color);
			return fixed4(lm*0.5, 0);
		}

	  	sampler2D _MainTex;
		half4 _Color;
		half _VCPower;
		struct Input {
			float2 uv_MainTex;
			half4 color;
		};
      
		void vert (inout appdata_full v, out Input o) { 
			UNITY_INITIALIZE_OUTPUT(Input,o);
			o.color = lerp(half4(1,1,1,1),v.color,_VCPower) * _Color;
		}
		
		fixed4 	_EnhanceColor;
		fixed 	_EnhanceContrast, _EnhanceSaturation;
		void EnhanceColor (Input IN, SurfaceOutput o, inout fixed4 color)
		{
			color.rgb = lerp(half3(0.5, 0.5, 0.5),color.rgb,_EnhanceContrast+1);
			half lum = dot(half3(0.2126, 0.7152, 0.0722), color.rgb);
			color.rgb = lerp(half3(lum, lum, lum),color.rgb,_EnhanceSaturation+1);
			color.rgb *= 1-_EnhanceColor.rgb;
		}

		void surf (Input IN, inout SurfaceOutput o) {
			half4 c = tex2D (_MainTex, IN.uv_MainTex) * IN.color;
			o.Albedo = c.rgb;
			o.Alpha = c.a;
		}
		
		ENDCG
		
    }
	
}

Shader "Custom/ObjectSurfaceShader" {
	Properties {
		_MainTex ("Main texture", 2D) = "white" {}
		[Normal] _BumpMap ("Bump map", 2D) = "bump" {}
		
		_Glossiness ("Smoothness", Range(0,1)) = 0.5
		_Metallic ("Metallic", Range(0,1)) = 0.0

		[HideInInspector] _PaintingTex ("Painting texture", 2D) = "white" {}
	}
	SubShader {
		Tags { "RenderType"="Opaque" }
		LOD 200
		
		CGPROGRAM
		#pragma surface surf Standard addshadow
		#pragma target 3.0

		sampler2D _MainTex;
		sampler2D _BumpMap;
		sampler2D _PaintingTex;

		struct Input {
			float2 uv_MainTex;
			float2 uv_BumpMap;
			float2 uv_PaintingTex;
		};

		half _Glossiness;
		half _Metallic;

		void surf (Input IN, inout SurfaceOutputStandard o) {
			fixed4 paintingTex = tex2D (_PaintingTex, IN.uv_PaintingTex);
			fixed4 mainTex = tex2D (_MainTex, IN.uv_MainTex);

			o.Albedo = (paintingTex.a == 0) ? mainTex.rgb : lerp(mainTex.rgb, paintingTex.rgb, paintingTex.a);
			o.Normal = UnpackNormal (tex2D (_BumpMap, IN.uv_BumpMap));
			o.Metallic = _Metallic;
			o.Smoothness = _Glossiness;
			o.Alpha = mainTex.a;
		}
		ENDCG
	}
	FallBack "Diffuse"
}

Shader "Custom/ObjectSurfaceShader2" {
	Properties {
		_MainTex ("Main texture", 2D) = "white" {}
		[Normal] _MainBumpMap ("Main bump map", 2D) = "bump" {}
		_SecondaryTex ("Secondary texture", 2D) = "white" {}
		[Normal] _SecondaryBumpMap ("Secondary bump map", 2D) = "bump" {}
		
		_Displacement ("Displacement amount", Range(-1,1)) = 0.02
		_Glossiness ("Smoothness", Range(0,1)) = 0.5
		_Metallic ("Metallic", Range(0,1)) = 0.0

		[HideInInspector] _PaintingTex ("Painting texture", 2D) = "white" {}
	}
	SubShader {
		Tags { "RenderType"="Opaque" }
		LOD 200
		
		CGPROGRAM
		#pragma surface surf Standard vertex:vert addshadow
		#pragma target 3.0

		sampler2D _MainTex;
		sampler2D _MainBumpMap;
		sampler2D _SecondaryTex;
		sampler2D _SecondaryBumpMap;
		sampler2D _PaintingTex;

		struct Input {
			float2 uv_MainTex;
			float2 uv_MainBumpMap;
			float2 uv_SecondaryTex;
			float2 uv_SecondaryBumpMap;
			float2 uv_PaintingTex;
		};

		half _Displacement;

		void vert (inout appdata_full v) {
			fixed4 paintingTex = tex2Dlod(_PaintingTex, float4(v.texcoord.xy,0,0));
          	v.vertex.xyz += v.normal * paintingTex.a * _Displacement;
      	}

		half _Glossiness;
		half _Metallic;

		void surf (Input IN, inout SurfaceOutputStandard o) {
			fixed4 paintingTex = tex2D (_PaintingTex, IN.uv_PaintingTex);
			fixed4 mainTex = tex2D (_MainTex, IN.uv_MainTex);
			fixed4 secondaryTex = tex2D (_SecondaryTex, IN.uv_SecondaryTex);

			fixed3 mainBumpMap = UnpackNormal (tex2D (_MainBumpMap, IN.uv_MainBumpMap));
			fixed3 secondaryBumpMap = UnpackNormal (tex2D (_SecondaryBumpMap, IN.uv_SecondaryBumpMap));

			o.Albedo = (paintingTex.a == 0) ? mainTex.rgb : lerp(mainTex.rgb, (secondaryTex.rgb  * paintingTex.rgb), paintingTex.a * 0.5f);
			o.Normal = (paintingTex.a == 0) ? mainBumpMap : secondaryBumpMap;
			o.Metallic = _Metallic;
			o.Smoothness = _Glossiness;
			o.Alpha = mainTex.a;
		}
		ENDCG
	}
	FallBack "Diffuse"
}

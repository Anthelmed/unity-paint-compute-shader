Shader "Custom/ObjectSurfaceShader" {
	Properties {
		[NoScaleOffset] _MainTex ("Main texture", 2D) = "white" {}
		[NoScaleOffset] [Normal] _BumpMap ("Bump map", 2D) = "bump" {}
		[NoScaleOffset] _SecondaryTex ("Secondary texture", 2D) = "white" {}
		[NoScaleOffset] [Normal] _SecondaryBumpMap ("Secondary bump map", 2D) = "bump" {}
		
		_Glossiness ("Smoothness", Range(0,1)) = 0.5
		_Metallic ("Metallic", Range(0,1)) = 0.0
		_Amount ("Extrusion Amount", Range(-1,1)) = 0.5

		[HideInInspector] _DecalTex ("Decal texture", 2D) = "white" {}
	}
	SubShader {
		Tags { "RenderType"="Opaque" }
		LOD 200
		
		CGPROGRAM
		#pragma surface surf Standard vertex:vert addshadow
		#pragma target 3.0

		sampler2D _MainTex;
		sampler2D _BumpMap;
		sampler2D _SecondaryTex;
		sampler2D _SecondaryBumpMap;
		sampler2D _DecalTex;

		half _Amount;

		struct Input {
			float2 uv_MainTex;
			float2 uv_BumpMap;
			float2 uv_SecondaryTex;
			float2 uv_SecondaryBumpMap;
			float2 uv_DecalTex;
		};

		void vert (inout appdata_full v) {
			fixed4 d = tex2Dlod (_DecalTex, float4(v.texcoord.xy,0,0));
          	v.vertex.xyz += v.normal * _Amount * d.r;
      	}

		half _Glossiness;
		half _Metallic;
		fixed4 _DecalColor;

		void surf (Input IN, inout SurfaceOutputStandard o) {
			fixed4 d = tex2D (_DecalTex, IN.uv_DecalTex);

			fixed4 m = tex2D (_MainTex, IN.uv_MainTex);
			fixed4 s = tex2D (_SecondaryTex, IN.uv_SecondaryTex);
			o.Albedo = (d.a == 0) ? m.rgb : m.rgb + s.rgb;

			fixed3 mn = UnpackNormal (tex2D (_BumpMap, IN.uv_BumpMap));
			fixed3 sn = UnpackNormal (tex2D (_SecondaryBumpMap, IN.uv_SecondaryBumpMap));
			o.Normal = (d.a == 0) ? mn : sn;

			o.Metallic = _Metallic;
			o.Smoothness = _Glossiness;
			o.Alpha = m.a;
		}
		ENDCG
	}
	FallBack "Diffuse"
}

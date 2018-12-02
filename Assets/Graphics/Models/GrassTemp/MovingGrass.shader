Shader "Custom/MovingGrass" {
	Properties{
	  _MainTex("Texture", 2D) = "white" {}
	  _Displacement("Displacement", 2D) = "white" {}
	  _Amount("Extrusion Amount", Range(-1,1)) = 0.5
	  _Speed("Extrusion Speed", Range(0,100)) = 1
	}
		SubShader{
		  Tags { "RenderType" = "Transparent" "Queue" = "Transparent" }
		  LOD 200
		  CGPROGRAM

		  #pragma target 3.0
		  #pragma surface surf Lambert vertex:vert alpha

		  

		  struct Input {
			  float2 uv_MainTex;
		  };

		  float _Amount;
		  float _Speed;
		  sampler2D _MainTex;
		  sampler2D _Displacement;

		  void vert(inout appdata_full v) 
		  {
			  float3 move = float3(1, 0, 0);
			  float4 uv = ComputeScreenPos(v.vertex);
			  float3 effect = tex2Dlod(_Displacement, uv * 0.1).rgb;
			  v.vertex.xyz += move * v.texcoord.y * _Amount * sin(_Time.y * _Speed * effect.r);
		  }

		  void surf(Input IN, inout SurfaceOutput o) 
		  {
			  float4 color = tex2D(_MainTex, IN.uv_MainTex).rgba;
			  o.Albedo = color.rgb;
			  o.Emission = color.rgb;
			  o.Alpha = color.a;
		  }
		  ENDCG
	  }
		  Fallback "Diffuse"
}
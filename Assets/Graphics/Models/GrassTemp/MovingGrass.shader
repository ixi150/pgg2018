Shader "Custom/MovingGrass" {
	Properties{
	  _MainTex("Texture", 2D) = "white" {}
	  _Displacement("Displacement", 2D) = "white" {}
	  _Amount("Extrusion Amount", Range(-1,1)) = 0.5
	  _Speed("Extrusion Amount", Range(0,100)) = 1
	}
		SubShader{
		  Tags { "RenderType" = "Transparent" "Queue" = "Transparent" }
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
			  float3 tex = tex2Dlod(_Displacement, float4(v.texcoord.xy, 0, 0)).rgb;
			  v.vertex.xyz += move * v.vertex.y * _Amount * sin(_Time.y * _Speed*tex.r);
		  }

		  void surf(Input IN, inout SurfaceOutput o) 
		  {
			  o.Albedo = tex2D(_MainTex, IN.uv_MainTex).rgb;
			  o.Alpha = tex2D(_MainTex, IN.uv_MainTex).a;
		  }
		  ENDCG
	  }
		  Fallback "Diffuse"
}
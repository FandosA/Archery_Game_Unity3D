Shader "Custom/Textures"
{
	Properties{
		_MainTex("Texture", 2D) = "white" {}
	}
		SubShader{
					CGPROGRAM
			#pragma surface surf Standard
			struct Input {
				float2 uv_MainTex;
			};
			sampler2D _MainTex;
			void surf(Input IN, inout SurfaceOutputStandard o) {
				o.Albedo = tex2D(_MainTex, IN.uv_MainTex);
			}
			ENDCG
	}
		Fallback "Diffuse"
}
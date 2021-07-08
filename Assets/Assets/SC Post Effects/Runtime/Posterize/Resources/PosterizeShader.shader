Shader "Hidden/SC Post Effects/Posterize"
{
	HLSLINCLUDE

	#include "../../../Shaders/Pipeline/Pipeline.hlsl"

	float _Depth;

	float4 Frag(Varyings i): SV_Target
	{
		STEREO_EYE_INDEX_POST_VERTEX(i);

		float4 screenColor = ScreenColor(UV_VR);
		float bits = _Depth  * (0.5 + 0.5);	
		float k = pow(2, bits);

		screenColor = floor(screenColor * k + 0.5) /k;

		return screenColor;
	}

	ENDHLSL

	SubShader
	{
		Cull Off ZWrite Off ZTest Always

		Pass
		{
			Name "Posterize"
			HLSLPROGRAM
			#pragma vertex Vert
			#pragma fragment Frag

			ENDHLSL
		}
	}
}
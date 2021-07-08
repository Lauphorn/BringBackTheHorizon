Shader "Hidden/SC Post Effects/Cloud Shadows"
{
	HLSLINCLUDE

	#define REQUIRE_DEPTH
	#include "../../../Shaders/Pipeline/Pipeline.hlsl"

	DECLARE_TEX(_NoiseTex);

	//Prefer high precision depth
	//#pragma fragmentoption ARB_precision_hint_nicest

	float4 _CloudParams;
	uniform float4x4 clipToWorld;

	struct v2f {
		float4 positionCS : SV_POSITION;
		float2 texcoord : TEXCOORD0;
		float2 texcoordStereo : TEXCOORD1;
		float3 worldDirection : TEXCOORD2;
	};

	v2f VertWSRecontruction(Attributes v) {
		v2f o;

		UNITY_SETUP_INSTANCE_ID(v);
		UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);

		o.positionCS = OBJECT_TO_CLIP(v);
		o.texcoord.xy = GET_TRIANGLE_UV(v);

		o.texcoord = FlipUV(o.texcoord);

		float4 clip = float4(o.texcoord.xy * 2 - 1, 0.0, 1.0);
		o.worldDirection.rgb = (mul((float4x4)clipToWorld, clip.rgba).xyz - _WorldSpaceCameraPos.rgb).xyz;

		//UNITY_SINGLE_PASS_STEREO
		o.texcoordStereo = GET_TRIANGLE_UV_VR(o.texcoord, v.vertexID);

		return o;
	}

	float4 Frag(v2f i) : SV_Target
	{
		STEREO_EYE_INDEX_POST_VERTEX(i);

		half4 sceneColor = ScreenColor(UV_VR);
		float depth = SAMPLE_DEPTH(UV_VR);
		float3 worldPos = i.worldDirection.xyz * LINEAR_EYE_DEPTH(depth) + _WorldSpaceCameraPos.xyz;

		float2 uv = worldPos.xz * _CloudParams.x + (_Time.y * float2(_CloudParams.y, _CloudParams.z));
		float clouds = 1- SAMPLE_TEX(_NoiseTex, sampler_LinearRepeat, uv).r;

		//Clip skybox
		if (LINEAR_DEPTH(depth) > 0.99) clouds = 1;

		clouds = lerp(1, clouds, _CloudParams.w);

		float3 cloudsBlend = sceneColor.rgb * clouds;

		return float4(cloudsBlend.rgb, sceneColor.a);
	}


	ENDHLSL

	SubShader
	{
		Cull Off ZWrite Off ZTest Always

		Pass
		{
			Name "Cloud Shadows"
			HLSLPROGRAM

			#pragma vertex VertWSRecontruction
			#pragma fragment Frag

			ENDHLSL
		}
	}
}
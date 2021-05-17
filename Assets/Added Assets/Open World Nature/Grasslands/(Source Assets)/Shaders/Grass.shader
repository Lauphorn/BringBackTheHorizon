// Upgrade NOTE: upgraded instancing buffer 'OpenWorldNatureGrass' to new syntax.

// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Open World Nature/Grass"
{
	Properties
	{
		_Cutoff( "Mask Clip Value", Float ) = 0.33
		[Toggle(_BAKEDMASK_ON)] _BakedMask("Baked Mask", Float) = 1
		[Toggle(_VERTEXPOSITIONMASK_ON)] _VertexPositionMask("Vertex Position Mask", Float) = 0
		[Toggle(_UVMASK_ON)] _UVMask("UV Mask", Float) = 0
		_Hue("Hue", Range( -0.5 , 0.5)) = 0
		_Saturation("Saturation", Range( -1 , 1)) = 0
		_Lightness("Lightness", Range( -1 , 1)) = 0
		[NoScaleOffset]_MainTex("MainTex", 2D) = "white" {}
		_BumpScale("Bump Scale", Range( 0 , 1)) = 1
		[NoScaleOffset]_BumpMap("Bump Map", 2D) = "bump" {}
		_OcclusionRemap("Occlusion Remap", Vector) = (0,1,0,0)
		[NoScaleOffset]_OcclusionMap("Occlusion Map", 2D) = "white" {}
		_WindDirectionAndStrength("WindDirectionAndStrength", Vector) = (1,1,1,1)
		_Shiver("Shiver", Vector) = (1,1,1,1)
		[Toggle(_DEBUGGUST_ON)] _DebugGust("Debug Gust", Float) = 0
		[NoScaleOffset]_MetallicGlossMap("Metallic Gloss Map", 2D) = "white" {}
		_StiffnessVariation("StiffnessVariation", Range( 0 , 0.99)) = 0
		[Toggle(_METALLICGLOSSMAP_ON)] _METALLICGLOSSMAP("_METALLICGLOSSMAP", Float) = 0
		_GlossRemap("Gloss Remap", Vector) = (0,1,0,0)
		_Glossiness("_Glossiness", Range( 0 , 1)) = 0.2
		_Metallic("_Metallic", Range( 0 , 1)) = 0
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "TransparentCutout"  "Queue" = "AlphaTest+0" "DisableBatching" = "True" }
		Cull Off
		CGPROGRAM
		#include "UnityShaderVariables.cginc"
		#include "UnityStandardUtils.cginc"
		#pragma target 3.0
		#pragma multi_compile_instancing
		#pragma shader_feature _BAKEDMASK_ON
		#pragma shader_feature _UVMASK_ON
		#pragma shader_feature _VERTEXPOSITIONMASK_ON
		#pragma shader_feature _DEBUGGUST_ON
		#pragma shader_feature _METALLICGLOSSMAP_ON
		#pragma surface surf Standard keepalpha addshadow fullforwardshadows exclude_path:deferred dithercrossfade vertex:vertexDataFunc 
		struct Input
		{
			float3 worldPos;
			float2 uv_texcoord;
			float4 vertexColor : COLOR;
		};

		uniform float4 GlobalWindDirectionAndStrength;
		uniform float4 _WindDirectionAndStrength;
		uniform sampler2D _GustNoise;
		uniform float _StiffnessVariation;
		uniform sampler2D _ShiverNoise;
		uniform float4 GlobalShiver;
		uniform float4 _Shiver;
		uniform float _BumpScale;
		uniform sampler2D _BumpMap;
		uniform sampler2D _MainTex;
		uniform float _Metallic;
		uniform sampler2D _MetallicGlossMap;
		uniform float _Glossiness;
		uniform float2 _GlossRemap;
		uniform sampler2D _OcclusionMap;
		uniform float2 _OcclusionRemap;
		uniform float _Cutoff = 0.33;

		UNITY_INSTANCING_BUFFER_START(OpenWorldNatureGrass)
			UNITY_DEFINE_INSTANCED_PROP(float, _Hue)
#define _Hue_arr OpenWorldNatureGrass
			UNITY_DEFINE_INSTANCED_PROP(float, _Saturation)
#define _Saturation_arr OpenWorldNatureGrass
			UNITY_DEFINE_INSTANCED_PROP(float, _Lightness)
#define _Lightness_arr OpenWorldNatureGrass
		UNITY_INSTANCING_BUFFER_END(OpenWorldNatureGrass)


		float3 RotateAroundAxis( float3 center, float3 original, float3 u, float angle )
		{
			original -= center;
			float C = cos( angle );
			float S = sin( angle );
			float t = 1 - C;
			float m00 = t * u.x * u.x + C;
			float m01 = t * u.x * u.y - S * u.z;
			float m02 = t * u.x * u.z + S * u.y;
			float m10 = t * u.x * u.y + S * u.z;
			float m11 = t * u.y * u.y + C;
			float m12 = t * u.y * u.z - S * u.x;
			float m20 = t * u.x * u.z - S * u.y;
			float m21 = t * u.y * u.z + S * u.x;
			float m22 = t * u.z * u.z + C;
			float3x3 finalMatrix = float3x3( m00, m01, m02, m10, m11, m12, m20, m21, m22 );
			return mul( finalMatrix, original ) + center;
		}


		float3 HSVToRGB( float3 c )
		{
			float4 K = float4( 1.0, 2.0 / 3.0, 1.0 / 3.0, 3.0 );
			float3 p = abs( frac( c.xxx + K.xyz ) * 6.0 - K.www );
			return c.z * lerp( K.xxx, saturate( p - K.xxx ), c.y );
		}


		float3 RGBToHSV(float3 c)
		{
			float4 K = float4(0.0, -1.0 / 3.0, 2.0 / 3.0, -1.0);
			float4 p = lerp( float4( c.bg, K.wz ), float4( c.gb, K.xy ), step( c.b, c.g ) );
			float4 q = lerp( float4( p.xyw, c.r ), float4( c.r, p.yzx ), step( p.x, c.r ) );
			float d = q.x - min( q.w, q.y );
			float e = 1.0e-10;
			return float3( abs(q.z + (q.w - q.y) / (6.0 * d + e)), d / (q.x + e), q.x);
		}

		void vertexDataFunc( inout appdata_full v, out Input o )
		{
			UNITY_INITIALIZE_OUTPUT( Input, o );
			float4 break15_g336 = ( GlobalWindDirectionAndStrength * _WindDirectionAndStrength );
			float4 appendResult3_g336 = (float4(break15_g336.x , 0.0 , break15_g336.y , 0.0));
			float clampResult36_g336 = clamp( ( length( appendResult3_g336 ) * 1000.0 ) , 0.0 , 1.0 );
			float4 lerpResult29_g336 = lerp( float4( float3(0.7,0,0.3) , 0.0 ) , appendResult3_g336 , clampResult36_g336);
			float4 normalizeResult5_g336 = normalize( lerpResult29_g336 );
			float4 globalGustDirection107 = normalizeResult5_g336;
			float4 transform15_g337 = mul(unity_ObjectToWorld,float4(0,0,0,1));
			float4 objectPivotInWS113 = transform15_g337;
			float3 break9_g434 = objectPivotInWS113.xyz;
			float4 appendResult11_g434 = (float4(break9_g434.x , break9_g434.z , 0.0 , 0.0));
			float4 pivotXY50_g434 = appendResult11_g434;
			#ifdef _UVMASK_ON
				float staticSwitch478 = ( v.texcoord.xy.y * 0.1 );
			#else
				float staticSwitch478 = 0.0;
			#endif
			float3 ase_worldPos = mul( unity_ObjectToWorld, v.vertex );
			#ifdef _BAKEDMASK_ON
				float staticSwitch483 = ( ( 1.0 - v.color.g ) * 0.05 );
			#else
				float staticSwitch483 = ( staticSwitch478 + frac( ( ( ase_worldPos.x + ase_worldPos.z ) * 0.02 ) ) );
			#endif
			float temp_output_204_0_g434 = staticSwitch483;
			float3 break12_g434 = globalGustDirection107.xyz;
			float4 appendResult13_g434 = (float4(break12_g434.x , break12_g434.z , 0.0 , 0.0));
			float4 gustDirection53_g434 = appendResult13_g434;
			float time109_g434 = _Time.y;
			float globalGustSpeed105 = max( ( GlobalWindDirectionAndStrength.z * _WindDirectionAndStrength.z ) , 0.01 );
			float4 tex2DNode6_g435 = tex2Dlod( _GustNoise, float4( ( ( ( pivotXY50_g434.xy + ( temp_output_204_0_g434 * 2.0 ) ) * 0.01 ) - ( gustDirection53_g434.xy * time109_g434 * globalGustSpeed105 ) ), 0, 0.0) );
			float gustNoise153_g434 = max( tex2DNode6_g435.r , 0.01 );
			float globalGustStrength106 = max( ( GlobalWindDirectionAndStrength.w * _WindDirectionAndStrength.w ) , 0.01 );
			float temp_output_18_0_g438 = ( gustNoise153_g434 * globalGustStrength106 );
			float clampResult16_g438 = clamp( temp_output_18_0_g438 , 0.1 , 0.9 );
			#ifdef _BAKEDMASK_ON
				float staticSwitch9_g408 = ( 1.0 - v.color.a );
			#else
				float staticSwitch9_g408 = 1.0;
			#endif
			#ifdef _UVMASK_ON
				float staticSwitch7_g408 = ( 1.0 - v.texcoord.xy.y );
			#else
				float staticSwitch7_g408 = 1.0;
			#endif
			float3 ase_vertex3Pos = v.vertex.xyz;
			float clampResult16_g408 = clamp( ase_vertex3Pos.y , 0.0 , 1.0 );
			#ifdef _VERTEXPOSITIONMASK_ON
				float staticSwitch8_g408 = ( 1.0 - clampResult16_g408 );
			#else
				float staticSwitch8_g408 = 1.0;
			#endif
			float relativeHeightMask101_g434 = max( ( 1.0 - ( staticSwitch9_g408 * staticSwitch7_g408 * staticSwitch8_g408 ) ) , 0.001 );
			float2 break2_g437 = ( pivotXY50_g434.xy * 10.0 );
			float clampResult8_g437 = clamp( pow( frac( ( break2_g437.x * break2_g437.y ) ) , 2.0 ) , ( 1.0 - _StiffnessVariation ) , 1.0 );
			float randomStiffness90_g434 = clampResult8_g437;
			float vertexMask103_g434 = 1.0;
			float gustStrength105_g434 = ( ( temp_output_18_0_g438 * ( 1.0 - clampResult16_g438 ) * 1.5 ) * relativeHeightMask101_g434 * randomStiffness90_g434 * vertexMask103_g434 );
			float gustStrengthAtPosition118 = gustStrength105_g434;
			float gustStrength25_g440 = gustStrengthAtPosition118;
			float3 scaledGustDirection155_g440 = ( globalGustDirection107.xyz * gustStrength25_g440 );
			float3 normalizeResult13_g440 = normalize( cross( float3(0,1,0) , scaledGustDirection155_g440 ) );
			float3 verticalAxis159_g440 = normalizeResult13_g440;
			float3 pivot118_g440 = objectPivotInWS113.xyz;
			float3 vertexOffset162_g440 = ( ase_worldPos - pivot118_g440 );
			float dotResult129_g440 = dot( verticalAxis159_g440 , vertexOffset162_g440 );
			float clampResult136_g440 = clamp( ( dotResult129_g440 * 1.0 ) , 0.0 , 1.0 );
			float3 lerpResult142_g440 = lerp( float3(0,1,0) , float3(0,-1,0) , clampResult136_g440);
			float3 horizontalAxis164_g440 = lerpResult142_g440;
			float3 lerpResult144_g440 = lerp( horizontalAxis164_g440 , verticalAxis159_g440 , v.color.b);
			#ifdef _BAKEDMASK_ON
				float3 staticSwitch178_g440 = lerpResult144_g440;
			#else
				float3 staticSwitch178_g440 = verticalAxis159_g440;
			#endif
			float3 rotationAxis166_g440 = staticSwitch178_g440;
			float3 rotatedValue12_g440 = RotateAroundAxis( pivot118_g440, ase_worldPos, rotationAxis166_g440, ( gustStrength25_g440 * 1.0 ) );
			float3 positionWithGust169_g440 = rotatedValue12_g440;
			float globalShiverSpeed149 = max( ( GlobalShiver.x * _Shiver.x ) , 0.01 );
			float4 tex2DNode11_g439 = tex2Dlod( _ShiverNoise, float4( ( ( pivotXY50_g434.xy + ( temp_output_204_0_g434 * 2.0 ) ) - ( gustDirection53_g434.xy * time109_g434 * globalShiverSpeed149 ) ), 0, 0.0) );
			float4 appendResult6_g439 = (float4(tex2DNode11_g439.r , tex2DNode11_g439.g , tex2DNode11_g439.b , 0.0));
			float4 temp_cast_10 = (0.5).xxxx;
			float4 shiverNoise155_g434 = ( ( appendResult6_g439 - temp_cast_10 ) * 2.0 );
			float temp_output_28_0_g436 = 0.2;
			float3 appendResult32_g436 = (float3(1.0 , temp_output_28_0_g436 , 1.0));
			float3 temp_output_29_0_g436 = ( shiverNoise155_g434.xyz * appendResult32_g436 );
			float2 break10_g436 = gustDirection53_g434.xy;
			float4 appendResult4_g436 = (float4(break10_g436.x , temp_output_28_0_g436 , break10_g436.y , 0.0));
			float temp_output_21_0_g436 = gustStrength105_g434;
			float clampResult45_g436 = clamp( ( ( temp_output_21_0_g436 * 0.8 ) + 0.2 ) , 0.0 , 1.0 );
			float4 lerpResult26_g436 = lerp( float4( temp_output_29_0_g436 , 0.0 ) , appendResult4_g436 , clampResult45_g436);
			float4 shiverDirectionAtPosition132 = lerpResult26_g436;
			float3 shiverDirection29_g440 = shiverDirectionAtPosition132.xyz;
			float temp_output_5_0_g436 = length( temp_output_29_0_g436 );
			float globalShiverStrength141 = max( ( GlobalShiver.y * _Shiver.y ) , 0.01 );
			float temp_output_6_0_g436 = globalShiverStrength141;
			float shiverStrengthAtPosition125 = ( relativeHeightMask101_g434 * ( ( temp_output_21_0_g436 * 0.5 ) + ( temp_output_5_0_g436 * temp_output_6_0_g436 ) ) * vertexMask103_g434 * randomStiffness90_g434 );
			float shiverStrength30_g440 = shiverStrengthAtPosition125;
			float3 shiverPositionOffset170_g440 = ( shiverDirection29_g440 * shiverStrength30_g440 );
			float4 appendResult87 = (float4(( positionWithGust169_g440 + shiverPositionOffset170_g440 ) , 1.0));
			float4 transform29 = mul(unity_WorldToObject,appendResult87);
			float4 vertexPositionWithWind411 = transform29;
			v.vertex.xyz = vertexPositionWithWind411.xyz;
		}

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float2 uv_BumpMap387 = i.uv_texcoord;
			o.Normal = UnpackScaleNormal( tex2D( _BumpMap, uv_BumpMap387 ), _BumpScale );
			float _Hue_Instance = UNITY_ACCESS_INSTANCED_PROP(_Hue_arr, _Hue);
			float2 uv_MainTex1 = i.uv_texcoord;
			float4 tex2DNode1 = tex2D( _MainTex, uv_MainTex1 );
			float4 albedo424 = tex2DNode1;
			float3 hsvTorgb432 = RGBToHSV( albedo424.rgb );
			float _Saturation_Instance = UNITY_ACCESS_INSTANCED_PROP(_Saturation_arr, _Saturation);
			float _Lightness_Instance = UNITY_ACCESS_INSTANCED_PROP(_Lightness_arr, _Lightness);
			float3 hsvTorgb435 = HSVToRGB( float3(( _Hue_Instance + hsvTorgb432.x ),( hsvTorgb432.y + _Saturation_Instance ),( hsvTorgb432.z + _Lightness_Instance )) );
			float4 transform15_g337 = mul(unity_ObjectToWorld,float4(0,0,0,1));
			float4 objectPivotInWS113 = transform15_g337;
			float3 break9_g434 = objectPivotInWS113.xyz;
			float4 appendResult11_g434 = (float4(break9_g434.x , break9_g434.z , 0.0 , 0.0));
			float4 pivotXY50_g434 = appendResult11_g434;
			#ifdef _UVMASK_ON
				float staticSwitch478 = ( i.uv_texcoord.y * 0.1 );
			#else
				float staticSwitch478 = 0.0;
			#endif
			float3 ase_worldPos = i.worldPos;
			#ifdef _BAKEDMASK_ON
				float staticSwitch483 = ( ( 1.0 - i.vertexColor.g ) * 0.05 );
			#else
				float staticSwitch483 = ( staticSwitch478 + frac( ( ( ase_worldPos.x + ase_worldPos.z ) * 0.02 ) ) );
			#endif
			float temp_output_204_0_g434 = staticSwitch483;
			float4 break15_g336 = ( GlobalWindDirectionAndStrength * _WindDirectionAndStrength );
			float4 appendResult3_g336 = (float4(break15_g336.x , 0.0 , break15_g336.y , 0.0));
			float clampResult36_g336 = clamp( ( length( appendResult3_g336 ) * 1000.0 ) , 0.0 , 1.0 );
			float4 lerpResult29_g336 = lerp( float4( float3(0.7,0,0.3) , 0.0 ) , appendResult3_g336 , clampResult36_g336);
			float4 normalizeResult5_g336 = normalize( lerpResult29_g336 );
			float4 globalGustDirection107 = normalizeResult5_g336;
			float3 break12_g434 = globalGustDirection107.xyz;
			float4 appendResult13_g434 = (float4(break12_g434.x , break12_g434.z , 0.0 , 0.0));
			float4 gustDirection53_g434 = appendResult13_g434;
			float time109_g434 = _Time.y;
			float globalGustSpeed105 = max( ( GlobalWindDirectionAndStrength.z * _WindDirectionAndStrength.z ) , 0.01 );
			float4 tex2DNode6_g435 = tex2D( _GustNoise, ( ( ( pivotXY50_g434.xy + ( temp_output_204_0_g434 * 2.0 ) ) * 0.01 ) - ( gustDirection53_g434.xy * time109_g434 * globalGustSpeed105 ) ) );
			float gustNoise153_g434 = max( tex2DNode6_g435.r , 0.01 );
			float globalGustStrength106 = max( ( GlobalWindDirectionAndStrength.w * _WindDirectionAndStrength.w ) , 0.01 );
			float temp_output_18_0_g438 = ( gustNoise153_g434 * globalGustStrength106 );
			float clampResult16_g438 = clamp( temp_output_18_0_g438 , 0.1 , 0.9 );
			#ifdef _BAKEDMASK_ON
				float staticSwitch9_g408 = ( 1.0 - i.vertexColor.a );
			#else
				float staticSwitch9_g408 = 1.0;
			#endif
			#ifdef _UVMASK_ON
				float staticSwitch7_g408 = ( 1.0 - i.uv_texcoord.y );
			#else
				float staticSwitch7_g408 = 1.0;
			#endif
			float3 ase_vertex3Pos = mul( unity_WorldToObject, float4( i.worldPos , 1 ) );
			float clampResult16_g408 = clamp( ase_vertex3Pos.y , 0.0 , 1.0 );
			#ifdef _VERTEXPOSITIONMASK_ON
				float staticSwitch8_g408 = ( 1.0 - clampResult16_g408 );
			#else
				float staticSwitch8_g408 = 1.0;
			#endif
			float relativeHeightMask101_g434 = max( ( 1.0 - ( staticSwitch9_g408 * staticSwitch7_g408 * staticSwitch8_g408 ) ) , 0.001 );
			float2 break2_g437 = ( pivotXY50_g434.xy * 10.0 );
			float clampResult8_g437 = clamp( pow( frac( ( break2_g437.x * break2_g437.y ) ) , 2.0 ) , ( 1.0 - _StiffnessVariation ) , 1.0 );
			float randomStiffness90_g434 = clampResult8_g437;
			float vertexMask103_g434 = 1.0;
			float gustStrength105_g434 = ( ( temp_output_18_0_g438 * ( 1.0 - clampResult16_g438 ) * 1.5 ) * relativeHeightMask101_g434 * randomStiffness90_g434 * vertexMask103_g434 );
			float gustStrengthAtPosition118 = gustStrength105_g434;
			float3 temp_cast_7 = (gustStrengthAtPosition118).xxx;
			#ifdef _DEBUGGUST_ON
				float3 staticSwitch497 = temp_cast_7;
			#else
				float3 staticSwitch497 = hsvTorgb435;
			#endif
			o.Albedo = staticSwitch497;
			float2 uv_MetallicGlossMap500 = i.uv_texcoord;
			float4 tex2DNode500 = tex2D( _MetallicGlossMap, uv_MetallicGlossMap500 );
			#ifdef _METALLICGLOSSMAP_ON
				float staticSwitch504 = tex2DNode500.r;
			#else
				float staticSwitch504 = _Metallic;
			#endif
			float MetallicMap507 = staticSwitch504;
			o.Metallic = MetallicMap507;
			#ifdef _METALLICGLOSSMAP_ON
				float staticSwitch505 = (_GlossRemap.x + (tex2DNode500.a - 0.0) * (_GlossRemap.y - _GlossRemap.x) / (1.0 - 0.0));
			#else
				float staticSwitch505 = _Glossiness;
			#endif
			float GlossMap506 = staticSwitch505;
			o.Smoothness = GlossMap506;
			float2 uv_OcclusionMap398 = i.uv_texcoord;
			o.Occlusion = (_OcclusionRemap.x + (tex2D( _OcclusionMap, uv_OcclusionMap398 ).r - 0.0) * (_OcclusionRemap.y - _OcclusionRemap.x) / (1.0 - 0.0));
			o.Alpha = 1;
			float alpha425 = tex2DNode1.a;
			clip( alpha425 - _Cutoff );
		}

		ENDCG
	}
	Fallback "Diffuse"
	CustomEditor "VisualDesignCafe.Nature.Editor.NatureMaterialEditor"
}
/*ASEBEGIN
Version=17500
129;18;960;905;4221.003;1048.752;1;True;False
Node;AmplifyShaderEditor.WorldPosInputsNode;472;-5399.052,2224.493;Float;False;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.TextureCoordinatesNode;473;-5215.584,2038.674;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleAddOpNode;474;-5127.228,2237.217;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;477;-4967.029,2122.982;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0.1;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;476;-5125.142,1960.948;Float;False;Constant;_Float9;Float 9;2;0;Create;True;0;0;False;0;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.VertexColorNode;479;-4763.928,2433.691;Inherit;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.CommentaryNode;112;-4004.226,860.4534;Inherit;False;652.8523;550.6835;Global Wind Parameters;6;106;105;107;141;149;407;Global Wind;1,1,1,1;0;0
Node;AmplifyShaderEditor.CommentaryNode;116;-4008.191,1529.564;Inherit;False;672.4673;168.7953;Comment;2;89;113;World Space Object Pivot;1,1,1,1;0;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;475;-4924.296,2225.514;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0.02;False;1;FLOAT;0
Node;AmplifyShaderEditor.StaticSwitch;478;-4745.414,2041.573;Float;False;Property;_UVMask;UV Mask;8;0;Create;True;0;0;False;0;0;0;0;True;;Toggle;2;Key0;Key1;Create;False;9;1;FLOAT;0;False;0;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT;0;False;7;FLOAT;0;False;8;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.FunctionNode;407;-3962.294,1033.771;Inherit;False;GlobalWindParameters;17;;336;ef55991c5ff9f3747b20a326fd322e36;0;0;5;FLOAT;11;FLOAT;10;FLOAT;8;FLOAT;6;FLOAT4;0
Node;AmplifyShaderEditor.FunctionNode;89;-3958.191,1579.564;Inherit;False;GetPivotInWorldSpace;-1;;337;264e0929a81902742a5a4e0e0a62ac57;0;0;1;FLOAT4;0
Node;AmplifyShaderEditor.FractNode;480;-4658.766,2210.612;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;482;-4550.997,2442.727;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;117;-4016,1824;Inherit;False;1569.851;1216.966;Calculate wind strength at vertex position;14;55;114;200;150;111;142;135;146;110;132;125;118;202;265;Wind Strength;1,1,1,1;0;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;141;-3626.28,1007.885;Float;False;globalShiverStrength;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;105;-3605.74,1085.918;Float;False;globalGustSpeed;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;149;-3621.772,922.121;Float;False;globalShiverSpeed;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;481;-4372.683,2123.713;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;498;-4353.017,2426.556;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0.05;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;113;-3578.724,1583.359;Float;False;objectPivotInWS;-1;True;1;0;FLOAT4;0,0,0,0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;106;-3607.401,1174.849;Float;False;globalGustStrength;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;107;-3613.996,1265.174;Float;False;globalGustDirection;-1;True;1;0;FLOAT4;0,0,0,0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.GetLocalVarNode;142;-3856,2192;Inherit;False;141;globalShiverStrength;1;0;OBJECT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.StaticSwitch;483;-4204.556,2297.605;Float;False;Property;_BakedMask;Baked Mask;3;0;Create;True;0;0;False;0;0;1;0;True;;Toggle;2;Key0;Key1;Create;False;9;1;FLOAT;0;False;0;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT;0;False;7;FLOAT;0;False;8;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;111;-3824,2128;Inherit;False;105;globalGustSpeed;1;0;OBJECT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;200;-3840,2640;Inherit;False;106;globalGustStrength;1;0;OBJECT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleTimeNode;146;-3760,2480;Inherit;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;202;-3872,1936;Float;False;Property;_StiffnessVariation;StiffnessVariation;22;0;Create;True;0;0;False;0;0;0.3;0;0.99;0;1;FLOAT;0
Node;AmplifyShaderEditor.TexturePropertyNode;55;-3808,2816;Float;True;Global;_GustNoise;_GustNoise;24;0;Create;True;0;0;False;0;None;156c5c844ac14b042b7dacdcfcd0981b;False;black;Auto;Texture2D;-1;0;1;SAMPLER2D;0
Node;AmplifyShaderEditor.GetLocalVarNode;110;-3840,2560;Inherit;False;107;globalGustDirection;1;0;OBJECT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.GetLocalVarNode;114;-3808,2720;Inherit;False;113;objectPivotInWS;1;0;OBJECT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.TexturePropertyNode;135;-3808,2272;Float;True;Global;_ShiverNoise;ShiverNoise;23;0;Create;False;0;0;False;0;None;66dd7d1835f20b8419dbc5544a12688a;False;gray;Auto;Texture2D;-1;0;1;SAMPLER2D;0
Node;AmplifyShaderEditor.FunctionNode;491;-3808.245,1852.675;Inherit;False;GetHeightMask;4;;408;d64c2e0885795d34cb76988e77bd8660;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;150;-3840,2048;Inherit;False;149;globalShiverSpeed;1;0;OBJECT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.FunctionNode;496;-3360,2224;Inherit;False;GetWindStrength;-1;;434;81b67046328b6f44f9bbfde7e0fba2b2;0;14;204;FLOAT;0;False;171;FLOAT;0.2;False;165;FLOAT;1;False;164;FLOAT;1;False;114;FLOAT;0;False;60;FLOAT;1;False;55;FLOAT;1;False;49;FLOAT;0.1;False;24;SAMPLER2D;0;False;4;FLOAT;0;False;5;FLOAT3;0,0,0;False;28;FLOAT;1;False;1;FLOAT3;0,0,0;False;3;SAMPLER2D;0,0,0,0;False;4;FLOAT;138;FLOAT;43;FLOAT4;44;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;423;-3965.596,-228.4894;Inherit;False;748;273;;3;424;1;425;Albedo;1,1,1,1;0;0
Node;AmplifyShaderEditor.SamplerNode;1;-3915.596,-178.4894;Inherit;True;Property;_MainTex;MainTex;12;1;[NoScaleOffset];Create;True;0;0;False;0;-1;None;081af5c0e04008140b98f70f21251b8d;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RegisterLocalVarNode;118;-2864,2448;Float;False;gustStrengthAtPosition;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;132;-2864,2320;Float;False;shiverDirectionAtPosition;-1;True;1;0;FLOAT4;0,0,0,0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.CommentaryNode;120;-2242.677,1836.51;Inherit;False;1849.186;659.9193;Apply wind displacement to vertex position;10;29;87;411;88;86;109;129;133;115;119;Wind Displacement;1,1,1,1;0;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;125;-2864,2192;Float;False;shiverStrengthAtPosition;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;115;-2122.556,2363.043;Inherit;False;113;objectPivotInWS;1;0;OBJECT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.GetLocalVarNode;133;-2183.851,1990.018;Inherit;False;132;shiverDirectionAtPosition;1;0;OBJECT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.GetLocalVarNode;119;-2163.99,2143.075;Inherit;False;118;gustStrengthAtPosition;1;0;OBJECT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;129;-2185.187,1906.63;Inherit;False;125;shiverStrengthAtPosition;1;0;OBJECT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;109;-2148.943,2066.18;Inherit;False;107;globalGustDirection;1;0;OBJECT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.WorldPosInputsNode;86;-2076.601,2219.387;Float;False;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.CommentaryNode;431;-212.3872,1836.851;Inherit;False;1911.01;1897.957;;21;0;398;387;426;416;427;432;441;433;436;434;443;442;435;444;445;446;471;497;508;509;Output;1,1,1,1;0;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;424;-3470.022,-165.6478;Float;False;albedo;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.FunctionNode;487;-1495.972,2045.205;Inherit;False;ApplyWindDisplacement;1;;440;739735a3fc284b84ca40e29145dfcbfd;0;6;20;FLOAT;1;False;19;FLOAT3;1,0,0;False;6;FLOAT3;0,0,0;False;7;FLOAT;0;False;1;FLOAT3;0,0,0;False;3;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RangedFloatNode;88;-1316.945,2258.902;Float;False;Constant;_Float2;Float 2;8;0;Create;True;0;0;False;0;1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;500;-4054.036,-856.9245;Inherit;True;Property;_MetallicGlossMap;Metallic Gloss Map;21;1;[NoScaleOffset];Create;True;0;0;False;0;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.GetLocalVarNode;427;-157.9902,2029.062;Inherit;False;424;albedo;1;0;OBJECT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.Vector2Node;499;-3943.159,-537.0719;Inherit;False;Property;_GlossRemap;Gloss Remap;27;0;Create;True;0;0;False;0;0,1;0,1;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.RangedFloatNode;501;-3664.765,-887.8676;Inherit;False;Property;_Metallic;_Metallic;30;0;Create;True;0;0;False;0;0;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;433;16.66673,1945.194;Float;False;InstancedProperty;_Hue;Hue;9;0;Create;True;0;0;False;0;0;0;-0.5;0.5;0;1;FLOAT;0
Node;AmplifyShaderEditor.RGBToHSVNode;432;76.74555,2036.564;Float;False;1;0;FLOAT3;0,0,0;False;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.RangedFloatNode;503;-3651.85,-712.6447;Inherit;False;Property;_Glossiness;_Glossiness;29;0;Create;True;0;0;False;0;0.2;0.2;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.TFHCRemapNode;502;-3565.318,-583.9532;Inherit;False;5;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;0;False;4;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;441;18.55271,2299.164;Float;False;InstancedProperty;_Lightness;Lightness;11;0;Create;True;0;0;False;0;0;0;-1;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode;87;-1130.095,2089.546;Inherit;False;FLOAT4;4;0;FLOAT3;0,0,0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.RangedFloatNode;436;19.10178,2198.11;Float;False;InstancedProperty;_Saturation;Saturation;10;0;Create;True;0;0;False;0;0;0;-1;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.StaticSwitch;504;-3332.272,-824.369;Inherit;False;Property;_METALLICGLOSSMAP;_METALLICGLOSSMAP;25;0;Create;True;0;0;False;0;0;0;0;True;_METALLICGLOSSMAP;Toggle;2;Key0;Key1;Create;False;9;1;FLOAT;0;False;0;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT;0;False;7;FLOAT;0;False;8;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;434;364.0157,1978.522;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.StaticSwitch;505;-3321.112,-647.7083;Inherit;False;Property;_METALLICGLOSSMAP;_METALLICGLOSSMAP;26;0;Create;True;0;0;False;0;0;0;0;True;_METALLICGLOSSMAP;Toggle;2;Key0;Key1;Create;False;9;1;FLOAT;0;False;0;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT;0;False;7;FLOAT;0;False;8;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;443;364.5528,2224.164;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;442;363.5528,2106.165;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.WorldToObjectTransfNode;29;-955.1426,2082.968;Inherit;False;1;0;FLOAT4;0,0,0,1;False;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.Vector2Node;445;196.8776,2814.611;Float;False;Property;_OcclusionRemap;Occlusion Remap;15;0;Create;True;0;0;False;0;0,1;0,1;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.SamplerNode;398;114.4048,2585.993;Inherit;True;Property;_OcclusionMap;Occlusion Map;16;1;[NoScaleOffset];Create;True;0;0;False;0;-1;None;922fd6006138eaf41a11499065b47004;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.CommentaryNode;422;-3995.063,191.1917;Inherit;False;1055.5;488.6848;;4;402;401;403;420;Translucency;1,1,1,1;0;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;506;-3008.134,-606.0787;Inherit;False;GlossMap;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;471;611.134,2252.942;Inherit;False;118;gustStrengthAtPosition;1;0;OBJECT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.HSVToRGBNode;435;571.1867,2056.534;Float;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.RegisterLocalVarNode;425;-3455.624,-44.04722;Float;False;alpha;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;444;29.33274,2460.332;Float;False;Property;_BumpScale;Bump Scale;13;0;Create;True;0;0;False;0;1;1;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;411;-682.4496,2143.209;Float;False;vertexPositionWithWind;-1;True;1;0;FLOAT4;0,0,0,0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;507;-3016.684,-861.9533;Inherit;False;MetallicMap;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;508;990.2744,2521.775;Inherit;False;507;MetallicMap;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;387;505.4387,2382.049;Inherit;True;Property;_BumpMap;Bump Map;14;1;[NoScaleOffset];Create;True;0;0;False;0;-1;None;18ecaaa71b2c4644ca8640598f0e03af;True;0;True;bump;Auto;True;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RegisterLocalVarNode;265;-2864,2048;Float;False;gustHighlightAtPosition;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;402;-3945.063,449.8767;Inherit;True;Property;_TranslucencyMap;TranslucencyMap;28;1;[NoScaleOffset];Create;True;0;0;False;0;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.GetLocalVarNode;509;1018.63,2673.481;Inherit;False;506;GlossMap;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;401;-3867.631,241.1917;Float;False;Property;_TranslucencyTint;TranslucencyTint;26;0;Create;True;0;0;False;0;0,0,0,0;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.StaticSwitch;497;1158.784,2184.318;Float;False;Property;_DebugGust;Debug Gust;20;0;Create;True;0;0;False;0;0;0;0;True;;Toggle;2;Key0;Key1;Create;False;9;1;FLOAT3;0,0,0;False;0;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT3;0,0,0;False;4;FLOAT3;0,0,0;False;5;FLOAT3;0,0,0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.TFHCRemapNode;446;611.7875,2631.054;Inherit;False;5;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;0;False;4;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;420;-3184.064,493.4647;Float;False;translucency;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;416;543.2245,2998.747;Inherit;False;411;vertexPositionWithWind;1;0;OBJECT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.GetLocalVarNode;426;631.1324,2862.147;Inherit;False;425;alpha;1;0;OBJECT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;403;-3496.244,370.9784;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;1382.359,2432.312;Float;False;True;-1;2;VisualDesignCafe.Nature.Editor.NatureMaterialEditor;0;0;Standard;Open World Nature/Grass;False;False;False;False;False;False;False;False;False;False;False;False;True;True;False;False;True;False;False;False;False;Off;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Masked;0.33;True;True;0;False;TransparentCutout;;AlphaTest;ForwardOnly;14;all;True;True;True;False;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;True;0;5;False;-1;10;False;-1;0;5;False;-1;10;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Spherical;False;Absolute;0;;0;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;474;0;472;1
WireConnection;474;1;472;3
WireConnection;477;0;473;2
WireConnection;475;0;474;0
WireConnection;478;1;476;0
WireConnection;478;0;477;0
WireConnection;480;0;475;0
WireConnection;482;0;479;2
WireConnection;141;0;407;10
WireConnection;105;0;407;8
WireConnection;149;0;407;11
WireConnection;481;0;478;0
WireConnection;481;1;480;0
WireConnection;498;0;482;0
WireConnection;113;0;89;0
WireConnection;106;0;407;6
WireConnection;107;0;407;0
WireConnection;483;1;481;0
WireConnection;483;0;498;0
WireConnection;496;204;483;0
WireConnection;496;165;491;0
WireConnection;496;114;202;0
WireConnection;496;60;150;0
WireConnection;496;55;111;0
WireConnection;496;49;142;0
WireConnection;496;24;135;0
WireConnection;496;4;146;0
WireConnection;496;5;110;0
WireConnection;496;28;200;0
WireConnection;496;1;114;0
WireConnection;496;3;55;0
WireConnection;118;0;496;0
WireConnection;132;0;496;44
WireConnection;125;0;496;43
WireConnection;424;0;1;0
WireConnection;487;20;129;0
WireConnection;487;19;133;0
WireConnection;487;6;109;0
WireConnection;487;7;119;0
WireConnection;487;1;86;0
WireConnection;487;3;115;0
WireConnection;432;0;427;0
WireConnection;502;0;500;4
WireConnection;502;3;499;1
WireConnection;502;4;499;2
WireConnection;87;0;487;0
WireConnection;87;3;88;0
WireConnection;504;1;501;0
WireConnection;504;0;500;1
WireConnection;434;0;433;0
WireConnection;434;1;432;1
WireConnection;505;1;503;0
WireConnection;505;0;502;0
WireConnection;443;0;432;3
WireConnection;443;1;441;0
WireConnection;442;0;432;2
WireConnection;442;1;436;0
WireConnection;29;0;87;0
WireConnection;506;0;505;0
WireConnection;435;0;434;0
WireConnection;435;1;442;0
WireConnection;435;2;443;0
WireConnection;425;0;1;4
WireConnection;411;0;29;0
WireConnection;507;0;504;0
WireConnection;387;5;444;0
WireConnection;265;0;496;138
WireConnection;497;1;435;0
WireConnection;497;0;471;0
WireConnection;446;0;398;1
WireConnection;446;3;445;1
WireConnection;446;4;445;2
WireConnection;420;0;403;0
WireConnection;403;0;401;0
WireConnection;403;1;402;0
WireConnection;0;0;497;0
WireConnection;0;1;387;0
WireConnection;0;3;508;0
WireConnection;0;4;509;0
WireConnection;0;5;446;0
WireConnection;0;10;426;0
WireConnection;0;11;416;0
ASEEND*/
//CHKSM=DFCF35BE0144D1DBD15A76EA6E60EE5208208BB9
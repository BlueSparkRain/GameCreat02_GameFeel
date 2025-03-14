// Made with Amplify Shader Editor v1.9.1.5
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "SA_CircleDisslove"
{
	Properties
	{
		[PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}
		_Color ("Tint", Color) = (1,1,1,1)
		[MaterialToggle] PixelSnap ("Pixel snap", Float) = 0
		[PerRendererData] _AlphaTex ("External Alpha", 2D) = "white" {}
		_TansBar("TansBar", Range( 0 , 1)) = 0.6726901
		_CircleTilling("CircleTilling", Vector) = (16,9,0,0)
		_CircelRim("CircelRim", Range( 0 , 0.5)) = 0.108
		[HDR]_RimColorA("RimColorA", Color) = (0,0.5121677,1,0)
		[HDR]_RimColorB("RimColorB", Color) = (1,0,0,0)
		_NoiseTex("NoiseTex", 2D) = "white" {}
		_UV_Noise_Intensity("UV_Noise_Intensity", Range( 0 , 0.05)) = 0.05
		_NoiseSpeed("NoiseSpeed", Vector) = (0,0.5,0,0)
		_RB_Offset("RB_Offset", Vector) = (0.02,0,-0.02,0)

	}

	SubShader
	{
		LOD 0

		Tags { "Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent" "PreviewType"="Plane" "CanUseSpriteAtlas"="True" }

		Cull Off
		Lighting Off
		ZWrite Off
		Blend One OneMinusSrcAlpha
		
		GrabPass{ "_GrabScreen0" }

		Pass
		{
		CGPROGRAM
			#if defined(UNITY_STEREO_INSTANCING_ENABLED) || defined(UNITY_STEREO_MULTIVIEW_ENABLED)
			#define ASE_DECLARE_SCREENSPACE_TEXTURE(tex) UNITY_DECLARE_SCREENSPACE_TEXTURE(tex);
			#else
			#define ASE_DECLARE_SCREENSPACE_TEXTURE(tex) UNITY_DECLARE_SCREENSPACE_TEXTURE(tex)
			#endif

			#ifndef UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX
			#define UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(input)
			#endif
			#pragma vertex vert
			#pragma fragment frag
			#pragma target 3.0
			#pragma multi_compile _ PIXELSNAP_ON
			#pragma multi_compile _ ETC1_EXTERNAL_ALPHA
			#include "UnityCG.cginc"
			#include "UnityShaderVariables.cginc"
			#define ASE_NEEDS_FRAG_COLOR


			struct appdata_t
			{
				float4 vertex   : POSITION;
				float4 color    : COLOR;
				float2 texcoord : TEXCOORD0;
				UNITY_VERTEX_INPUT_INSTANCE_ID
				
			};

			struct v2f
			{
				float4 vertex   : SV_POSITION;
				fixed4 color    : COLOR;
				float2 texcoord  : TEXCOORD0;
				UNITY_VERTEX_INPUT_INSTANCE_ID
				UNITY_VERTEX_OUTPUT_STEREO
				float4 ase_texcoord1 : TEXCOORD1;
			};
			
			uniform fixed4 _Color;
			uniform float _EnableExternalAlpha;
			uniform sampler2D _MainTex;
			uniform sampler2D _AlphaTex;
			uniform sampler2D _NoiseTex;
			uniform float2 _NoiseSpeed;
			uniform float4 _NoiseTex_ST;
			uniform float _UV_Noise_Intensity;
			uniform float4 _RB_Offset;
			ASE_DECLARE_SCREENSPACE_TEXTURE( _GrabScreen0 )
			uniform float2 _CircleTilling;
			uniform float _TansBar;
			uniform float _CircelRim;
			uniform float4 _RimColorA;
			uniform float4 _RimColorB;

			
			v2f vert( appdata_t IN  )
			{
				v2f OUT;
				UNITY_SETUP_INSTANCE_ID(IN);
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(OUT);
				UNITY_TRANSFER_INSTANCE_ID(IN, OUT);
				float4 ase_clipPos = UnityObjectToClipPos(IN.vertex);
				float4 screenPos = ComputeScreenPos(ase_clipPos);
				OUT.ase_texcoord1 = screenPos;
				
				
				IN.vertex.xyz +=  float3(0,0,0) ; 
				OUT.vertex = UnityObjectToClipPos(IN.vertex);
				OUT.texcoord = IN.texcoord;
				OUT.color = IN.color * _Color;
				#ifdef PIXELSNAP_ON
				OUT.vertex = UnityPixelSnap (OUT.vertex);
				#endif

				return OUT;
			}

			fixed4 SampleSpriteTexture (float2 uv)
			{
				fixed4 color = tex2D (_MainTex, uv);

#if ETC1_EXTERNAL_ALPHA
				// get the color from an external texture (usecase: Alpha support for ETC1 on android)
				fixed4 alpha = tex2D (_AlphaTex, uv);
				color.a = lerp (color.a, alpha.r, _EnableExternalAlpha);
#endif //ETC1_EXTERNAL_ALPHA

				return color;
			}
			
			fixed4 frag(v2f IN  ) : SV_Target
			{
				UNITY_SETUP_INSTANCE_ID( IN );
				UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX( IN );

				float2 texCoord41 = IN.texcoord.xy * float2( 1,1 ) + float2( 0,0 );
				float2 uv_NoiseTex = IN.texcoord.xy * _NoiseTex_ST.xy + _NoiseTex_ST.zw;
				float2 panner42 = ( _Time.y * _NoiseSpeed + uv_NoiseTex);
				float2 temp_output_57_0 = ( texCoord41 + ( tex2D( _NoiseTex, panner42 ).r * _UV_Noise_Intensity ) );
				float2 appendResult60 = (float2(_RB_Offset.x , _RB_Offset.y));
				float4 tex2DNode67 = tex2D( _MainTex, temp_output_57_0 );
				float2 appendResult59 = (float2(_RB_Offset.z , _RB_Offset.w));
				float4 appendResult69 = (float4(tex2D( _MainTex, ( temp_output_57_0 + ( appendResult60 * IN.color.a ) ) ).r , tex2DNode67.g , tex2D( _MainTex, ( temp_output_57_0 + ( appendResult59 * IN.color.a ) ) ).b , tex2DNode67.a));
				float4 RB_OffsetColor70 = appendResult69;
				float4 screenPos = IN.ase_texcoord1;
				float4 ase_screenPosNorm = screenPos / screenPos.w;
				ase_screenPosNorm.z = ( UNITY_NEAR_CLIP_VALUE >= 0 ) ? ase_screenPosNorm.z : ase_screenPosNorm.z * 0.5 + 0.5;
				float4 screenColor32 = UNITY_SAMPLE_SCREENSPACE_TEXTURE(_GrabScreen0,ase_screenPosNorm.xy);
				float4 appendResult34 = (float4((screenColor32).rgb , 0.0));
				float4 ScreenColor73 = appendResult34;
				float2 texCoord2 = IN.texcoord.xy * _CircleTilling + float2( 0,0 );
				float2 _Vector0 = float2(0.5,0.5);
				float2 texCoord9 = IN.texcoord.xy * float2( 1,1 ) + float2( 0,0 );
				float cos7 = cos( _Time.y );
				float sin7 = sin( _Time.y );
				float2 rotator7 = mul( texCoord9 - _Vector0 , float2x2( cos7 , -sin7 , sin7 , cos7 )) + _Vector0;
				float temp_output_12_0 = ( distance( frac( texCoord2 ) , _Vector0 ) + rotator7.x );
				float temp_output_50_0 = (2.0 + (_TansBar - 0.0) * (0.0 - 2.0) / (1.0 - 0.0));
				float temp_output_17_0 = step( temp_output_12_0 , temp_output_50_0 );
				float4 lerpResult1 = lerp( RB_OffsetColor70 , ScreenColor73 , temp_output_17_0);
				float2 texCoord27 = IN.texcoord.xy * float2( 1,1 ) + float2( 0,0 );
				float4 lerpResult25 = lerp( _RimColorA , _RimColorB , texCoord27.y);
				float4 RimColor28 = ( ( temp_output_17_0 - step( ( temp_output_12_0 + _CircelRim ) , temp_output_50_0 ) ) * lerpResult25 );
				
				fixed4 c = ( lerpResult1 + RimColor28 );
				c.rgb *= c.a;
				return c;
			}
		ENDCG
		}
	}
	CustomEditor "ASEMaterialInspector"
	
	Fallback Off
}
/*ASEBEGIN
Version=19105
Node;AmplifyShaderEditor.CommentaryNode;75;-2919.462,-608.2057;Inherit;False;1067.583;258.6947;ScreenColor;6;34;33;35;30;32;73;;1,0.827044,0.8734061,1;0;0
Node;AmplifyShaderEditor.CommentaryNode;72;-2925.797,-1677.928;Inherit;False;2472.55;1019.738;Comment;23;59;60;63;62;61;64;42;46;44;45;36;40;58;41;57;66;65;37;67;38;68;69;70;RBOffestColor;1,0.8954363,0.8396226,1;0;0
Node;AmplifyShaderEditor.DynamicAppendNode;59;-2245.62,-832.9952;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.DynamicAppendNode;60;-2245.543,-956.3448;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.Vector4Node;63;-2515.233,-953.539;Inherit;False;Property;_RB_Offset;RB_Offset;8;0;Create;True;0;0;0;False;0;False;0.02,0,-0.02,0;0,0,0,0;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;62;-2051.567,-968.345;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;61;-2054.555,-833.5089;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.VertexColorNode;64;-2274.725,-1165.369;Inherit;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.PannerNode;42;-2638.77,-1478.006;Inherit;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.Vector2Node;46;-2818.057,-1457.888;Inherit;False;Property;_NoiseSpeed;NoiseSpeed;7;0;Create;True;0;0;0;False;0;False;0,0.5;0,0;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.SimpleTimeNode;44;-2819.013,-1336.15;Inherit;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;45;-2875.797,-1581.81;Inherit;False;0;36;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;36;-2469.331,-1505.491;Inherit;True;Property;_NoiseTex;NoiseTex;5;0;Create;True;0;0;0;False;0;False;-1;4caad65a41cca98419a1ec895e1ddbf5;4caad65a41cca98419a1ec895e1ddbf5;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;40;-2470.885,-1309.31;Inherit;False;Property;_UV_Noise_Intensity;UV_Noise_Intensity;6;0;Create;True;0;0;0;False;0;False;0.05;0;0;0.05;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;58;-2148.654,-1430.491;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;41;-2163.794,-1566.589;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleAddOpNode;57;-1853.099,-1458.175;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleAddOpNode;66;-1545.995,-859.1802;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleAddOpNode;65;-1571.982,-1455.578;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SamplerNode;37;-1286.529,-1477.974;Inherit;True;Property;_TextureSample0;Texture Sample 0;6;0;Create;True;0;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;67;-1287.487,-1191.259;Inherit;True;Property;_TextureSample1;Texture Sample 0;6;0;Create;True;0;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TemplateShaderPropertyNode;38;-1566.665,-1627.928;Inherit;False;0;0;_MainTex;Shader;False;0;5;SAMPLER2D;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;68;-1293.828,-888.1907;Inherit;True;Property;_TextureSample2;Texture Sample 0;6;0;Create;True;0;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.DynamicAppendNode;69;-904.4088,-1165.738;Inherit;False;FLOAT4;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;70;-692.6469,-1170.053;Inherit;False;RB_OffsetColor;-1;True;1;0;FLOAT4;0,0,0,0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.DynamicAppendNode;34;-2241.479,-510.2473;Inherit;False;FLOAT4;4;0;FLOAT3;0,0,0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.ComponentMaskNode;33;-2472.061,-556.8721;Inherit;False;True;True;True;False;1;0;COLOR;0,0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RangedFloatNode;35;-2424.588,-468.7088;Inherit;False;Constant;_Float0;Float 0;5;0;Create;True;0;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.ScreenPosInputsNode;30;-2869.462,-557.311;Float;False;0;False;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ScreenColorNode;32;-2649.482,-558.2057;Inherit;False;Global;_GrabScreen0;Grab Screen 0;5;0;Create;True;0;0;0;True;0;False;Instance;-1;True;False;False;False;2;0;FLOAT2;0,0;False;1;FLOAT;0;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RegisterLocalVarNode;73;-2091.28,-514.2847;Inherit;False;ScreenColor;-1;True;1;0;FLOAT4;0,0,0,0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;2;-2840.315,-219.8551;Inherit;True;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.FractNode;4;-2613.132,-220.1595;Inherit;True;1;0;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.Vector2Node;6;-2718.597,65.39382;Inherit;False;Constant;_Vector0;Vector 0;1;0;Create;True;0;0;0;False;0;False;0.5,0.5;0,0;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.DistanceOpNode;5;-2407.55,-93.76833;Inherit;True;2;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RotatorNode;7;-2420.278,141.3379;Inherit;True;3;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;2;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;9;-2770.91,202.3451;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleTimeNode;10;-2718.175,320.2485;Inherit;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.BreakToComponentsNode;15;-2179.734,141.2039;Inherit;False;FLOAT2;1;0;FLOAT2;0,0;False;16;FLOAT;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4;FLOAT;5;FLOAT;6;FLOAT;7;FLOAT;8;FLOAT;9;FLOAT;10;FLOAT;11;FLOAT;12;FLOAT;13;FLOAT;14;FLOAT;15
Node;AmplifyShaderEditor.SimpleAddOpNode;12;-2024.513,-13.59062;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;16;-2090.443,-266.4068;Inherit;False;Property;_TansBar;TansBar;0;0;Create;True;0;0;0;False;0;False;0.6726901;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;19;-2092.381,356.2296;Inherit;False;Property;_CircelRim;CircelRim;2;0;Create;True;0;0;0;False;0;False;0.108;0;0;0.5;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;8;-1755.078,249.1522;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TFHCRemapNode;50;-1794.98,-263.3666;Inherit;False;5;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;2;False;4;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.StepOpNode;20;-1475.437,308.0735;Inherit;True;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;21;-1222.895,121.4097;Inherit;True;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;23;-1241.317,361.133;Inherit;False;Property;_RimColorA;RimColorA;3;1;[HDR];Create;True;0;0;0;False;0;False;0,0.5121677,1,0;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;24;-1243.149,532.941;Inherit;False;Property;_RimColorB;RimColorB;4;1;[HDR];Create;True;0;0;0;False;0;False;1,0,0,0;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.LerpOp;25;-982.7618,516.8036;Inherit;True;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;22;-732.475,125.728;Inherit;True;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;27;-1255.803,704.4665;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RegisterLocalVarNode;28;-511.517,122.2847;Inherit;False;RimColor;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.StepOpNode;17;-1505.339,-14.88343;Inherit;True;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.Vector2Node;3;-3011.693,-201.2548;Inherit;False;Property;_CircleTilling;CircleTilling;1;0;Create;True;0;0;0;False;0;False;16,9;0,0;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.LerpOp;1;-720.6636,-520.1033;Inherit;False;3;0;FLOAT4;0,0,0,0;False;1;FLOAT4;0,0,0,0;False;2;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.GetLocalVarNode;71;-1016.372,-554.4792;Inherit;False;70;RB_OffsetColor;1;0;OBJECT;;False;1;FLOAT4;0
Node;AmplifyShaderEditor.GetLocalVarNode;74;-1010.619,-465.248;Inherit;False;73;ScreenColor;1;0;OBJECT;;False;1;FLOAT4;0
Node;AmplifyShaderEditor.SimpleAddOpNode;47;-539.3278,-451.2005;Inherit;False;2;2;0;FLOAT4;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;0;-394.3831,-450.1982;Float;False;True;-1;2;ASEMaterialInspector;0;10;SA_CircleDisslove;0f8ba0101102bb14ebf021ddadce9b49;True;SubShader 0 Pass 0;0;0;SubShader 0 Pass 0;2;False;True;3;1;False;;10;False;;0;1;False;;0;False;;False;False;False;False;False;False;False;False;False;False;False;False;True;2;False;;False;False;False;False;False;False;False;False;False;False;False;True;2;False;;False;False;True;5;Queue=Transparent=Queue=0;IgnoreProjector=True;RenderType=Transparent=RenderType;PreviewType=Plane;CanUseSpriteAtlas=True;False;False;0;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;2;False;0;;0;0;Standard;0;0;1;True;False;;False;0
Node;AmplifyShaderEditor.GetLocalVarNode;49;-737.2378,-357.7885;Inherit;False;28;RimColor;1;0;OBJECT;;False;1;COLOR;0
WireConnection;59;0;63;3
WireConnection;59;1;63;4
WireConnection;60;0;63;1
WireConnection;60;1;63;2
WireConnection;62;0;60;0
WireConnection;62;1;64;4
WireConnection;61;0;59;0
WireConnection;61;1;64;4
WireConnection;42;0;45;0
WireConnection;42;2;46;0
WireConnection;42;1;44;0
WireConnection;36;1;42;0
WireConnection;58;0;36;1
WireConnection;58;1;40;0
WireConnection;57;0;41;0
WireConnection;57;1;58;0
WireConnection;66;0;57;0
WireConnection;66;1;61;0
WireConnection;65;0;57;0
WireConnection;65;1;62;0
WireConnection;37;0;38;0
WireConnection;37;1;65;0
WireConnection;67;0;38;0
WireConnection;67;1;57;0
WireConnection;68;0;38;0
WireConnection;68;1;66;0
WireConnection;69;0;37;1
WireConnection;69;1;67;2
WireConnection;69;2;68;3
WireConnection;69;3;67;4
WireConnection;70;0;69;0
WireConnection;34;0;33;0
WireConnection;34;3;35;0
WireConnection;33;0;32;0
WireConnection;32;0;30;0
WireConnection;73;0;34;0
WireConnection;2;0;3;0
WireConnection;4;0;2;0
WireConnection;5;0;4;0
WireConnection;5;1;6;0
WireConnection;7;0;9;0
WireConnection;7;1;6;0
WireConnection;7;2;10;0
WireConnection;15;0;7;0
WireConnection;12;0;5;0
WireConnection;12;1;15;0
WireConnection;8;0;12;0
WireConnection;8;1;19;0
WireConnection;50;0;16;0
WireConnection;20;0;8;0
WireConnection;20;1;50;0
WireConnection;21;0;17;0
WireConnection;21;1;20;0
WireConnection;25;0;23;0
WireConnection;25;1;24;0
WireConnection;25;2;27;2
WireConnection;22;0;21;0
WireConnection;22;1;25;0
WireConnection;28;0;22;0
WireConnection;17;0;12;0
WireConnection;17;1;50;0
WireConnection;1;0;71;0
WireConnection;1;1;74;0
WireConnection;1;2;17;0
WireConnection;47;0;1;0
WireConnection;47;1;49;0
WireConnection;0;0;47;0
ASEEND*/
//CHKSM=EAD1DD19875DFEDDD0027B6E88D6ECEFF737A95D
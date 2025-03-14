// Made with Amplify Shader Editor v1.9.1.5
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "SA_SquareDisslove"
{
	Properties
	{
		[PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}
		_Color ("Tint", Color) = (1,1,1,1)
		[MaterialToggle] PixelSnap ("Pixel snap", Float) = 0
		[PerRendererData] _AlphaTex ("External Alpha", 2D) = "white" {}
		_TransBar("TransBar", Range( 0 , 1)) = 0.4730825
		_SquareTilling("SquareTilling", Vector) = (16,9,0,0)
		_SquareRim("SquareRim", Range( 0 , 0.5)) = 0.148
		_ColorLerp("ColorLerp", Range( 0 , 2)) = 0.618284
		[HDR]_RimColorA("RimColorA", Color) = (0.9937825,0.5817609,1,0)
		[HDR]_RimColorB("RimColorB", Color) = (0,0.8108971,1,0)
		_ColorLerpHardness("ColorLerpHardness", Range( 0.51 , 1)) = 0.8382303
		_UV_NoiseTex("UV_NoiseTex", 2D) = "white" {}
		_UV_Noise_Intensity("UV_Noise_Intensity", Range( 0 , 1)) = 0
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
		
		GrabPass{ }

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
			ASE_DECLARE_SCREENSPACE_TEXTURE( _GrabTexture )
			uniform float _UV_Noise_Intensity;
			uniform sampler2D _UV_NoiseTex;
			uniform float4 _UV_NoiseTex_ST;
			uniform float4 _RB_Offset;
			uniform float _TransBar;
			uniform float2 _SquareTilling;
			uniform float4 _RimColorA;
			uniform float4 _RimColorB;
			uniform float _ColorLerpHardness;
			uniform float _ColorLerp;
			uniform float _SquareRim;

			
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

				float4 screenPos = IN.ase_texcoord1;
				float4 ase_screenPosNorm = screenPos / screenPos.w;
				ase_screenPosNorm.z = ( UNITY_NEAR_CLIP_VALUE >= 0 ) ? ase_screenPosNorm.z : ase_screenPosNorm.z * 0.5 + 0.5;
				float4 screenColor39 = UNITY_SAMPLE_SCREENSPACE_TEXTURE(_GrabTexture,ase_screenPosNorm.xy);
				float4 appendResult41 = (float4((screenColor39).rgb , 0.0));
				float2 texCoord53 = IN.texcoord.xy * float2( 1,1 ) + float2( 0,0 );
				float2 uv_UV_NoiseTex = IN.texcoord.xy * _UV_NoiseTex_ST.xy + _UV_NoiseTex_ST.zw;
				float2 panner48 = ( _Time.y * float2( 0,0.5 ) + uv_UV_NoiseTex);
				float2 temp_output_63_0 = ( ( texCoord53 + ( _UV_Noise_Intensity * tex2D( _UV_NoiseTex, panner48 ).r ) ) * IN.color.a );
				float2 appendResult58 = (float2(_RB_Offset.x , _RB_Offset.y));
				float4 tex2DNode45 = tex2D( _MainTex, temp_output_63_0 );
				float2 appendResult59 = (float2(_RB_Offset.z , _RB_Offset.w));
				float4 appendResult64 = (float4(tex2D( _MainTex, ( temp_output_63_0 + ( appendResult58 * IN.color.a ) ) ).r , tex2DNode45.g , tex2D( _MainTex, ( temp_output_63_0 + ( appendResult59 * IN.color.a ) ) ).b , tex2DNode45.a));
				float4 RB_OffsetColor65 = appendResult64;
				float temp_output_24_0 = (2.2 + (_TransBar - 0.0) * (0.0 - 2.2) / (1.0 - 0.0));
				float2 texCoord1 = IN.texcoord.xy * _SquareTilling + float2( 0,0 );
				float2 texCoord23 = IN.texcoord.xy * float2( 1,1 ) + float2( 0,0 );
				float cos20 = cos( _Time.y );
				float sin20 = sin( _Time.y );
				float2 rotator20 = mul( texCoord23 - float2( 0.5,0.5 ) , float2x2( cos20 , -sin20 , sin20 , cos20 )) + float2( 0.5,0.5 );
				float temp_output_18_0 = ( ( abs( ( frac( texCoord1.x ) + -0.5 ) ) + abs( ( frac( texCoord1.y ) + -0.5 ) ) ) + rotator20.x );
				float temp_output_11_0 = step( temp_output_24_0 , temp_output_18_0 );
				float4 lerpResult13 = lerp( appendResult41 , RB_OffsetColor65 , temp_output_11_0);
				float2 texCoord31 = IN.texcoord.xy * float2( 1,1 ) + float2( 0,0 );
				float smoothstepResult34 = smoothstep( ( 1.0 - _ColorLerpHardness ) , _ColorLerpHardness , pow( distance( texCoord31 , float2( 0.5,0.5 ) ) , _ColorLerp ));
				float4 lerpResult28 = lerp( _RimColorA , _RimColorB , smoothstepResult34);
				float4 RimColor37 = ( ( lerpResult28 * IN.color ) * ( step( temp_output_24_0 , ( temp_output_18_0 + _SquareRim ) ) - temp_output_11_0 ) );
				
				fixed4 c = ( lerpResult13 + RimColor37 );
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
Node;AmplifyShaderEditor.CommentaryNode;75;-968.8037,-1859.443;Inherit;False;2742.115;891.9271;Comment;24;47;53;67;63;55;43;44;45;46;64;65;48;50;51;49;68;54;58;61;62;56;57;59;60;RB_OffsetColor;0.916321,1,0.7012578,1;0;0
Node;AmplifyShaderEditor.GetLocalVarNode;70;1457.564,-410.1182;Inherit;False;37;RimColor;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.Vector2Node;5;-1203.591,-35.68753;Inherit;False;Property;_SquareTilling;SquareTilling;1;0;Create;True;0;0;0;False;0;False;16,9;0,0;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.TextureCoordinatesNode;1;-1022.589,-53.71954;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.FractNode;2;-776.0489,-165.5957;Inherit;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.FractNode;3;-775.945,56.14198;Inherit;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.DistanceOpNode;30;1463.094,35.71708;Inherit;True;2;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.PowerNode;32;1712.674,65.99616;Inherit;True;False;2;0;FLOAT;0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;33;1421.828,264.2503;Inherit;False;Property;_ColorLerp;ColorLerp;3;0;Create;True;0;0;0;False;0;False;0.618284;0;0;2;0;1;FLOAT;0
Node;AmplifyShaderEditor.Vector2Node;29;1238.228,159.2504;Inherit;False;Constant;_Vector1;Vector 1;3;0;Create;True;0;0;0;False;0;False;0.5,0.5;0,0;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.ColorNode;26;1933.679,-234.5951;Inherit;False;Property;_RimColorA;RimColorA;4;1;[HDR];Create;True;0;0;0;False;0;False;0.9937825,0.5817609,1,0;0.9937825,0.5817609,1,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;27;1931.544,-67.75951;Inherit;False;Property;_RimColorB;RimColorB;5;1;[HDR];Create;True;0;0;0;False;0;False;0,0.8108971,1,0;0,0.8108971,1,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.LerpOp;28;2199.039,-9.806895;Inherit;True;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.OneMinusNode;36;1768.094,288.7751;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;35;1480.662,363.3897;Inherit;False;Property;_ColorLerpHardness;ColorLerpHardness;6;0;Create;True;0;0;0;False;0;False;0.8382303;0;0.51;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SmoothstepOpNode;34;1972.661,265.5985;Inherit;True;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;31;1204.916,38.77707;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.VertexColorNode;73;2247.056,252.6526;Inherit;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;71;2511.85,128.3649;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;25;2677.173,399.2563;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;37;2891.68,396.6021;Inherit;False;RimColor;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;8;1386.623,502.1437;Inherit;True;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RotatorNode;20;-403.089,395.6867;Inherit;False;3;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;2;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;23;-766.8939,306.3936;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.Vector2Node;21;-716.8177,419.5557;Inherit;False;Constant;_Vector0;Vector 0;3;0;Create;True;0;0;0;False;0;False;0.5,0.5;0,0;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.SimpleTimeNode;22;-710.5331,548.7685;Inherit;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;14;-591.4666,-165.4123;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;-0.5;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;15;-584.4328,55.45681;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;-0.5;False;1;FLOAT;0
Node;AmplifyShaderEditor.AbsOpNode;17;-375.9434,54.55061;Inherit;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.AbsOpNode;16;-383.3435,-165.0858;Inherit;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;4;-176.589,-67.8211;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;18;38.24283,175.8297;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.BreakToComponentsNode;19;-176.8526,397.0074;Inherit;True;FLOAT2;1;0;FLOAT2;0,0;False;16;FLOAT;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4;FLOAT;5;FLOAT;6;FLOAT;7;FLOAT;8;FLOAT;9;FLOAT;10;FLOAT;11;FLOAT;12;FLOAT;13;FLOAT;14;FLOAT;15
Node;AmplifyShaderEditor.TFHCRemapNode;24;388.4553,-162.5132;Inherit;False;5;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;2.2;False;4;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;12;13.70755,-167.7779;Inherit;False;Property;_TransBar;TransBar;0;0;Create;True;0;0;0;False;0;False;0.4730825;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;0;2064.045,-563.0564;Float;False;True;-1;2;ASEMaterialInspector;0;10;SA_SquareDisslove;0f8ba0101102bb14ebf021ddadce9b49;True;SubShader 0 Pass 0;0;0;SubShader 0 Pass 0;2;False;True;3;1;False;;10;False;;0;1;False;;0;False;;False;False;False;False;False;False;False;False;False;False;False;False;True;2;False;;False;False;False;False;False;False;False;False;False;False;False;True;2;False;;False;False;True;5;Queue=Transparent=Queue=0;IgnoreProjector=True;RenderType=Transparent=RenderType;PreviewType=Plane;CanUseSpriteAtlas=True;False;False;0;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;2;False;0;;0;0;Standard;0;0;1;True;False;;False;0
Node;AmplifyShaderEditor.SimpleAddOpNode;69;1765.036,-561.6034;Inherit;False;2;2;0;FLOAT4;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.LerpOp;13;1473.166,-585.5638;Inherit;False;3;0;FLOAT4;0,0,0,0;False;1;FLOAT4;0,0,0,0;False;2;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.GetLocalVarNode;66;1150.786,-566.6813;Inherit;False;65;RB_OffsetColor;1;0;OBJECT;;False;1;FLOAT4;0
Node;AmplifyShaderEditor.DynamicAppendNode;41;1203.723,-670.4665;Inherit;False;FLOAT4;4;0;FLOAT3;0,0,0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.RangedFloatNode;42;963.2601,-655.2593;Inherit;False;Constant;_Float0;Float 0;5;0;Create;True;0;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.ScreenColorNode;39;714.9197,-751.6498;Inherit;False;Global;_GrabScreen0;Grab Screen 0;5;0;Create;True;0;0;0;False;0;False;Object;-1;False;False;False;False;2;0;FLOAT2;0,0;False;1;FLOAT;0;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ComponentMaskNode;40;916.2232,-751.6497;Inherit;False;True;True;True;False;1;0;COLOR;0,0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.ScreenPosInputsNode;38;492.0598,-752.2228;Float;False;0;False;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;47;-473.8355,-1501.505;Inherit;True;Property;_UV_NoiseTex;UV_NoiseTex;7;0;Create;True;0;0;0;False;0;False;-1;84482ce3c84ecd942b57d70843bd5b2e;84482ce3c84ecd942b57d70843bd5b2e;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TextureCoordinatesNode;53;-216.1077,-1748.2;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleAddOpNode;67;128.1953,-1666.347;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;63;257.4989,-1666.429;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleAddOpNode;55;546.3958,-1661.097;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.TemplateShaderPropertyNode;43;565.4863,-1809.443;Inherit;False;0;0;_MainTex;Shader;False;0;5;SAMPLER2D;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;44;917.2828,-1689.622;Inherit;True;Property;_TextureSample0;Texture Sample 0;5;0;Create;True;0;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;45;922.8504,-1453.753;Inherit;True;Property;_TextureSample1;Texture Sample 0;5;0;Create;True;0;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;46;926.559,-1197.516;Inherit;True;Property;_TextureSample2;Texture Sample 0;5;0;Create;True;0;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.DynamicAppendNode;64;1351.14,-1432.868;Inherit;False;FLOAT4;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;65;1533.911,-1437.099;Inherit;False;RB_OffsetColor;-1;True;1;0;FLOAT4;0,0,0,0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.PannerNode;48;-672.6802,-1472.83;Inherit;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.Vector2Node;50;-861.684,-1451.552;Inherit;False;Constant;_Vector2;Vector 2;6;0;Create;True;0;0;0;False;0;False;0,0.5;0,0;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.TextureCoordinatesNode;51;-918.8037,-1594.35;Inherit;False;0;47;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleTimeNode;49;-858.532,-1316.1;Inherit;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;68;-133.104,-1527.355;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;54;-449.2953,-1589.096;Inherit;False;Property;_UV_Noise_Intensity;UV_Noise_Intensity;8;0;Create;True;0;0;0;False;0;False;0;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode;58;-66.53557,-1272.347;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;61;221.7039,-1273.984;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;62;234.2345,-1145.658;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleAddOpNode;56;625.7482,-1170.5;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.Vector4Node;57;-323.3979,-1268.272;Inherit;False;Property;_RB_Offset;RB_Offset;9;0;Create;True;0;0;0;False;0;False;0.02,0,-0.02,0;0,0,0,0;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.DynamicAppendNode;59;-56.33679,-1141.846;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.VertexColorNode;60;-33.81175,-1459.149;Inherit;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.StepOpNode;11;855.8392,-38.41147;Inherit;True;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.StepOpNode;9;1024.791,501.8231;Inherit;True;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;6;763.0042,525.6524;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;7;440.711,627.5546;Inherit;False;Property;_SquareRim;SquareRim;2;0;Create;True;0;0;0;False;0;False;0.148;0;0;0.5;0;1;FLOAT;0
WireConnection;1;0;5;0
WireConnection;2;0;1;1
WireConnection;3;0;1;2
WireConnection;30;0;31;0
WireConnection;30;1;29;0
WireConnection;32;0;30;0
WireConnection;32;1;33;0
WireConnection;28;0;26;0
WireConnection;28;1;27;0
WireConnection;28;2;34;0
WireConnection;36;0;35;0
WireConnection;34;0;32;0
WireConnection;34;1;36;0
WireConnection;34;2;35;0
WireConnection;71;0;28;0
WireConnection;71;1;73;0
WireConnection;25;0;71;0
WireConnection;25;1;8;0
WireConnection;37;0;25;0
WireConnection;8;0;9;0
WireConnection;8;1;11;0
WireConnection;20;0;23;0
WireConnection;20;1;21;0
WireConnection;20;2;22;0
WireConnection;14;0;2;0
WireConnection;15;0;3;0
WireConnection;17;0;15;0
WireConnection;16;0;14;0
WireConnection;4;0;16;0
WireConnection;4;1;17;0
WireConnection;18;0;4;0
WireConnection;18;1;19;0
WireConnection;19;0;20;0
WireConnection;24;0;12;0
WireConnection;0;0;69;0
WireConnection;69;0;13;0
WireConnection;69;1;70;0
WireConnection;13;0;41;0
WireConnection;13;1;66;0
WireConnection;13;2;11;0
WireConnection;41;0;40;0
WireConnection;41;3;42;0
WireConnection;39;0;38;0
WireConnection;40;0;39;0
WireConnection;47;1;48;0
WireConnection;67;0;53;0
WireConnection;67;1;68;0
WireConnection;63;0;67;0
WireConnection;63;1;60;4
WireConnection;55;0;63;0
WireConnection;55;1;61;0
WireConnection;44;0;43;0
WireConnection;44;1;55;0
WireConnection;45;0;43;0
WireConnection;45;1;63;0
WireConnection;46;0;43;0
WireConnection;46;1;56;0
WireConnection;64;0;44;1
WireConnection;64;1;45;2
WireConnection;64;2;46;3
WireConnection;64;3;45;4
WireConnection;65;0;64;0
WireConnection;48;0;51;0
WireConnection;48;2;50;0
WireConnection;48;1;49;0
WireConnection;68;0;54;0
WireConnection;68;1;47;1
WireConnection;58;0;57;1
WireConnection;58;1;57;2
WireConnection;61;0;58;0
WireConnection;61;1;60;4
WireConnection;62;0;59;0
WireConnection;62;1;60;4
WireConnection;56;0;63;0
WireConnection;56;1;62;0
WireConnection;59;0;57;3
WireConnection;59;1;57;4
WireConnection;11;0;24;0
WireConnection;11;1;18;0
WireConnection;9;0;24;0
WireConnection;9;1;6;0
WireConnection;6;0;18;0
WireConnection;6;1;7;0
ASEEND*/
//CHKSM=0AE076DB4897F1530531D24857AC54DCCDE740B1
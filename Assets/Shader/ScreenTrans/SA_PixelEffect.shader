// Made with Amplify Shader Editor v1.9.1.5
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "SA_PixelEffect"
{
	Properties
	{
		[PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}
		_Color ("Tint", Color) = (1,1,1,1)
		[MaterialToggle] PixelSnap ("Pixel snap", Float) = 0
		[PerRendererData] _AlphaTex ("External Alpha", 2D) = "white" {}
		_TransBar("TransBar", Range( 0 , 1)) = 0.2167204
		_DissloveRim("DissloveRim", Range( 0 , 0.2)) = 0.07106964
		_VorNoiseScale("VorNoiseScale", Range( 0 , 20)) = 4.27428
		[HDR]_RiMColorA("RiMColorA", Color) = (0.3364779,0.4119374,1,0)
		[HDR]_RiMColorB("RiMColorB", Color) = (0.1918237,0.8334433,1,0)
		_NoiseTex("NoiseTex", 2D) = "white" {}
		_NoiseSpeed("NoiseSpeed", Vector) = (0.5,0,0,0)
		_NoiseIntensity("NoiseIntensity", Range( 0 , 1)) = 0
		_RB_Offset("RB_Offset", Vector) = (0.01,0,-0.01,0)

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
			uniform float _TransBar;
			uniform float _VorNoiseScale;
			uniform float _DissloveRim;
			uniform float4 _RiMColorA;
			uniform float4 _RiMColorB;
			ASE_DECLARE_SCREENSPACE_TEXTURE( _GrabTexture )
			uniform sampler2D _NoiseTex;
			uniform float2 _NoiseSpeed;
			uniform float4 _NoiseTex_ST;
			uniform float _NoiseIntensity;
			uniform float4 _RB_Offset;
					float2 voronoihash14( float2 p )
					{
						
						p = float2( dot( p, float2( 127.1, 311.7 ) ), dot( p, float2( 269.5, 183.3 ) ) );
						return frac( sin( p ) *43758.5453);
					}
			
					float voronoi14( float2 v, float time, inout float2 id, inout float2 mr, float smoothness, inout float2 smoothId )
					{
						float2 n = floor( v );
						float2 f = frac( v );
						float F1 = 8.0;
						float F2 = 8.0; float2 mg = 0;
						for ( int j = -1; j <= 1; j++ )
						{
							for ( int i = -1; i <= 1; i++ )
						 	{
						 		float2 g = float2( i, j );
						 		float2 o = voronoihash14( n + g );
								o = ( sin( time + o * 6.2831 ) * 0.5 + 0.5 ); float2 r = f - g - o;
								float d = 0.5 * dot( r, r );
						 		if( d<F1 ) {
						 			F2 = F1;
						 			F1 = d; mg = g; mr = r; id = o;
						 		} else if( d<F2 ) {
						 			F2 = d;
						
						 		}
						 	}
						}
						return F2 - F1;
					}
			

			
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

				float temp_output_32_0 = ( 1.0 - _TransBar );
				float time14 = _Time.y;
				float2 voronoiSmoothId14 = 0;
				float2 texCoord42 = IN.texcoord.xy * float2( 1,1 ) + float2( 0,0 );
				float2 coords14 = texCoord42 * _VorNoiseScale;
				float2 id14 = 0;
				float2 uv14 = 0;
				float voroi14 = voronoi14( coords14, time14, id14, uv14, 0, voronoiSmoothId14 );
				float temp_output_8_0 = step( temp_output_32_0 , voroi14 );
				float2 texCoord24 = IN.texcoord.xy * float2( 1,1 ) + float2( 0,0 );
				float4 lerpResult23 = lerp( _RiMColorA , _RiMColorB , texCoord24.y);
				float4 RimColor22 = ( ( step( temp_output_32_0 , ( voroi14 + _DissloveRim ) ) - temp_output_8_0 ) * lerpResult23 );
				float4 screenPos = IN.ase_texcoord1;
				float4 ase_screenPosNorm = screenPos / screenPos.w;
				ase_screenPosNorm.z = ( UNITY_NEAR_CLIP_VALUE >= 0 ) ? ase_screenPosNorm.z : ase_screenPosNorm.z * 0.5 + 0.5;
				float4 screenColor3 = UNITY_SAMPLE_SCREENSPACE_TEXTURE(_GrabTexture,ase_screenPosNorm.xy);
				float4 appendResult5 = (float4((screenColor3).rgb , 0.0));
				float2 uv_NoiseTex = IN.texcoord.xy * _NoiseTex_ST.xy + _NoiseTex_ST.zw;
				float2 panner57 = ( _Time.y * _NoiseSpeed + uv_NoiseTex);
				float2 texCoord44 = IN.texcoord.xy * float2( 1,1 ) + float2( 0,0 );
				float2 break47 = ( float2( 10,10 ) * (0.0 + (_TransBar - 0.0) * (60.0 - 0.0) / (1.0 - 0.0)) );
				float pixelWidth43 =  1.0f / break47.x;
				float pixelHeight43 = 1.0f / break47.y;
				half2 pixelateduv43 = half2((int)(texCoord44.x / pixelWidth43) * pixelWidth43, (int)(texCoord44.y / pixelHeight43) * pixelHeight43);
				float2 temp_output_55_0 = ( ( tex2D( _NoiseTex, panner57 ).r * _NoiseIntensity ) + pixelateduv43 );
				float2 appendResult68 = (float2(_RB_Offset.x , _RB_Offset.y));
				float4 tex2DNode71 = tex2D( _MainTex, temp_output_55_0 );
				float2 appendResult69 = (float2(_RB_Offset.z , _RB_Offset.w));
				float4 appendResult73 = (float4(tex2D( _MainTex, ( temp_output_55_0 + appendResult68 ) ).r , tex2DNode71.g , tex2D( _MainTex, ( temp_output_55_0 + appendResult69 ) ).b , tex2DNode71.a));
				float4 RBOffsetColor74 = appendResult73;
				float4 lerpResult1 = lerp( appendResult5 , RBOffsetColor74 , temp_output_8_0);
				
				fixed4 c = ( RimColor22 + lerpResult1 );
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
Node;AmplifyShaderEditor.CommentaryNode;79;-1220.531,234.3767;Inherit;False;2032.368;830.6388;Rim_Color;12;21;25;24;23;10;11;12;8;13;20;22;32;;0.6194968,0.682133,1,1;0;0
Node;AmplifyShaderEditor.CommentaryNode;78;-1952.023,-1263.868;Inherit;False;2939.11;906.002;RB_Offset;25;62;47;55;71;28;74;29;70;43;56;44;54;45;46;67;68;69;76;73;72;57;58;59;61;60;;1,0.4999999,0.4999999,1;0;0
Node;AmplifyShaderEditor.RangedFloatNode;62;-1247.124,-958.5868;Inherit;False;Property;_NoiseIntensity;NoiseIntensity;7;0;Create;True;0;0;0;False;0;False;0;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.BreakToComponentsNode;47;-1137.879,-607.3297;Inherit;False;FLOAT2;1;0;FLOAT2;0,0;False;16;FLOAT;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4;FLOAT;5;FLOAT;6;FLOAT;7;FLOAT;8;FLOAT;9;FLOAT;10;FLOAT;11;FLOAT;12;FLOAT;13;FLOAT;14;FLOAT;15
Node;AmplifyShaderEditor.SimpleAddOpNode;55;-583.0475,-976.1616;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SamplerNode;71;125.0624,-801.6268;Inherit;True;Property;_TextureSample1;Texture Sample 0;5;0;Create;True;0;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;28;115.1427,-996.8856;Inherit;True;Property;_TextureSample0;Texture Sample 0;5;0;Create;True;0;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RegisterLocalVarNode;74;747.6875,-791.268;Inherit;False;RBOffsetColor;-1;True;1;0;FLOAT4;0,0,0,0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.TemplateShaderPropertyNode;29;-175.305,-1086.465;Inherit;False;0;0;_MainTex;Shader;False;0;5;SAMPLER2D;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleAddOpNode;70;-234.6835,-972.5579;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.TFHCPixelate;43;-834.4108,-803.7575;Inherit;False;3;0;FLOAT2;0,0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SamplerNode;56;-1439.379,-1213.868;Inherit;True;Property;_NoiseTex;NoiseTex;5;0;Create;True;0;0;0;False;0;False;-1;25c0d61f6beb34148b13a05ad971b7e9;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TextureCoordinatesNode;44;-1219.795,-801.8659;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TFHCRemapNode;54;-1505.011,-579.8421;Inherit;False;5;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;0;False;4;FLOAT;60;False;1;FLOAT;0
Node;AmplifyShaderEditor.Vector2Node;45;-1486.264,-716.8053;Inherit;False;Constant;_Vector0;Vector 0;5;0;Create;True;0;0;0;False;0;False;10,10;0,0;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;46;-1295.681,-606.635;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.Vector4Node;67;-947.559,-582.9427;Inherit;False;Property;_RB_Offset;RB_Offset;8;0;Create;True;0;0;0;False;0;False;0.01,0,-0.01,0;0,0,0,0;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.DynamicAppendNode;68;-702.3307,-641.7783;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.DynamicAppendNode;69;-696.8671,-503.7374;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleAddOpNode;76;-231.24,-539.6389;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.DynamicAppendNode;73;532.1643,-778.8752;Inherit;False;FLOAT4;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.SamplerNode;72;127.4322,-587.8657;Inherit;True;Property;_TextureSample2;Texture Sample 0;5;0;Create;True;0;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.PannerNode;57;-1634.918,-1057.704;Inherit;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.Vector2Node;58;-1840.272,-1041.234;Inherit;False;Property;_NoiseSpeed;NoiseSpeed;6;0;Create;True;0;0;0;False;0;False;0.5,0;0,0;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.SimpleTimeNode;59;-1846.916,-917.0828;Inherit;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;61;-866.8652,-1069.11;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;60;-1902.023,-1169.004;Inherit;False;0;56;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;21;-142.0235,524.2198;Inherit;False;Property;_RiMColorA;RiMColorA;3;1;[HDR];Create;True;0;0;0;False;0;False;0.3364779,0.4119374,1,0;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;25;-133.0308,703.4658;Inherit;False;Property;_RiMColorB;RiMColorB;4;1;[HDR];Create;True;0;0;0;False;0;False;0.1918237,0.8334433,1,0;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TextureCoordinatesNode;24;-133.1256,908.5355;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.LerpOp;23;167.8228,682.3169;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleAddOpNode;10;-787.9203,576.082;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;11;-1113.359,594.7778;Inherit;False;Property;_DissloveRim;DissloveRim;1;0;Create;True;0;0;0;False;0;False;0.07106964;0;0;0.2;0;1;FLOAT;0
Node;AmplifyShaderEditor.StepOpNode;12;-469.5276,554.6745;Inherit;True;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.StepOpNode;8;-466.043,314.3344;Inherit;True;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;13;-139.8949,290.237;Inherit;True;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;20;335.0922,291.7157;Inherit;True;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;22;572.4366,284.3767;Inherit;False;RimColor;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.OneMinusNode;32;-1170.531,311.7167;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;42;-1335.819,-126.1756;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.VoronoiNode;14;-1049.617,-66.1237;Inherit;True;0;0;1;2;1;False;1;False;False;False;4;0;FLOAT2;0,0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;0;False;3;FLOAT;0;FLOAT2;1;FLOAT2;2
Node;AmplifyShaderEditor.SimpleTimeNode;16;-1279.312,-7.445227;Inherit;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;17;-1374.817,73.41264;Inherit;False;Property;_VorNoiseScale;VorNoiseScale;2;0;Create;True;0;0;0;False;0;False;4.27428;0;0;20;0;1;FLOAT;0
Node;AmplifyShaderEditor.ScreenPosInputsNode;2;-1113.568,-306.9566;Float;False;0;False;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ScreenColorNode;3;-877.8635,-307.8956;Inherit;False;Global;_GrabScreen0;Grab Screen 0;0;0;Create;True;0;0;0;False;0;False;Object;-1;False;False;False;False;2;0;FLOAT2;0,0;False;1;FLOAT;0;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ComponentMaskNode;4;-706.9545,-306.9565;Inherit;False;True;True;True;False;1;0;COLOR;0,0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RangedFloatNode;6;-646.9578,-224.8065;Inherit;False;Constant;_Float0;Float 0;0;0;Create;True;0;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode;5;-334.6496,-308.869;Inherit;False;FLOAT4;4;0;FLOAT3;0,0,0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.GetLocalVarNode;75;-379.2663,-177.1533;Inherit;False;74;RBOffsetColor;1;0;OBJECT;;False;1;FLOAT4;0
Node;AmplifyShaderEditor.LerpOp;1;-82.76773,-200.8218;Inherit;False;3;0;FLOAT4;0,0,0,0;False;1;FLOAT4;0,0,0,0;False;2;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.SimpleAddOpNode;26;241.8813,-223.5086;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT4;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;27;-4.682737,-306.4792;Inherit;False;22;RimColor;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;7;-2098.375,12.92163;Inherit;False;Property;_TransBar;TransBar;0;0;Create;True;0;0;0;False;0;False;0.2167204;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;0;1388.409,-223.5464;Float;False;True;-1;2;ASEMaterialInspector;0;10;SA_PixelEffect;0f8ba0101102bb14ebf021ddadce9b49;True;SubShader 0 Pass 0;0;0;SubShader 0 Pass 0;2;False;True;3;1;False;;10;False;;0;1;False;;0;False;;False;False;False;False;False;False;False;False;False;False;False;False;True;2;False;;False;False;False;False;False;False;False;False;False;False;False;True;2;False;;False;False;True;5;Queue=Transparent=Queue=0;IgnoreProjector=True;RenderType=Transparent=RenderType;PreviewType=Plane;CanUseSpriteAtlas=True;False;False;0;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;2;False;0;;0;0;Standard;0;0;1;True;False;;False;0
WireConnection;47;0;46;0
WireConnection;55;0;61;0
WireConnection;55;1;43;0
WireConnection;71;0;29;0
WireConnection;71;1;55;0
WireConnection;28;0;29;0
WireConnection;28;1;70;0
WireConnection;74;0;73;0
WireConnection;70;0;55;0
WireConnection;70;1;68;0
WireConnection;43;0;44;0
WireConnection;43;1;47;0
WireConnection;43;2;47;1
WireConnection;56;1;57;0
WireConnection;54;0;7;0
WireConnection;46;0;45;0
WireConnection;46;1;54;0
WireConnection;68;0;67;1
WireConnection;68;1;67;2
WireConnection;69;0;67;3
WireConnection;69;1;67;4
WireConnection;76;0;55;0
WireConnection;76;1;69;0
WireConnection;73;0;28;1
WireConnection;73;1;71;2
WireConnection;73;2;72;3
WireConnection;73;3;71;4
WireConnection;72;0;29;0
WireConnection;72;1;76;0
WireConnection;57;0;60;0
WireConnection;57;2;58;0
WireConnection;57;1;59;0
WireConnection;61;0;56;1
WireConnection;61;1;62;0
WireConnection;23;0;21;0
WireConnection;23;1;25;0
WireConnection;23;2;24;2
WireConnection;10;0;14;0
WireConnection;10;1;11;0
WireConnection;12;0;32;0
WireConnection;12;1;10;0
WireConnection;8;0;32;0
WireConnection;8;1;14;0
WireConnection;13;0;12;0
WireConnection;13;1;8;0
WireConnection;20;0;13;0
WireConnection;20;1;23;0
WireConnection;22;0;20;0
WireConnection;32;0;7;0
WireConnection;14;0;42;0
WireConnection;14;1;16;0
WireConnection;14;2;17;0
WireConnection;3;0;2;0
WireConnection;4;0;3;0
WireConnection;5;0;4;0
WireConnection;5;3;6;0
WireConnection;1;0;5;0
WireConnection;1;1;75;0
WireConnection;1;2;8;0
WireConnection;26;0;27;0
WireConnection;26;1;1;0
WireConnection;0;0;26;0
ASEEND*/
//CHKSM=E3DB6315072363ECBCFEBF7739438CFF67B831CD
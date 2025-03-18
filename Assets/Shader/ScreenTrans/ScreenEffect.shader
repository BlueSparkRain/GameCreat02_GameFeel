// Made with Amplify Shader Editor v1.9.1.5
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "ScreenEffect"
{
	Properties
	{
		[PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}
		_Color ("Tint", Color) = (1,1,1,1)
		[MaterialToggle] PixelSnap ("Pixel snap", Float) = 0
		[PerRendererData] _AlphaTex ("External Alpha", 2D) = "white" {}
		_MaskScale("MaskScale", Range( 0 , 2)) = 1.361766
		_MaskHardness("MaskHardness", Range( 0.51 , 1)) = 0.8587281
		_TextureSample0("Texture Sample 0", 2D) = "white" {}
		_NoiseTex("NoiseTex", 2D) = "white" {}
		[HDR]_ColorA("ColorA", Color) = (1,0,0,0)
		[HDR]_ColorB("ColorB", Color) = (0,0.2334146,1,0)
		_NoiseSpeed("NoiseSpeed", Vector) = (0.5,0,0,0)
		_NoiseIntensity("NoiseIntensity", Range( 0 , 1)) = 0.3956692
		_Alpha("Alpha", Range( 0 , 1)) = 0.3776782

	}

	SubShader
	{
		LOD 0

		Tags { "Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent" "PreviewType"="Plane" "CanUseSpriteAtlas"="True" }

		Cull Off
		Lighting Off
		ZWrite Off
		Blend One OneMinusSrcAlpha
		
		
		Pass
		{
		CGPROGRAM
			
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
				
			};
			
			uniform fixed4 _Color;
			uniform float _EnableExternalAlpha;
			uniform sampler2D _MainTex;
			uniform sampler2D _AlphaTex;
			uniform sampler2D _TextureSample0;
			uniform sampler2D _NoiseTex;
			uniform float2 _NoiseSpeed;
			uniform float4 _NoiseTex_ST;
			uniform float _NoiseIntensity;
			uniform float _MaskHardness;
			uniform float4 _TextureSample0_ST;
			uniform float _MaskScale;
			uniform float _Alpha;
			uniform float4 _ColorA;
			uniform float4 _ColorB;

			
			v2f vert( appdata_t IN  )
			{
				v2f OUT;
				UNITY_SETUP_INSTANCE_ID(IN);
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(OUT);
				UNITY_TRANSFER_INSTANCE_ID(IN, OUT);
				
				
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

				float2 uv_NoiseTex = IN.texcoord.xy * _NoiseTex_ST.xy + _NoiseTex_ST.zw;
				float2 panner19 = ( _Time.y * _NoiseSpeed + uv_NoiseTex);
				float temp_output_25_0 = ( tex2D( _NoiseTex, panner19 ).r * _NoiseIntensity );
				float2 texCoord27 = IN.texcoord.xy * float2( 1,1 ) + float2( 0,0 );
				float4 tex2DNode10 = tex2D( _TextureSample0, ( temp_output_25_0 + texCoord27 ) );
				float2 uv_TextureSample0 = IN.texcoord.xy * _TextureSample0_ST.xy + _TextureSample0_ST.zw;
				float smoothstepResult7 = smoothstep( ( 1.0 - _MaskHardness ) , _MaskHardness , pow( distance( ( uv_TextureSample0 + temp_output_25_0 ) , float2( 0.5,0.5 ) ) , _MaskScale ));
				float4 appendResult12 = (float4((tex2DNode10).rgb , ( tex2DNode10.a * smoothstepResult7 * _Alpha )));
				float2 texCoord17 = IN.texcoord.xy * float2( 1,1 ) + float2( 0,0 );
				float4 lerpResult14 = lerp( _ColorA , _ColorB , texCoord17.y);
				
				fixed4 c = ( appendResult12 * lerpResult14 );
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
Node;AmplifyShaderEditor.Vector2Node;4;-956.5176,-8.996004;Inherit;False;Constant;_Vector0;Vector 0;0;0;Create;True;0;0;0;False;0;False;0.5,0.5;0,0;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.DistanceOpNode;3;-742.451,-76.59595;Inherit;True;2;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;6;-814.3842,179.0707;Inherit;False;Property;_MaskScale;MaskScale;0;0;Create;True;0;0;0;False;0;False;1.361766;0;0;2;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;8;-447.7841,272.6707;Inherit;False;Property;_MaskHardness;MaskHardness;1;0;Create;True;0;0;0;False;0;False;0.8587281;0;0.51;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;15;512.0767,-228.9922;Inherit;False;Property;_ColorA;ColorA;4;1;[HDR];Create;True;0;0;0;False;0;False;1,0,0,0;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;16;524.8846,-39.575;Inherit;False;Property;_ColorB;ColorB;5;1;[HDR];Create;True;0;0;0;False;0;False;0,0.2334146,1,0;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TextureCoordinatesNode;17;582.8555,160.6276;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleTimeNode;22;-874.4835,-324.2355;Inherit;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.PannerNode;19;-651.7932,-480.3978;Inherit;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SamplerNode;24;-475.8568,-764.436;Inherit;True;Property;_NoiseTex;NoiseTex;3;0;Create;True;0;0;0;False;0;False;-1;4caad65a41cca98419a1ec895e1ddbf5;4caad65a41cca98419a1ec895e1ddbf5;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;25;-140.5233,-715.1024;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.Vector2Node;21;-902.4812,-517.1576;Inherit;False;Property;_NoiseSpeed;NoiseSpeed;6;0;Create;True;0;0;0;False;0;False;0.5,0;0.5,0;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.TextureCoordinatesNode;20;-952.3163,-755.8246;Inherit;False;0;24;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;10;-54.60736,-389.5676;Inherit;True;Property;_TextureSample0;Texture Sample 0;2;0;Create;True;0;0;0;False;0;False;-1;4caad65a41cca98419a1ec895e1ddbf5;4caad65a41cca98419a1ec895e1ddbf5;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;0;1879.406,-247.2285;Float;False;True;-1;2;ASEMaterialInspector;0;10;ScreenEffect;0f8ba0101102bb14ebf021ddadce9b49;True;SubShader 0 Pass 0;0;0;SubShader 0 Pass 0;2;False;True;3;1;False;;10;False;;0;1;False;;0;False;;False;False;False;False;False;False;False;False;False;False;False;False;True;2;False;;False;False;False;False;False;False;False;False;False;False;False;True;2;False;;False;False;True;5;Queue=Transparent=Queue=0;IgnoreProjector=True;RenderType=Transparent=RenderType;PreviewType=Plane;CanUseSpriteAtlas=True;False;False;0;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;2;False;0;;0;0;Standard;0;0;1;True;False;;False;0
Node;AmplifyShaderEditor.LerpOp;14;844.6672,-67.27265;Inherit;True;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;18;1365.096,-230.5544;Inherit;False;2;2;0;FLOAT4;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.ComponentMaskNode;13;254.8931,-413.1118;Inherit;False;True;True;True;False;1;0;COLOR;0,0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RangedFloatNode;28;7.011135,245.5425;Inherit;False;Property;_Alpha;Alpha;8;0;Create;True;0;0;0;False;0;False;0.3776782;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;11;260.3489,-142.5294;Inherit;False;3;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;9;-279.4508,60.0707;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;26;-403.19,-553.7692;Inherit;False;Property;_NoiseIntensity;NoiseIntensity;7;0;Create;True;0;0;0;False;0;False;0.3956692;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode;12;538.1361,-381.2085;Inherit;False;FLOAT4;4;0;FLOAT3;0,0,0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.SimpleAddOpNode;23;5.233002,-671.911;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;27;-377.7227,-445.1067;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SmoothstepOpNode;7;-96.75002,76.26355;Inherit;True;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.PowerNode;5;-477.251,-157.3292;Inherit;True;False;2;0;FLOAT;0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;2;-1040.128,-184.5576;Inherit;False;0;10;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleAddOpNode;35;-711.1143,-209.0164;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT;0;False;1;FLOAT2;0
WireConnection;3;0;35;0
WireConnection;3;1;4;0
WireConnection;19;0;20;0
WireConnection;19;2;21;0
WireConnection;19;1;22;0
WireConnection;24;1;19;0
WireConnection;25;0;24;1
WireConnection;25;1;26;0
WireConnection;10;1;23;0
WireConnection;0;0;18;0
WireConnection;14;0;15;0
WireConnection;14;1;16;0
WireConnection;14;2;17;2
WireConnection;18;0;12;0
WireConnection;18;1;14;0
WireConnection;13;0;10;0
WireConnection;11;0;10;4
WireConnection;11;1;7;0
WireConnection;11;2;28;0
WireConnection;9;0;8;0
WireConnection;12;0;13;0
WireConnection;12;3;11;0
WireConnection;23;0;25;0
WireConnection;23;1;27;0
WireConnection;7;0;5;0
WireConnection;7;1;9;0
WireConnection;7;2;8;0
WireConnection;5;0;3;0
WireConnection;5;1;6;0
WireConnection;35;0;2;0
WireConnection;35;1;25;0
ASEEND*/
//CHKSM=4E369915FE2D839235F8D9D3EFF4F5FDE850EE5C
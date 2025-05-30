Shader "Unlit/Button"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _LevelStatusTex ("LevelStatusTex", 2D) = "white" {}
        _alpha("alpha", 2D) = "white"{}
        _MousePos("mouse position", Vector) = (0,0,0,0)
        _R("radius", Range(0,1)) = 0.08
        _levelStar("levelStar", float) = -1
    }
    SubShader
    {
        // Tags { "RenderType"="Opaque" "RenderPipeline"="UniversalRenderPipeline" }
        Tags { "RenderType"="Opaque"}
        LOD 100

        Pass
        {
            Blend SrcAlpha OneMinusSrcAlpha
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            sampler2D _LevelStatusTex;
            sampler2D _alpha;
            float4 _MainTex_ST;
            float4 _MousePos;
            float _R;
            float _levelStar;



            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                 float2 scrolledUV = i.uv+ _Time.y ;
                 scrolledUV = frac(scrolledUV);
                 float2 mousePosScreenSpace = _MousePos.xy / _ScreenParams.xy;
                 float2 pixelPosScreenSpace = i.vertex.xy / _ScreenParams.xy;
                 mousePosScreenSpace.y *=_ScreenParams.y/ _ScreenParams.x;
                 pixelPosScreenSpace.y *= _ScreenParams.y/ _ScreenParams.x;
                 float distance = length(mousePosScreenSpace - pixelPosScreenSpace);
                 fixed4 col;
                 col = fixed4(min(mousePosScreenSpace.x,0.8),min(mousePosScreenSpace.y,0.8),1,1);
                 _R=_R*(abs(sin(40*_Time.x)))*0.3+0.08;
                  
                //col*=tex2D(_MainTex, i.uv);

                if(_levelStar >= 0.9)
                    col*=tex2D(_LevelStatusTex, scrolledUV);
                else
                    col*=tex2D(_LevelStatusTex, i.uv);
                if(_levelStar <=-0.9)
                {
                    float noise = frac(sin(dot(i.uv, float2(12.9898,78.233))) * 43758.5453 + _Time.y );
                    float bw = step(0.5, noise);
                    col=fixed4(bw,bw,bw,1);
                }
                    if(distance < _R&&(tex2D(_alpha,i.uv).a>0.9))
                {
                    //col =col+(col*(0.1,0.3,1,1)*(1/_R)*(_R-distance))/(1-(0.1,0.3,1,1)*(1/_R)*(_R-distance));
                    col = col*(distance/_R)+fixed4(0.6,0.6,0.8,1)*(1-distance/_R);
                }
                 if(distance < _R&(tex2D(_alpha, i.uv).a>0.5&&tex2D(_alpha,i.uv).a<0.9))
                {
                    //col =col+(col*(0.1,0.3,1,1)*(1/_R)*(_R-distance))/(1-(0.1,0.3,1,1)*(1/_R)*(_R-distance));
                    col = col*(distance/_R)+fixed4(0.8,1,1,1)*(1-distance/_R);
                }
                col.a = tex2D(_alpha, i.uv).a;
                return col;
            }
            ENDCG
        }
    }
}

Shader "Custom/PostEffectShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
    }
    SubShader
    {
        // No culling or depth
        Cull Off ZWrite Off ZTest Always

        Pass
        {
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

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            sampler2D _MainTex;
            sampler2D _CameraDepthTexture;

            fixed4 frag (v2f IN) : SV_Target
            {
                fixed4 color = tex2D(_MainTex, IN.uv);
                float depth = tex2D(_CameraDepthTexture, IN.uv).r;
                depth = 1 - clamp(depth * 10, 0, 1);

                float2 texCoord = IN.uv;
                float zOverW = depth;
                float4 currentPos = float4(texCoord.x * 2 - 1, (1 - texCoord.y) * 2 - 1, zOverW, 1); 
                float2 velocity = -currentPos / 360.f;

                velocity.y = velocity.y * (abs(velocity.x) + 0.3);

                texCoord += velocity;
                for(int i = 1; i < 6; ++i, texCoord += velocity) {
                    float4 currentColor = tex2D(_MainTex, texCoord);  
                    color += currentColor;
                } 
                color = color / 6; 

                color = pow(color, 1/1.4);
                color = color + pow(depth, 12) * float4(0.9,0.9,0.9,1);
                   
                return color;
            }
            ENDCG
        }
    }
}

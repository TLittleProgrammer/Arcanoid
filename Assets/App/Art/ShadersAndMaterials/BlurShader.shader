Shader "Custom/BlurShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _BlurRadius ("Blur Radius", Range(0, 10)) = 5
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        
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
            
            sampler2D _MainTex;
            float _BlurRadius;
            float2 _ScreenSize;
            
            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }
            
            half4 frag (v2f i) : SV_Target
            {
                half4 sum = half4(0.0f, 0.0f, 0.0f, 0.0f);
                float2 texelSize = 1 / _ScreenSize;
                float step = _BlurRadius * 0.5;
                
                for (float x = -_BlurRadius; x <= _BlurRadius; x += step)
                {
                    for (float y = -_BlurRadius; y <= _BlurRadius; y += step)
                    {
                        float2 offset = float2(x, y) * texelSize;
                        sum += tex2D(_MainTex, i.uv + offset);
                    }
                }
                
                return sum / ((_BlurRadius * 2) * (_BlurRadius * 2));
            }
            ENDCG
        }
    }
}

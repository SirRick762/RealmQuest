Shader "Hidden/vertice"
{
    Properties
    {
        [MainColor] _BaseColor("Color", Color) = (1,1,1,1)
        _AnimationSpeed("Animation Speed", Range(0,3)) = 0
        _OffsetSize("OffSet Size", Range(0,10)) = 0    
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

            float _AnimationSpeed;
            float _OffsetSize;

            v2f vert (appdata v)
            {
                v2f o;
                v.vertex.x += sin(_Time.y * _AnimationSpeed + v.vertex.y * _OffsetSize) ;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }
            

            float4 _BaseColor;
            sampler2D _MainTex;

            fixed4 frag (v2f i) : SV_Target
            {
                
                // just invert the colors
                
                return _BaseColor;
            }
            ENDCG
        }
    }
}

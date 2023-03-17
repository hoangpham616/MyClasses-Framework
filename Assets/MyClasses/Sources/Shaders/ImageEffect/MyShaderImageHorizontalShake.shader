/*
 * Copyright (c) 2016 Phạm Minh Hoàng
 * Email:       hoangpham61691@gmail.com
 * Framework:   MyClasses
 * Shader:      Image Effect/Horizontal Shake (version 1.0)
 * Based on KinoGlitch of Keijiro Takahashi
 */

Shader "MyClasses/Image Effect/Horizontal Shake"
{
    Properties
    {
        _MainTex ("-", 2D) = "" {}
        _Intensity ("Intensity", Range(0, 1)) = 0.1
    }

    SubShader
    {
        Cull Off
        ZWrite Off
        ZTest Always

        Pass
        {
            CGPROGRAM
            #pragma vertex vert_img
            #pragma fragment frag
            
            #include "UnityCG.cginc"
    
            sampler2D _MainTex;
            float _Intensity;
            float _ShakeTime;

            float nrand(float x, float y)
            {
                return frac(sin(dot(float2(x, y), float2(12.9898, 78.233))) * 43758.5453);
            }

            half4 frag(v2f_img i) : SV_Target
            {
                float u = i.uv.x;
                float v = i.uv.y;
                float jump = lerp(v, frac(v + _ShakeTime), 0);
                float shake = (nrand(_Time.x, 2) - 0.5) * _Intensity;
                half4 src = tex2D(_MainTex, frac(float2(u + shake, jump)));
                return half4(src.r, src.g, src.b, 1);
            }
            ENDCG
        }
    }
}
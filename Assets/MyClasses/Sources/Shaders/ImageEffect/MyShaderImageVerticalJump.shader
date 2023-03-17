/*
 * Copyright (c) 2016 Phạm Minh Hoàng
 * Email:       hoangpham61691@gmail.com
 * Framework:   MyClasses
 * Shader:      Image Effect/Vertical Jump (version 1.0)
 * Based on KinoGlitch of Keijiro Takahashi
 */

Shader "MyClasses/Image Effect/Vertical Jump"
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
            float _JumpTime;

            half4 frag(v2f_img i) : SV_Target
            {
                float u = i.uv.x;
                float v = i.uv.y;
                float jump = lerp(v, frac(v + _JumpTime), _Intensity);
                half4 src = tex2D(_MainTex, frac(float2(u, jump)));
                return half4(src.r, src.g, src.b, 1);
            }
            ENDCG
        }
    }
}
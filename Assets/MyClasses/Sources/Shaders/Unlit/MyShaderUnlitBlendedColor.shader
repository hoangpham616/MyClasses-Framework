/*
 * Copyright (c) 2016 Phạm Minh Hoàng
 * Email:       hoangpham61691@gmail.com
 * Framework:   MyClasses
 * Shader:      Unlit/Blended Color (version 1.0)
 */


Shader "MyClasses/Unlit/Blended Color"
{
    SubShader
    {
        Pass
        {
            BindChannels { Bind "Color", color }
            Blend SrcAlpha OneMinusSrcAlpha
            Cull Back
            Fog { Mode Off }
            ZTest Always
            ZWrite On
        } 
    } 
}
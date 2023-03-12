﻿/*
 * Copyright (c) 2016 Phạm Minh Hoàng
 * Email:       hoangpham61691@gmail.com
 * Framework:   MyClasses
 * Shader:      Unlit/Darkening (version 1.1)
 */

Shader "MyClasses/Unlit/Darkening"
{
    Properties
    {
    	_DarkAmount("Dark Amount", Range(0, 1)) = 0.8

    	[HideIninspector] _White ("White", Color) = (1, 1, 1, 1)

		// UI system default properties
		_StencilComp("Stencil Comparison", Float) = 8
		_Stencil("Stencil ID", Float) = 0
		_StencilOp("Stencil Operation", Float) = 0
		_StencilWriteMask("Stencil Write Mask", Float) = 255
		_StencilReadMask("Stencil Read Mask", Float) = 255
		_ColorMask("Color Mask", Float) = 15
    }

    SubShader
    {
        Tags
	    {
	        "Queue" = "Transparent"
	        "IgnoreProjector" = "True"
	        "RenderType" = "Transparent"
	        "PreviewType" = "Plane"
	        "CanUseSpriteAtlas" = "True"
	    }

        Cull Off
        Lighting Off
        ZWrite Off
        Fog { Mode Off }
        Blend SrcAlpha OneMinusSrcAlpha

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
		        float4 vertex : SV_POSITION;
		        float2 uv : TEXCOORD0;
		    };

	    	sampler2D _MainTex;
	    	fixed4 _White;
	    	fixed _DarkAmount;

		    v2f vert(appdata i)
		    {
		        v2f o;
		        o.vertex = UnityObjectToClipPos(i.vertex);
		        o.uv = i.uv;
		        return o;
		    }

		    fixed4 frag(v2f i) : SV_Target
		    {
		    	fixed4 dim = _White * (1 - _DarkAmount);
		    	dim.a = 1;
		        return tex2D(_MainTex, i.uv) * dim;
		    }
	        ENDCG
	    }
    }

    Fallback "Sprites/Default"
}
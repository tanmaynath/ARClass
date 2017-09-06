// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Custom/Outline" {
	Properties{
		_Color("Color", Color) = (1., 1., 1. ,1.)
		_Outline("_Outline", Range(0,0.5)) = 0
		_OutlineColor("Color", Color) = (1, 1, 1, 1)
	}
		SubShader{
		Pass{
		Tags{ "RenderType" = "Opaque" }
		Cull Front

		CGPROGRAM

#pragma vertex vert
#pragma fragment frag
#include "UnityCG.cginc"

		struct v2f {
		float4 pos : SV_POSITION;
	};

	float _Outline;
	float4 _OutlineColor;

	float4 vert(appdata_base v) : SV_POSITION{
		v2f o;
	o.pos = UnityObjectToClipPos(v.vertex);
	float3 normal = mul((float3x3) UNITY_MATRIX_MV, v.normal);
	normal.x *= UNITY_MATRIX_P[0][0];
	normal.y *= UNITY_MATRIX_P[1][1];
	o.pos.xy += normal.xy * _Outline;
	return o.pos;
	}

		half4 frag(v2f i) : COLOR{
		return _OutlineColor;
	}

		ENDCG
	}

		CGPROGRAM
#pragma surface surf Standard

		fixed4 _Color;

	struct Input {
		fixed4 col : COLOR;
	};

	void surf(Input IN, inout SurfaceOutputStandard o) {
		o.Albedo = _Color;
		o.Smoothness = 0.5f;
	}

	ENDCG
	}
		FallBack "Transparent"
}
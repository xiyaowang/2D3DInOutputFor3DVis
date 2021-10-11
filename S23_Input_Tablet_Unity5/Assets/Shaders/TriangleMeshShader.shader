Shader "StudyMismatch2D3D/TriangleMeshShader"
{
	Properties
	{
		_Color("main color", float) = (1, 1, 1, 1)
	}
		SubShader
	{
		Tags {
			"Queue" = "Transparent"
			"RenderType" = "Transparent"
		//"LightMode" = "ForwardBase" 
	}
	ZWrite On
	Cull Back
	Blend SrcAlpha OneMinusSrcAlpha
	Fog{ Mode Off }

	LOD 100

	Pass
	{
		CGPROGRAM
		#pragma vertex vert
		#pragma fragment frag

		#include "UnityCG.cginc"
		//#include "UnityLightingCommon.cginc" 

		struct v2f
		{
			float4 vertex : SV_POSITION;
			fixed4 diff : COLOR0; // diffuse lighting color
		};

		fixed4 _Color;

		v2f vert(appdata_base v)
		{
			v2f o;
			o.vertex = UnityObjectToClipPos(v.vertex);
			half3 worldNormal = UnityObjectToWorldNormal(v.normal);
			half nl = max(0, dot(worldNormal, UNITY_MATRIX_V[2].xyz));
			o.diff = nl * fixed4(1,1,1,1);
			return o;
		}

		fixed4 frag(v2f i) : SV_Target
		{
			fixed4 c = _Color * i.diff;
			return fixed4(c.xyz, _Color.w);
		}
		ENDCG
	}
	}
}

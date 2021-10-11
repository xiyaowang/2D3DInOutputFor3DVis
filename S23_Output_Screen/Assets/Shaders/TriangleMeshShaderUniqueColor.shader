Shader "StudyMismatch2D3D/TriangleMeshShaderUniqueColor"
{
	Properties
	{
		_Color("main color", float) = (1, 1, 1, 1)
	}
		SubShader
	{
		Tags {
		"RenderType" = "Transparent"
			"Queue" = "Geometry"
		}
		ZWrite On
		Cull Back
		Blend SrcAlpha OneMinusSrcAlpha
		Fog{ Mode Off }

		//LOD 100

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
			};

			fixed4 _Color;

			v2f vert(appdata_base v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				return o;
			}

			fixed4 frag(v2f i) : SV_Target
			{
				return _Color;
			}
			ENDCG
		}
	}
}

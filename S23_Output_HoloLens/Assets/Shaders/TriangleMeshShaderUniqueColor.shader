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
			"Queue" = "Transparent"
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



			struct appData {
				float4 vertex: POSITION;
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};

	struct v2f
	{
		float4 vertex : SV_POSITION;
		UNITY_VERTEX_INPUT_INSTANCE_ID
			UNITY_VERTEX_OUTPUT_STEREO
	};

			fixed4 _Color;

			v2f vert(appData v)
			{
				v2f o;
				UNITY_SETUP_INSTANCE_ID(v);
				UNITY_TRANSFER_INSTANCE_ID(v, o);
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);
				o.vertex = UnityObjectToClipPos(v.vertex);
				return o;
			}

			fixed4 frag(v2f i) : SV_Target
			{
					UNITY_SETUP_INSTANCE_ID(i);
				return _Color;
			}
			ENDCG
		}
	}
}

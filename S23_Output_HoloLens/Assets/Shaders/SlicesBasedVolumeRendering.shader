Shader "StudyMismatch2D3D/SlicesBasedVolumeRendering"
{
	Properties
	{
		_Texture3D("Texture3D", 3D) = "" {}
		_Invert("Invert Vector", Vector) = (0,0,0)

		_ClipPlaneCenter("Clipping Plane Center", Vector) = (0,0,0)
		_ClipPlaneNormal("Clipping Plane Normal", Vector) = (0,0,1)

		_RectColor("Color to fill inside the rectangle", Color) = (1, 1, 1, 1)
		_RectHalfDimensions("Colored Rectangle half Dimension", Vector) = (0.3,0.2,0.005)
		_RectCenter("Center of the rectangle", Vector) = (0,0,0)
		_RectNormal("Normal of the rectangle", Vector) = (0,0,1)


	}
		SubShader
		{
			Tags {
			"RenderType" = "Opaque"
			"Queue" = "Geometry"

		}
		ZWrite Off
		Cull Off
			Blend SrcAlpha OneMinusSrcAlpha
			Fog{ Mode Off }

			LOD 100

			Pass
			{
				CGPROGRAM
				#pragma multi_compile RECT_OFF RECT_ON
				#pragma vertex vert
				#pragma fragment frag

				#include "UnityCG.cginc"

		sampler3D _Texture3D;
		half3 _Invert;
		half4x4 _MatInv;

		half3 _ClipPlaneCenter;
		half3 _ClipPlaneNormal;

		half3 _RectHalfDimensions;
		half4 _RectColor;
		half3 _RectCenter;
		half3 _RectNormal;

		struct appData {
			float4 vertex: POSITION;
			UNITY_VERTEX_INPUT_INSTANCE_ID

		};


			struct v2f
			{
				half4 vertex : SV_POSITION;
				half3 texCoord:TEXCOORD1;

				half4 posWorld : POSITION1;
				half4 posOriginal : POSITION2;
				UNITY_VERTEX_INPUT_INSTANCE_ID
					UNITY_VERTEX_OUTPUT_STEREO
			};

			//https://github.com/toxicFork/Unity3D-Plane-Clipping/blob/master/Shaders/plane_clipping.cginc
			//http://mathworld.wolfram.com/Point-PlaneDistance.html
			float distanceToPlane(half3 planePosition, half3 planeNormal, half3 pointInWorld) {
				//w = vector from plane to point
				//planeNormal = -planeNormal;
				//planeNormal = mul(_MatInv, half4(planeNormal, 1.0)).xyz;;
				half3 w = -(planePosition - pointInWorld);
				half res = (planeNormal.x * w.x +
					planeNormal.y * w.y +
					planeNormal.z * w.z)
					/ sqrt(planeNormal.x * planeNormal.x +
						planeNormal.y * planeNormal.y +
						planeNormal.z * planeNormal.z);
				return res;
			}

			half distanceToPoint(half3 pt1, half3 pt2) {
				half3 v = pt2 - pt1;
				return sqrt(v.x * v.x + v.y * v.y + v.z * v.z);
			}

			v2f vert(appData v)
			{
				v2f o;
				UNITY_SETUP_INSTANCE_ID(v);
				UNITY_TRANSFER_INSTANCE_ID(v, o);
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);
				half4x4 modelView = mul(UNITY_MATRIX_MV, _MatInv);
				half4x4 modelViewProj = mul(UNITY_MATRIX_P, modelView);
				o.vertex = mul(modelViewProj, v.vertex);

				o.texCoord = v.vertex + 0.5;
				if (_Invert.x != 0 || _Invert.y != 0 || _Invert.z != 0)
					o.texCoord = lerp(o.texCoord,1.0 - o.texCoord,_Invert);

				o.posOriginal = mul(_MatInv, v.vertex);
				o.posWorld = mul(mul(UNITY_MATRIX_M, _MatInv), v.vertex);

				return o;
			}


			fixed4 frag(v2f i) : SV_Target{
						UNITY_SETUP_INSTANCE_ID(i);
				half distanceToClippingPlane = distanceToPlane(_ClipPlaneCenter, _ClipPlaneNormal, i.posWorld);
				if (distanceToClippingPlane < 0)
						discard;

#if RECT_ON
				half distanceToRect = distanceToPlane(_RectCenter, _RectNormal, i.posOriginal);
				if (distanceToRect >= -_RectHalfDimensions.z && distanceToRect <= _RectHalfDimensions.z) {
					half3 diffToRectCenter = abs(i.posOriginal - _RectCenter);
					//if(diffToRectCenter.x <= _RectHalfDimensions.x && diffToRectCenter.y <= _RectHalfDimensions.y )
					if (distanceToClippingPlane <= 2*_RectHalfDimensions.z ) {
						if (i.posOriginal.x < -0.45 || i.posOriginal.x > 0.45 || i.posOriginal.y < -0.45 || i.posOriginal.y > 0.45 || i.posOriginal.z < -0.45 || i.posOriginal.z > 0.45)
							return fixed4(0, 1, 0, 1);
						else
							return fixed4(1, 1, 1, 1);
					}else{ //if (distanceToClippingPlane <= _RectHalfDimensions.z * 10){
						if (i.posOriginal.x < -0.45 || i.posOriginal.x > 0.45 || i.posOriginal.y < -0.45 || i.posOriginal.y > 0.45 || i.posOriginal.z < -0.45 || i.posOriginal.z > 0.45) 
							return fixed4(1, 1, 0, 0.6);
						else
							return fixed4(1, 1, 1, 0.6);
					}
				}


#endif
				fixed4 col = tex3D(_Texture3D, i.texCoord);
				return col;

			}

				ENDCG
			}
		}
}

Shader "Unlit/DitheredShader"
{
	Properties
	{
		_Transparency ("Transparency", Float) = 0.5 
		_Resolution ("Resolution", Int) = 512
	}
	SubShader
	{
		Tags { "RenderType"="Opaque" }
		LOD 100

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			// make fog work
			#pragma multi_compile_fog
			
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

			// Transparency 0 to 1, 0 being transparent, 1 being opaque
			float _Transparency;

			// Current resolution of the material
			int _Resolution;

			// Side of the bayer matrix
			int _ArraySide = 0;

			// Total number of bayer matrix values
			int _ArrayLength = 0;

			// Matrix to store bayer matrix values
			float _Array[256];

			bool CheckIfDiscard(float threshold, int cordX, int cordY) {
			    int x = cordX % _ArraySide;
			    int y = cordY % _ArraySide;
			    float result = _Array[(x + y * _ArraySide)] / _ArrayLength;
			    return (result < threshold) ? false : true;
			}

			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = v.uv;
				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				fixed4 col = fixed4(0,0,1,1);

				if(CheckIfDiscard(_Transparency, i.uv.y * _Resolution,i.uv.x * _Resolution))
				{
			   	 	discard;
				}

				return col;
			}
			ENDCG
		}
	}
}

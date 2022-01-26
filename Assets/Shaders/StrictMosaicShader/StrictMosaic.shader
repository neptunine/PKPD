/**
 ** Copyright 2021 生チョコ教団/ヨドコロちゃん
 ** Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files(the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and / or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions :
 ** The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.
 **
 ** THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
 **/

 /**
  ** 2021/10/22 v1.0 Release
  ** 2021/11/20 v1.1 Fix unnessesary light influence, Improve VR view
  **/

Shader "Yodokorochan/StrictMosaic/StrictMosaic"
{
    Properties
    {
		_MinPx ("最小ピクセルサイズ",int) = 4
		_Ratio ("長辺に対する割合(%)",Range(0,20)) = 1.0
        _Color ("色のオーバーレイ", Color) = (1,1,1,1)
    }
    SubShader
    {
        Tags { "RenderType" = "Opaque" "Queue" = "Overlay" }
		GrabPass{"_StrictMosaicBGTexture"}

		ZTest LEqual
		Zwrite Off

        LOD 100

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma target 3.0
			#include "UnityCG.cginc"

			sampler2D _StrictMosaicBGTexture;
			float4 _StrictMosaicBGTexture_ST;
			int _MinPx;
			half _Ratio;
			fixed4 _Color;

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float4 vertex : SV_POSITION;
				float4 uv : TEXCOORD0;
				float4 grabPos: TEXCOORD1;
			};

			v2f vert(appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = ComputeScreenPos(o.vertex);
				o.grabPos = ComputeGrabScreenPos(o.vertex);
				return o;
			}

			fixed4 frag(v2f i) : SV_Target
			{
				// 長辺のPixel数
				int refLength = max(_ScreenParams.x, _ScreenParams.y);

				// 割合モザイクのPixel数
				int ratioMosaicPixel = (int)ceil((float)refLength * (_Ratio / 100.0));

				// 採用モザイクPixel数
				int mosaicPixel = max(_MinPx, ratioMosaicPixel);

				// 1ピクセル当たりの正規化スケール
				float2 pixelSize = 1.0 / (float2)_ScreenParams.xy;

				// モザイクの正規化スケール
				float2 mosaicSize = pixelSize * mosaicPixel;

				// モザイクを0にした時の0除算対策
				float2 mosaicSizeFixed = max(mosaicSize, 0.00001);

				// 正規化スケールに対する長辺の分割数
				float2 divideNum = 1.0 / mosaicSizeFixed;

				fixed2 grabUV = i.grabPos.xy / i.grabPos.w;
				float2 screenUV = i.uv.xy / i.uv.w;
#if UNITY_SINGLE_PASS_STEREO
				divideNum.x = divideNum.x * 2;
				mosaicSizeFixed.x = mosaicSizeFixed.x / 2;
#endif

				fixed2 posUV = (floor(screenUV * divideNum) / divideNum) + (mosaicSizeFixed / 2.0); //分割して中央ピクセルをサンプル
				fixed4 grabColor = tex2D(_StrictMosaicBGTexture, screenUV);
				fixed4 posColor = tex2D(_StrictMosaicBGTexture, posUV) * _Color;
				fixed4 blendedColor = lerp(grabColor, posColor, _Color.a);

				fixed4 finalColor = fixed4(blendedColor.rgb, 1.0);
				return finalColor;
			}
			ENDCG
		}
    }
    FallBack "Unlit/Color"
}

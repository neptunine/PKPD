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

Shader "Yodokorochan/StrictMosaic/StrictMosaic_StencilWrite"
{
	Properties
	{
		_StencilNum("Stencil" , int) = 42
	}
	SubShader
	{
		Tags { "RenderType" = "Opaque" "Queue" = "Overlay-1" }
		GrabPass{"_StrictMosaicBGTexture"}
		Stencil
		{
			Ref [_StencilNum]
			Comp Always
			Pass Replace
		}

		ZTest LEqual
		Zwrite Off

		LOD 200

		CGPROGRAM
		// Physically based Standard lighting model, and enable shadows on all light types
		#pragma surface surf NoLighting noambient

		// Use shader model 3.0 target, to get nicer looking lighting
		#pragma target 3.0

		inline fixed4 LightingNoLighting(SurfaceOutput s, fixed3 lightDir, fixed atten)
		{
			return fixed4(s.Albedo, s.Alpha);
		}

		struct Input
		{
			float2 uv_MainTex;
		};

		void surf(Input IN, inout SurfaceOutput o)
		{
		}
		ENDCG
	}
		FallBack "Diffuse"
}

Shader "Custom/SpriteOutline"
{
    Properties
    {
        [PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}
        _Color ("Tint Color", Color) = (1,1,1,1)
        _TintColorFactor ("Tint Color Factor", Range(0, 1)) = 1
        _FillColor ("Fill Color", Color) = (1,1,1,1)
        _FillColorFactor ("Fill Color Factor", Range(0, 1)) = 0
        _OutlineColor("Outline Color", Color) = (1,1,1,1)
        _LineWidth("Outline Width", Float) = 0.39
        _CheckRange("Check Range", Range(0, 1)) = 0
        _CheckAccuracy("Check Accuracy", Range(0.1, 0.99)) = 0.9
        [MaterialToggle] PixelSnap ("Pixel Snap", Float) = 0

        _StencilComp ("Stencil Comparison", Float) = 8
        _Stencil ("Stencil ID", Float) = 0
        _StencilOp ("Stencil Operation", Float) = 0
        _StencilWriteMask ("Stencil Write Mask", Float) = 255
        _StencilReadMask ("Stencil Read Mask", Float) = 255
        _ColorMask ("Color Mask", Float) = 15
    }

    SubShader
    {
        Tags
        {
            "Queue"="Transparent" 
            "IgnoreProjector"="True" 
            "RenderType"="Transparent" 
            "PreviewType"="Plane"
            "CanUseSpriteAtlas"="True"
        }

        Cull Off
        Lighting Off
        ZWrite Off
        Blend One OneMinusSrcAlpha

        Stencil
        {
            Ref[_Stencil]
            Comp[_StencilComp]
            Pass[_StencilOp]
            ReadMask[_StencilReadMask]
            WriteMask[_StencilWriteMask]
        }
        ColorMask[_ColorMask]

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma target 2.0
            #pragma multi_compile _ PIXELSNAP_ON
            #pragma multi_compile _ ETC1_EXTERNAL_ALPHA
            #include "UnityCG.cginc"
            sampler2D _MainTex;
            float4 _MainTex_TexelSize;
            sampler2D _AlphaTex;
            fixed4 _Color;
            float _TintColorFactor;
            fixed4 _FillColor;
            float _FillColorFactor;
            fixed4 _OutlineColor;
            float _CheckRange;
            float _LineWidth;
            float _CheckAccuracy;
            struct appdata_t
            {
                float4 vertex   : POSITION;
                float4 color    : COLOR;
                float2 texcoord : TEXCOORD0;
            };

            struct v2f
            {
                float4 vertex   : SV_POSITION;
                fixed4 color    : COLOR;
                float2 texcoord : TEXCOORD0;
            };

            v2f vert(appdata_t IN)
            {
                v2f OUT;
                OUT.vertex = UnityObjectToClipPos(IN.vertex);
                OUT.texcoord = IN.texcoord;
                OUT.color = IN.color * _Color;
                #ifdef PIXELSNAP_ON
                OUT.vertex = UnityPixelSnap(OUT.vertex);
                #endif
                return OUT;
            }

            fixed4 SampleSpriteTexture(float2 uv)
            {
                fixed4 color = tex2D(_MainTex, uv);

#if ETC1_EXTERNAL_ALPHA
                // get the color from an external texture (usecase: Alpha support for ETC1 on android)
                color.a = tex2D(_AlphaTex, uv).r;
#endif

                return color;
            }

            fixed4 frag(v2f IN) : SV_Target
            {
                fixed4 c = SampleSpriteTexture(IN.texcoord) * IN.color;
                c.rgb *= c.a;
                float isOut = step(abs(1 / _LineWidth), c.a);
                if (isOut != 0)
                {
                    fixed4 pixelUp    = tex2D(_MainTex, IN.texcoord + fixed2(0, _MainTex_TexelSize.y * _CheckRange));
                    fixed4 pixelDown  = tex2D(_MainTex, IN.texcoord - fixed2(0, _MainTex_TexelSize.y * _CheckRange));
                    fixed4 pixelRight = tex2D(_MainTex, IN.texcoord + fixed2(_MainTex_TexelSize.x * _CheckRange, 0));
                    fixed4 pixelLeft  = tex2D(_MainTex, IN.texcoord - fixed2(_MainTex_TexelSize.x * _CheckRange, 0));
                    float bOut = step((1 - _CheckAccuracy), pixelUp.a * pixelDown.a * pixelRight.a * pixelLeft.a);
                    c = lerp(_OutlineColor, c * _TintColorFactor * (1 - _FillColorFactor) + _FillColor * _FillColorFactor, bOut);
                    return c;
                }
                else
                {
                    return c;
                }
            }
            ENDCG
        }
    }

}

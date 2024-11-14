Shader "Custom/URPLightBeamsTexture"
{

    Properties
    {
        _Color("Color", Color) = (1,1,1,1)
        _MainTex("Albedo (RGB)", 2D) = "white" {}
        _FadeDist("Fade Distance", Float) = 12
        _TimeXInc("Time movement x", Float) = 0.01
        _TimeYInc("Time movement y", Float) = 0.02
        _LerpStart("Lerp start", Float) = -0.5
        _LerpEnd("Lerp end", Float) = 2.5
        _Power("Fade Power",Float) = 2
        _NormalPower("Normal Power", Float) = 1
    }

    SubShader
    {
            PackageRequirements
            {
                "com.unity.render-pipelines.universal"
            }

        Tags
        {
            "Queue" = "Transparent"
            "IgnoreProjector" = "True"
            "RenderType" = "Transparent"
            "DisableBatching" = "True"
            "RenderPipeline" = "UniversalRenderPipeline"
        }
        LOD 200

        Blend SrcAlpha OneMinusSrcAlpha
        ZWrite Off

        Pass
        {
            


            HLSLPROGRAM



            #pragma vertex vert
            #pragma fragment frag

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"            



            struct Attributes
            {
                float4 positionOS   : POSITION;
                half3 normal        : NORMAL;
                float2 uv       : TEXCOORD0;
            };

            struct Varyings
            {
                float4 positionHCS  : SV_POSITION;
                float4 posWS : TEXCOORD0;
                float3 modelPos : TEXCOORD1;
                float3 normalWS : TEXCOORD2;
                float2 uv       : TEXCOORD3;
            };

            TEXTURE2D(_MainTex);
            SAMPLER(sampler_MainTex);

            CBUFFER_START(UnityPerMaterial)
                half4 _Color;
                float4 _MainTex_ST;
                float _FadeDist;
                float _TimeXInc;
                float _TimeYInc;
                float _LerpStart;
                float _LerpEnd;
                float _Power;
                float _NormalPower;
            CBUFFER_END


            Varyings vert(Attributes IN)
            {
                Varyings OUT;
                OUT.positionHCS = TransformObjectToHClip(IN.positionOS.xyz);

                float4 posWS = mul(unity_ObjectToWorld, IN.positionOS);
                OUT.posWS = posWS;

                float3 p;
                p.x = unity_ObjectToWorld[0].w;
                p.y = unity_ObjectToWorld[1].w;
                p.z = unity_ObjectToWorld[2].w;
                OUT.modelPos = p;

                OUT.normalWS = TransformObjectToWorldNormal(IN.normal);

                OUT.uv = TRANSFORM_TEX(IN.uv, _MainTex);

                return OUT;
            }

            // The fragment shader definition.            
            half4 frag(Varyings IN) : SV_Target
            {
                //half4 color = _Color;
                //return color;

                float2 uv = IN.uv;
                //uv.x += _Time.y * _TimeXInc;
                //uv.y -= _Time.y * _TimeYInc;

                half4 col = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, IN.uv); // *_Color;

                float fadeStart = 0;
                float fadeEnd = _FadeDist;
                float d = length(IN.posWS.xyz - IN.modelPos);
                float fade = 1 - saturate((d - fadeStart) / (fadeEnd - fadeStart));

                fade = pow(fade, _Power);

                float3 dir2Cam = _WorldSpaceCameraPos.xyz - IN.posWS.xyz;
                dir2Cam = normalize(dir2Cam);
                float3 normal = IN.normalWS;
                float dotVal = max(0.0, dot(normalize(normal), dir2Cam));
                float val = pow(dotVal, _NormalPower);
                fade *= max(0.0f, lerp(_LerpStart, _LerpEnd, val));

                return half4(_Color.rgb + col.rgb, _Color.a * fade);

            }
            ENDHLSL
        }
    }
}

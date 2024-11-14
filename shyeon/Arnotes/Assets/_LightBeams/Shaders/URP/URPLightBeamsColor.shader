
//#ifdef URP_LIGHTBEAMS

Shader "Custom/URPLightBeamsColor"
{

    Properties
    {
            _Color("Color", Color) = (1,1,1,1)
            _FadeDist("Fade Distance", Float) = 12
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
            };

            struct Varyings
            {
                float4 positionHCS  : SV_POSITION;
                float4 posWS : TEXCOORD0;
                float3 modelPos : TEXCOORD1;
                float3 normalWS : TEXCOORD2;
            };

            CBUFFER_START(UnityPerMaterial)
                half4 _Color;
                float _FadeDist;
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
                return OUT;
            }

            // The fragment shader definition.            
            half4 frag(Varyings IN) : SV_Target
            {
                //half4 color = _Color;
                //return color;

                float fadeStart = 0;
                float fadeEnd = _FadeDist;

                float3 dir2pos = IN.posWS.xyz - IN.modelPos;
                float d = length(dir2pos);
                float fade = 1 - saturate((d - fadeStart) / (fadeEnd - fadeStart));

                fade = pow(fade, _Power);

                float3 dir2Cam = _WorldSpaceCameraPos.xyz - IN.posWS.xyz;
                dir2Cam = normalize(dir2Cam);

                float3 normal = IN.normalWS;

                float dotVal = max(0.0, dot(normalize(normal), dir2Cam));
                float val = pow(dotVal, _NormalPower);
                fade *= max(0.0f, lerp(_LerpStart, _LerpEnd, val));

                return half4(_Color.rgb, _Color.a * fade);


            }
            ENDHLSL
        }
    }
}
//#endif
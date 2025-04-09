Shader "Custom/ShadowOnlyShader"
{
    SubShader
    {
        Tags { "RenderType"="Opaque" }

        Pass
        {
            Tags { "LightMode" = "ShadowCaster" }
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata_t {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
            };

            struct v2f {
                float4 pos : POSITION;
            };

            v2f vert(appdata_t v) {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                return o;
            }

            half4 frag(v2f i) : SV_Target {
                discard; // Görünmeyen yap
                return half4(0,0,0,0); // Tamamen şeffaf
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
}

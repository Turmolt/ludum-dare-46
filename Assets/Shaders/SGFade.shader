// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "SG/Fade"
{
    Properties
    {
		_Fade ("Fade Amount", Range(-.1,1.1)) = 0.0
    }
    SubShader
    {
        Tags { "RenderType"="Transparent" "Queue" = "Transparent" }
        LOD 100
		Blend SrcAlpha OneMinusSrcAlpha

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
                UNITY_FOG_COORDS(1)
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
			sampler2D _FadeTexture;
            sampler2D _Logo;
            float4 _MainTex_ST;
			float _Fade;

			float2 _Resolution;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 col = tex2D(_FadeTexture, i.uv);
                if(col.b>=_Fade)return float4(0,0,0,0);
				float ratio = (_Resolution.x / _Resolution.y);
				i.uv.x *= ratio;// float2(1.0, _VideoResolution.x / _VideoResolution.y);
				i.uv.x -= .22*ratio;
				fixed4 logo = tex2D(_Logo, i.uv);
				fixed4 ret = fixed4(logo.r, logo.g, logo.b, 1);
				if (logo.a == 0)ret.rgb = float3(0, 0.6117647, 0.937255);
                return ret;
            }
            ENDCG
        }
    }
}

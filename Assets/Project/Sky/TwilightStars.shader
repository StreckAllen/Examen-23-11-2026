Shader "Skybox/Twilight Stars"
{
    Properties
    {
        _TopColor ("Color superior", Color) = (0.025, 0.04, 0.12, 1)
        _HorizonColor ("Color del horizonte", Color) = (0.36, 0.19, 0.3, 1)
        _GroundColor ("Color bajo el horizonte", Color) = (0.035, 0.035, 0.06, 1)
        _SunsetColor ("Color del crepusculo", Color) = (1, 0.36, 0.14, 1)
        _SunsetDirection ("Direccion del crepusculo", Range(0, 360)) = 55
        _StarDensity ("Cantidad de estrellas", Range(0, 0.08)) = 0.018
        _StarSize ("Tamano de estrellas", Range(0.3, 2)) = 1
        _StarBrightness ("Brillo de estrellas", Range(0, 5)) = 1.5
        _Exposure ("Exposicion del cielo", Range(0.2, 3)) = 1
    }
    SubShader
    {
        Tags { "Queue"="Background" "RenderType"="Background" "PreviewType"="Skybox" }
        Cull Off ZWrite Off
        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma target 3.0
            #include "UnityCG.cginc"
            float4 _TopColor, _HorizonColor, _GroundColor, _SunsetColor;
            float _SunsetDirection, _StarDensity, _StarSize, _StarBrightness, _Exposure;
            struct Input { float4 vertex : POSITION; };
            struct Output { float4 vertex : SV_POSITION; float3 direction : TEXCOORD0; };
            Output vert(Input v)
            {
                Output o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.direction = v.vertex.xyz;
                return o;
            }
            float Hash(float2 p) { return frac(sin(dot(p, float2(127.1, 311.7))) * 43758.5453); }
            half4 frag(Output i) : SV_Target
            {
                float3 d = normalize(i.direction);
                float height = saturate(d.y);
                float3 sky = lerp(_HorizonColor.rgb, _TopColor.rgb, pow(height, 0.45));
                float angle = radians(_SunsetDirection);
                float glow = pow(saturate(dot(normalize(d.xz + 0.00001), float2(sin(angle), cos(angle)))), 8);
                sky += _SunsetColor.rgb * glow * exp(-abs(d.y) * 12) * 0.65;
                sky = lerp(_GroundColor.rgb, sky, smoothstep(-0.12, 0.025, d.y));

                // Estrellas fijas: posiciones aleatorias repetibles, sin texturas ni objetos.
                float2 uv = float2(atan2(d.z, d.x) / (2 * UNITY_PI) + 0.5, asin(d.y) / UNITY_PI + 0.5);
                float2 grid = uv * float2(600, 300);
                float2 cell = floor(grid);
                float2 center = 0.2 + 0.6 * float2(Hash(cell + 13), Hash(cell + 37));
                float2 offset = frac(grid) - center;
                offset.x *= max(0.1, sqrt(saturate(1 - d.y * d.y)));
                float radius = lerp(0.025, 0.085, Hash(cell + 71)) * _StarSize;
                float pointDistance = length(offset);
                float aa = max(fwidth(pointDistance), 0.005);
                float star = 1 - smoothstep(max(0, radius - aa), radius + aa, pointDistance);
                star *= step(1 - _StarDensity, Hash(cell));
                star *= smoothstep(0.08, 0.4, d.y) * (1 - smoothstep(0.97, 1, d.y));
                sky += float3(0.8, 0.88, 1) * star * _StarBrightness;
                return half4(sky * _Exposure, 1);
            }
            ENDCG
        }
    }
    Fallback Off
}

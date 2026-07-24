Shader "Custom/MaskStencilShader"
{
    SubShader
    {
        // 1. Очередь рендера Transparent-1 (2999). 
        // Заставляет FOV рисоваться ДО того, как нарисуются спрайты лабиринта и тьмы (у них очередь 3000).
        Tags { "Queue"="Transparent-1" }
        
        // 2. Магия невидимости: запрещаем выводить какой-либо цвет на экран.
        ColorMask 0
        ZWrite Off

        // 3. Записываем "1" в Stencil-буфер там, где есть лучи FOV
        Stencil
        {
            Ref 1
            Comp Always
            Pass Replace
        }

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata { float4 vertex : POSITION; };
            struct v2f { float4 vertex : SV_POSITION; };

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // Цвет тут уже не важен из-за ColorMask 0, но мы всё равно возвращаем пустоту
                return fixed4(0,0,0,0); 
            }
            ENDCG
        }
    }
}
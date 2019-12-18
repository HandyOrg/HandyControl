sampler2D implicitInput : register(s0);
float scale : register(c0);

float4 main(float2 uv : TEXCOORD) : COLOR
{
    float4 color = tex2D(implicitInput, uv);
   
    float4 complement;
    float intensity = (color.r + color.g + color.b) / 3;

    complement.rgb = color.rgb * (1 - scale) + intensity * scale;
    complement.a = color.a;
    
    return complement;
}
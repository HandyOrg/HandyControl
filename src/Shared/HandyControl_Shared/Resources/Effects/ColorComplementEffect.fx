sampler2D implicitInput : register(s0);

float4 main(float2 uv : TEXCOORD) : COLOR
{
    float4 color = tex2D(implicitInput, uv);
    float4 complement;

    complement.rgb = color.a - color.rgb;
    complement.a = color.a;

    return complement;
}
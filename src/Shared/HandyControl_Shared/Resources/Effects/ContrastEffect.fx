sampler2D implicitInput : register(s0);
float contrast : register(C0);

float4 main(float2 uv : TEXCOORD) : COLOR
{
    float4 color = tex2D(implicitInput, uv); 
    float value = (1 - contrast) / 2;

    color.rgba = color.rgba * contrast + value;
    
    return color;
}
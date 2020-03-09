sampler2D implicitInput : register(s0);
float brightness : register(C0);

float4 main(float2 uv : TEXCOORD) : COLOR
{
    float4 color = tex2D(implicitInput, uv); 
    color.rgba *= brightness;
    
    return color;
}
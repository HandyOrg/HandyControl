sampler2D background : register(s0);
sampler2D foreground : register(s1);

float4 main(float2 uv : TEXCOORD) : COLOR
{
	float4 b = tex2D(background, uv);
	float4 f = tex2D(foreground, uv);
	float4 o = b;

	f.rgb *= f.a;
	b.rgb *= b.a;
	o.rgb = f.rgb * b.rgb + f.rgb * (1 - b.a) + b.rgb * (1 - f.a);

	return o;
}
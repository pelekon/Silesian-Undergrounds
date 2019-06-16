#if OPENGL
	#define SV_POSITION POSITION
	#define VS_SHADERMODEL vs_3_0
	#define PS_SHADERMODEL ps_3_0
#else
	#define VS_SHADERMODEL vs_4_0_level_9_1
	#define PS_SHADERMODEL ps_4_0_level_9_1
#endif

Texture2D SpriteTexture;
sampler s0;
float rnd;

sampler2D SpriteTextureSampler = sampler_state
{
	Texture = <SpriteTexture>;
};

struct VertexShaderInput
{
	float4 Position : POSITION0;
	float2 TextCoord : TEXCOORD0;
};

struct VertexShaderOutput
{
	float4 Position : SV_POSITION;
	float2 TextCoord : TEXCOORD0;
	float4 Color : COLOR0;
};

float rand(float2 co){
    return frac(sin(dot(co.xy ,float2(12.9898, 78.233))) * 43758.5453);
}

float4 MainPS(VertexShaderOutput input) : COLOR
{
	float intensity = 10.0f;
	float colorswap = 0.0f;

	float f = max(0, rand(float2(input.TextCoord.y, input.TextCoord.x+rnd)) - rand(float2(0, input.TextCoord.x+rnd))*intensity);

	float4 color = float4(0, f, 0, tex2D(s0, input.TextCoord.xy).w * input.Color.w * f);

	float4 c = tex2D(s0, input.TextCoord.xy);
	float r = c.r;
	float b = c.b;
	float g = c.g;

	float swap2 = (0.5f - abs(0.5f - colorswap))*2.0f;
	c.r = r*(1.0f-swap2) + g*swap2;
	c.b = b*(1.0f-swap2) + g*swap2;
	c.g = g*(1.0f-swap2) + b*swap2;

	float swap1 = colorswap * (1.0f - swap2);
	r = c.r; b = c.b;

	c.r = r*(1.0f-swap1) + b*swap1;
	c.b = b*(1.0f-swap1) + r*swap1;

	float4 c1 = c * input.Color;

    float4 toReturn = c1 * (1-intensity * 2) + color * intensity * 2;

	return toReturn;
}

technique BasicColorDrawing
{
	pass P0
	{
		PixelShader = compile PS_SHADERMODEL MainPS();
	}
};

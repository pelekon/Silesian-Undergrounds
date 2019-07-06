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

struct VertexShaderOutput
{
	float4 Position : SV_POSITION;
	float2 TextCoord : TEXCOORD0;
	float4 Color : COLOR0;
};

float4 MainPS(VertexShaderOutput input) : COLOR
{
	float2 center   = float2(0.5f, 0.5f);
	float border = 0.16f;
	float blur  = 0.02f;

	float radius = 1.0f;

	float2 p = input.TextCoord.xy;
	float a = abs(distance(p, center));

	float t = 1 - smoothstep(radius - border, radius - border + blur, a) + smoothstep(radius - blur, radius, a);
	float c = lerp(1.0f, 0.0f, t);

	return float4((1.0f-a*2.0f) / 2.0f, c, c, max(0.0f, 0.75f-a*1.5f) * c);
}

technique BasicColorDrawing
{
	pass P0
	{
		PixelShader = compile PS_SHADERMODEL MainPS();
	}
};

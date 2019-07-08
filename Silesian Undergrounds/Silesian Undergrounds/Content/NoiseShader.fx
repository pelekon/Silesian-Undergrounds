#if OPENGL
	#define SV_POSITION POSITION
	#define VS_SHADERMODEL vs_3_0
	#define PS_SHADERMODEL ps_3_0
#else
	#define VS_SHADERMODEL vs_4_0_level_9_1
	#define PS_SHADERMODEL ps_4_0_level_9_1
#endif

int rnd_time;

struct VertexShaderOutput
{
	float4 Position : SV_POSITION;
	float4 Color : COLOR0;
	float2 TextureCoordinates : TEXCOORD0;
};

Texture2D SpriteTexture;
sampler s0;

 sampler2D SpriteTextureSampler = sampler_state
{
	Texture = <SpriteTexture>;
};

float rand(float2 coord) {
	 return frac(sin(dot(coord.xy, float2(12.9898,78.233))) * 43758.5453123);
}

float generate_noise(float2 coord) {
	float2 i = floor(coord);
    float2 f = frac(coord);

    float a = rand(i);
    float b = rand(i + float2(1.0, 0.0));
    float c = rand(i + float2(0.0, 1.0));
    float d = rand(i + float2(1.0, 1.0));

    float2 u = f * f * (3.0 - 2.0 * f);

    return lerp(a, b, u.x) +
            (c - a)* u.y * (1.0 - u.x) +
            (d - b) * u.x * u.y;
}

float generate_fractal_noise(float2 coord) {
    float value_to_return = 0.0;
    float amplitude = 0.5;
    float frequency = 0.0;

    for (int i = 0; i < 6; i++) {
        value_to_return += amplitude * generate_noise(coord);
        coord *= 2.0;
        amplitude *= 0.5;
    }

    return value_to_return;
}


float4 MainPS(VertexShaderOutput input) : COLOR
{
	float2 coord = input.TextureCoordinates * 20.0;

	float2 motion = float2(generate_fractal_noise(coord + float2(rnd_time * -0.5, rnd_time * 0.5)), generate_fractal_noise(input.TextureCoordinates + float2(rnd_time * -0.5, rnd_time * 0.5)));

	float final = generate_fractal_noise(input.TextureCoordinates + motion);

	float3 color = float3(0.0 + final, 0.0 + final, 0.0 + final);

	return float4(color, final * 0.5);
}

technique SpriteDrawing
{
	pass P0
	{
		PixelShader = compile PS_SHADERMODEL MainPS();
	}
};
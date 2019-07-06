#if OPENGL
	#define SV_POSITION POSITION
	#define VS_SHADERMODEL vs_3_0
	#define PS_SHADERMODEL ps_3_0
#else
	#define VS_SHADERMODEL vs_4_0_level_9_1
	#define PS_SHADERMODEL ps_4_0_level_9_1
#endif

sampler s0;
Texture2D rainbow;
int gameTime;


sampler rainbow_sampler = sampler_state
{
	Texture = <rainbow>;
};

float random(int seed)
{
    return frac(sin(dot(seed + seed, seed)) * 43758.5453 + seed);
}


struct VertexShaderOutput
{
	float4 Position : SV_Position;
	float4 Color : COLOR1;
	float2 TextureCoordinates : TEXCOORD0;
};


float4 MainPS(VertexShaderOutput input) : COLOR
{
	float4 color = tex2D(s0, input.TextureCoordinates);

	float4 lightcolor = float4(0.0, 0.0, 0.0, 0.0);

	float rand = random(gameTime);

	if(rand > 0.0 && rand <= 0.2){
		lightcolor = tex2D(rainbow_sampler, input.TextureCoordinates.xy + random(gameTime));
	}
	else if (rand > 0.2 && rand <= 0.4){
		lightcolor = tex2D(rainbow_sampler, input.TextureCoordinates.xy - random(gameTime));
	}
	else if (rand > 0.4 && rand <= 0.6){
		lightcolor = tex2D(rainbow_sampler, input.TextureCoordinates.xy + random(gameTime));
	}
	else if (rand > 0.6 && rand <= 0.8){
		lightcolor = tex2D(rainbow_sampler, input.TextureCoordinates.xy - random(gameTime));
	}
	else{
		lightcolor = tex2D(rainbow_sampler, input.TextureCoordinates.xy + random(gameTime));
	}

	lightcolor.a = 1;

	if(input.TextureCoordinates.x == 0.5 && input.TextureCoordinates.y == 0.5){
		color.rgb = float3(0.0, 0.0, 0.0);
		lightcolor.a = 0;
		return color;
	}

	return color * lightcolor;
}

technique SpriteDrawing
{
	pass P0
	{
		PixelShader = compile PS_SHADERMODEL MainPS();
	}
};
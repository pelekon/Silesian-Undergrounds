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

float4 generate_mask_movement(int c, float2 coordinates, int gT)
{
	float4 result = float4(0.0, 0.0, 0.0, 0.0);
	
	if(c < 0){
		result = tex2D(rainbow_sampler, coordinates.xy - (random(gT) / 5));
	}
	else{
		result = tex2D(rainbow_sampler, coordinates.xy + (random(gT) / 5));
	}
	
	return result;
}

float4 move_mask(int gT, float rand, float2 coordinates)
{
	float4 lightcolor = float4(0.0, 0.0, 0.0, 0.0);
	
	if(rand > 0.0 && rand <= 0.2){ 
		lightcolor = generate_mask_movement(random(gT), coordinates, gT);
	}
	else if (rand > 0.2 && rand <= 0.4){
		lightcolor = generate_mask_movement(random(gT), coordinates, gT);
	}
	else if (rand > 0.4 && rand <= 0.6){
		lightcolor = generate_mask_movement(random(gT), coordinates, gT);
	}
	else if (rand > 0.6 && rand <= 0.8){
		lightcolor = generate_mask_movement(random(gT), coordinates, gT);
	}
	else{
		lightcolor = generate_mask_movement(random(gT), coordinates, gT);
	}
	
	return lightcolor;
}

float4 generate_color(float4 color, int gT){

	color.r = smoothstep(0.1, random(gT + 0.5), color.r);
	color.g = smoothstep(0.1, random(gT + 0.7), color.g);
	color.b = smoothstep(0.1, random(gT + 0.6), color.b);
	
	return color;
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
	
	int gT = gameTime / 250;
	
	float rand = random(gT);
	
	float2 coordinates = input.TextureCoordinates.xy;
		
	lightcolor = move_mask(gT, rand, coordinates);
	
	lightcolor.a = 0.6;
	
	color = generate_color(color, gT);

	return color * lightcolor;
}

technique SpriteDrawing
{
	pass P0
	{
		PixelShader = compile PS_SHADERMODEL MainPS();
	}
};
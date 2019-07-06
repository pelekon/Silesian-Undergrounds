#if OPENGL
	#define VS_SHADERMODEL vs_3_0
	#define PS_SHADERMODEL ps_3_0
#else
	#define VS_SHADERMODEL vs_4_0_level_9_1
	#define PS_SHADERMODEL ps_4_0_level_9_1
#endif

float2 centerPos;
float radiusOfAlphaArea;
float currentLayer;
sampler s0;

struct VertexShaderOutput
{
	float4 Position : SV_POSITION;
	float4 Color : COLOR0;
	float2 TextureCoordinates : TEXCOORD0;
};

float GetDistanceToPlayer(float2 pos)
{
	return sqrt(pow(centerPos.x - pos.x, 2) + pow(centerPos.y - pos.y, 2));
}

float4 MainPS(VertexShaderOutput input) : COLOR
{
	float4 currentFragmentColor = tex2D(s0, input.TextureCoordinates.xy);
	float dist = GetDistanceToPlayer(input.Position.xy);
	
	if (dist <= radiusOfAlphaArea)
	{
		if (currentFragmentColor.a > 0.2)
		{
			currentFragmentColor.r = 0.2;
			currentFragmentColor.g = 0.2;
			currentFragmentColor.b = 0.2;
			currentFragmentColor.a = 0.40f;
		}
	}
	
	return currentFragmentColor;
}

technique BasicColorDrawing
{
	pass P0
	{
		PixelShader = compile PS_SHADERMODEL MainPS();
	}
};
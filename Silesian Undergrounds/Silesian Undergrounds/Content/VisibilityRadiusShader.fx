#if OPENGL
#define SV_POSITION POSITION
#define VS_SHADERMODEL vs_3_0
#define PS_SHADERMODEL ps_3_0
#else
#define VS_SHADERMODEL vs_4_0_level_9_1
#define PS_SHADERMODEL ps_4_0_level_9_1
#endif
float2 lightSource;
Texture2D SpriteTexture;
sampler s0;
int gameTime;

sampler2D SpriteTextureSampler = sampler_state
{
    Texture = <SpriteTexture>;
};

struct VertexShaderOutput
{
    float4 Position : SV_Position;
    float4 Color : COLOR0;
    float2 TextureCoordinates : TEXCOORD0;
};

float random(float3 position, float3 scale, int seed)
{
    return frac(sin(dot(position.xyz + seed, scale)) * 43758.5453 + seed);
}

float getDistance(float2 point1, float2 point2)
{
    return sqrt(pow(point2.x - point1.x, 2) + pow(point2.y - point1.y, 2));
}


float2 getXY(float2 point1, float2 point2, float segment)
{
    float2 fisrt = point1;
    float2 second = point2;
    if (point1.x > point2.x)
    {
        fisrt = point2;
        second = point1;
    }
    float distance = getDistance(point1, point2);
    float ratio = segment / distance;
    float x3 = ratio * point2.x + (1 - ratio) * point1.x;
    float y3 = ratio * point2.y + (1 - ratio) * point1.y;
    return float2(x3, y3);
}


float4 MainPS(VertexShaderOutput input) : COLOR
{
    float4 currentFragmentColor = tex2D(s0, input.TextureCoordinates);
    float distance = getDistance(lightSource, input.Position.xy);
    float rand = random(input.Position.xyx, input.Position.yxy, gameTime);
   
    if (distance >= 200 && distance < 215 && rand < 0.1)
    {
        currentFragmentColor.rgba = 0;
    }
    else if (distance >= 215 && distance < 220 && rand < 0.3)
    {
        currentFragmentColor.rgba = 0;
    }
    else if (distance >= 220 && distance < 225 && rand < 0.6)
    {
        currentFragmentColor.rgba = 0;
    }
    else if (distance >= 225 && distance < 230 && rand < 0.9)
    {
        currentFragmentColor.rgba = 0;
    }
    else if (distance >= 230)
    {
        currentFragmentColor.rgba = 0;
    }
    return currentFragmentColor;
}

technique SpriteDrawing
{
    pass P0
    {
        PixelShader = compile PS_SHADERMODEL MainPS();
    }
};
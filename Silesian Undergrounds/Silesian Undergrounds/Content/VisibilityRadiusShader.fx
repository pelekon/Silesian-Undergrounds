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

// float4 blur(sampler2D image, float2 uv, float2 resolution, float2 direction) {
//   float4 color = float4(0.0);
//   float2 off1 = float2(1.3333333333333333) * direction;
//   color += texture2D(image, uv) * 0.29411764705882354;
//   color += texture2D(image, uv + (off1 / resolution)) * 0.35294117647058826;
//   color += texture2D(image, uv - (off1 / resolution)) * 0.35294117647058826;
//   return color; 
// }
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
    // return blur(s0, input.TextureCoordinate.xy, input.TextureCoordinates.xy, float2(0,0.5))
}

technique SpriteDrawing
{
    pass P0
    {
        PixelShader = compile PS_SHADERMODEL MainPS();
    }
};
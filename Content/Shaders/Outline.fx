#if OPENGL
	#define SV_POSITION POSITION
	#define VS_SHADERMODEL vs_3_0
	#define PS_SHADERMODEL ps_3_0
#else
	#define VS_SHADERMODEL vs_4_0_level_9_1
	#define PS_SHADERMODEL ps_4_0_level_9_1
#endif

Texture2D SpriteTexture;
sampler2D SpriteTextureSampler = sampler_state 
{
	Texture = <SpriteTexture>;
};

float2 TextureSize;
float4 OutlineColor;

struct VertexShaderOutput 
{
	float4 Position : SV_POSITION;
	float4 Color : COLOR0;
	float2 TextureCoordinates : TEXCOORD0;
};

float4 MainPS(VertexShaderOutput input) : COLOR 
{
	float4 currentColor = tex2D(SpriteTextureSampler, input.TextureCoordinates) * input.Color;
    if (currentColor.a > 0) 
		return currentColor;

    float2 pixel = 1.0 / TextureSize;
    float a = 0;
    a += tex2D(SpriteTextureSampler, input.TextureCoordinates + float2( pixel.x, 0)).a;
    a += tex2D(SpriteTextureSampler, input.TextureCoordinates + float2(-pixel.x, 0)).a;
    a += tex2D(SpriteTextureSampler, input.TextureCoordinates + float2(0,  pixel.y)).a;
    a += tex2D(SpriteTextureSampler, input.TextureCoordinates + float2(0, -pixel.y)).a;

    //a += tex2D(SpriteTextureSampler, input.TextureCoordinates + float2( pixel.x,  pixel.y)).a;
    //a += tex2D(SpriteTextureSampler, input.TextureCoordinates + float2(-pixel.x,  pixel.y)).a;
    //a += tex2D(SpriteTextureSampler, input.TextureCoordinates + float2( pixel.x, -pixel.y)).a;
    //a += tex2D(SpriteTextureSampler, input.TextureCoordinates + float2(-pixel.x, -pixel.y)).a;

    if (a > 0) 
		return OutlineColor;
    return currentColor;
}

technique SpriteDrawing 
{
	pass P0
	{
		PixelShader = compile PS_SHADERMODEL MainPS();
	}
};
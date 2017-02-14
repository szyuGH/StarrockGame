#if OPENGL
	#define SV_POSITION POSITION
	#define VS_SHADERMODEL vs_3_0
	#define PS_SHADERMODEL ps_3_0
#else
	#define VS_SHADERMODEL vs_4_0_level_9_1
	#define PS_SHADERMODEL ps_4_0_level_9_1
#endif

float2 TexDim;
float4 OutlineColor;

Texture2D SpriteTexture;

sampler2D SpriteTextureSampler = sampler_state
{
	Texture = <SpriteTexture>;
};

struct VertexShaderOutput
{
	float4 Position : SV_POSITION;
	float4 Color : COLOR0;
	float2 TextureCoordinates : TEXCOORD0;
};

float4 OutlinePixelShader(VertexShaderOutput input) : COLOR
{
	float4 color = tex2D(SpriteTextureSampler,input.TextureCoordinates) * input.Color;
	if (color.a < 0.5f)
	{
		if (tex2D(SpriteTextureSampler, input.TextureCoordinates + float2(1,0) / TexDim).a >= 0.5f
			|| tex2D(SpriteTextureSampler, input.TextureCoordinates + float2(-1,0) / TexDim).a >= 0.5f
			|| tex2D(SpriteTextureSampler, input.TextureCoordinates + float2(0,1) / TexDim).a >= 0.5f
			|| tex2D(SpriteTextureSampler, input.TextureCoordinates + float2(0,-1) / TexDim).a >= 0.5f
			|| tex2D(SpriteTextureSampler, input.TextureCoordinates + float2(1,1) / TexDim).a >= 0.5f
			|| tex2D(SpriteTextureSampler, input.TextureCoordinates + float2(-1,1) / TexDim).a >= 0.5f
			|| tex2D(SpriteTextureSampler, input.TextureCoordinates + float2(1,-1) / TexDim).a >= 0.5f
			|| tex2D(SpriteTextureSampler, input.TextureCoordinates + float2(-1,-1) / TexDim).a >= 0.5f)
		{
			color = OutlineColor;
		}
	}
	return color;
}

technique SpriteDrawing
{
	pass P0
	{
		PixelShader = compile PS_SHADERMODEL OutlinePixelShader();
	}
};
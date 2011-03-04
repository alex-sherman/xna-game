
uniform extern texture UserTextureA;
uniform extern texture UserTextureB;
uniform extern texture UserTextureC;
uniform extern texture UserTextureD;
float4x4 WorldViewProj;
float3 cameraPosition;
float4x4 world;
float4x4 view;
float4x4 proj;
//light properties
float3 lightPosition;
float4 ambientLightColor;
float4 diffuseLightColor;
float4 specularLightColor;

//material properties
float specularPower;
float specularIntensity;


struct VertexShaderOutputPerVertexDiffuse
{
     float4 Position : POSITION;
     float3 WorldNormal : TEXCOORD0;
     float3 WorldPosition : TEXCOORD1;
	 float4 textureCoord : TEXCOORD2;
     float4 Color : COLOR0;
};


struct PixelShaderInputPerVertexDiffuse
{
	float4 textureCoord : TEXCOORD2;
     float3 WorldNormal : TEXCOORD0;
     float3 WorldPosition : TEXCOORD1;
     float4 Color: COLOR0;
};


sampler sand = sampler_state
{
    Texture = <UserTextureA>;
    mipfilter = LINEAR; 
	AddressU = wrap;
	AddressV = wrap;
};
sampler grass = sampler_state
{
    Texture = <UserTextureB>;
    mipfilter = LINEAR; 
};
sampler rock = sampler_state
{
    Texture = <UserTextureC>;
    mipfilter = LINEAR; 
};
sampler textureSampler4 = sampler_state
{
    Texture = <UserTextureA>;
    mipfilter = LINEAR; 
};
struct VS_OUTPUT
{
    float4 position  : POSITION;
    float4 textureCoordinate : TEXCOORD0;
	float4 color : TEXCOORD1;
	float4 blend : TEXCOORD2;

};

 
VS_OUTPUT Transform(
    float4 Position  : POSITION, 
    float4 TextureCoordinate : TEXCOORD0,
	float3 normal : NORMAL, 
	float4 blend : TEXCOORD1)
{
    VS_OUTPUT Out = (VS_OUTPUT)0;
	
	float4 worldPosition = mul(Position, world);
	float4 viewPosition = mul(Position, view);
    Out.position = mul(viewPosition, proj);
    Out.textureCoordinate = TextureCoordinate;
	Out.blend = blend;

    return Out;
}

VS_OUTPUT InstanceTransform(
    float4 Position  : POSITION, 
    float4 TextureCoordinate : TEXCOORD0,
	float3 normal : NORMAL,
	float4x4 instances : BLENDWEIGHT)
{
    VS_OUTPUT Out = (VS_OUTPUT)0;
	float4 worldPosition = mul(Position, transpose(instances));
	float4 viewPosition = mul(worldPosition, view);
    Out.position = mul(viewPosition, proj);
    Out.textureCoordinate = TextureCoordinate;

    return Out;
}

float4 ApplyTexture(VS_OUTPUT vsout) : COLOR
{
	float4 color = ambientLightColor*tex2D(textureSampler4, vsout.textureCoordinate).rgba;
    return color;
}
float4 ApplyMultiTexture(VS_OUTPUT vsout) : COLOR
{
	float4 color = tex2D(sand, vsout.textureCoordinate).rgba*vsout.blend.x;
	color += tex2D(grass, vsout.textureCoordinate).rgba*vsout.blend.y;
	color += tex2D(rock, vsout.textureCoordinate).rgba*vsout.blend.z;
	color += tex2D(textureSampler4, vsout.textureCoordinate).rgba*vsout.blend.w;
    return color;
}
technique Texture
{
	pass P0
    {
        vertexShader = compile vs_3_0 Transform();
        pixelShader  = compile ps_3_0 ApplyTexture();
    }
	 
}
technique MultiTexture
{
	pass P0
    {
        vertexShader = compile vs_3_0 Transform();
        pixelShader  = compile ps_3_0 ApplyMultiTexture();
    }
}
technique InstanceTexture
{
	pass P0
	{
		vertexShader = compile vs_3_0 InstanceTransform();
        pixelShader  = compile ps_3_0 ApplyTexture();
	}
}
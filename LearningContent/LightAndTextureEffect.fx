
uniform extern texture UserTextureA;
uniform extern texture UserTextureB;
uniform extern texture UserTextureC;
uniform extern texture UserTextureD;
uniform extern texture WaterBump;
uniform extern texture Reflection;
uniform extern texture Refraction;
float4x4 WorldViewProj;
float3 cameraPosition;
float4 ClipPlane1;
float4 ClipPlane2;
float4x4 world;
float4x4 view;
float4x4 reflView;
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
sampler waterReflection = sampler_state
{
    Texture = <Reflection>;
    mipfilter = LINEAR; 
};
sampler waterRefraction = sampler_state
{
    Texture = <Refraction>;
    mipfilter = LINEAR; 
};
sampler waterBump = sampler_state
{
    Texture = <WaterBump>;
    mipfilter = LINEAR; 
	AddressU = mirror;
	AddressV = mirror;
};
struct VS_OUTPUT
{
    float4 position  : POSITION;
    float4 textureCoordinate : TEXCOORD0;
	float4 color : TEXCOORD1;
	float4 blend : TEXCOORD2;
	float4 clipDistance : TEXCOORD3;

};
struct WaterVSOutput
{
	float4 position  : POSITION;
	float4 vPosition : POSITION1;
    float4 textureCoordinate : TEXCOORD0;
    float2 BumpMapSamplingPos        : TEXCOORD2;
};
 
VS_OUTPUT Transform(
    float4 Position  : POSITION, 
    float4 TextureCoordinate : TEXCOORD0)
{
    VS_OUTPUT Out = (VS_OUTPUT)0;
	float4 viewPosition = mul(Position, view);
    Out.position = mul(viewPosition, proj);
    Out.textureCoordinate = TextureCoordinate;

    return Out;
}
VS_OUTPUT TransformClip1(
    float4 Position  : POSITION, 
    float4 TextureCoordinate : TEXCOORD0,
	float3 normal : NORMAL, 
	float4 blend : TEXCOORD1)
{
    VS_OUTPUT Out = (VS_OUTPUT)0;
	
	
	float4 worldPosition = mul(Position, world);
	float4 viewPosition = mul(Position, view);
    Out.position = mul(viewPosition, proj);
	Out.clipDistance.x = dot(Position, ClipPlane1);
	Out.clipDistance.y = 0;
	Out.clipDistance.z = 0;
	Out.clipDistance.w = 0;
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
	float4 color = ambientLightColor*tex2D(grass, vsout.textureCoordinate).rgba;
    return color;
}
WaterVSOutput WaterVS(
    float4 Position  : POSITION, 
    float4 TextureCoordinate : TEXCOORD0)
{
    WaterVSOutput Out = (WaterVSOutput)0;
	float4 viewPosition = mul(Position, view);
    Out.position = mul(viewPosition, proj);
	Out.vPosition = Position;
	float4x4 reflectionViewProj = mul(reflView, proj);
    Out.textureCoordinate = mul(Position,reflectionViewProj);
	Out.BumpMapSamplingPos = TextureCoordinate*50;
    return Out;
}
float4 ApplyWaterTexture(WaterVSOutput vsout) : COLOR
{
	float2 reflTexCoord;
	float4 bumpColor = tex2D(waterBump, vsout.BumpMapSamplingPos);
	float2 perturbation = (bumpColor.rg - 0.5f)*2;

	float2 ProjectedRefrTexCoords;
	ProjectedRefrTexCoords = vsout.textureCoordinate/vsout.textureCoordinate.w/2.0f+.5f;
	reflTexCoord = ProjectedRefrTexCoords;
	reflTexCoord.y = - reflTexCoord.y;
	float2 perturbatedTexCoords = reflTexCoord +  perturbation*.1f;


	float2 perturbatedRefrTexCoords = ProjectedRefrTexCoords + perturbation;    
	float4 refractiveColor = tex2D(waterRefraction, perturbatedRefrTexCoords);

	float3 eyeVector = normalize(cameraPosition - vsout.vPosition);

	float fresnelTerm = dot(eyeVector, float3(0,1,0));

	float4 reflectionColor;
	float4 refractionColor;
	float4 color;
	reflectionColor = tex2D(waterReflection,perturbatedTexCoords);
	refractionColor = tex2D(waterRefraction,ProjectedRefrTexCoords);
	color = lerp(reflectionColor, refractionColor, fresnelTerm);
	color = lerp(color,float4(0.3f, 0.3f, 0.5f, 1.0f),.2f);
    return color;
}
float4 ApplyMultiTexture(VS_OUTPUT vsout) : COLOR
{
	float4 color = tex2D(sand, vsout.textureCoordinate).rgba*vsout.blend.x;
	color += tex2D(grass, vsout.textureCoordinate).rgba*vsout.blend.y;
	color += tex2D(rock, vsout.textureCoordinate).rgba*vsout.blend.z;
	color += tex2D(rock, vsout.textureCoordinate).rgba*vsout.blend.w;
    return color;
}
float4 ApplyMultiAndClip(VS_OUTPUT vsout) : COLOR
{
	clip(vsout.clipDistance);
	float4 color = tex2D(sand, vsout.textureCoordinate).rgba*vsout.blend.x;
	color += tex2D(grass, vsout.textureCoordinate).rgba*vsout.blend.y;
	color += tex2D(rock, vsout.textureCoordinate).rgba*vsout.blend.z;
	color += tex2D(rock, vsout.textureCoordinate).rgba*vsout.blend.w;
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
technique WaterEffect
{
	pass P0
    {
        vertexShader = compile vs_3_0 WaterVS();
        pixelShader  = compile ps_3_0 ApplyWaterTexture();
    }
}
technique MultiTextureClip
{
	pass P1
    {
        vertexShader = compile vs_3_0 TransformClip1();
        pixelShader  = compile ps_3_0 ApplyMultiAndClip();
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

uniform extern texture UserTexture;
float4x4 WorldViewProj;
float3 cameraPosition;
float4x4 world;
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


sampler textureSampler = sampler_state
{
    Texture = <UserTexture>;
    mipfilter = LINEAR; 
};

VertexShaderOutputPerVertexDiffuse PerVertexDiffuseVS(
     float3 position : POSITION,
	 float4 textureCoord : TEXCOORD0,
     float3 normal : NORMAL )
{
     VertexShaderOutputPerVertexDiffuse output;
	 output.textureCoord = textureCoord;
     //generate the world-view-projection matrix
     float4x4 wvp = WorldViewProj;
     
     //transform the input position to the output
     output.Position = mul(float4(position, 1.0), wvp);
     
     output.WorldNormal =  mul(normal, world);
     float4 worldPosition =  mul(float4(position, 1.0), world);
     output.WorldPosition = worldPosition/ worldPosition.w;;
     
     //calculate diffuse component
     float3 directionToLight = normalize(lightPosition - output.WorldPosition);
     float diffuseIntensity = saturate( dot(directionToLight, output.WorldNormal));
     float4 diffuse = diffuseLightColor * 1/length(lightPosition - output.WorldPosition)*5;

     output.Color = diffuse + ambientLightColor;


     //return the output structure
     return output;
}
float4 PhongPS(PixelShaderInputPerVertexDiffuse input) : COLOR
{

     float3 directionToLight = normalize(lightPosition - input.WorldPosition);
     float3 reflectionVector = normalize(reflect(-directionToLight, input.WorldNormal));
     float3 directionToCamera = normalize(cameraPosition - input.WorldPosition);
     
     //calculate specular component
     float4 specular = specularLightColor * specularIntensity * 
                       pow( saturate(dot(reflectionVector, directionToCamera)), 
                       specularPower);
     
     float4 color = input.Color + specular;
     color.a = 1;
     
     return color;
}
struct VS_OUTPUT
{
    float4 position  : POSITION;
    float4 textureCoordinate : TEXCOORD0;
};

 
VS_OUTPUT Transform(
    float4 Position  : POSITION, 
    float4 TextureCoordinate : TEXCOORD0,
	float3 normal : NORMAL )
{
    VS_OUTPUT Out = (VS_OUTPUT)0;

    Out.position = mul(Position, WorldViewProj);
    Out.textureCoordinate = TextureCoordinate;

    return Out;
}

float4 ApplyTexture(VS_OUTPUT vsout) : COLOR
{
    return diffuseLightColor*tex2D(textureSampler, vsout.textureCoordinate).rgba;
}

technique TransformAndTexture
{
	/*pass P0
    {
        vertexShader = compile vs_3_0 Transform();
        pixelShader  = compile ps_3_0 ApplyTexture();
    }*/
    pass P1
    {
          //Per-vertex diffuse calculation and preparation of inputs
          //for the phong pixel shader
          VertexShader = compile vs_3_0 PerVertexDiffuseVS();
          
          //set the pixel shader to the per-pixel phong function      
          PixelShader = compile ps_3_0 PhongPS();
    }
	 
}
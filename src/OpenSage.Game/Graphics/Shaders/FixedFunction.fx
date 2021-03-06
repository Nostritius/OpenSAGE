#include "Common.hlsli"
#include "MeshCommon.hlsli"

struct VSOutputFixedFunction
{
    PSInputCommon TransferCommon;
    VSOutputCommon VSOutput;
};

struct PSInputFixedFunction
{
    PSInputCommon TransferCommon;

    float4 ScreenPosition : SV_Position;
};

VSOutputFixedFunction VS(VSInputSkinned input)
{
    VSOutputFixedFunction result = (VSOutputFixedFunction)0;

    VSSkinnedInstanced(input, result.VSOutput, result.TransferCommon);

    return result;
}

#define SPECULAR_ENABLED
#define LIGHTING_TYPE Object
#include "Lighting.hlsli"

struct TextureMapping
{
    uint MappingType;

    float Speed;
    float FPS;
    uint Log2Width;

    float2 UVPerSec;
    float2 UVScale;
    float2 UVCenter;
    float2 UVAmplitude;
    float2 UVFrequency;
    float2 UVPhase;
};

struct VertexMaterial
{
    float3 Ambient;
    float3 Diffuse;
    float3 Specular;
    float Shininess;
    float3 Emissive;
    float Opacity;

    TextureMapping TextureMappingStage0;
    TextureMapping TextureMappingStage1;
};

struct ShadingConfiguration
{
    uint DiffuseLightingType;
    bool SpecularEnabled;
    bool TexturingEnabled;
    uint SecondaryTextureColorBlend;
    uint SecondaryTextureAlphaBlend;
    bool AlphaTest;
};

cbuffer MaterialConstants : register(b2)
{
    uint NumTextureStages;
    VertexMaterial Material;
    ShadingConfiguration Shading;
};

#define TEXTURE_MAPPING_UV                 0
#define TEXTURE_MAPPING_ENVIRONMENT        1
#define TEXTURE_MAPPING_LINEAR_OFFSET      2
#define TEXTURE_MAPPING_ROTATE             3
#define TEXTURE_MAPPING_SINE_LINEAR_OFFSET 4
#define TEXTURE_MAPPING_SCREEN             5
#define TEXTURE_MAPPING_SCALE              6
#define TEXTURE_MAPPING_GRID               7

#define DIFFUSE_LIGHTING_DISABLE       0
#define DIFFUSE_LIGHTING_MODULATE      1
#define DIFFUSE_LIGHTING_ADD           2

#define SECONDARY_TEXTURE_BLEND_DISABLE      0
#define SECONDARY_TEXTURE_BLEND_DETAIL       1
#define SECONDARY_TEXTURE_BLEND_SCALE        2
#define SECONDARY_TEXTURE_BLEND_INV_SCALE    3
#define SECONDARY_TEXTURE_BLEND_DETAIL_BLEND 4

Texture2D<float4> Texture0 : register(t0);
Texture2D<float4> Texture1 : register(t1);

SamplerState Sampler : register(s0);

#define TWO_PI (2 * 3.1415926535)

float4 SampleTexture(
    float3 worldNormal, float2 uv, float2 screenPosition,
    TextureMapping textureMapping,
    Texture2D<float4> diffuseTexture,
    float3 viewVector)
{
    float t = TimeInSeconds;

    switch (textureMapping.MappingType)
    {
    case TEXTURE_MAPPING_UV:
        uv = float2(uv.x, 1 - uv.y);
        break;

    case TEXTURE_MAPPING_ENVIRONMENT:
        uv = (reflect(viewVector, worldNormal).xy / 2.0f) + float2(0.5f, 0.5f);
        break;

    case TEXTURE_MAPPING_LINEAR_OFFSET:
        float2 offset = textureMapping.UVPerSec * t;
        uv = float2(uv.x, 1 - uv.y) + offset;
        uv *= textureMapping.UVScale;
        break;

    case TEXTURE_MAPPING_ROTATE:
        float angle = textureMapping.Speed * t * TWO_PI;
        float s = sin(angle);
        float c = cos(angle);

        uv -= textureMapping.UVCenter;

        float2 rotatedPoint = float2(
            uv.x * c - uv.y * s,
            uv.x * s + uv.y * c);

        uv = rotatedPoint + textureMapping.UVCenter;

        uv *= textureMapping.UVScale;

        break;

    case TEXTURE_MAPPING_SINE_LINEAR_OFFSET:
        uv.x += textureMapping.UVAmplitude.x * sin(textureMapping.UVFrequency.x * t * TWO_PI - textureMapping.UVPhase.x * TWO_PI);
        uv.y += textureMapping.UVAmplitude.y * cos(textureMapping.UVFrequency.y * t * TWO_PI - textureMapping.UVPhase.y * TWO_PI);
        break;

    case TEXTURE_MAPPING_SCREEN:
        uv = (screenPosition / ViewportSize) * textureMapping.UVScale;
        break;

    case TEXTURE_MAPPING_SCALE:
        uv *= textureMapping.UVScale;
        break;

    case TEXTURE_MAPPING_GRID:
        uv = float2(uv.x, 1 - uv.y);
        uint numFramesPerSide = pow(2, textureMapping.Log2Width);
        uint numFrames = numFramesPerSide * numFramesPerSide;
        uint currentFrame = (t * textureMapping.FPS) % numFrames;
        uint currentFrameU = currentFrame % numFramesPerSide;
        uint currentFrameV = currentFrame / numFramesPerSide;
        uv.x += currentFrameU / (float)numFramesPerSide;
        uv.y += currentFrameV / (float)numFramesPerSide;
        break;
    }

    return diffuseTexture.Sample(Sampler, uv);
}

float4 PS(PSInputFixedFunction input) : SV_Target
{
    LightingParameters lightingParams;
    lightingParams.WorldPosition = input.TransferCommon.WorldPosition;
    lightingParams.WorldNormal = input.TransferCommon.WorldNormal;
    lightingParams.MaterialAmbient = Material.Ambient;
    lightingParams.MaterialDiffuse = Material.Diffuse;
    lightingParams.MaterialSpecular = Material.Specular;
    lightingParams.MaterialShininess = Material.Shininess;

    float3 diffuseColor;
    float3 specularColor;
    DoLighting(lightingParams, diffuseColor, specularColor);

    float4 diffuseTextureColor;
    if (Shading.TexturingEnabled)
    {
        float3 v = CalculateViewVector(input.TransferCommon.WorldPosition);

        diffuseTextureColor = SampleTexture(
            input.TransferCommon.WorldNormal, input.TransferCommon.UV0, input.ScreenPosition.xy,
            Material.TextureMappingStage0,
            Texture0,
            v);

        if (NumTextureStages > 1)
        {
            float4 secondaryTextureColor = SampleTexture(
                input.TransferCommon.WorldNormal, input.TransferCommon.UV1, input.ScreenPosition.xy,
                Material.TextureMappingStage1,
                Texture1,
                v);

            switch (Shading.SecondaryTextureColorBlend)
            {
            case SECONDARY_TEXTURE_BLEND_DETAIL:
                diffuseTextureColor.rgb = secondaryTextureColor.rgb;
                break;

            case SECONDARY_TEXTURE_BLEND_SCALE:
                diffuseTextureColor.rgb *= secondaryTextureColor.rgb;
                break;

            case SECONDARY_TEXTURE_BLEND_INV_SCALE:
                diffuseTextureColor.rgb += (float3(1, 1, 1) - diffuseTextureColor.rgb) * secondaryTextureColor.rgb;
                break;

            case SECONDARY_TEXTURE_BLEND_DETAIL_BLEND:
                // (otherAlpha)*local + (~otherAlpha)*other
                diffuseTextureColor.rgb += (secondaryTextureColor.a * diffuseTextureColor.rgb) + ((1 - secondaryTextureColor.a) * secondaryTextureColor.rgb);
                break;
            }

            switch (Shading.SecondaryTextureAlphaBlend)
            {
            case SECONDARY_TEXTURE_BLEND_DETAIL:
                diffuseTextureColor.a = secondaryTextureColor.a;
                break;

            case SECONDARY_TEXTURE_BLEND_SCALE:
                diffuseTextureColor.a *= secondaryTextureColor.a;
                break;

            case SECONDARY_TEXTURE_BLEND_INV_SCALE:
                diffuseTextureColor.a += (1 - diffuseTextureColor.a) * secondaryTextureColor.a;
                break;
            }
        }

        if (Shading.AlphaTest)
        {
            if (diffuseTextureColor.a < AlphaTestThreshold)
            {
                discard;
            }
        }
    }
    else
    {
        diffuseTextureColor = float4(1, 1, 1, 1);
    }

    float3 totalObjectLighting = saturate(diffuseColor + Material.Emissive);

    float3 objectColor = diffuseTextureColor.rgb;

    switch (Shading.DiffuseLightingType)
    {
    case DIFFUSE_LIGHTING_MODULATE:
        objectColor *= totalObjectLighting;
        break;

    case DIFFUSE_LIGHTING_ADD:
        objectColor += totalObjectLighting;
        break;
    }

    if (Shading.SpecularEnabled)
    {
        objectColor += specularColor;
    }

    return float4(
        objectColor,
        Material.Opacity * diffuseTextureColor.a);
}
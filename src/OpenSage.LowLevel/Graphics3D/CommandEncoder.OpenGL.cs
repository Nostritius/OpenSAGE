using OpenSage.LowLevel.Graphics3D.Util;

namespace OpenSage.LowLevel.Graphics3D
{
    partial class CommandEncoder
    {
        internal CommandEncoder(GraphicsDevice graphicsDevice)
            : base(graphicsDevice)
        {
        }
        
        private void PlatformClose()
        {
        }

        private void PlatformDraw(
            PrimitiveType primitiveType,
            uint vertexStart,
            uint vertexCount)
        {
        }

        private void PlatformDrawIndexed(
            PrimitiveType primitiveType,
            uint indexCount,
            Buffer<ushort> indexBuffer,
            uint indexBufferOffset)
        {
        }

        private void PlatformDrawIndexedInstanced(
            PrimitiveType primitiveType,
            uint indexCount,
            uint instanceCount,
            Buffer<ushort> indexBuffer,
            uint indexBufferOffset)
        {
        }

        private void PlatformSetVertexShaderTexture(int slot, Texture texture)
        {
        }

        private void PlatformSetPixelShaderTexture(int slot, Texture texture)
        {
        }

        private void PlatformSetVertexShaderSampler(int slot, SamplerState sampler)
        {
        }

        private void PlatformSetPixelShaderSampler(int slot, SamplerState sampler)
        {
        }

        private void PlatformSetVertexShaderStructuredBuffer(int slot, Buffer buffer)
        {
        }

        private void PlatformSetPixelShaderStructuredBuffer(int slot, Buffer buffer)
        {
        }

        private void PlatformSetVertexShaderConstantBuffer(int slot, Buffer buffer)
        {
        }

        private void PlatformSetPixelShaderConstantBuffer(int slot, Buffer buffer)
        {
        }

        private void PlatformSetPipelineState(PipelineState pipelineState)
        {
        }

        private void PlatformSetVertexBuffer(int slot, Buffer vertexBuffer)
        {
        }

        private void PlatformSetViewport(Viewport viewport)
        {
        }
    }
}

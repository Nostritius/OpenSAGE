namespace OpenSage.LowLevel.Graphics3D
{
    partial class CommandQueue
    {
        private CommandBuffer _commandBuffer;
        
        private void PlatformConstruct(GraphicsDevice graphicsDevice)
        {
            _commandBuffer = new CommandBuffer(graphicsDevice, this);
        }

        private CommandBuffer PlatformGetCommandBuffer()
        {
            return _commandBuffer;
        }
    }
}

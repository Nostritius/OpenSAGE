namespace OpenSage.LowLevel.Graphics3D
{
    partial class CommandBuffer
    {
        internal CommandBuffer(GraphicsDevice graphicsDevice, CommandQueue parent)
            : base(graphicsDevice)
        {
        }
        
        private CommandEncoder PlatformGetCommandEncoder(RenderPassDescriptor renderPassDescriptor)
        {
            return new CommandEncoder(GraphicsDevice);
        }

        private void PlatformCommit()
        {
        }

        private void PlatformCommitAndPresent(SwapChain swapChain)
        {
        }
    }
}

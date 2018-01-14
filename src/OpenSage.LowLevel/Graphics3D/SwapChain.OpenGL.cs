using OpenSage.LowLevel.Graphics3D;

namespace OpenSage.LowLevel.Graphics3D
{
    partial class SwapChain
    {
        private int PlatformBackBufferWidth { get; set; }
        private int PlatformBackBufferHeight { get; set; }
        
        private RenderTarget PlatformGetNextRenderTarget()
        {
            return null;
        }
    }
}

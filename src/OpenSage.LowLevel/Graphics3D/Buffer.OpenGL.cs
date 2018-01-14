namespace OpenSage.LowLevel.Graphics3D
{
    partial class Buffer
    {
        private void PlatformConstruct(
            GraphicsDevice graphicsDevice,
            uint sizeInBytes,
            uint elementSizeInBytes,
            BufferBindFlags flags,
            ResourceUsage usage,
            byte[] initialData)
        {
        }
        
        private uint PlatformGetAlignedSize(uint sizeInBytes)
        {
            return sizeInBytes;
        }
        
        internal void PlatformSetData(byte[] data, int dataSizeInBytes)
        {
        }
    }
}

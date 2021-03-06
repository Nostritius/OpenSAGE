using System.Runtime.InteropServices;
using OpenSage.LowLevel.Graphics3D.Util;
using SharpDX;
using SharpDX.Direct3D11;
using D3D11 = SharpDX.Direct3D11;

namespace OpenSage.LowLevel.Graphics3D
{
    partial class Buffer
    {
        private const int ConstantBufferAlignment = 256;

        private const int ConstantBufferAlignmentMask = ConstantBufferAlignment - 1;

        internal D3D11.Buffer DeviceBuffer { get; set; }

        internal override string PlatformGetDebugName() => DeviceBuffer.DebugName;
        internal override void PlatformSetDebugName(string value) => DeviceBuffer.DebugName = value;

        private D3D11.ShaderResourceView _deviceShaderResourceView;
        internal D3D11.ShaderResourceView DeviceShaderResourceView
        {
            get
            {
                if (_deviceShaderResourceView == null)
                {
                    _deviceShaderResourceView = AddDisposable(new D3D11.ShaderResourceView(
                        GraphicsDevice.Device,
                        DeviceBuffer,
                        new D3D11.ShaderResourceViewDescription
                        {
                            Dimension = SharpDX.Direct3D.ShaderResourceViewDimension.Buffer,
                            Format = SharpDX.DXGI.Format.Unknown,
                            Buffer =
                            {
                                FirstElement = 0,
                                ElementCount = (int) ElementCount
                            }
                        }));
                }
                return _deviceShaderResourceView;
            }
        }

        private void PlatformConstruct(
            GraphicsDevice graphicsDevice,
            uint sizeInBytes,
            uint elementSizeInBytes,
            BufferBindFlags flags,
            ResourceUsage usage,
            byte[] initialData)
        {
            var optionFlags = flags.HasFlag(BufferBindFlags.ShaderResource)
                ? D3D11.ResourceOptionFlags.BufferStructured
                : D3D11.ResourceOptionFlags.None;

            var cpuAccessFlags = usage == ResourceUsage.Dynamic
                ? D3D11.CpuAccessFlags.Write
                : D3D11.CpuAccessFlags.None;

            var d3d11Usage = usage == ResourceUsage.Dynamic
                ? D3D11.ResourceUsage.Dynamic
                : D3D11.ResourceUsage.Immutable;

            var description = new D3D11.BufferDescription
            {
                BindFlags = flags.ToBindFlags(),
                CpuAccessFlags = cpuAccessFlags,
                OptionFlags = optionFlags,
                SizeInBytes = (int) sizeInBytes,
                StructureByteStride = (int) elementSizeInBytes,
                Usage = d3d11Usage
            };

            if (usage == ResourceUsage.Static)
            {
                using (var dataStream = DataStream.Create(initialData, true, false))
                {
                    DeviceBuffer = AddDisposable(new D3D11.Buffer(
                        graphicsDevice.Device,
                        dataStream,
                        description));
                }
            }
            else
            {
                DeviceBuffer = AddDisposable(new D3D11.Buffer(
                    graphicsDevice.Device,
                    description));
            }
        }

        private uint PlatformGetAlignedSize(uint sizeInBytes)
        {
            // Align to 256 bytes.
            return (uint) ((sizeInBytes + ConstantBufferAlignmentMask) & ~ConstantBufferAlignmentMask);
        }

        internal void PlatformSetData(byte[] data, int dataSizeInBytes)
        {
            var dataBox = GraphicsDevice.Device.ImmediateContext.MapSubresource(
                DeviceBuffer,
                0,
                D3D11.MapMode.WriteDiscard,
                D3D11.MapFlags.None);

            Marshal.Copy(data, 0, dataBox.DataPointer, dataSizeInBytes);

            GraphicsDevice.Device.ImmediateContext.UnmapSubresource(DeviceBuffer, 0);
        }
    }
}

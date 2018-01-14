namespace OpenSage.LowLevel.Graphics3D
{
    partial class Shader
    {
        private void PlatformConstruct(
            GraphicsDevice graphicsDevice,
            string functionName,
            byte[] deviceBytecode,
            out ShaderType shaderType,
            out ShaderResourceBinding[] resourceBindings)
        {
            shaderType = ShaderType.VertexShader;
            resourceBindings = null;
        }
    }
}

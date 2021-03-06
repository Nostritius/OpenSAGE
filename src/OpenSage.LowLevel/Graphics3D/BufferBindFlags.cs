﻿using System;

namespace OpenSage.LowLevel.Graphics3D
{
    [Flags]
    public enum BufferBindFlags
    {
        None = 0,

        VertexBuffer = 0x1,

        IndexBuffer = 0x2,

        ConstantBuffer = 0x4,

        ShaderResource = 0x8
    }
}

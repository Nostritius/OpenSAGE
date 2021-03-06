﻿namespace OpenSage.LowLevel.Graphics3D.Util.BlockCompression
{
    internal sealed class Bc1 : BcBase
    {
        public override int BytesPerBlock => 8;

        public override void DecompressBlock(
            byte[] data,
            int startIndex,
            byte[] result,
            int resultX,
            int resultY,
            int resultStride)
        {
            DecompressColors(
                data,
                startIndex,
                true,
                result,
                resultX,
                resultY,
                resultStride);
        }
    }
}
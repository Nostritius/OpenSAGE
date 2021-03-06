﻿using System;

namespace OpenSage.Mathematics
{
    /// <summary>
    /// Describes a floating-point rectangle.
    /// </summary>
    public struct RectangleF
    {
        /// <summary>
        /// Gets or sets the x component of the <see cref="RectangleF"/>.
        /// </summary>
        public float X;

        /// <summary>
        /// Gets or sets the x component of the <see cref="RectangleF"/>.
        /// </summary>
        public float Y;

        /// <summary>
        /// Gets or sets the width of the <see cref="RectangleF"/>.
        /// </summary>
        public float Width;

        /// <summary>
        /// Gets or sets the height of the <see cref="RectangleF"/>.
        /// </summary>
        public float Height;

        public SizeF Size => new SizeF(Width, Height);

        /// <summary>
        /// Creates a new <see cref="RectangleF"/>.
        /// </summary>
        public RectangleF(float x, float y, float width, float height)
        {
            X = x;
            Y = y;
            Width = width;
            Height = height;
        }

        /// <inheritdoc />
        public override string ToString()
        {
            return $"{{X:{X} Y:{Y} Width:{Width} Height:{Height}}}";
        }

        public static Rectangle CalculateRectangleFittingAspectRatio(
            in RectangleF rect,
            in SizeF boundsSize,
            in Size viewportSize,
            out float scale)
        {
            // Figure out the ratio.
            var ratioX = viewportSize.Width / boundsSize.Width;
            var ratioY = viewportSize.Height / boundsSize.Height;

            // Use whichever multiplier is smaller.
            var ratio = ratioX < ratioY ? ratioX : ratioY;

            scale = ratio;

            // Now we can get the new height and width
            var newWidth = (int) Math.Round(rect.Width * ratio);
            var newHeight = (int) Math.Round(rect.Height * ratio);

            newWidth = Math.Max(newWidth, 1);
            newHeight = Math.Max(newHeight, 1);

            var newX = (int) Math.Round(rect.X * ratio);
            var newY = (int) Math.Round(rect.Y * ratio);

            // Now calculate the X,Y position of the upper-left corner 
            // (one of these will always be zero for the top level window)
            var posX = (int) Math.Round((viewportSize.Width - (boundsSize.Width * ratio)) / 2.0) + newX;
            var posY = (int) Math.Round((viewportSize.Height - (boundsSize.Height * ratio)) / 2.0) + newY;

            return new Rectangle(posX, posY, newWidth, newHeight);
        }
    }
}

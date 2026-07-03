using Accept_Reject2.Model;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace Accept_Reject2.Helper
{
    public class BitmapGate
    {
        private bool[,] lookup = new bool[1, 1];
        private int bitmapWidth;
        private int bitmapHeight;

        /// <summary>
        /// Builds the lookup table whenever the gate changes.
        /// Call this after Update, Drag, Resize or Rotate.
        /// </summary>
        public void Build(Rectangle clientRect, EllipseGate gate)
        {
            bitmapWidth = clientRect.Width;
            bitmapHeight = clientRect.Height;

            lookup = new bool[bitmapWidth, bitmapHeight];

            using Bitmap bitmap = new Bitmap(bitmapWidth, bitmapHeight);

            using (Graphics g = Graphics.FromImage(bitmap))
            {
                g.SmoothingMode = SmoothingMode.None;

                // Outside gate = Black
                g.Clear(Color.Black);

                // Same transform used by GateRenderer
                float centerX = bitmapWidth / 2f + gate.CenterX;
                float centerY = bitmapHeight / 2f - gate.CenterY;

                g.TranslateTransform(centerX, centerY);
                g.RotateTransform(gate.Angle);

                // Inside gate = White
                g.FillEllipse(
                    Brushes.White,
                    -gate.Width / 2f,
                    -gate.Height / 2f,
                    gate.Width,
                    gate.Height);
            }

            // Convert bitmap into lookup table
            for (int y = 0; y < bitmapHeight; y++)
            {
                for (int x = 0; x < bitmapWidth; x++)
                {
                    lookup[x, y] = bitmap.GetPixel(x, y).R == 255;
                }
            }
        }

        /// <summary>
        /// Returns true if the sample lies inside the gate.
        /// </summary.>
        public bool IsInside(Rectangle clientRect, float sampleX, float sampleY)
        {
            int screenX = (int)Math.Round(clientRect.Width / 2f + sampleX);
            int screenY = (int)Math.Round(clientRect.Height / 2f - sampleY);

            if (screenX < 0 ||
                screenX >= bitmapWidth ||
                screenY < 0 ||
                screenY >= bitmapHeight)
            {
                return false;
            }

            return lookup[screenX, screenY];
        }
    }
}
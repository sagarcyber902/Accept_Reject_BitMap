using Accept_Reject2.Model;
using System.Drawing;
using System.Drawing.Drawing2D;


namespace Accept_Reject2.Renderer
{
    public class GateRenderer
    {
        private static PointF GetScreenCenter(Rectangle rect, EllipseGate gate)
        {
            return new PointF(rect.Width / 2f + gate.CenterX, rect.Height / 2f - gate.CenterY);
        }
        private static GraphicsState BeginGateTransform(Graphics g, Rectangle rect, EllipseGate gate)
        {
            PointF center = GetScreenCenter(rect, gate);
            GraphicsState state = g.Save();
            g.TranslateTransform(center.X, center.Y);
            g.RotateTransform(gate.Angle);
            return state;
        }
        public void Draw(Graphics g, Rectangle clientRect, EllipseGate gate, IReadOnlyList<Sample> samples, Font font, int handleRadius, int rotationHandleOffset, bool editMode)
        {
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.Clear(Color.White);
            DrawAxes(g, clientRect);
            DrawCenter(g, clientRect, gate);
            DrawEllipse(g, clientRect, gate);
            if (editMode)
            {
                DrawHandles(g, clientRect, gate, handleRadius);
                DrawRotationHandle(g, clientRect, gate, handleRadius, rotationHandleOffset);
            }
            foreach (var sample in samples)
            {
                DrawSample(g, clientRect, gate, sample, font);
            }
            DrawStatus(g, samples, font);
        }
        //Draw the axes in the center of the client rectangle
        public void DrawAxes(Graphics g, Rectangle clientRect)
        {
            float originX = clientRect.Width / 2f;
            float originY = clientRect.Height / 2f;
            using (Pen axisPen = new Pen(Color.Gray, 0.1f))
            {
                g.DrawLine(axisPen, 0, originY, clientRect.Width, originY);
                g.DrawLine(axisPen, originX, 0, originX, clientRect.Height);
            }

        }
        //Draw the center point of the gate
        public void DrawCenter(Graphics g, Rectangle rect, EllipseGate gate)
        {
            PointF center = GetScreenCenter(rect, gate);
            g.FillEllipse(Brushes.Red, center.X - 2, center.Y - 2, 4, 4);
        }
        //Draw the ellipse gate
        public void DrawEllipse(Graphics g, Rectangle rect, EllipseGate gate)
        {
            GraphicsState state = BeginGateTransform(g, rect, gate);
            using Pen pen = new Pen(Color.Black, 1);
            g.DrawEllipse(pen, -gate.Width / 2f, -gate.Height / 2f, gate.Width, gate.Height);
            g.Restore(state);
        }
        //Draw the handles for the ellipse gate
        public void DrawHandles(Graphics g, Rectangle rect, EllipseGate gate, int handleRadius)
        {
            if (!gate.Selected)
                return;
            GraphicsState state = BeginGateTransform(g, rect, gate);
            using SolidBrush brush = new(Color.Blue);
            g.FillEllipse(brush, gate.Width / 2f - handleRadius, -handleRadius, handleRadius * 2, handleRadius * 2);
            g.FillEllipse(brush, -handleRadius, -gate.Height / 2f - handleRadius, handleRadius * 2, handleRadius * 2);
            g.Restore(state);
        }
        //draw the rotation handle for the ellipse gate
        public void DrawRotationHandle(Graphics g, Rectangle rect, EllipseGate gate, int handleRadius, int rotationHandleOffset)
        {
            if (!gate.Selected)
                return;
            GraphicsState state = BeginGateTransform(g, rect, gate);
            float y = -gate.Height / 2f - rotationHandleOffset;
            using Pen pen = new(Color.Blue);
            g.DrawLine(pen, 0, -gate.Height / 2f, 0, y);
            using SolidBrush brush = new(Color.Blue);
            g.FillEllipse(brush, -handleRadius, y - handleRadius, handleRadius * 2, handleRadius * 2);
            g.Restore(state);
        }
        //Draw a sample point and its coordinates
        public void DrawSample(Graphics g, Rectangle rect, EllipseGate gate, Sample sample, Font font)
        {
            PointF center = GetScreenCenter(rect, gate);
            float x = rect.Width / 2f + sample.X;
            float y = rect.Height / 2f - sample.Y;
            g.FillEllipse(sample.Accepted ? Brushes.Green : Brushes.Red, x - 3, y - 3, 6, 6);
        }
        //draw the status of the sample (ACCEPT or REJECT) in the top left corner of the client rectangle
        public void DrawStatus(Graphics g, IReadOnlyList<Sample> samples, Font font)
        {
            if (samples == null || samples.Count == 0)
                return;
            int accepted = 0;
            int rejected = 0;
            foreach (var sample in samples)
            {
                if (!sample.Generated)
                    continue;
                if (sample.Accepted)
                    accepted++;
                else
                    rejected++;
            }
            using (Font statusFont = new Font(font.FontFamily, 8, FontStyle.Bold))
            {
                g.DrawString($"Accepted : {accepted}", statusFont, Brushes.Green, 10, 10);
                g.DrawString($"Rejected : {rejected}", statusFont, Brushes.Red, 10, 35);
            }
        }
    }
}
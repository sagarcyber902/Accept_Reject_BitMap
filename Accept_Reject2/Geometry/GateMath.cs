using Accept_Reject2.Model;
using Accept_Reject2.Model;
using System.Drawing.Drawing2D;
namespace Accept_Reject2.Geometry
{
    public static class GateMath
    {
        private static PointF ToLocalPoint(Rectangle rect, EllipseGate gate, PointF point)
        {
            float originX = rect.Width / 2f;
            float originY = rect.Height / 2f;
            PointF[] pts = { point };
            using (Matrix m = new Matrix())
            {
                m.Translate(originX + gate.CenterX, originY - gate.CenterY);
                m.Rotate(gate.Angle);
                m.Invert();
                m.TransformPoints(pts);
            }
            return pts[0];
        }
        // CalculateMovementLimits calculates the maximum movement limits for an ellipse gate within a given client rectangle. It takes into account the width, height, angle, and margin of the gate, and outputs the maximum X and Y coordinates that the gate can move to without exceeding the bounds of the client rectangle.
        public static void CalculateMovementLimits(Rectangle clientRect, int width, int height, float angle, float margin, out float maxX, out float maxY)
        {
            float originX = clientRect.Width / 2f;
            float originY = clientRect.Height / 2f;
            float a = width / 2f;
            float b = height / 2f;
            double rad = angle * Math.PI / 180.0;
            float rotatedHalfWidth = (float)(Math.Abs(a * Math.Cos(rad)) + Math.Abs(b * Math.Sin(rad)));
            float rotatedHalfHeight = (float)(Math.Abs(a * Math.Sin(rad)) + Math.Abs(b * Math.Cos(rad)));
            maxX = originX - margin - rotatedHalfWidth;
            maxY = originY - margin - rotatedHalfHeight;
        }
        // InsideGate checks if a point (sampleX, sampleY) is inside the ellipse gate defined by its width, height, angle, and center position. The point is first transformed into the local coordinate system of the ellipse gate, and then the standard ellipse equation is used to determine if the point lies within the ellipse.
        public static bool IsInsideGate(Rectangle rect, EllipseGate gate, float sampleX, float sampleY)
        {
            float originX = rect.Width / 2f;
            float originY = rect.Height / 2f;
            PointF p = ToLocalPoint(rect, gate, new PointF(originX + sampleX, originY - sampleY));
            float a = gate.Width / 2f;
            float b = gate.Height / 2f;
            return (p.X * p.X) / (a * a) + (p.Y * p.Y) / (b * b) < 1f;
        }
        // IsWidthHandle check
        public static bool IsWidthHandle(Rectangle rect, EllipseGate gate, Point mouse, int handleRadius)
        {
            PointF p = ToLocalPoint(rect, gate, mouse);
            float left = -gate.Width / 2f;
            float right = gate.Width / 2f;
            return (Math.Abs(p.X - left) <= handleRadius && Math.Abs(p.Y) <= handleRadius) || (Math.Abs(p.X - right) <= handleRadius && Math.Abs(p.Y) <= handleRadius);
        }
        // IsHeightHandle checks
        public static bool IsHeightHandle(Rectangle rect, EllipseGate gate, Point mouse, int handleRadius)
        {
            PointF p = ToLocalPoint(rect, gate, mouse);
            float top = -gate.Height / 2f;
            float bottom = gate.Height / 2f;
            return (Math.Abs(p.Y - top) <= handleRadius && Math.Abs(p.X) <= handleRadius) || (Math.Abs(p.Y - bottom) <= handleRadius && Math.Abs(p.X) <= handleRadius);
        }
        // IsRotationHandle checks 
        public static bool IsRotationHandle(Rectangle rect, EllipseGate gate, Point mouse, int handleRadius, int rotationHandleOffset)
        {
            PointF p = ToLocalPoint(rect, gate, mouse);
            float handleY = -gate.Height / 2f - rotationHandleOffset;
            return Math.Abs(p.X) <= handleRadius && Math.Abs(p.Y - handleY) <= handleRadius;
        }
    }
}
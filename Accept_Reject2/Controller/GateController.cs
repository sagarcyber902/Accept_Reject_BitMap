using Accept_Reject2.Geometry;
using Accept_Reject2.Helper;
using Accept_Reject2.Model;
using Accept_Reject2.Renderer;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace Accept_Reject2.Controllers
{
    public class GateController
    {
        private readonly EllipseGate gate;
        private readonly PictureBox pictureBox;
        private readonly GateRenderer renderer;
        private readonly BitmapGate bitmapGate = new BitmapGate();
        private readonly Random random = new Random();
        private bool isDragging = false;
        private bool isResizingWidth = false;
        private bool isResizingHeight = false;
        private bool isRotating = false;
        public event Action? GateChanged;
        public event Action? SampleChanged;
        private readonly int HandleRadius;
        private readonly int RotationHandleOffset;
        private bool editMode = false;
        private readonly Sample sample;
        private readonly List<Sample> samples = new();
        public IReadOnlyList<Sample> Samples => samples;

        private Point lastMouse;

        public GateController(EllipseGate gate, Sample sample, PictureBox pictureBox, GateRenderer renderer, int handleRadius, int rotationHandleOffset)
        {
            this.gate = gate;
            this.sample = sample;
            this.pictureBox = pictureBox;
            this.renderer = renderer;
            bitmapGate.Build(pictureBox.ClientRectangle, gate);
            HandleRadius = handleRadius;
            RotationHandleOffset = rotationHandleOffset;
        }
        //property to get or set the edit mode
        public bool EditMode
        {
            get => editMode;
            set => editMode = value;
        }
        private bool CanFit(int width, int height, float centerX, float centerY)
        {
            const float margin = 1;
            GateMath.CalculateMovementLimits(pictureBox.ClientRectangle, width, height, gate.Angle, margin, out float maxX, out float maxY);
            return Math.Abs(centerX) <= maxX && Math.Abs(centerY) <= maxY;
        }
        private static float Clamp(float value, float min, float max)
        {
            return Math.Min(Math.Max(value, min), max);
        }
        private void ResetMouseState()
        {
            isDragging = false;
            isResizingWidth = false;
            isResizingHeight = false;
            isRotating = false;
        }
        //generate a random sample
        public void GenerateRandomSamples(int count)
        {
            samples.Clear();
            for (int i = 0; i < count; i++)
            {
                float x = random.Next(-100, 101);
                float y = random.Next(-100, 101);
                Sample s = new Sample
                {
                    X = x,
                    Y = y,
                    Accepted = bitmapGate.IsInside(
    pictureBox.ClientRectangle,
    x,
    y),
                    Generated = true
                };
                samples.Add(s);
            }
            SampleChanged?.Invoke();
        }
        //handle mouse up event
        public void MouseUp()
        {
            if (!editMode)
                return;

            ResetMouseState();
        }
        //handle mouse down event
        public void MouseDown(MouseEventArgs e)
        {
            if (!editMode)
                return;
            float originX = pictureBox.Width / 2f;
            float originY = pictureBox.Height / 2f;

            // Mouse position in Cartesian coordinates
            float x = e.X - originX;
            float y = originY - e.Y;
            if (gate.Selected && GateMath.IsWidthHandle(pictureBox.ClientRectangle, gate, e.Location, HandleRadius))
            {
                isResizingWidth = true;
                lastMouse = e.Location;
                return;
            }
            if (gate.Selected && GateMath.IsHeightHandle(pictureBox.ClientRectangle, gate, e.Location, HandleRadius))
            {
                isResizingHeight = true;
                lastMouse = e.Location;
                return;
            }
            if (gate.Selected && GateMath.IsRotationHandle(pictureBox.ClientRectangle, gate, e.Location, HandleRadius, RotationHandleOffset))
            {
                isRotating = true;
                lastMouse = e.Location;
                return;
            }
            float mouseX = e.X - originX;
            float mouseY = originY - e.Y;

            if (bitmapGate.IsInside(pictureBox.ClientRectangle, mouseX, mouseY))
            {
                gate.Selected = true;
                isDragging = true;
                lastMouse = e.Location;

                pictureBox.Invalidate();
                return;
            }
            // Clicked outside
            gate.Selected = false;
            pictureBox.Invalidate();
        }
        //handle mouse move event
        public void MouseMove(MouseEventArgs e)
        {
            if (!editMode)
                return;
            if (isResizingWidth)
            {
                float dz = e.X - lastMouse.X;
                int newWidth = Math.Max(20, gate.Width + (int)dz);
                if (CanFit(newWidth, gate.Height, gate.CenterX, gate.CenterY))
                {
                    gate.Width = newWidth;
                    bitmapGate.Build(pictureBox.ClientRectangle, gate);
                    GateChanged?.Invoke();
                }
                lastMouse = e.Location;
                pictureBox.Invalidate();
                return;
            }
            if (isResizingHeight)
            {
                float dz = lastMouse.Y - e.Y;
                int newHeight = Math.Max(20, gate.Height + (int)dz);
                if (CanFit(gate.Width, newHeight, gate.CenterX, gate.CenterY))
                {
                    gate.Height = newHeight;
                    bitmapGate.Build(pictureBox.ClientRectangle, gate);
                    GateChanged?.Invoke();
                }
                lastMouse = e.Location;
                pictureBox.Invalidate();
                return;
            }
            if (isRotating)
            {
                float originx = pictureBox.Width / 2f + gate.CenterX;
                float originy = pictureBox.Height / 2f - gate.CenterY;
                float db = e.X - originx;
                float dc = e.Y - originy;
                gate.Angle = (float)(Math.Atan2(db, dc) * 180.0 / Math.PI) + 90f;
                bitmapGate.Build(pictureBox.ClientRectangle, gate);
                if (gate.Angle < 0)
                    gate.Angle += 360;
                GateChanged?.Invoke();
                pictureBox.Invalidate();
                return;
            }
            if (!isDragging)
                return;
            float dx = e.X - lastMouse.X;
            float dy = e.Y - lastMouse.Y;
            float newCenterX = gate.CenterX + dx;
            float newCenterY = gate.CenterY - dy;
            GateMath.CalculateMovementLimits(pictureBox.ClientRectangle, gate.Width, gate.Height, gate.Angle, 1, out float maxX, out float maxY);
            gate.CenterX = Clamp(newCenterX, -maxX, maxX);
            gate.CenterY = Clamp(newCenterY, -maxY, maxY);
            bitmapGate.Build(pictureBox.ClientRectangle, gate);
            GateChanged?.Invoke();
            lastMouse = e.Location;
        }
        //apply the gate settings
        public void ApplyGate(int width, int height, float angle, float centerX, float centerY)
        {
            gate.Width = width;
            gate.Height = height;
            gate.Angle = angle;
            gate.CenterX = centerX;
            gate.CenterY = centerY;
            bitmapGate.Build(pictureBox.ClientRectangle, gate);
            gate.Defined = true;
            sample.Generated = false;
            GateChanged?.Invoke();
        }
        //validate the gate settings
        public bool ValidateGate(int width, int height, float angle, float centerX, float centerY, out string error)
        {
            error = "";
            if (!ValidationHelper.ValidateWidth(width))
            {
                error = "Width must be between 20 and 350.";
                return false;
            }
            if (!ValidationHelper.ValidateHeight(height))
            {
                error = "Height must be between 20 and 350.";
                return false;
            }
            if (!ValidationHelper.ValidateAngle(angle))
            {
                error = "Angle must be between -360° and 360°.";
                return false;
            }
            float margin = 1;
            GateMath.CalculateMovementLimits(pictureBox.ClientRectangle, width, height, angle, margin, out float maxX, out float maxY);
            if (!ValidationHelper.ValidateCenter(centerX, centerY, maxX, maxY))
            {
                if (centerX < -maxX || centerX > maxX)
                {
                    int displayMaxX = (int)Math.Ceiling(maxX);
                    error = $"Center X must be between {-displayMaxX} and {displayMaxX}.";
                }
                else
                {
                    int displayMaxY = (int)Math.Ceiling(maxY);
                    error = $"Center Y must be between {-displayMaxY} and {displayMaxY}.";
                }
                return false;
            }
            return true;
        }
    }
}
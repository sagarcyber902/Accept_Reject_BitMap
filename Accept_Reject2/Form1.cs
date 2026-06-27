using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace Accept_Reject2
{
    public partial class Form1 : Form
    {
        private bool gateSelected = false;
        private bool isResizingHeight = false;
        private int ellipseWidth = 100;
        private int ellipseHeight = 130;

        private float ellipseAngle = 30;

        private float gateX = 0;
        private float gateY = 0;

        private bool gateDefined = false;
        // Incoming sample
        private float sampleX = 0;
        private float sampleY = 0;

        // Accept/Reject status
        private bool sampleAccepted = false;
        private bool sampleGenerated = false;

        // Random generator (for testing)
        private Random random = new Random();
        // Mouse drag
        private bool isDragging = false;
        private bool isResizingWidth = false;
        private bool isRotating = false;

        // Distance of rotation handle from ellipse
        private const int RotationHandleOffset = 25;

        private const int HandleRadius = 3;

        // Last mouse position
        private Point lastMouse;
        public Form1()
        {
            InitializeComponent();

            pictureBox1.Paint += PictureBox1_Paint;

            pictureBox1.MouseDown += PictureBox1_MouseDown;
            pictureBox1.MouseMove += PictureBox1_MouseMove;
            pictureBox1.MouseUp += PictureBox1_MouseUp;

            // Default gate exists immediately
            gateDefined = true;

            widthText.Text = ellipseWidth.ToString();
            heightText.Text = ellipseHeight.ToString();
            AngleText.Text = ellipseAngle.ToString();

            centerXText.Text = gateX.ToString();
            centerYText.Text = gateY.ToString();
        }
        private void PictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            

            float originX = pictureBox1.Width / 2f;
            float originY = pictureBox1.Height / 2f;

            // Mouse position in Cartesian coordinates
            float x = e.X - originX;
            float y = originY - e.Y;
            if (gateSelected && IsWidthHandle(e.Location))
            {
                isResizingWidth = true;
                lastMouse = e.Location;
                return;
            }
            if (gateSelected && IsHeightHandle(e.Location))
            {
                isResizingHeight = true;
                lastMouse = e.Location;
                return;
            }
            if (gateSelected && IsRotationHandle(e.Location))
            {
                isRotating = true;
                lastMouse = e.Location;
                return;
            }
            if (IsInsideGate(x, y))
            {
                gateSelected = true;

                isDragging = true;
                lastMouse = e.Location;

                pictureBox1.Invalidate();

                return;
            }

            // Clicked outside
            gateSelected = false;
            pictureBox1.Invalidate();
        }
        private bool IsRotationHandle(Point mouse)
        {
            float originX = pictureBox1.Width / 2f;
            float originY = pictureBox1.Height / 2f;

            PointF[] pt =
            {
        new PointF(mouse.X, mouse.Y)
    };

            using (Matrix m = new Matrix())
            {
                m.Translate(originX + gateX,
                            originY - gateY);

                m.Rotate(ellipseAngle);

                m.Invert();

                m.TransformPoints(pt);
            }

            float x = pt[0].X;
            float y = pt[0].Y;

            float handleY = -ellipseHeight / 2f - RotationHandleOffset;

            return
                Math.Abs(x) <= 8 &&
                Math.Abs(y - handleY) <= 8;
        }
        private void PictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            if (isResizingWidth)
            {
                float dz = e.X - lastMouse.X;

                ellipseWidth += (int)dz;

                if (ellipseWidth < 20)
                    ellipseWidth = 20;

                widthText.Text = ellipseWidth.ToString();

                lastMouse = e.Location;

                pictureBox1.Invalidate();

                return;
            }
            if (isResizingHeight)
            {
                float da = lastMouse.Y - e.Y;

                ellipseHeight += (int)da;

                if (ellipseHeight < 20)
                    ellipseHeight = 20;

                heightText.Text = ellipseHeight.ToString();

                lastMouse = e.Location;

                pictureBox1.Invalidate();

                return;
            }
            if (isRotating)
            {
                float originx = pictureBox1.Width / 2f + gateX;
                float originy = pictureBox1.Height / 2f - gateY;

                float db = e.X - originx;
                float dc = e.Y - originy;

                ellipseAngle =
                    (float)(Math.Atan2(db, dc) * 180.0 / Math.PI) + 90f;

                if (ellipseAngle < 0)
                    ellipseAngle += 360;

                AngleText.Text =
                    ((int)Math.Round(ellipseAngle)).ToString();

                pictureBox1.Invalidate();

                return;
            }
            if (!isDragging)
                return;

            float dx = e.X - lastMouse.X;
            float dy = e.Y - lastMouse.Y;

            // Screen → Cartesian
            float newGateX = gateX + dx;
            float newGateY = gateY - dy;
            float originX = pictureBox1.Width / 2f;
            float originY = pictureBox1.Height / 2f;

            float margin = 1;

            float a = ellipseWidth / 2f;
            float b = ellipseHeight / 2f;

            double rad = ellipseAngle * Math.PI / 180.0;

            // Rotated ellipse bounding box
            float rotatedHalfWidth =
                (float)(Math.Abs(a * Math.Cos(rad)) +
                        Math.Abs(b * Math.Sin(rad)));

            float rotatedHalfHeight =
                (float)(Math.Abs(a * Math.Sin(rad)) +
                        Math.Abs(b * Math.Cos(rad)));

            float maxX = originX - margin - rotatedHalfWidth;
            float maxY = originY - margin - rotatedHalfHeight;

            // Clamp X
            if (newGateX > maxX)
                newGateX = maxX;

            if (newGateX < -maxX)
                newGateX = -maxX;

            // Clamp Y
            if (newGateY > maxY)
                newGateY = maxY;

            if (newGateY < -maxY)
                newGateY = -maxY;

            gateX = newGateX;
            gateY = newGateY;
            centerXText.Text = Math.Round(gateX).ToString();
            centerYText.Text = Math.Round(gateY).ToString();

            lastMouse = e.Location;

            pictureBox1.Invalidate();
        }
        private void PictureBox1_MouseUp(object sender, MouseEventArgs e)
        {
            isDragging = false;
            isResizingWidth = false;
            isResizingHeight = false;
            isRotating = false;
        }
        private void btnSample_Click(object sender, EventArgs e)
        {
            if (!gateDefined)
            {
                MessageBox.Show("Please define the gate first.");
                return;
            }

            float x = random.Next(-100, 101);
            float y = random.Next(-100, 101);

            UpdateSample(x, y);
        }
        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (!int.TryParse(widthText.Text, out int width))
            {
                MessageBox.Show("Invalid Width");
                return;
            }

            if (!int.TryParse(heightText.Text, out int height))
            {
                MessageBox.Show("Invalid Height");
                return;
            }

            if (!float.TryParse(AngleText.Text, out float angle))
            {
                MessageBox.Show("Invalid Angle");
                return;
            }

            if (!float.TryParse(centerXText.Text, out float x))
            {
                MessageBox.Show("Invalid Center X");
                return;
            }

            if (!float.TryParse(centerYText.Text, out float y))
            {
                MessageBox.Show("Invalid Center Y");
                return;
            }

            //---------------------------------
            // Validation
            //---------------------------------

            if (width < 20 || width > 300)
            {
                MessageBox.Show("Width must be between 20 and 300.");
                return;
            }

            if (height < 20 || height > 300)
            {
                MessageBox.Show("Height must be between 20 and 300.");
                return;
            }
            float originX = pictureBox1.Width / 2f;
            float originY = pictureBox1.Height / 2f;

            float margin = 1;

            float a = width / 2f;
            float b = height / 2f;

            double rad = angle * Math.PI / 180.0;

            // Bounding box after rotation
            float rotatedHalfWidth =
                (float)(Math.Abs(a * Math.Cos(rad)) +
                        Math.Abs(b * Math.Sin(rad)));

            float rotatedHalfHeight =
                (float)(Math.Abs(a * Math.Sin(rad)) +
                        Math.Abs(b * Math.Cos(rad)));

            float maxX = originX - margin - rotatedHalfWidth;
            float maxY = originY - margin - rotatedHalfHeight;

            if (x < -maxX || x > maxX)
            {
                MessageBox.Show(
                    $"Center X must be between {-maxX:0} and {maxX:0}.",
                    "Invalid Center X",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                return;
            }

            if (y < -maxY || y > maxY)
            {
                MessageBox.Show(
                    $"Center Y must be between {-maxY:0} and {maxY:0}.",
                    "Invalid Center Y",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                return;
            }

            //---------------------------------
            // Save
            //---------------------------------

            ellipseWidth = width;
            ellipseHeight = height;

            ellipseAngle = angle;

            gateX = x;
            gateY = y;
            gateDefined = true;

            //---------------------------------
            // Redraw
            //---------------------------------
            sampleGenerated = false;
            pictureBox1.Invalidate();
        }
        private bool IsInsideGate(float x, float y)
        {
            float originX = pictureBox1.Width / 2f;
            float originY = pictureBox1.Height / 2f;

            PointF[] pts =
            {
        new PointF(originX + x, originY - y)
    };

            using (Matrix m = new Matrix())
            {
                m.Translate(originX + gateX,
                            originY - gateY);

                m.Rotate(ellipseAngle);

                m.Invert();

                m.TransformPoints(pts);
            }

            // NOW convert to ellipse-local coordinates
            float localX = pts[0].X;
            float localY = pts[0].Y;

            

            float a = ellipseWidth / 2f;
            float b = ellipseHeight / 2f;

            return (localX * localX) / (a * a) +
                   (localY * localY) / (b * b) <= 1.0;
        }
        private void UpdateSample(float x, float y)
        {
            sampleX = x;
            sampleY = y;

            sampleAccepted = IsInsideGate(sampleX, sampleY);
            sampleGenerated = true;
            pictureBox1.Invalidate();
        }
        private bool IsWidthHandle(Point mouse)
        {
            float originX = pictureBox1.Width / 2f;
            float originY = pictureBox1.Height / 2f;

            PointF[] pt =
            {
        new PointF(mouse.X, mouse.Y)
    };

            using (Matrix m = new Matrix())
            {
                m.Translate(originX + gateX,
                            originY - gateY);

                m.Rotate(ellipseAngle);

                m.Invert();

                m.TransformPoints(pt);
            }

            float x = pt[0].X;
            float y = pt[0].Y;

            float leftX = -ellipseWidth / 2f;
            float rightX = ellipseWidth / 2f;

            return
                Math.Abs(x - leftX) <= 10 &&
                Math.Abs(y) <= 10 ||

                Math.Abs(x - rightX) <= 10 &&
                Math.Abs(y) <= 10;
        }
        private bool IsHeightHandle(Point mouse)
        {
            float originX = pictureBox1.Width / 2f;
            float originY = pictureBox1.Height / 2f;

            PointF[] pt =
            {
        new PointF(mouse.X, mouse.Y)
    };

            using (Matrix m = new Matrix())
            {
                m.Translate(originX + gateX,
                            originY - gateY);

                m.Rotate(ellipseAngle);

                m.Invert();

                m.TransformPoints(pt);
            }

            float x = pt[0].X;
            float y = pt[0].Y;

            float topY = -ellipseHeight / 2f;
            float bottomY = ellipseHeight / 2f;

            return
                (Math.Abs(y - topY) <= 10 && Math.Abs(x) <= 10) ||

                (Math.Abs(y - bottomY) <= 10 && Math.Abs(x) <= 10);
        }
        private void PictureBox1_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;

            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.Clear(Color.White);
            //-------------------------------------
            // Origin (0,0)
            //-------------------------------------

            float originX = pictureBox1.Width / 2f;
            float originY = pictureBox1.Height / 2f;


            //-------------------------------------
            // Draw Axes
            //-------------------------------------

            using (Pen axisPen = new Pen(Color.Gray, 0.1f))
            {
                g.DrawLine(axisPen,
                    0, originY,
                    pictureBox1.Width, originY);

                g.DrawLine(axisPen,
                    originX, 0,
                    originX, pictureBox1.Height);
            }



            //-------------------------------------
            // Convert Cartesian to Screen
            //-------------------------------------

            float screenX = originX + gateX;
            float screenY = originY - gateY;

            //-------------------------------------
            // Draw Center Point
            //-------------------------------------

            g.FillEllipse(
                Brushes.Red,
                screenX - 2,
                screenY - 2,
                4,
                4);

            //-------------------------------------
            // Draw Ellipse
            //-------------------------------------

            GraphicsState state = g.Save();

            g.TranslateTransform(screenX, screenY);

            g.RotateTransform(ellipseAngle);

            using (Pen pen = new Pen(
    gateSelected ? Color.DodgerBlue : Color.Black,
    gateSelected ? 2.5f : 2f))
            {
                g.DrawEllipse(
                    pen,
                    -ellipseWidth / 2f,
                    -ellipseHeight / 2f,
                    ellipseWidth,
                    ellipseHeight);
            }

            g.Restore(state);
            //-------------------------------------
            // Width Handles
            //-------------------------------------

            if (gateSelected)
            {
                GraphicsState handleState = g.Save();

                g.TranslateTransform(screenX, screenY);
                g.RotateTransform(ellipseAngle);

                using (Brush handleBrush = new SolidBrush(Color.DodgerBlue))
                using (Pen handleBorder = new Pen(Color.Navy, 1))
                {
                    RectangleF leftHandle = new RectangleF(
                        -ellipseWidth / 2f - HandleRadius,
                        -HandleRadius,
                        HandleRadius * 2,
                        HandleRadius * 2);

                    RectangleF rightHandle = new RectangleF(
                        ellipseWidth / 2f - HandleRadius,
                        -HandleRadius,
                        HandleRadius * 2,
                        HandleRadius * 2);
                    // Top Handle
                    RectangleF topHandle = new RectangleF(
                        -HandleRadius,
                        -ellipseHeight / 2f - HandleRadius,
                        HandleRadius * 2,
                        HandleRadius * 2);

                    // Bottom Handle
                    RectangleF bottomHandle = new RectangleF(
                        -HandleRadius,
                        ellipseHeight / 2f - HandleRadius,
                        HandleRadius * 2,
                        HandleRadius * 2);
                    // Rotation line
                    g.DrawLine(
                        Pens.Gray,
                        0,
                        -ellipseHeight / 2f,
                        0,
                        -ellipseHeight / 2f - RotationHandleOffset);

                    // Rotation Handle
                    RectangleF rotateHandle = new RectangleF(
                        -HandleRadius,
                        -ellipseHeight / 2f - RotationHandleOffset - HandleRadius,
                        HandleRadius * 2,
                        HandleRadius * 2);

                    g.FillEllipse(Brushes.Orange, rotateHandle);
                    g.DrawEllipse(Pens.DarkOrange, rotateHandle);

                    g.FillEllipse(handleBrush, topHandle);
                    g.DrawEllipse(handleBorder, topHandle);

                    g.FillEllipse(handleBrush, bottomHandle);
                    g.DrawEllipse(handleBorder, bottomHandle);

                    g.FillEllipse(handleBrush, leftHandle);
                    g.DrawEllipse(handleBorder, leftHandle);

                    g.FillEllipse(handleBrush, rightHandle);
                    g.DrawEllipse(handleBorder, rightHandle);
                }

                g.Restore(handleState);
            }
            //-------------------------------------
            // Draw Current Sample
            //-------------------------------------

            float sampleScreenX = originX + sampleX;
            float sampleScreenY = originY - sampleY;

            Brush brush = sampleAccepted ? Brushes.Green : Brushes.Red;

            g.FillEllipse(
                brush,
                sampleScreenX - 4,
                sampleScreenY - 4,
                8,
                8);

            g.DrawString(
                $"({sampleX}, {sampleY})",
                Font,
                Brushes.DarkBlue,
                sampleScreenX + 8,
                sampleScreenY + 8);
            //-------------------------------------
            // Display Coordinates
            //-------------------------------------

            g.DrawString(
    $"({(int)Math.Round(gateX)}, {(int)Math.Round(gateY)})",
    Font,
    Brushes.Blue,
    screenX + 8,
    screenY + 8);
            if (sampleGenerated)
            {
                g.DrawString(
                    sampleAccepted ? "ACCEPT" : "REJECT",
                    new Font(Font.FontFamily, 12, FontStyle.Bold),
                    sampleAccepted ? Brushes.Green : Brushes.Red,
                    10,
                    10);
            }
        }

        }
}
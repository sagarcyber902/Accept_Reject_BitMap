using Accept_Reject2.Controllers;
using Accept_Reject2.Model;
using Accept_Reject2.Renderer;
namespace Accept_Reject2
{
    public partial class Form1 : Form
    {
        private EllipseGate gate = new EllipseGate();
        private Sample sample = new Sample();
        private GateRenderer renderer = new GateRenderer();
        private GateController controller;
        private const int HandleRadius = 4;
        private const int RotationHandleOffset = 25;
        int sampleCount = 10;

        public Form1()
        {
            InitializeComponent();
            controller = new GateController(gate, sample, pictureBox1, renderer, HandleRadius, RotationHandleOffset);
            controller.GateChanged += Controller_GateChanged;
            controller.SampleChanged += Controller_SampleChanged;

            pictureBox1.Paint += PictureBox1_Paint;
            pictureBox1.MouseDown += PictureBox1_MouseDown;
            pictureBox1.MouseMove += PictureBox1_MouseMove;
            pictureBox1.MouseUp += PictureBox1_MouseUp;
            // Default gate exists immediately
            gate.Defined = true;
            widthText.Text = gate.Width.ToString();
            heightText.Text = gate.Height.ToString();
            AngleText.Text = gate.Angle.ToString();
            centerXText.Text = gate.CenterX.ToString();
            centerYText.Text = gate.CenterY.ToString();
        }
        private void Controller_GateChanged()
        {
            widthText.Text = gate.Width.ToString();
            heightText.Text = gate.Height.ToString();
            AngleText.Text = ((int)Math.Round(gate.Angle)).ToString();
            centerXText.Text = ((int)Math.Round(gate.CenterX)).ToString();
            centerYText.Text = ((int)Math.Round(gate.CenterY)).ToString();
            pictureBox1.Invalidate();
        }
        private void Controller_SampleChanged()
        {
            pictureBox1.Invalidate();
        }
        private void PictureBox1_MouseDown(object sender, MouseEventArgs e)
        {

            controller.MouseDown(e);
        }
        private void PictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            controller.MouseMove(e);
        }
        private void PictureBox1_MouseUp(object sender, MouseEventArgs e)
        {
            controller.MouseUp();
        }
        private void btnSample_Click(object sender, EventArgs e)
        {
            if (!gate.Defined)
            {
                MessageBox.Show("Please define the gate first.");
                return;
            }
            controller.GenerateRandomSamples(sampleCount);
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
            if (!controller.ValidateGate(width, height, angle, x, y, out string error))
            {
                MessageBox.Show(error);
                return;
            }
            controller.ApplyGate(width, height, angle, x, y);
        }
        private void btnEditGate_Click(object sender, EventArgs e)
        {
            controller.EditMode = !controller.EditMode;
            btnEditGate.Text = controller.EditMode ? "Disable Edit" : "Enable Edit";
            btnSample.Enabled = !controller.EditMode;
            pictureBox1.Invalidate();
        }
        private void PictureBox1_Paint(object sender, PaintEventArgs e)
        {
            renderer.Draw(e.Graphics, pictureBox1.ClientRectangle, gate, controller.Samples, Font, HandleRadius, RotationHandleOffset, controller.EditMode);
        }


    }
}
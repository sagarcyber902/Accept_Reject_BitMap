using Font=System.Drawing.Font;
using System.Xml.Linq;
using static System.Net.Mime.MediaTypeNames;
namespace Accept_Reject2
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            pictureBox1 = new PictureBox();
            label1 = new Label();
            label2 = new Label();
            label3 = new Label();
            heightText = new TextBox();
            widthText = new TextBox();
            AngleText = new TextBox();
            label4 = new Label();
            centerXText = new TextBox();
            centerYText = new TextBox();
            label5 = new Label();
            btnUpdate = new Button();
            btnSample = new Button();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            SuspendLayout();
            // 
            // pictureBox1
            // 
            pictureBox1.Location = new Point(12, 12);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(350, 350);
            pictureBox1.TabIndex = 0;
            pictureBox1.TabStop = false;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            label1.Location = new Point(550, 34);
            label1.Name = "label1";
            label1.Size = new Size(45, 15);
            label1.TabIndex = 1;
            label1.Text = "Height";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            label2.Location = new Point(550, 69);
            label2.Name = "label2";
            label2.Size = new Size(41, 15);
            label2.TabIndex = 2;
            label2.Text = "Width";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            label3.Location = new Point(552, 105);
            label3.Name = "label3";
            label3.Size = new Size(39, 15);
            label3.TabIndex = 3;
            label3.Text = "Angle";
            // 
            // heightText
            // 
            heightText.Location = new Point(617, 31);
            heightText.Name = "heightText";
            heightText.Size = new Size(100, 23);
            heightText.TabIndex = 4;
            // 
            // widthText
            // 
            widthText.Location = new Point(617, 66);
            widthText.Name = "widthText";
            widthText.Size = new Size(100, 23);
            widthText.TabIndex = 5;
            // 
            // AngleText
            // 
            AngleText.Location = new Point(617, 102);
            AngleText.Name = "AngleText";
            AngleText.Size = new Size(100, 23);
            AngleText.TabIndex = 6;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            label4.Location = new Point(550, 141);
            label4.Name = "label4";
            label4.Size = new Size(53, 15);
            label4.TabIndex = 7;
            label4.Text = "CenterX";
            // 
            // centerXText
            // 
            centerXText.Location = new Point(617, 138);
            centerXText.Name = "centerXText";
            centerXText.Size = new Size(100, 23);
            centerXText.TabIndex = 8;
            // 
            // centerYText
            // 
            centerYText.Location = new Point(617, 176);
            centerYText.Name = "centerYText";
            centerYText.Size = new Size(100, 23);
            centerYText.TabIndex = 10;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            label5.Location = new Point(550, 179);
            label5.Name = "label5";
            label5.Size = new Size(52, 15);
            label5.TabIndex = 9;
            label5.Text = "CenterY";
            // 
            // btnUpdate
            // 
            btnUpdate.Location = new Point(679, 226);
            btnUpdate.Name = "btnUpdate";
            btnUpdate.Size = new Size(75, 23);
            btnUpdate.TabIndex = 11;
            btnUpdate.Text = "Update";
            btnUpdate.UseVisualStyleBackColor = true;
            btnUpdate.Click += btnUpdate_Click;
            // 
            // btnSample
            // 
            btnSample.Location = new Point(583, 227);
            btnSample.Name = "btnSample";
            btnSample.Size = new Size(75, 23);
            btnSample.TabIndex = 12;
            btnSample.Text = "Generate";
            btnSample.UseVisualStyleBackColor = true;
            btnSample.Click += btnSample_Click;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(btnSample);
            Controls.Add(btnUpdate);
            Controls.Add(centerYText);
            Controls.Add(label5);
            Controls.Add(centerXText);
            Controls.Add(label4);
            Controls.Add(AngleText);
            Controls.Add(widthText);
            Controls.Add(heightText);
            Controls.Add(label3);
            Controls.Add(label2);
            Controls.Add(label1);
            Controls.Add(pictureBox1);
            Name = "Form1";
            Text = "Accept_Reject";
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private PictureBox pictureBox1;
        private Label label1;
        private Label label2;
        private Label label3;
        private TextBox heightText;
        private TextBox widthText;
        private TextBox AngleText;
        private Label label4;
        private TextBox centerXText;
        private TextBox centerYText;
        private Label label5;
        private Button btnUpdate;
        private Button btnSample;
    }
}

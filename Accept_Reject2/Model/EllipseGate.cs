using System;
using System.Collections.Generic;
using System.Text;

namespace Accept_Reject2.Model
{
    public class EllipseGate
    {
        public int Width { get; set; } = 100;

        public int Height { get; set; } = 130;

        public float Angle { get; set; } = 30;

        public float CenterX { get; set; } = 0;

        public float CenterY { get; set; } = 0;

        public bool Selected { get; set; }

        public bool Defined { get; set; } = true;
    }
}

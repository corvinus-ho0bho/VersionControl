﻿using ho0bho_week08.Abstractions;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ho0bho_week08.Entities
{
    public class Present : Toy
    {
        public SolidBrush PresentColor { get; private set; }

        public Present(Color color)
        {
            PresentColor = new SolidBrush(color);
        }

        protected override void DrawImage(Graphics g)
        {
            g.FillEllipse(PresentColor, 0, 0, Width, Height);
        }
    }
}


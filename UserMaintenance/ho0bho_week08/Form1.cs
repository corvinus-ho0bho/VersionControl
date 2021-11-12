using ho0bho_week08.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ho0bho_week08
{
    public partial class Form1 : Form
    {
        private List<Ball> _ball = new List<Ball>();
        private BallFactory _factory;
        public BallFactory Factory
        {
            get { return _factory;  }
            set { }
        }
        public Form1()
        {
            InitializeComponent();
            Factory = new BallFactory();
        }
    }
}

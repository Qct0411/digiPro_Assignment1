using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Net.Mime.MediaTypeNames;

namespace Assignment1
{
    public partial class Intermediate : Form
    {
        private Bitmap image;
        public Intermediate(Bitmap interFrame)
        {
            InitializeComponent();
            this.image = interFrame;
        }

        private void Intermediate_Load(object sender, EventArgs e)
        {
            //this.DoubleBuffered = true;
            Paint += new PaintEventHandler(this.intermediate_Paint);
        }

        private void intermediate_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            if (image != null)
            {
                e.Graphics.DrawImage(image, 0, 0, ClientSize.Width, ClientSize.Height);
            }
        }
    }
}

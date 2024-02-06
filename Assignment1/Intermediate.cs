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
        private List<Bitmap> bitmaps = new List<Bitmap>();
        private bool auto = true;
        Thread thread;
        public Intermediate(Bitmap interFrame)
        {
            InitializeComponent();
            this.image = interFrame;
        }

        public Intermediate(List<Bitmap> bitmaps)
        {

            this.bitmaps = bitmaps;
            InitializeComponent();
        }

        private void Intermediate_Load(object sender, EventArgs e)
        {
            //this.DoubleBuffered = true;
            Paint += new PaintEventHandler(this.intermediate_Paint);
            this.DoubleBuffered = true;
        }

        private void intermediate_Paint(object sender, PaintEventArgs e)
        {
            if (image != null)
            {
                e.Graphics.DrawImage(image, 0, 0, ClientSize.Width, ClientSize.Height);
            }
        }

        private void animate()
        {
            int i = 0;
            while (true)
            {
                this.image = bitmaps[i];
                Invalidate();
                System.Threading.Thread.Sleep(100);
                i++;
                if (i == bitmaps.Count)
                {
                    i = 0;
                }
                if (auto == false)
                {
                    break;
                }
            }
        }

        private void autoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            auto = true;
            thread = new Thread(animate);
            thread.Start();
        }

        private void stopToolStripMenuItem_Click(object sender, EventArgs e)
        {
            auto = false;
            thread.Join();
        }

        private void button1_Click(object sender, EventArgs e)
        {

        }
    }
}

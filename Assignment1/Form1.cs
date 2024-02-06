using System.Diagnostics;

namespace Assignment1
{
    public partial class Form1 : Form
    {
        Source src;
        Source dest;
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            src = new Source(true);
            dest = new Source(false);

            src.setDest(dest);
            dest.setSrc(src);

            src.MdiParent = this;
            src.Show();

            dest.MdiParent = this;
            dest.Show();
            dest.Location = new Point(src.Location.X + src.Width, src.Location.Y);
        }

        private void toolStripLabel1_Click(object sender, EventArgs e)
        {

        }

        private void Auto_Click(object sender, EventArgs e)
        {
            List<Line> test = new List<Line>();
            test.Add(new Line(new Point(5, 12), new Point(15, 12)));
            List<Line> test2 = new List<Line>();
            test2.Add(new Line(new Point(10, 10), new Point(20, 20)));
            Morph morph = new Morph(5, src.getImage(), dest.getImage(), src.getLines(), dest.getLines());
            morph.generateInterlopingLines(src.getLines(), dest.getLines(), 5);
            List<Bitmap> forward = morph.warpImageOverFrames(src.getImage(), dest.getImage(), morph.generateInterlopingLines(src.getLines(), dest.getLines(), 5), src.getLines());
            List<Bitmap> backward = morph.warpImageOverFrames(dest.getImage(), src.getImage(), morph.generateInterlopingLines(dest.getLines(), src.getLines(), 5), dest.getLines());
            Debug.WriteLine("Number of inter frames:" + morph.getInterFrames().Count);
            backward.Reverse();
            List<Bitmap> final = morph.crossDissolve(forward, backward);
            /*            for (int i = 0; i < forward.Count; i++)
                        {
                            Intermediate inter = new Intermediate(forward[i]);
                            inter.MdiParent = this;
                            inter.Show();
                            inter.Location = new Point(inter.Location.X + inter.Width*i, src.Location.Y + src.Width);
                        }
                        for (int i = 0; i < backward.Count; i++)
                        {
                            Intermediate inter = new Intermediate(backward[i]);
                            inter.MdiParent = this;
                            inter.Show();
                            inter.Location = new Point(inter.Location.X + inter.Width * i, src.Location.Y + src.Width * 2);
                        }*/
            Intermediate inter = new Intermediate(final);
            inter.MdiParent = this;
            inter.Show();
            inter.Location = new Point(inter.Location.X, src.Location.Y + src.Width);
            
            //morph.printLines();
            //morph.warpImage();
            //Thread.Sleep(1);
            //Debug.WriteLine("Number of inter frames:" + morph.getInterFrames().Count);
            /*            Intermediate inter = new Intermediate(morph.getInterFrames()[0]);
                        inter.MdiParent = this;
                        inter.Show();
                        inter.Location = new Point(src.Location.X, src.Location.Y + src.Width);*/
        }
    }
}

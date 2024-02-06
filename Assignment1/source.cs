using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Assignment1
{
    public partial class Source : Form
    {
        private Bitmap image1;
        private List<Line> lines;
        // Temp point for new line
        private Point tempPoint1, tempPoint2;
        private bool drawing = false;
        private Source dest;
        private Source src;
        private bool isSource;
        private bool isDest;

        private int offSetX;
        private int offSetY;
        private bool isMovingEndPoint = false;
        private int lineIndex = -1;
        

        public Source(bool isSource)
        {
            InitializeComponent();
            lines = new List<Line>();
            if (isSource)
            {
                this.isSource = true;
                this.isDest = false;
            }
            else
            {
                this.isDest = true;
                this.isSource = false;
            }

        }

        private void Source_Load(object sender, EventArgs e)
        {
            this.DoubleBuffered = true;
            Paint += new PaintEventHandler(this.source_Paint);
            MouseDown += new MouseEventHandler(this.source_General_MouseDown);
            //MouseMove += new MouseEventHandler(this.source_DrawingLine_MouseMove_Down);
            MouseMove += new MouseEventHandler(this.source_General_MouseMove_Up);
            //MouseUp += new MouseEventHandler(this.source_DrawingLine_MouseUp);
        }

        private void Source_MouseMove(object? sender, MouseEventArgs e)
        {
            throw new NotImplementedException();
        }

        public void setDest(Source dest)
        {
            if (isSource)
            {
                this.dest = dest;
                this.src = this;
            }
            
            Debug.WriteLine("setDest");
        }

        public void setSrc(Source src)
        {
            if (isDest) {
                this.src = src;
                this.dest = this;
            }
            
            Debug.WriteLine("setSrc");
        }

        public Bitmap getImage()
        {
            return image1;
        }

        public List<Line> getLines()
        {
            return lines;
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog dialog = new OpenFileDialog())
            {
                dialog.Title = "Open File";
                dialog.Filter = "bmp files (*.bmp)|*.bmp";
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    image1 = new Bitmap(dialog.FileName);
                    //this.BackgroundImageLayout = ImageLayout.Stretch;
                    //this.BackgroundImage = image1;
                    
                    //this.BackgroundImageLayout = ImageLayout.Center;
                    Refresh();
                }
            }
        }

        public void source_Paint(object sender, PaintEventArgs e)
        {
            if (image1 != null)
            {
                e.Graphics.DrawImage(image1, 0, 0, ClientSize.Width, ClientSize.Height);
            }
            foreach (Line line in lines)
            {
                line.Draw(e);
            }

            if (drawing)
            {
                e.Graphics.DrawLine(Pens.Red, tempPoint1, tempPoint2);
            }
            
        }

        private bool checkMouseOnLine(Point p)
        {
            for (int i = 0; i < lines.Count; i++)
            {
                if (lines[i].checkStartPoint(p) || lines[i].checkEndPoint(p) || lines[i].checkMidPoint(p))
                {
                    lineIndex = i;
                    return true;
                }
            }
            lineIndex = -1;
            return false;
        }

        private void source_General_MouseMove_Up(object sender, MouseEventArgs e)
        {
            Cursor cursor = Cursors.Cross;
            if (checkMouseOnLine(e.Location))
            {
                if (lines[lineIndex].checkStartPoint(e.Location) || lines[lineIndex].checkEndPoint(e.Location))
                {
                    cursor = Cursors.Hand;
                }
                else if (lines[lineIndex].checkMidPoint(e.Location))
                {
                    cursor = Cursors.Hand;
                }
            }

            if (Cursor != cursor)
            {
                Cursor = cursor;
            }
        }

        private void source_General_MouseDown(object sender, MouseEventArgs e)
        {
            if (checkMouseOnLine(e.Location))
            {
                MouseMove -= source_General_MouseMove_Up;
                MouseMove += source_MovingEndPoint_MouseMove_Down;
                MouseUp += source_MovingEndPoint_MouseUp;
                

            }
            else {
                MouseMove -= source_General_MouseMove_Up;
                MouseMove += source_DrawingLine_MouseMove_Down;
                MouseUp += source_DrawingLine_MouseUp;
                if (e.Button == MouseButtons.Left)
                {
                    drawing = true;
                    tempPoint1 = e.Location;
                    tempPoint2 = e.Location;
                }

            }
        }

        private void source_MovingEndPoint_MouseMove_Down(object sender, MouseEventArgs e)
        {

            if (lines[lineIndex].checkStartPoint(e.Location))
            {
                lines[lineIndex].Start = e.Location;
            }
            else if (lines[lineIndex].checkEndPoint(e.Location))
            {
                lines[lineIndex].End = e.Location;
            }
            else if (lines[lineIndex].checkMidPoint(e.Location))
            {
                lines[lineIndex].moveLineSegment(e.Location);
            }
            this.Invalidate();
            
        }

        private void source_MovingEndPoint_MouseUp(object sender, MouseEventArgs e)
        {
            //isMovingEndPoint = false;
            MouseMove -= source_MovingEndPoint_MouseMove_Down;
            MouseMove += source_General_MouseMove_Up;
            MouseUp -= source_MovingEndPoint_MouseUp;
                

            this.Invalidate();
         
        }

        private void source_DrawingLine_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                drawing = true;
                tempPoint1 = e.Location;
                tempPoint2 = e.Location;
            }
        }

        private void source_DrawingLine_MouseMove_Down(object sender, MouseEventArgs e)
        {
            if (drawing)
            {
                tempPoint2 = e.Location;
                this.Invalidate();
            }
        }

        private void source_DrawingLine_MouseUp(object sender, MouseEventArgs e)
        {
            if (drawing)
            {
                drawing = false;

                MouseMove -= source_DrawingLine_MouseMove_Down;
                MouseMove += source_General_MouseMove_Up;
                MouseUp -= source_DrawingLine_MouseUp;
                

                addLine(tempPoint1, tempPoint2);
                updateOtherFormLines(tempPoint1, tempPoint2);
            }
        }

        public void addLine(Point start, Point end)
        {
            lines.Add(new Line(start, end));
            this.Refresh();
        }

        public void updateOtherFormLines(Point start, Point end)
        {
            if (isSource)
            {
                Debug.WriteLine("updateOtherFormLines dest");
                dest.addLine(start, end);
                
            }
            else
            {
                Debug.WriteLine("updateOtherFormLines source");
                src.addLine(start, end);
            }
        }

        public void checkLine() {
            Debug.WriteLine(lines.Count);
        }
    }
}

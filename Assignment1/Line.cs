using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assignment1
{
    public class Line
    {
        private Point start;
        private Point end;
        private int selectionRadius = 5;
        private Brush endPointBrush = new SolidBrush(Color.Blue);
        private Brush midPointBrush = new SolidBrush(Color.Red);

        public Line(Point start, Point end)
        {
            this.start = start;
            this.end = end;
        }

        public Point Start
        {
            get { return start; }
            set { start = value; }
        }

        public Point End
        {
            get { return end; }
            set { end = value; }
        }

        public bool checkSelection(Point p)
        {
            if (distanceFromStart(p) <= selectionRadius)
            {
                return true;
            }
            else if (distanceFromEnd(p) <= selectionRadius)
            {
                return true;
            }
            else if (distanceFromMidPoint(p) <= selectionRadius)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool checkStartPoint(Point p)
        {
            if (distanceFromStart(p) <= selectionRadius)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool checkEndPoint(Point p)
        {
            if (distanceFromEnd(p) <= selectionRadius)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool checkMidPoint(Point p)
        {
            if (distanceFromMidPoint(p) <= selectionRadius)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public void moveLineSegment(Point p)
        {
            Point midpoint = calculateMidPoint();
            int offsetX = start.X - midpoint.X;
            int offsetY = start.Y - midpoint.Y;

            int newX = p.X + offsetX;
            int newY = p.Y + offsetY;

            int dx = newX - start.X;
            int dy = newY - start.Y;

            if (dx != 0 || dy != 0)
            {
                start = new Point(newX, newY);
                end = new Point(end.X + dx, end.Y + dy);
            }

        }

        public int distanceFromStart(Point p)
        {
            return (int)Math.Sqrt(Math.Pow(p.X - start.X, 2) + Math.Pow(p.Y - start.Y, 2));
        }

        public int distanceFromEnd(Point p)
        {
            return (int)Math.Sqrt(Math.Pow(p.X - end.X, 2) + Math.Pow(p.Y - end.Y, 2));
        }

        public int distanceFromMidPoint(Point p)
        {
            return (int)Math.Sqrt(Math.Pow(p.X - calculateMidPoint().X, 2) + Math.Pow(p.Y - calculateMidPoint().Y, 2));
        }

        public Point calculateMidPoint()
        {
            return new Point((start.X + end.X) / 2, (start.Y + end.Y) / 2);
        }

        public void Draw(PaintEventArgs e)
        {
            e.Graphics.DrawLine(Pens.Black, start, end);
            e.Graphics.FillCircle(endPointBrush, start.X, start.Y, selectionRadius);
            e.Graphics.FillCircle(endPointBrush, end.X, end.Y, selectionRadius);
            e.Graphics.FillCircle(midPointBrush, calculateMidPoint().X, calculateMidPoint().Y, selectionRadius);
        }
    }
}

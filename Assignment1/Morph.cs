using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using static System.Windows.Forms.LinkLabel;
using System.Transactions;
using static System.Windows.Forms.AxHost;

namespace Assignment1
{
    public class Morph
    {
        private int numFrames;
        private Bitmap srcImg;
        private Bitmap destImg;
        private List<Bitmap> interFrames;
        private List<Line> srcLines;
        private List<Line> destLines;
        private List<List<Line>> interLines;
        public Morph(int numFrames, Bitmap srcImg, Bitmap destImg, List<Line> srcLines, List<Line> destLines) {
            this.numFrames = numFrames;
            this.srcImg = srcImg;
            this.destImg = destImg;
            this.srcLines = srcLines;
            this.destLines = destLines;
            this.interFrames = new List<Bitmap>();
            this.interLines = new List<List<Line>>();

        }

        public List<List<Line>> generateInterlopingLines(List<Line> srcLines, List<Line> destLines, int numOfFrames) {
            /*//interLines.Add(srcLines);
            for (int i = 0; i < srcLines.Count; i++) {
                //Debug.WriteLine("INdex:" + i);
                double startX = (double)(destLines[i].Start.X - srcLines[i].Start.X) / ((double)numOfFrames - 1);
                //Debug.WriteLine("Ans " + ((destLines[i].Start.X -srcLines[i].Start.X) / numOfFrames - 1));
                //Debug.WriteLine("StartX: " + startX);
                double startY = (double)(destLines[i].Start.Y - srcLines[i].Start.Y) / ((double)numOfFrames - 1);
                //Debug.WriteLine("StartY: " + startY);
                double endX = (double)(destLines[i].End.X - srcLines[i].End.X) / ((double)numOfFrames - 1);
                //Debug.WriteLine("EndX: " + endX);
                double endY = (double)(destLines[i].End.Y - srcLines[i].End.Y) / ((double)numOfFrames - 1);
                //Debug.WriteLine("EndY: " + endY);
                for (int j = 1; j < numOfFrames; j++)
                {
                    interLines.Add(new List<Line>());
                    interLines[i].Add(
                        new Line(
                            new Point((int)Math.Round(srcLines[i].Start.X + startX * j), (int)Math.Round(srcLines[i].Start.Y + startY * j)),
                            new Point((int)Math.Round(srcLines[i].End.X + endX * j), (int)Math.Round(srcLines[i].End.Y + endY * j))
                            ));
                }

            }
            Debug.WriteLine("Intermediate Lines Count" + interLines.Count);*/
            List<List<Line>> result = new List<List<Line>>();
            for (int i = 0; i < numOfFrames; i++)
            {
                result.Add(new List<Line>());
                for (int j = 0; j < srcLines.Count; j++) {
                    double startX = (double)(destLines[j].Start.X - srcLines[j].Start.X) / ((double)numOfFrames - 1);
                    double startY = (double)(destLines[j].Start.Y - srcLines[j].Start.Y) / ((double)numOfFrames - 1);
                    double endX = (double)(destLines[j].End.X - srcLines[j].End.X) / ((double)numOfFrames - 1);
                    double endY = (double)(destLines[j].End.Y - srcLines[j].End.Y) / ((double)numOfFrames - 1);
                    result[i].Add(
                        new Line(
                            new Point((int)Math.Round(srcLines[j].Start.X + startX * i), (int)Math.Round(srcLines[j].Start.Y + startY * i)),
                            new Point((int)Math.Round(srcLines[j].End.X + endX * i), (int)Math.Round(srcLines[j].End.Y + endY * i))
                        ));
                }
            }
            return result;
        }

        public void warpImage() {
            interFrames.Add(new Bitmap(destImg.Width, destImg.Height));
            for (int x = 0; x < destImg.Width; ++x) {
                for (int y = 0; y < destImg.Height; ++y)
                {
                    float weightSum = 0;
                    Vector2 deltaSum = new Vector2(0, 0);

                    for (int j = 0; j < srcLines.Count; j++)
                    {
                        Vector2 P = new Vector2(destLines[j].Start.X, destLines[j].Start.Y);
                        Vector2 Q = new Vector2(destLines[j].End.X, destLines[j].End.Y);
                        Vector2 PQ = getVectorMag(P, Q);
                        Vector2 n = new Vector2(-PQ.Y, PQ.X);
                        Vector2 T = new Vector2(x, y);
                        Vector2 PT = getVectorMag(P, T);
                        Vector2 TP = getVectorMag(T, P);
                        float d = vectorProjection(TP, n);
                        float f = vectorProjection(PT, PQ);
                        float fl = f / PQ.Length();
                        Vector2 Pprime = new Vector2(srcLines[j].Start.X, srcLines[j].Start.Y);
                        Vector2 Qprime = new Vector2(srcLines[j].End.X, srcLines[j].End.Y);
                        Vector2 PQprime = getVectorMag(Pprime, Qprime);
                        Vector2 nprime = new Vector2(-PQprime.Y, PQprime.X);
                        Vector2 Tprime = calculatePointCoords(Pprime, fl, PQprime, nprime, d);
                        Vector2 delta = Tprime - T;
                        float weight = weightPoint(d, fl, P, Q, T);

                        weightSum += weight;
                        deltaSum += Vector2.Multiply((float)weight, delta);
                        

                    }
                    
                    Vector2 deltaAvg = Vector2.Divide(deltaSum, (float)weightSum);
                    Vector2 TprimeAvg = new Vector2(x, y) + deltaAvg;
                    TprimeAvg = validatePixel(TprimeAvg, destImg.Width, destImg.Height);
/*                    Debug.WriteLine("TprimeAvg: " + TprimeAvg.X + ", " + TprimeAvg.Y);
                    Debug.WriteLine("T: " + x + ", " + y);*/
                    interFrames[0].SetPixel(x, y, srcImg.GetPixel((int)TprimeAvg.X, (int)TprimeAvg.Y));
                    //Thread.Sleep(0);

                }

            }

        }
        public List<Bitmap> warpImageOverFrames(Bitmap srcImg,Bitmap destImg, List<List<Line>> interLines, List<Line> srcLines)
        {
            List<Bitmap> interFrames = new List<Bitmap>();
            for (int i = 0; i < interLines.Count; i++) {
                interFrames.Add(new Bitmap(srcImg.Width, srcImg.Height));
                List<Line> destLines = interLines[i];
                //Debug.WriteLine("Count" + interLines.Count);
                for (int x = 0; x < destImg.Width; ++x)
                {
                    for (int y = 0; y < destImg.Height; ++y)
                    {
                        float weightSum = 0;
                        Vector2 deltaSum = new Vector2(0, 0);

                        for (int j = 0; j < srcLines.Count; j++)
                        {
                            Vector2 P = new Vector2(destLines[j].Start.X, destLines[j].Start.Y);
                            Vector2 Q = new Vector2(destLines[j].End.X, destLines[j].End.Y);
                            Vector2 PQ = getVectorMag(P, Q);
                            Vector2 n = new Vector2(-PQ.Y, PQ.X);
                            Vector2 T = new Vector2(x, y);
                            Vector2 PT = getVectorMag(P, T);
                            Vector2 TP = getVectorMag(T, P);
                            float d = vectorProjection(TP, n);
                            float f = vectorProjection(PT, PQ);
                            float fl = f / PQ.Length();
                            Vector2 Pprime = new Vector2(srcLines[j].Start.X, srcLines[j].Start.Y);
                            Vector2 Qprime = new Vector2(srcLines[j].End.X, srcLines[j].End.Y);
                            Vector2 PQprime = getVectorMag(Pprime, Qprime);
                            Vector2 nprime = new Vector2(-PQprime.Y, PQprime.X);
                            Vector2 Tprime = calculatePointCoords(Pprime, fl, PQprime, nprime, d);
                            Vector2 delta = Tprime - T;
                            float weight = weightPoint(d, fl, P, Q, T);

                            weightSum += weight;
                            deltaSum += Vector2.Multiply((float)weight, delta);


                        }

                        Vector2 deltaAvg = Vector2.Divide(deltaSum, (float)weightSum);
                        Vector2 TprimeAvg = new Vector2(x, y) + deltaAvg;
                        TprimeAvg = validatePixel(TprimeAvg, destImg.Width, destImg.Height);
                        interFrames[i].SetPixel(x, y, srcImg.GetPixel((int)TprimeAvg.X, (int)TprimeAvg.Y));

                    }

                }
            }
            return interFrames;

        }

        public List<Bitmap> crossDissolve(List<Bitmap> a, List<Bitmap> b) {
            List<Bitmap> result = new List<Bitmap>();
            for (int i = 0; i < a.Count; i++)
            {
                result.Add(new Bitmap(a[i].Width, a[i].Height));
                for (int x = 0; x < a[i].Width; x++)
                {
                    for (int y = 0; y < a[i].Height; y++)
                    {
                        double t = (double)(i+1) / (a.Count);
                        Color colorA = a[i].GetPixel(x, y);
                        Color colorB = b[i].GetPixel(x, y);
                        int newR = (int)Math.Round((1 - t) * colorA.R + t * colorB.R);
                        int newG = (int)Math.Round((1 - t) * colorA.G + t * colorB.G);
                        int newB = (int)Math.Round((1 - t) * colorA.B + t * colorB.B);
                        int newA = (int)Math.Round((1 - t) * colorA.A + t * colorB.A);
                        result[i].SetPixel(x, y, Color.FromArgb(newA, newR, newG, newB));
                    }
                }
            }
           return result;
        }


        public void printLines() {
            Debug.WriteLine("Source Lines" + srcLines.Count);
            for (int i = 0; i < srcLines.Count; i++)
            {
                Debug.WriteLine("Line " + i + ": " + srcLines[i].Start.X + ", " + srcLines[i].Start.Y + " to " + srcLines[i].End.X + ", " + srcLines[i].End.Y);

            }
            Debug.WriteLine("Intermediate Lines");
            for (int i = 0; i < interLines.Count; i++) {
                for (int j = 0; j < interLines[i].Count; j++)
                {
                    Debug.WriteLine("Line " + i + ": " + interLines[i][j].Start.X + ", " + interLines[i][j].Start.Y + " to " + interLines[i][j].End.X + ", " + interLines[i][j].End.Y);
                }
            }

            for (int i = 0; i < destLines.Count; i++)
            {
                Debug.WriteLine("Line " + i + ": " + destLines[i].Start.X + ", " + destLines[i].Start.Y + " to " + destLines[i].End.X + ", " + destLines[i].End.Y);

            }
        }

        private Vector2 getVectorMag(Vector2 start, Vector2 end) {
            return end - start;
        }

        // proj u onto v
        private float vectorProjection(Vector2 u, Vector2 v)
        {
            return Vector2.Dot(u, v) / v.Length();
        }

        private Vector2 calculatePointCoords(Vector2 P, float fl, Vector2 PQ, Vector2 n, float d)
        {
            Vector2 fractionOnPQ = Vector2.Multiply(fl, PQ);
            Vector2 final = Vector2.Multiply(d, Vector2.Divide(n, n.Length()));
            return P + Vector2.Multiply(fl, PQ) - Vector2.Multiply(d, Vector2.Divide(n, n.Length()));

        }

        private float weightPoint(float d, float fl, Vector2 P, Vector2 Q, Vector2 X)
        {
            float temp = d;
            if (fl <= 0)
            {
                temp = Vector2.Distance(X, P);
            }
            else if (fl >= 1)
            {
                temp = Vector2.Distance(X, Q);
            }
            return (float)Math.Pow(1 / (temp + 0.01), 2);
        }

        private Vector2 validatePixel(Vector2 coord, int width, int height) {
            Vector2 temp = new Vector2((int)Math.Round(coord.X), (int)Math.Round(coord.Y));
            if (temp.X < 0)
            {
                temp.X = 0;
            }
            else if (temp.X >= width)
            {
                temp.X = width-1;
            }
            if (temp.Y < 0)
            {
                temp.Y = 0;
            }
            else if (temp.Y >= height)
            {
                temp.Y = height-1;
            }
            return temp;
        }

        public List<Bitmap> getInterFrames()
        {
            return interFrames;
        }
    }
}

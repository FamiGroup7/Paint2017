using System;

namespace Paint
{
    public class ChartPoint
    {
        public ChartPoint(double x, double y)
        {
            X = x;
            Y = y;
        }

        public ChartPoint(double[] array)
        {
            X = array[0];
            Y = array[1];
        }

        public double X { get; set; }

        public double Y { get; set; }
        
        public double Distance(ChartPoint point)
        {
            return Math.Sqrt(Math.Pow(X - point.X, 2) + Math.Pow(Y - point.Y, 2));
        }
    }
}
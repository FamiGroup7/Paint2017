using System;
using System.Collections.Generic;
using System.Linq;

namespace Paint
{
    public class ChartManager
    {
        public ChartManager()
        {
            ChartDataList = new List<ChartData>();
        }

        public List<ChartData> ChartDataList { get; set; }

        public double MinX
        {
            get {
                return ChartDataList.Select(chartData => chartData.MinX).Concat(new[] {double.MaxValue}).Min();
            }
        }

        public double MaxX
        {
            get { return ChartDataList.Select(chartData => chartData.MaxX).Concat(new[] {double.MinValue}).Max(); }
        }

        public double MinY
        {
            get { return ChartDataList.Select(chartData => chartData.MinY).Concat(new[] {double.MaxValue}).Min(); }
        }

        public double MaxY
        {
            get { return ChartDataList.Select(chartData => chartData.MaxY).Concat(new[] {double.MinValue}).Max(); }
        }

        public void CreateSumChart()
        {
            SortedSet<double> tmp = new SortedSet<double>();
            foreach (var chartData in ChartDataList)
            {
                foreach (var chartPoint in chartData.Points)
                {
                    tmp.Add(chartPoint.X);
                }
            }
            var points = new List<ChartPoint>();
            foreach (double x in tmp)
            {
                double y = 0;
                foreach (var chartData in ChartDataList)
                {
                    for (int i = 0; i<chartData.Points.Count-1;i++)
                    {
                        ChartPoint p1 = (i == chartData.Points.Count-1) ? chartData.Points[i-1] : chartData.Points[i];
                        ChartPoint p2 = (i == chartData.Points.Count - 1) ? chartData.Points[i] : chartData.Points[i+1];
                        ChartPoint point;
                        if (p1.X <= x && p2.X > x)
                        {
                            double k = (p1.Y - p2.Y) / (p1.X - p2.X);
                            double b = p1.Y - k * p1.X;
                            double yy = k * x + b;
                            point = new ChartPoint(x, yy);

                            if (point != null)
                            {
                                y += point.Y;
                            }
                        }
                    }
                    int j = chartData.Points.Count - 1;
                    ChartPoint pp1 = (j == chartData.Points.Count - 1) ? chartData.Points[j - 1] : chartData.Points[j];
                    ChartPoint pp2 = (j == chartData.Points.Count - 1) ? chartData.Points[j] : chartData.Points[j + 1];
                    ChartPoint ppoint;
                    if (pp1.X < x && pp2.X >= x)
                    {
                        double k = (pp1.Y - pp2.Y) / (pp1.X - pp2.X);
                        double b = pp1.Y - k * pp1.X;
                        double yy = k * x + b;
                        ppoint = new ChartPoint(x, yy);

                        if (ppoint != null)
                        {
                            y += ppoint.Y;
                        }
                    }

                }
                points.Add(new ChartPoint(x,y));
            }
            ChartDataList.Add(new ChartData(points));
        }

        public void CreateRaznChart()
        {
            SortedSet<double> tmp = new SortedSet<double>();
            foreach (var chartData in ChartDataList)
            {
                foreach (var chartPoint in chartData.Points)
                {
                    tmp.Add(chartPoint.X);
                }
            }
            var points = new List<ChartPoint>();
            foreach (double x in tmp)
            {
                double y = 0;
                foreach (var chartData in ChartDataList)
                {
                    for (int i = 0; i < chartData.Points.Count - 1; i++)
                    {
                        ChartPoint p1 = (i == chartData.Points.Count - 1) ? chartData.Points[i - 1] : chartData.Points[i];
                        ChartPoint p2 = (i == chartData.Points.Count - 1) ? chartData.Points[i] : chartData.Points[i + 1];
                        ChartPoint point;
                        if (p1.X <= x && p2.X > x)
                        {
                            double k = (p1.Y - p2.Y) / (p1.X - p2.X);
                            double b = p1.Y - k * p1.X;
                            double yy = k * x + b;
                            point = new ChartPoint(x, yy);

                            if (point != null)
                            {
                                y -= point.Y;
                            }
                        }
                    }
                    int j = chartData.Points.Count - 1;
                    ChartPoint pp1 = (j == chartData.Points.Count - 1) ? chartData.Points[j - 1] : chartData.Points[j];
                    ChartPoint pp2 = (j == chartData.Points.Count - 1) ? chartData.Points[j] : chartData.Points[j + 1];
                    ChartPoint ppoint;
                    if (pp1.X < x && pp2.X >= x)
                    {
                        double k = (pp1.Y - pp2.Y) / (pp1.X - pp2.X);
                        double b = pp1.Y - k * pp1.X;
                        double yy = k * x + b;
                        ppoint = new ChartPoint(x, yy);

                        if (ppoint != null)
                        {
                            y -= ppoint.Y;
                        }
                    }

                }
                points.Add(new ChartPoint(x, y));
            }
            ChartDataList.Add(new ChartData(points));
        }

        private ChartPoint getPointinLine(ChartPoint p1, ChartPoint p2, double x)
        {
            if(p1.X <= x && p2.X >= x)
            {
                double k = (p1.Y - p2.Y) / (p1.X - p2.X);
                double b = p1.Y - k * p1.X;
                double y = k*x+b;
                return new ChartPoint(x, y);
                
            }
            return null;
        }

        public void CreateRandomChart()
        {
            var random = new Random();
            var minX = random.Next(-10, -5);
            var maxX = random.Next(5, 10);
            var minY = random.Next(-10, -5);
            var maxY = random.Next(5, 10);
            var count = random.Next(10, 20);
            var step = (maxX - minX)/(double) count;
            var points = new List<ChartPoint>();
            for (var i = 0; i < count; i++)
            {
                var x = minX + step*i;
                var y = random.NextDouble()*(maxY - minY) + minY;
                var point = new ChartPoint(x, y);
                points.Add(point);
            }
            ChartDataList.Add(new ChartData(points));
        }

        public void AbsoluteChars()
        {
            foreach (var chartData in ChartDataList)
                foreach (var chartPoint in chartData.Points)
                    chartPoint.Y = Math.Abs(chartPoint.Y);
        }
    }
}
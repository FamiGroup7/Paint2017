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
            var points = new List<ChartPoint>();
            foreach (var chartData in ChartDataList)
            {
                foreach (var chartPoint in chartData.Points)
                {
                    var point = points.FirstOrDefault(p => Math.Abs(p.X - chartPoint.X) < double.Epsilon);
                    if (point == null)
                        points.Add(new ChartPoint(chartPoint.X, chartPoint.Y));
                    else
                        point.Y += chartPoint.Y;
                }
            }
            ChartDataList.Add(new ChartData(points));
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
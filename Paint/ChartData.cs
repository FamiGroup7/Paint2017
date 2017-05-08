using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace Paint
{
    public class ChartData
    {
        public ChartData()
        {
            Initialize();
        }

        public ChartData(ChartPoint point)
        {
            Initialize();
            List<ChartPoint> list = new List<ChartPoint>();
            list.Add(point);
            Points = list;
        }

        public ChartData(List<ChartPoint> points)
        {
            Initialize();
            Points = points;
        }

        private void Initialize()
        {
            Points = new List<ChartPoint>();
            ChartColor = Color.Black;
        }
        
        public List<ChartPoint> Points
        {
            get;
            set;
        }

        public Color ChartColor
        {
            get;
            set;
        }

        public bool ShowPoints
        {
            get;
            set;
        }

        public double MinX
        {
            get
            {
                return Points.Select(chartPoint => chartPoint.X).Min();
            }
        }

        public double MaxX
        {
            get
            {
                return Points.Select(chartPoint => chartPoint.X).Max();
            }
        }

        public double MinY
        {
            get
            {
                return Points.Select(chartPoint => chartPoint.Y).Min();
            }
        }

        public double MaxY
        {
            get
            {
                return Points.Select(chartPoint => chartPoint.Y).Max();
            }
        }
    }
}
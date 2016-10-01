using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Paint
{
    public class LinePoint
    {
        double x;
        double y;

        public LinePoint(double x, double y)
        {
            this.x = x;
            this.y = y;
        }

        public LinePoint(double[] array)
        {
            this.x = array[0];
            this.y = array[1];
        }

        public Point ConvertToPoint()
        {
            //int x;
            //int y;
            Point point = new Point((int)x, (int)y);
            return point;
        }

        public double X
        {
            get
            {
                return x;
            }
            set
            {
                x = value;
            }
        }

        public double Y
        {
            get
            {
                return y;
            }
            set
            {
                y = value;
            }
        }
    }
}

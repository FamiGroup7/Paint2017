using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

namespace Paint
{
    public abstract class FileManager
    {
        public static List<ChartPoint> LoadFromFile(string path)
        {
            var points = new List<ChartPoint>();
            using (var reader = new StreamReader(path))
            {
                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();
                    if (line != null)
                    {
                        var values = line.Split(' ');
                        var valueX = Convert.ToDouble(values[0], CultureInfo.InvariantCulture);
                        var valueY = Convert.ToDouble(values[1], CultureInfo.InvariantCulture);
                        var point = new ChartPoint(valueX, valueY);
                        points.Add(point);
                    }
                }
            }
            return points;
        }

        public static void SaveToFile(string path, List<ChartPoint> points)
        {
            using (var writer = new StreamWriter(path))
            {
                foreach (var point in points)
                {
                    writer.WriteLine(point.X.ToString(CultureInfo.InvariantCulture) + " " + point.Y.ToString(CultureInfo.InvariantCulture));
                }
            }
        }

        public static bool InputFromFile(string FileName, out List<ChartData> lines)
        {
            try
            {
                lines = new List<ChartData>();
                string[] file = System.IO.File.ReadAllLines(FileName);
                int n = Int32.Parse(file[0]);
                for (int i = 0, k = 1; i < n; i++, k++)
                {
                    int m = Int32.Parse(file[k]);

                    ChartData line = new ChartData();
                    for (int j = 0; j < m; j++, k++)
                    {
                        file[k + 1] = file[k + 1].Trim();
                        file[k + 1] = file[k + 1].Replace('\t', ' ');
                        file[k + 1] = file[k + 1].Replace("  ", " ");
                        file[k + 1] = file[k + 1].Replace('.', ',');
                        ChartPoint point = new ChartPoint(file[k + 1].Split(' ').Select(nn => Convert.ToDouble(nn)).ToArray());
                        line.Points.Add(point);
                    }
                    lines.Add(line);
                }
                return true;
            }
            catch (Exception error)
            {
                lines = new List<ChartData>();
                return false;
            }
        }

        public static bool InputLineFromFile(string FileName, out ChartData lines)
        {
            try
            {
                lines = new ChartData();
                string[] file = System.IO.File.ReadAllLines(FileName);
                for (int i = 0; i < file.Length; i++)
                {
                    file[i] = file[i].Trim();
                    file[i] = file[i].Replace('\t', ' ');
                    file[i] = file[i].Replace("  ", " ");
                    file[i] = file[i].Replace('.', ',');
                    ChartPoint point = new ChartPoint(file[i].Split(' ').Select(nn => Convert.ToDouble(nn)).ToArray());
                    lines.Points.Add(point);
                }
                return true;
            }
            catch (Exception error)
            {
                lines = new ChartData();
                return false;
            }
        }
    }
}
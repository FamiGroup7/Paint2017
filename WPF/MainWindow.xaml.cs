using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Controls;
using System.Text.RegularExpressions;
using System.Globalization;
using Microsoft.Win32;
using Paint;

namespace WPF
{
    public partial class MainWindow : Window
    {

        public const double ChartMargin = 16;

        private ChartManager _chartManager;

        public MainWindow()
        {
            InitializeComponent();
            init();
        }

        private void init()
        {
            //initWindow();
            _chartManager = new ChartManager();
            List<ChartData> chartData;
            FileManager.InputFromFile("Resources/input.txt", out chartData);
            _chartManager.ChartDataList.AddRange(chartData);
            SetupPanel();
        }

        private void initWindow()
        {
            double height = ((Panel)Application.Current.MainWindow.Content).ActualHeight;
            panel1.Height = height;
            pictureBox1.Height = height;
            pictureBox1.Width = ((Panel)Application.Current.MainWindow.Content).Width * 2 / 3;
            panel1.Width = ((Panel)Application.Current.MainWindow.Content).Width / 3;
        }

        private void SetupPanel()
        {
            panel1.Children.Clear();
            for (int i = 0, k = 0; i < _chartManager.ChartDataList.Count; i++, k++)
            {
                List<MyColor> colors = new List<MyColor>
                {
                    new MyColor {color = System.Drawing.Color.Black, Name = "Black" },
                    new MyColor {color = System.Drawing.Color.Blue, Name = "Blue" },
                    new MyColor {color = System.Drawing.Color.Red, Name = "Red" },
                    new MyColor {color = System.Drawing.Color.Green, Name = "Green" }
                };

                ChartData chartData = _chartManager.ChartDataList[i];
                
                Button buttonShowPoints = new Button();
                buttonShowPoints.Width = 160;
                buttonShowPoints.Height = 20;
                buttonShowPoints.Name = "ButtonShowPoints" + i + "";
                buttonShowPoints.Content = "Show points";
                buttonShowPoints.Click += new RoutedEventHandler(buttonShowPoints_Click);
                panel1.Children.Add(buttonShowPoints);
                Canvas.SetLeft(buttonShowPoints, 100);
                Canvas.SetTop(buttonShowPoints, 20 + (k * 40));

                ComboBox comboBox = new ComboBox();
                comboBox.Name = "ButtonColor" + i + "";
                comboBox.ItemsSource = colors;
                comboBox.Width = 75;
                comboBox.Height = 20;
                comboBox.DisplayMemberPath = "Name";
                comboBox.SelectionChanged += new SelectionChangedEventHandler(comboBox_SelectedIndexChanged);
                panel1.Children.Add(comboBox);
                Canvas.SetLeft(comboBox, 20);
                Canvas.SetTop(comboBox, 20 + (k * 40));

                Button buttonColor = new Button();
                buttonColor.Width = 60;
                buttonColor.Height = 20;
                buttonColor.Name = "ButtonColor" + i + "";
                buttonColor.Background = new SolidColorBrush(System.Windows.Media.Color.FromArgb(chartData.ChartColor.A, chartData.ChartColor.R, chartData.ChartColor.G, chartData.ChartColor.B));
                panel1.Children.Add(buttonColor);
                Canvas.SetLeft(buttonColor, 20);
                Canvas.SetTop(buttonColor, 20 + (k * 40));

                if (chartData.ShowPoints)
                {
                    Button buttonUpdate = new Button();
                    buttonUpdate.Width = 160;
                    buttonUpdate.Height = 20;
                    buttonUpdate.Name = "ButtonUpdate" + i + "";
                    buttonUpdate.Content = "Update";
                    buttonUpdate.Click += new RoutedEventHandler(buttonUpdate_Click);
                    Canvas.SetLeft(buttonUpdate, 280);
                    Canvas.SetTop(buttonUpdate, 20 + (k * 40));
                    panel1.Children.Add(buttonUpdate);

                    for (int j = 0; j < chartData.Points.Count; j++, k++)
                    {
                        TextBox textBoxX = new TextBox();
                        textBoxX.Width = 60;
                        textBoxX.Height = 20;
                        textBoxX.Name = "Line" + i + "X" + j + "";
                        Canvas.SetLeft(textBoxX, 100);
                        Canvas.SetTop(textBoxX, 20 + ((k + 1) * 40));
                        textBoxX.Text = "" + chartData.Points[j].X + "";
                        panel1.Children.Add(textBoxX);

                        TextBox textBoxY = new TextBox();
                        textBoxY.Width = 60;
                        textBoxY.Height = 20;
                        textBoxY.Name = "Line" + i + "Y" + j + "";
                        Canvas.SetLeft(textBoxY, 180);
                        Canvas.SetTop(textBoxY, 20 + ((k + 1) * 40));
                        textBoxY.Text = "" + chartData.Points[j].Y + "";
                        panel1.Children.Add(textBoxY);
                    }
                }
            }
        }

        private void buttonShowPoints_Click(object sender, EventArgs e)
        {
            Button button = (Button)sender;
            string mynumber = Regex.Replace(button.Name, @"\D", "");
            ChartData chartData = _chartManager.ChartDataList[Convert.ToInt32(mynumber)];
            chartData.ShowPoints = !chartData.ShowPoints;
            SetupPanel();
        }

        private void buttonUpdate_Click(object sender, EventArgs e)
        {
            //    Button button = (Button)sender;
            //    int i = Convert.ToInt32(button.Name);
            //    ChartData chartData = _chartManager.ChartDataList[i];
            //    for (int j = 0; j < chartData.Points.Count; j++)
            //    {
            //        ChartPoint point = chartData.Points[j];
            //        TextBox textBoxX = (TextBox)panel1.Children.Find("Line" + i + "X" + j + "", false).FirstOrDefault();
            //        TextBox textBoxY = (TextBox)panel1.Children.Find("Line" + i + "Y" + j + "", false).FirstOrDefault();
            //        point.X = Convert.ToDouble(textBoxX.Text);
            //        point.Y = Convert.ToDouble(textBoxY.Text);
            //    }
            //    pictureBox1.Invalidate();
        }
        
        private void comboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox combox = (ComboBox)sender;
            MyColor color = (MyColor)combox.SelectedItem;
            Button button = (Button)findByName(combox.Name, panel1.Children);
            button.Background = new SolidColorBrush(System.Windows.Media.Color.FromArgb(color.color.A, color.color.R, color.color.G, color.color.B));
            string mynumber = Regex.Replace(button.Name, @"\D", "");
            _chartManager.ChartDataList[Convert.ToInt32(mynumber)].ChartColor = color.color;
            Invalidate();
        }

        private UIElement findByName(string name, UIElementCollection collection)
        {
            foreach(UIElement element in collection)
            {
                if (element is TextBox)
                {
                    TextBox txt = (TextBox)element;
                    if (txt.Name == name)
                    {
                        return txt;
                    }
                }
                else if (element is ListBox)
                {
                    ListBox lst = (ListBox)element;
                    if (lst.Name == name)
                    {
                        return lst;
                    }
                }
                else if (element is Button)
                {
                    Button btn = (Button)element;
                    if (btn.Name == name)
                    {
                        return btn;
                    }
                }
            }
            return null;
        }

        //private void addLineToolStripMenuItem_Click(object sender, EventArgs e)
        //{
        //    OpenFileDialog OFD = new OpenFileDialog();


        //    OFD.Filter = "TXT files (*.txt)|*.txt|All files (*.*)|*.*";
        //    OFD.RestoreDirectory = true;
        //    string fileName;

        //    if (OFD.ShowDialog() == DialogResult.OK)
        //    {
        //        fileName = OFD.FileName;   //Путь файла с начальным приближением
        //        ChartData chartData = new ChartData();
        //        FileManager.InputLineFromFile(fileName, out chartData);
        //        _chartManager.ChartDataList.Add(chartData);
        //        SetupPanel();
        //        pictureBox1.Invalidate();
        //    }
        //}

        #region Drawing

        private void Invalidate()
        {
            pictureBox1.Children.Clear();
            if (_chartManager.ChartDataList.Count != 0)
            {
                DrawChart();
                DrawAxisX();
                DrawAxisY();
            }
        }

        private void DrawChart()
        {
            foreach (var chartData in _chartManager.ChartDataList)
            {
                var points = chartData.Points;
                var width = pictureBox1.ActualWidth;
                var height = pictureBox1.ActualHeight;
                var margin = ChartMargin;
                var minX = Math.Floor(_chartManager.MinX);
                var maxX = Math.Ceiling(_chartManager.MaxX);
                var minY = Math.Floor(_chartManager.MinY);
                var maxY = Math.Ceiling(_chartManager.MaxY);
                for (var i = 0; i < points.Count - 1; i++)
                {
                    var x1 = margin + (points[i].X - minX) / (maxX - minX) * (width - margin);
                    var y1 = height - margin - (points[i].Y - minY) / (maxY - minY) * (height - margin);
                    var x2 = margin + (points[i + 1].X - minX) / (maxX - minX) * (width - margin);
                    var y2 = height - margin - (points[i + 1].Y - minY) / (maxY - minY) * (height - margin);
                    var r = chartData.ChartColor.R;
                    var g = chartData.ChartColor.G;
                    var b = chartData.ChartColor.B;
                    var color = new SolidColorBrush(System.Windows.Media.Color.FromRgb(r, g, b));
                    DrawLine(x1, y1, x2, y2, color);
                }
            }
        }

        private void DrawAxisX()
        {
            var width = pictureBox1.ActualWidth;
            var height = pictureBox1.ActualHeight;
            var margin = ChartMargin;
            var minX = Math.Floor(_chartManager.MinX);
            var maxX = Math.Ceiling(_chartManager.MaxX);
            var length = maxX - minX;
            var stepX = (length / 10);
            var count = length / stepX;
            var stepW = (width - margin) / count;
            DrawLine(margin, height - margin, width, height - margin, Brushes.Black);
            var x = margin;
            var y = height - margin;
            for (var i = 0; x < width; i++)
            {
                DrawLine(x, y, x, y - 5, Brushes.Black);
                var text = Math.Round(minX + stepX * i, 2).ToString(CultureInfo.InvariantCulture);
                DrawText(x, y, text);
                x += stepW;
            }
        }

        private void DrawAxisY()
        {
            var height = pictureBox1.ActualHeight;
            var margin = ChartMargin;
            var minY = Math.Floor(_chartManager.MinY);
            var maxY = Math.Ceiling(_chartManager.MaxY);
            var length = maxY - minY;
            var stepY = (length / 10);
            var count = length / stepY;
            var stepH = (height - margin) / count;
            DrawLine(margin, 0, margin, height - margin, Brushes.Black);
            var x = margin;
            var y = height - margin;
            for (var i = 0; y > 0; i++)
            {
                DrawLine(x, y, x + 5, y, Brushes.Black);
                var text = Math.Round(minY + stepY * i, 2).ToString(CultureInfo.InvariantCulture);
                DrawText(x, y, text, 90);
                y -= stepH;
            }
        }


        private void DrawText(double x, double y, string text, double angle = 0)
        {
            var textBlock = new TextBlock
            {
                Text = text,
                RenderTransform = new RotateTransform(angle)
            };
            Canvas.SetLeft(textBlock, x);
            Canvas.SetTop(textBlock, y);
            pictureBox1.Children.Add(textBlock);
        }

        private void DrawLine(double x1, double y1, double x2, double y2, System.Windows.Media.Brush color)
        {
            var line = new Line
            {
                Stroke = color,
                X1 = x1,
                Y1 = y1,
                X2 = x2,
                Y2 = y2
            };
            pictureBox1.Children.Add(line);
        }

        #endregion Drawing

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

            Invalidate();
        }
    }
}

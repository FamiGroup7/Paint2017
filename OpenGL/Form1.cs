using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Globalization;
using System.Windows.Forms;
using System.Linq;
using OpenTK.Graphics.OpenGL;
using Paint;

namespace OpenGL
{
    public partial class Form1 : Form
    {
        Graphics graphics;
        private ChartManager _chartManager;
        public const double ChartMargin = 15;
        private bool _loaded;

        public Form1()
        {
            InitializeComponent();
            init();
        }

        private void init()
        {
            initWindow();
            _chartManager = new ChartManager();
            List<ChartData> chartData;
            FileManager.InputFromFile("Resources/input.txt", out chartData);
            _chartManager.ChartDataList.AddRange(chartData);
            CenterToScreen();
            SetupPanel();            
        }

        private void initWindow()
        {
            int height = ClientSize.Height - MainMenuStrip.Height;
            panel1.Height = height;
            pictureBox1.Height = height;
            pictureBox1.Width = ClientSize.Width * 2 / 3;
            panel1.Width = ClientSize.Width / 3;
        }

        private void SetupPanel()
        {
            panel1.Controls.Clear();
            for (int i = 0, k = 0; i < _chartManager.ChartDataList.Count; i++, k++)
            {
                List<MyColor> colors = new List<MyColor>
                {
                    new MyColor {color = Color.Black, Name = "Black" },
                    new MyColor {color = Color.Blue, Name = "Blue" },
                    new MyColor {color = Color.Red, Name = "Red" },
                    new MyColor {color = Color.Green, Name = "Green" }
                };

                ChartData chartData = _chartManager.ChartDataList[i];
                chartData.ChartColor = colors[i % colors.Count].color;

                Button buttonColor = new Button();
                buttonColor.Size = new Size(60, 20);
                buttonColor.Name = "" + i + "";
                buttonColor.Location = new Point(20, 20 + (k * 40));
                buttonColor.BackColor = chartData.ChartColor;
                panel1.Controls.Add(buttonColor);

                Button buttonShowPoints = new Button();
                buttonShowPoints.Size = new Size(160, 20);
                buttonShowPoints.Name = "" + i + "";
                buttonShowPoints.Text = "Show points";
                buttonShowPoints.Click += buttonShowPoints_Click;
                buttonShowPoints.Location = new Point(100, 20 + (k * 40));
                panel1.Controls.Add(buttonShowPoints);

                ComboBox comboBox = new ComboBox();
                comboBox.Name = "" + i + "";
                comboBox.DataSource = colors;
                comboBox.Size = new Size(75, 20);
                comboBox.DisplayMember = "Name";
                comboBox.Location = new Point(buttonColor.Location.X, buttonColor.Location.Y);
                comboBox.SelectedIndexChanged += comboBox_SelectedIndexChanged;
                panel1.Controls.Add(comboBox);

                if (chartData.ShowPoints)
                {
                    Button buttonUpdate = new Button();
                    buttonUpdate.Size = new Size(160, 20);
                    buttonUpdate.Name = "" + i + "";
                    buttonUpdate.Text = "Update";
                    buttonUpdate.Click += buttonUpdate_Click;
                    buttonUpdate.Location = new Point(280, 20 + (k * 40));
                    panel1.Controls.Add(buttonUpdate);

                    for (int j = 0; j < chartData.Points.Count; j++, k++)
                    {
                        TextBox textBoxX = new TextBox();
                        textBoxX.Size = new Size(60, 20);
                        textBoxX.Name = "Line" + i + "X" + j + "";
                        textBoxX.Location = new Point(100, 20 + ((k + 1) * 40));
                        textBoxX.Text = "" + chartData.Points[j].X + "";
                        textBoxX.TextChanged += textBox_TextChanged;
                        panel1.Controls.Add(textBoxX);

                        TextBox textBoxY = new TextBox();
                        textBoxY.Size = new Size(60, 20);
                        textBoxY.Name = "Line" + i + "Y" + j + "";
                        textBoxY.Location = new Point(180, 20 + ((k + 1) * 40));
                        textBoxY.Text = "" + chartData.Points[j].Y + "";
                        textBoxY.TextChanged += textBox_TextChanged;
                        panel1.Controls.Add(textBoxY);
                    }
                }
            }
        }

        private void textBox_TextChanged(object sender, EventArgs e)
        {
            TextBox textBox = (TextBox)sender;
        }

        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            if (_loaded)
            {
                e.Graphics.Clear(Color.White);
                GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
                GL.MatrixMode(MatrixMode.Modelview);
                GL.LoadIdentity();
                if (_chartManager.ChartDataList.Count != 0)
                {
                    DrawChart();
                    DrawAxisX();
                    DrawAxisY();
                }
                pictureBox1.SwapBuffers();
                if (_chartManager.ChartDataList.Count != 0)
                {
                    DrawTextX(e.Graphics);
                    DrawTextY(e.Graphics);
                }
            }
        }

        private void DrawChart()
        {
            foreach (var chartData in _chartManager.ChartDataList)
            {
                var points = chartData.Points;
                var width = pictureBox1.Width;
                var height = pictureBox1.Height;
                var margin = ChartMargin;
                var minX = Math.Floor(_chartManager.MinX);
                var maxX = Math.Ceiling(_chartManager.MaxX);
                var minY = Math.Floor(_chartManager.MinY);
                var maxY = Math.Ceiling(_chartManager.MaxY);
                GL.Color3(chartData.ChartColor);
                GL.Begin(PrimitiveType.LineStrip);
                foreach (var point in points)
                {
                    var x = margin + (point.X - minX) / (maxX - minX) * (width - margin);
                    var y = height - margin - (point.Y - minY) / (maxY - minY) * (height - margin);
                    GL.Vertex2(x, y);
                }
                GL.End();
            }
        }

        private void DrawTextX(Graphics graphics)
        {
            var width = pictureBox1.Width;
            var height = pictureBox1.Height;
            var margin = ChartMargin;
            var minX = Math.Floor(_chartManager.MinX);
            var maxX = Math.Ceiling(_chartManager.MaxX);
            var length = maxX - minX;
            var stepX = (length / 10);
            var count = length / stepX;
            var stepW = (width - margin) / count;
            var x = margin;
            var y = height - margin;
            for (var i = 0; x < width; i++)
            {
                var text = Math.Round(minX + stepX * i, 2).ToString(CultureInfo.InvariantCulture);
                var font = new Font("Arial", 8);
                var brush = new SolidBrush(Color.Black);
                var point = new PointF((float)x, (float)y);
                graphics.DrawString(text, font, brush, point);
                x += stepW;
            }
        }

        private void DrawAxisX()
        {
            var width = pictureBox1.Width;
            var height = pictureBox1.Height;
            var margin = ChartMargin;
            var minX = Math.Floor(_chartManager.MinX);
            var maxX = Math.Ceiling(_chartManager.MaxX);
            var length = maxX - minX;
            var stepX = (length / 10);
            var count = length / stepX;
            var stepW = (width - margin) / count;
            GL.Color3(Color.Black);
            GL.Begin(PrimitiveType.Lines);
            GL.Vertex2(margin, height - margin);
            GL.Vertex2(width, height - margin);
            GL.End();
            var x = margin;
            var y = height - margin;
            for (var i = 0; x < width; i++)
            {
                GL.Begin(PrimitiveType.Lines);
                GL.Vertex2(x, y);
                GL.Vertex2(x, y - 5);
                GL.End();
                x += stepW;
            }
        }

        private void DrawTextY(Graphics graphics)
        {
            var height = pictureBox1.Height;
            var margin = ChartMargin;
            var minY = Math.Floor(_chartManager.MinY);
            var maxY = Math.Ceiling(_chartManager.MaxY);
            var length = maxY - minY;
            var stepY = (length / 10);
            var count = length / stepY;
            var stepH = (height - margin) / count;
            var y = height - margin;
            for (var i = 0; y > 0; i++)
            {
                var text = Math.Round(minY + stepY * i, 2).ToString(CultureInfo.InvariantCulture);
                var font = new Font("Arial", 8);
                var brush = new SolidBrush(Color.Black);
                var point = new PointF(0, (float)y);
                var format = new StringFormat { FormatFlags = StringFormatFlags.DirectionVertical };
                graphics.DrawString(text, font, brush, point, format);
                y -= stepH;
            }
        }

        private void DrawAxisY()
        {
            var height = pictureBox1.Height;
            var margin = ChartMargin;
            var minY = Math.Floor(_chartManager.MinY);
            var maxY = Math.Ceiling(_chartManager.MaxY);
            var length = maxY - minY;
            var stepY = (length / 10);
            var count = length / stepY;
            var stepH = (height - margin) / count;
            GL.Color3(Color.Black);
            GL.Begin(PrimitiveType.Lines);
            GL.Vertex2(margin, 0);
            GL.Vertex2(margin, height - margin);
            GL.End();
            var x = margin;
            var y = height - margin;
            for (var i = 0; y > 0; i++)
            {
                GL.Begin(PrimitiveType.Lines);
                GL.Vertex2(x, y);
                GL.Vertex2(x + 5, y);
                GL.End();
                y -= stepH;
            }
        }

        private void buttonShowPoints_Click(object sender, EventArgs e)
        {
            Button button = (Button)sender;
            ChartData chartData = _chartManager.ChartDataList[Convert.ToInt32(button.Name)];
            chartData.ShowPoints = !chartData.ShowPoints;
            SetupPanel();
        }

        private void buttonUpdate_Click(object sender, EventArgs e)
        {
            Button button = (Button)sender;
            int i = Convert.ToInt32(button.Name);
            ChartData chartData = _chartManager.ChartDataList[i];
            for (int j = 0; j < chartData.Points.Count; j++)
            {
                ChartPoint point = chartData.Points[j];
                TextBox textBoxX = (TextBox)panel1.Controls.Find("Line" + i + "X" + j + "", false).FirstOrDefault();
                TextBox textBoxY = (TextBox)panel1.Controls.Find("Line" + i + "Y" + j + "", false).FirstOrDefault();
                point.X = Convert.ToDouble(textBoxX.Text);
                point.Y = Convert.ToDouble(textBoxY.Text);
            }
            pictureBox1.Invalidate();
        }

        private void comboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox combox = (ComboBox)sender;
            MyColor color = (MyColor)combox.SelectedItem;
            Button button = (Button)panel1.Controls[combox.Name];
            button.BackColor = color.color;
            _chartManager.ChartDataList[Convert.ToInt32(combox.Name)].ChartColor = color.color;
            pictureBox1.Invalidate();
        }

        private void addLineToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog OFD = new OpenFileDialog();


            OFD.Filter = "TXT files (*.txt)|*.txt|All files (*.*)|*.*";
            OFD.RestoreDirectory = true;
            string fileName;

            if (OFD.ShowDialog() == DialogResult.OK)
            {
                fileName = OFD.FileName;   //Путь файла с начальным приближением
                ChartData chartData = new ChartData();
                FileManager.InputLineFromFile(fileName, out chartData);
                _chartManager.ChartDataList.Add(chartData);
                SetupPanel();
                pictureBox1.Invalidate();
            }
        }

        private void Form1_Resize(object sender, EventArgs e)
        {
            if (_loaded)
            {
                SetupViewport();
            }
            initWindow();
            pictureBox1.Invalidate();
            SetupPanel();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            _loaded = true;
            GL.ClearColor(Color.White);
            SetupViewport();
        }

        private void SetupViewport()
        {
            var w = pictureBox1.Width;
            var h = pictureBox1.Height;
            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadIdentity();
            GL.Ortho(0, w, h, 0, -1, 1);
            GL.Viewport(0, 0, w, h);
        }

        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {

        }
    }
}

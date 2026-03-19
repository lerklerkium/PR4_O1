using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Legends;
using OxyPlot.Series;
using OxyPlot.Wpf;
using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace PR4_O.Pages
{
    public partial class Task3Page : System.Windows.Controls.Page
    {
        private PlotModel plotModel;
        private LineSeries lineSeries;

        public Task3Page()
        {
            InitializeComponent();
            LoadImage(FormulaImage, "3.jpg");
            InitPlot();
        }

        static void LoadImage(System.Windows.Controls.Image img, string name)
        {
            string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "images", name);
            if (!File.Exists(path)) return;
            var bmp = new BitmapImage();
            bmp.BeginInit();
            bmp.UriSource = new Uri(path);
            bmp.CacheOption = BitmapCacheOption.OnLoad;
            bmp.EndInit();
            img.Source = bmp;
        }

        void InitPlot()
        {
            plotModel = new PlotModel { Title = "y(x)" };

            plotModel.Legends.Add(new Legend
            {
                LegendPosition = LegendPosition.RightTop,
                LegendPlacement = LegendPlacement.Outside
            });

            plotModel.Axes.Add(new LinearAxis
            {
                Position = AxisPosition.Bottom,
                Title = "x",
                MajorGridlineStyle = LineStyle.Solid,
                MajorGridlineColor = OxyColors.LightGray
            });

            plotModel.Axes.Add(new LinearAxis
            {
                Position = AxisPosition.Left,
                Title = "y",
                MajorGridlineStyle = LineStyle.Solid,
                MajorGridlineColor = OxyColors.LightGray
            });

            lineSeries = new LineSeries
            {
                Title = "y(x)",
                Color = OxyColors.Blue,
                StrokeThickness = 2
            };
            plotModel.Series.Add(lineSeries);

            PlotView.Model = plotModel;
        }

        static bool Parse(System.Windows.Controls.TextBox tb, string name, out double val)
        {
            val = 0;
            if (string.IsNullOrWhiteSpace(tb.Text))
            { Warn($"Поле \"{name}\" не заполнено."); return false; }
            if (!double.TryParse(tb.Text.Replace('.', ','), out val))
            { Warn($"Поле \"{name}\" содержит некорректное значение."); return false; }
            return true;
        }

        static void Warn(string msg) =>
            MessageBox.Show(msg, "Ошибка ввода", MessageBoxButton.OK, MessageBoxImage.Warning);

        static double CalcY(double x, double b)
        {
            double absXB = Math.Abs(x - b);
            double absB3X3 = Math.Abs(b * b * b - x * x * x);
            if (absXB < 1e-15 || absB3X3 < 1e-15) return double.NaN;
            return Math.Sqrt(absXB) / Math.Pow(absB3X3, 1.5) + Math.Log(absXB);
        }

        public void Calculate_Click3(object sender, RoutedEventArgs e)
        {
            if (!Parse(TbB, "b", out double b)) return;
            if (!Parse(TbX0, "x0", out double x0)) return;
            if (!Parse(TbXk, "xk", out double xk)) return;
            if (!Parse(TbDx, "dx", out double dx)) return;

            if (dx <= 0) { Warn("Шаг dx должен быть положительным."); return; }
            if (x0 >= xk) { Warn("x0 должно быть меньше xk."); return; }

            lineSeries.Points.Clear();
            string result = $"{"x",-15}{"y",-15}\n{new string('-', 30)}\n";

            for (double x = x0; x <= xk + 1e-10; x += dx)
            {
                double y = CalcY(x, b);
                if (!double.IsNaN(y) && !double.IsInfinity(y))
                {
                    lineSeries.Points.Add(new DataPoint(x, y));
                    result += $"{x,-15:G6}{y,-15:G6}\n";
                }
                else
                    result += $"{x,-15:G6}{"не опр.",-15}\n";
            }

            TbResult.Text = result;
            plotModel.InvalidatePlot(true);
        }

        private void Clear_Click(object sender, RoutedEventArgs e)
        {
            TbB.Text = TbX0.Text = TbXk.Text = TbDx.Text = TbResult.Text = "";
            lineSeries.Points.Clear();
            plotModel.InvalidatePlot(true);
        }
    }
}
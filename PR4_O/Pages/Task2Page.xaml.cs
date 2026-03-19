using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace PR4_O.Pages
{
    public partial class Task2Page : Page
    {
        public Task2Page()
        {
            InitializeComponent();
            LoadImage(FormulaImage, "2.jpg");
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

        static bool Parse(TextBox tb, string name, out double val)
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

        double Fx(double x) =>
            RbSh.IsChecked == true ? Math.Sinh(x) :
            RbX2.IsChecked == true ? x * x : Math.Exp(x);

        public void Calculate_Click2(object sender, RoutedEventArgs e)
        {
            if (!Parse(TbX, "x", out double x)) return;
            if (!Parse(TbY, "y", out double y)) return;

            double fx = Fx(x);
            double diff = x - y;
            double c;

            if (Math.Abs(diff) < 1e-15)
                c = fx * fx + y * y + Math.Sin(y);
            else if (diff > 0)
                c = Math.Pow(fx - y, 2) + Math.Cos(y);
            else
                c = Math.Pow(y - fx, 2) + Math.Tan(y);

            TbResult.Text = c.ToString("G10");
        }

        private void Clear_Click(object sender, RoutedEventArgs e)
        {
            TbX.Text = TbY.Text = TbResult.Text = "";
            RbSh.IsChecked = true;
        }
    }
}

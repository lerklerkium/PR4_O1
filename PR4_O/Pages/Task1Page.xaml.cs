using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace PR4_O.Pages
{
    public partial class Task1Page : Page
    {
        public Task1Page()
        {
            InitializeComponent();
            LoadImage(FormulaImage, "1.jpg");
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

        public void Calculate_Click1(object sender, RoutedEventArgs e)
        {
            if (!Parse(TbX, "x", out double x)) return;
            if (!Parse(TbY, "y", out double y)) return;
            if (!Parse(TbZ, "z", out double z)) return;

            double inner = y + Math.Cbrt(x - 1);
            if (inner < 0) { Warn("Выражение под корнем 4-й степени отрицательно."); return; }

            double den = Math.Abs(x - y) * (Math.Sin(z) * Math.Sin(z) + Math.Tan(z));
            if (Math.Abs(den) < 1e-15) { Warn("Знаменатель равен нулю."); return; }

            TbResult.Text = (Math.Pow(inner, 0.25) / den).ToString("G10");
        }

        private void Clear_Click(object sender, RoutedEventArgs e)
        {
            TbX.Text = TbY.Text = TbZ.Text = TbResult.Text = "";
        }
    }
}

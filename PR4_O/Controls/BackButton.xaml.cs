using System.Windows;
using System.Windows.Controls;

namespace PR4_O.Controls
{
    public partial class BackButton : UserControl
    {
        public BackButton() => InitializeComponent();

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            if (Window.GetWindow(this) is MainWindow mw)
                mw.MainFrame.Navigate(new Pages.MainPage());
        }
    }
}

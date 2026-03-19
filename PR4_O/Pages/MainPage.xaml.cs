using System.Windows;
using System.Windows.Controls;

namespace PR4_O.Pages
{
    public partial class MainPage : Page
    {
        public MainPage() => InitializeComponent();

        private void Task1_Click(object sender, RoutedEventArgs e) => NavigationService.Navigate(new Task1Page());
        private void Task2_Click(object sender, RoutedEventArgs e) => NavigationService.Navigate(new Task2Page());
        private void Task3_Click(object sender, RoutedEventArgs e) => NavigationService.Navigate(new Task3Page());

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Вы действительно хотите выйти? Если нет, просто закройте это окно.",
                "Подтверждение выхода", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                Application.Current.Shutdown();
        }
    }
}

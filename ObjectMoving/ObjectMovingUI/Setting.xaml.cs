using System.Windows;

namespace ObjectMovingUI
{
    public partial class Setting : Window
    {
        MainWindow mw;
        public Setting(MainWindow mwa)
        {
            InitializeComponent();
            mw = mwa;
            isOnline.IsChecked = MainWindow.isOnline;
            Closing += Window_Closing;
            if (MainWindow.isOnline == true)
            {
                FTPType.Visibility = Visibility.Visible;
                SocketType.Visibility = Visibility.Visible;
                SocketType.IsChecked = true;
            }
        }

        private void hideButton(object sender, RoutedEventArgs e)
        { 
            Sp.Visibility = Visibility.Collapsed;
        }

        private void showButton(object sender, RoutedEventArgs e)
        {
            Sp.Visibility = Visibility.Visible;
        }

        private void showType(object sender, RoutedEventArgs e)
        {
            FTPType.Visibility = Visibility.Visible;
            SocketType.Visibility = Visibility.Visible;
            Sp.Visibility = Visibility.Visible;
        }

        private void save_Click(object sender, RoutedEventArgs e)
        {
            mw.changeSetting(isOnline.IsChecked, FTPType.IsChecked, "http://" + IPAdress.Text + ":" + Port.Text);
            Close();
        }

        private void cancel_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            e.Cancel = true;
            this.Visibility = Visibility.Hidden;
        }
    }
}

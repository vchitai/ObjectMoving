using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Check;
using System.Windows.Media.Imaging;
using System;

namespace ObjectMovingUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public KhoHang khoHang;
        public const int buttonHeight = 20;
        public const int buttonWidth = 40;
        public const int offset = 50;

        public MainWindow()
        {
            InitializeComponent();
            khoHang = new KhoHang();
            DrawArea.HorizontalScrollBarVisibility = ScrollBarVisibility.Auto;
            DrawArea.VerticalScrollBarVisibility = ScrollBarVisibility.Auto;
            StackPanel sp = new StackPanel();
            sp.Orientation = Orientation.Horizontal;
            for (int k = 0; k < khoHang.getSoLuongKhu(); k++)
            {
                KhuHang khuHang = khoHang.getKhu(k);
                WrapPanel wp = new WrapPanel();
                int row = khuHang.getNRow();
                int col = khuHang.getNCol();
                wp.Height = buttonHeight * row;
                wp.Width = buttonWidth * col;
                
                for (int j = 0; j < row; j++)
                {
                    for (int i = 0; i < col; i++)
                    {
                        Button btn = new Button();
                        btn.FontSize += 10;
                        btn.Height = buttonHeight;
                        btn.Width = buttonWidth;
                        btn.Content = btn.Name;
                        btn.Click += ButtonOnClick;

                        if (khuHang.get(j, i).getWidth() == 1)
                        {
                            var brush = new ImageBrush();
                            brush.ImageSource = new BitmapImage(new Uri("Resources/Box1.png", UriKind.Relative));
                            btn.Background = brush;
                        } else if (khuHang.get(j, i).getWidth() == 2)
                        {
                            var brush = new ImageBrush();
                            brush.ImageSource = new BitmapImage(new Uri("Resources/BoxHead.png", UriKind.Relative));
                            btn.Background = brush;
                        } else if (khuHang.get(j, i).getWidth() == -2)
                        {

                            var brush = new ImageBrush();
                            brush.ImageSource = new BitmapImage(new Uri("Resources/BoxTail.png", UriKind.Relative));
                            btn.Background = brush;
                        }
                        wp.Children.Add(btn);
                    }
                }

                RotateTransform rt = new RotateTransform();
                rt.Angle = khuHang.getAngle();

                wp.LayoutTransform = rt;
                wp.Margin = new Thickness(offset, 0, 0, 0);
                sp.Children.Add(wp);
            }
            DrawArea.Content = sp;
        }

        void ButtonOnClick(object sender, RoutedEventArgs args)
        {
            Button btn = args.Source as Button;

            MessageBox.Show("Button " + btn.Name + " has been clicked",
                            "Button Click");
        }
    }
}

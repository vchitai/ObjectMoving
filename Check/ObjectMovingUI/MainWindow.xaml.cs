using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Check;
using System.Windows.Media.Imaging;
using System;
using System.Collections.Generic;
using System.Windows.Input;

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

                List<List<KButton>> button_list;

                button_list = new List<List<ObjectMovingUI.KButton>>();

                for(int i = 0; i < row; ++i)
                {
                    button_list.Add(new List<KButton>());
                    for(int j = 0; j < col; ++j)
                    {
                        KButton btn = new KButton(khuHang.get(i, j));
                        btn.FontSize += 10;
                        btn.Height = buttonHeight;
                        btn.Width = buttonWidth;
                        btn.Content = btn.Name;
                        btn.Click += Btn_Click;
                        btn.MouseDown += Btn_MouseDown;
                        btn.MouseEnter += Btn_MouseEnter;

                        btn.setBackGround(khuHang.get(i, j).getWidth());

                        button_list[i].Add(btn);
                    }
                }

                for (int i = 0; i < row; ++i)
                {                    
                    for (int j = 0; j < col; ++j)
                    {
                        if (j == 0)
                            button_list[i][j].setLeft(null);
                        else
                            button_list[i][j].setLeft(button_list[i][j - 1]);

                        if (j == col - 1)
                            button_list[i][j].setRight(null);
                        else
                            button_list[i][j].setRight(button_list[i][j + 1]);
                    }
                }

                for (int i = 0; i < row; i++)
                {
                    for (int j = 0; j < col; j++)
                    {                        
                        wp.Children.Add(button_list[i][j]);
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

        private void Btn_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            KButton btn = e.Source as KButton;

            if (e.ChangedButton == MouseButton.Right)
            {
                KienHang k = btn.getKienHang();
                if (k.getWidth() != 0)
                {
                    PopUp p = new ObjectMovingUI.PopUp(btn.getKienHang());
                    p.Show();
                }
            }
            else
            {

            }
        }

        private void Btn_Click(object sender, RoutedEventArgs e)
        {
            KButton btn = e.Source as KButton;            
        }

        private void Btn_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            KButton btn = e.Source as KButton;            

            btn.ToolTip = btn.getInfo();            
            //btn.ToolTip = new PopUp();
        }
       
    }
}

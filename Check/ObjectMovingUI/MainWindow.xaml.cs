using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Check;
using System.Windows.Media.Imaging;
using System;
using System.Collections.Generic;
using System.Windows.Input;
using System.Windows.Threading;

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
        public const int offset = 200;
        public KButton draggedButton;
        public KButton droppedButton;
        private string title = "Hệ thống quản lý kho hàng";
        private Uri iconUri = new Uri("Resources/ico.png", UriKind.Relative);

        private List<List<List<KButton>>> list_button; 

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            App.Current.Shutdown();
        }

        public MainWindow()
        {
            InitializeComponent();
            this.Title = title;
            this.Icon = BitmapFrame.Create(new BitmapImage(iconUri));

            khoHang = new KhoHang();
            DrawArea.HorizontalScrollBarVisibility = ScrollBarVisibility.Auto;
            DrawArea.VerticalScrollBarVisibility = ScrollBarVisibility.Auto;
            StackPanel sp = new StackPanel();
            sp.Orientation = Orientation.Horizontal;
            list_button = new List<List<List<KButton>>>();
            for (int k = 0; k < khoHang.getSoLuongKhu(); k++)
            {
                KhuHang khuHang = khoHang.getKhu(k);
                WrapPanel wp = new WrapPanel();
                int row = khuHang.getNRow();
                int col = khuHang.getNCol();
                wp.Height = buttonHeight * (row+1);
                wp.Width = buttonWidth * col;

                List<List<KButton>> button_list;

                button_list = new List<List<ObjectMovingUI.KButton>>();
                Label khuHangName = new Label();
                khuHangName.Height = buttonHeight;
                khuHangName.Width = wp.Width;
                khuHangName.Content = "Khu hàng " + (k+1).ToString();
                khuHangName.HorizontalContentAlignment = HorizontalAlignment.Center;
                khuHangName.Padding = new Thickness(0, 0, 0, 0);

                wp.Children.Add(khuHangName);
                for(int i = 0; i < row; ++i)
                {
                    button_list.Add(new List<KButton>());
                    for(int j = 0; j < col; ++j)
                    {
                        KButton btn = new KButton(k + 1, i, j, khuHang.get(i, j));
                        btn.FontSize += 10;
                        btn.Height = buttonHeight;
                        btn.Width = buttonWidth;
                        btn.Content = btn.Name;
                        btn.Click += Btn_Click;
                        btn.MouseDown += Btn_MouseDown;
                        btn.MouseEnter += Btn_MouseEnter;
                        btn.MouseLeave += Btn_MouseLeave;
                        btn.setBackGround(khuHang.get(i, j).getWidth());
                        btn.AllowDrop = true;
                        btn.PreviewMouseMove += Btn_PreviewMouseMove;
                        btn.Drop += Btn_Drop;
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

                list_button.Add(button_list);
            }
            DrawArea.Content = sp;

            DrawArea.Loaded += Page_Loaded;
        }

        private void Btn_MouseLeave(object sender, MouseEventArgs e)
        {
            KButton btn = e.Source as KButton;

            btn.updateBackGround();
        }

        private void Btn_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            KButton btn = e.Source as KButton;

            if (e.ChangedButton == MouseButton.Right)
            {
                KienHang k = btn.getKienHang();
                if (k.getWidth() != 0)
                {                         
                    PopUp p = new ObjectMovingUI.PopUp(btn.getKienHang(), btn, khoHang);
                    if (p != null)
                    {
                        p.kichThuocEdit.Visibility = Visibility.Collapsed;
                        p.Show();
                    }
                }
                else
                {
                    PopUp p = new ObjectMovingUI.PopUp(btn.getKienHang(), btn, khoHang);
                    if (p != null)
                    {
                        p.kichThuoc.Visibility = Visibility.Collapsed;
                        p.delete.Visibility = Visibility.Collapsed;
                        p.Show();
                    }                    
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

            btn.Background = Brushes.LightBlue;     
            if (btn.getKienHang().getWidth() == -2)
            {
                btn.getLeft().Background = Brushes.LightBlue;                
            }
            if (btn.getKienHang().getWidth() == 2)
            {
                btn.getRight().Background = Brushes.LightBlue;                
            }
        }

        private void Btn_PreviewMouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                draggedButton = (KButton)sender;
                int x = 0;
                DragDrop.DoDragDrop((KButton)sender, x, DragDropEffects.All);
                e.Handled = true;
            }
        }

        private void Btn_Drop(object sender, DragEventArgs e)
        {
            if (draggedButton != null)
            {
                droppedButton = (KButton)sender;
                draggedButton.move(khoHang, droppedButton);
                draggedButton = null;
                khoHang.writeData();
            }
        }

        //On PageLoad, populate the grid, and set a timer to repeat ever 60 seconds
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {            
            RefreshData();
            SetTimer();            
        }

        //Refreshes grid data on timer tick
        protected void dispatcherTimer_Tick(object sender, EventArgs e)
        {
            RefreshData();
        }

        //Get data and bind to the grid
        private void RefreshData()
        {
            DrawArea.InvalidateVisual();
        }

        //Set and start the timer
        private void SetTimer()
        {
            DispatcherTimer dispatcherTimer = new DispatcherTimer();
            dispatcherTimer.Tick += new EventHandler(dispatcherTimer_Tick);
            dispatcherTimer.Interval = new TimeSpan(0, 0, 20);
            dispatcherTimer.Start();
        }
    }
}

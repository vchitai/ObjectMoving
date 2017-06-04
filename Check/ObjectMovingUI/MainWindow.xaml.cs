using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Check;
using System.Windows.Media.Imaging;
using System;
using System.Collections.Generic;
using System.Windows.Input;
using System.Windows.Threading;
using System.IO;

namespace ObjectMovingUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private KhoHang khoHang;
        private const int buttonHeight = 20;
        private const int buttonWidth = 40;
        private const int offset = 200;
        private StackPanel sp;
        private KButton draggedButton;
        private KButton droppedButton;
        private string title = "Hệ thống quản lý kho hàng";
        private string guideFile = "../../Resources/Guide.txt";
        private Uri iconUri = new Uri("Resources/ico.png", UriKind.Relative);
        private WrapPanel wpCurrent = null;
        private float angleCurrent = 0;

        System.Windows.Threading.DispatcherTimer dispatcherTimer = new System.Windows.Threading.DispatcherTimer();
                
        private TextBox selTb = null;

        private List<List<List<KButton>>> list_button; 

        protected override void OnClosed(EventArgs e)
        {
            if (isOnline.IsChecked == true)
                KhoHang.uploadFile();
            base.OnClosed(e);
            App.Current.Shutdown();
        }

        public MainWindow()
        {
            InitializeComponent();
            this.Title = title;
            this.Icon = BitmapFrame.Create(new BitmapImage(iconUri));

            dispatcherTimer.Tick += new EventHandler(dispatcherTimer_Tick);
            dispatcherTimer.Interval = new TimeSpan(0, 0, 10);
            dispatcherTimer.Start();
            
            startPos.GotMouseCapture += StartPos_GotMouseCapture;
            endPos.GotMouseCapture += EndPos_GotMouseCapture;
            startPos.IsReadOnly = false;
            endPos.IsReadOnly = false;            

            if (isOnline.IsChecked == true)
                KhoHang.downloadFile();
            GenerateMap();
        }

        private void GenerateMap(string fileName = "../../Resources/input.txt")
        {
            sldZoom.IsEnabled = false;
            khoHang = new KhoHang(fileName);
            DrawArea.HorizontalScrollBarVisibility = ScrollBarVisibility.Auto;
            DrawArea.VerticalScrollBarVisibility = ScrollBarVisibility.Auto;
            sp = new StackPanel();
            sp.Orientation = Orientation.Horizontal;
            if (list_button != null)
                list_button.Clear();
            list_button = new List<List<List<KButton>>>();
            for (int k = 0; k < khoHang.getSoLuongKhu(); k++)
            {
                KhuHang khuHang = khoHang.getKhu(k);
                WrapPanel wp = new WrapPanel();
                int row = khuHang.getNRow();
                int col = khuHang.getNCol();
                wp.Height = buttonHeight * (row + 1);
                wp.Width = buttonWidth * col;

                List<List<KButton>> button_list;

                button_list = new List<List<ObjectMovingUI.KButton>>();
                Label khuHangName = new Label();
                khuHangName.Height = buttonHeight;
                khuHangName.Width = wp.Width;
                khuHangName.Content = "Khu hàng " + (k + 1).ToString();
                khuHangName.HorizontalContentAlignment = HorizontalAlignment.Center;
                khuHangName.Padding = new Thickness(0, 0, 0, 0);

                wp.Children.Add(khuHangName);
                for (int i = 0; i < row; ++i)
                {
                    button_list.Add(new List<KButton>());
                    for (int j = 0; j < col; ++j)
                    {
                        KButton btn = new KButton(k + 1, i, j, khuHang.get(i, j));
                        btn.FontSize += 10;
                        btn.Height = buttonHeight;
                        btn.Width = buttonWidth;
                        btn.Content = btn.Name;
                        btn.Click += Btn_Click;
                        btn.MouseDown += Btn_MouseDown;
                        btn.GotMouseCapture += Btn_GotMouseCapture;
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

        private void GenerateMapZoom1Khu(int khu = 1, string fileName = "../../Resources/input.txt")
        {
            sldZoom.IsEnabled = true;
            khoHang = new KhoHang(fileName);
            DrawArea.HorizontalScrollBarVisibility = ScrollBarVisibility.Auto;
            DrawArea.VerticalScrollBarVisibility = ScrollBarVisibility.Auto;
            sp = new StackPanel();
            sp.Orientation = Orientation.Horizontal;
            if (list_button != null)
                list_button.Clear();
            list_button = new List<List<List<KButton>>>();
            khu--;
            {
                KhuHang khuHang = khoHang.getKhu(khu);
                WrapPanel wp = new WrapPanel();
                int row = khuHang.getNRow();
                int col = khuHang.getNCol();
                wp.Height = buttonHeight * (row + 1);
                wp.Width = buttonWidth * col;

                List<List<KButton>> button_list;

                button_list = new List<List<ObjectMovingUI.KButton>>();
                Label khuHangName = new Label();
                khuHangName.Height = buttonHeight;
                khuHangName.Width = wp.Width;
                khuHangName.Content = "Khu hàng " + (khu + 1).ToString();
                khuHangName.HorizontalContentAlignment = HorizontalAlignment.Center;
                khuHangName.Padding = new Thickness(0, 0, 0, 0);

                wp.Children.Add(khuHangName);
                for (int i = 0; i < row; ++i)
                {
                    button_list.Add(new List<KButton>());
                    for (int j = 0; j < col; ++j)
                    {
                        KButton btn = new KButton(khu + 1, i, j, khuHang.get(i, j));
                        btn.FontSize += 10;
                        btn.Height = buttonHeight;
                        btn.Width = buttonWidth;
                        btn.Content = btn.Name;
                        btn.Click += Btn_Click;
                        btn.MouseDown += Btn_MouseDown;
                        btn.GotMouseCapture += Btn_GotMouseCapture;
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
               
                sp.Children.Add(wp);
                sp.HorizontalAlignment = HorizontalAlignment.Center;
                list_button.Add(button_list);
                wpCurrent = wp;
                angleCurrent = khuHang.getAngle();
                sldZoom.Value = 50;
            }
            
            DrawArea.Content = sp;

            DrawArea.Loaded += Page_Loaded;
        }

        private void Btn_GotMouseCapture(object sender, MouseEventArgs e)
        {
            if (selTb != null)
                selTb.Text = ((KButton)sender).getPos();
        }

        private void EndPos_GotMouseCapture(object sender, MouseEventArgs e)
        {
            selTb = (TextBox)sender;
            selTb.Text = "";
        }

        private void StartPos_GotMouseCapture(object sender, MouseEventArgs e)
        {
            selTb = (TextBox)sender;
            selTb.Text = "";
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
                    PopUp p = new ObjectMovingUI.PopUp(btn.getKienHang(), btn, khoHang, (bool)isOnline.IsChecked);
                    if (p != null)
                    {
                        p.kichThuocEdit.Visibility = Visibility.Collapsed;
                        p.Show();
                    }
                }
                else
                {
                    PopUp p = new ObjectMovingUI.PopUp(btn.getKienHang(), btn, khoHang, (bool)isOnline.IsChecked);
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
                if (isOnline.IsChecked == true)
                    KhoHang.uploadFile();
            }
        }
        
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {            
            RefreshData();                  
        }
        
        protected void dispatcherTimer_Tick(object sender, EventArgs e)
        {
            if (isOnline.IsChecked == true)
            {
                KhoHang.downloadFile();
                khoHang.loadData();
            }
            foreach (var lv1 in list_button)
            {
                foreach (var lv2 in lv1)
                {
                    foreach (KButton k in lv2)
                        if (k.Background != Brushes.LightBlue)
                            k.updateBackGround();
                }
            }
            RefreshData();
        }
        
        private void RefreshData()
        {
            DrawArea.InvalidateVisual();
        }          

        private void move_Click(object sender, RoutedEventArgs e)
        {
            string command = "M " + startPos.Text + " " + endPos.Text;
            khoHang.moveByCommand(command);
            foreach (var lv1 in list_button)
            {
                foreach (var lv2 in lv1)
                {
                    foreach (KButton k in lv2)
                        k.updateBackGround();
                }
            }
            khoHang.writeData();
            if (isOnline.IsChecked == true)
                KhoHang.uploadFile();
        }       

        public void open_Click(object sender, RoutedEventArgs e)
        {
            // Create OpenFileDialog 
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();

            // Set filter for file extension and default file extension 
            dlg.DefaultExt = ".txt";
            dlg.Filter = "Text Files (*.txt)|*.txt";

            // Display OpenFileDialog by calling ShowDialog method 
            Nullable<bool> result = dlg.ShowDialog();


            // Get the selected file name and display in a TextBox 
            if (result == true)
            {
                // Open document 
                string filename = dlg.FileName;
                GenerateMap(filename);
                isOnline.Visibility = Visibility.Hidden;
                DrawArea.InvalidateVisual();
            }
        }

        public void save_Click(object sender, RoutedEventArgs e)
        {
            // Create OpenFileDialog 
            Microsoft.Win32.SaveFileDialog dlg = new Microsoft.Win32.SaveFileDialog();

            // Set filter for file extension and default file extension 
            dlg.DefaultExt = ".txt";
            dlg.Filter = "Text Files (*.txt)|*.txt|All Files (*.*)|*.*";

            // Display OpenFileDialog by calling ShowDialog method 
            Nullable<bool> result = dlg.ShowDialog();


            // Get the selected file name and display in a TextBox 
            if (result == true)
            {
                // Open document 
                string filename = dlg.FileName;
                khoHang.saveTo(filename);
            }
        }

        public void quit_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
        
        public void guide_Click(object sender, RoutedEventArgs e)
        {
            StreamReader file;
            try
            {
                file = new StreamReader(guideFile);
            }
            catch (IOException)
            {
                //the file is unavailable because it is:
                //still being written to
                //or being processed by another thread
                //or does not exist (has already been processed)
                return;
            }

            MessageBox.Show(file.ReadToEnd(), "Hướng dẫn sử dụng");

        }

        public void zoom_Kho1(object sender, RoutedEventArgs e)
        {
            GenerateMapZoom1Khu(1);
        }

        public void zoom_Kho2(object sender, RoutedEventArgs e)
        {
            GenerateMapZoom1Khu(2);
        }

        public void zoom_Kho3(object sender, RoutedEventArgs e)
        {
            GenerateMapZoom1Khu(3);
        }

        public void zoom_All(object sender, RoutedEventArgs e)
        {
            GenerateMap();
            wpCurrent = null;
        }

        private void sldZoom_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            Slider sld = sender as Slider;
            TransformGroup transfrom = new TransformGroup();
            RotateTransform rt = new RotateTransform();
            rt.Angle = angleCurrent;
            ScaleTransform sc = new ScaleTransform(0.5 + 0.5 * (sld.Value / 50), 0.5 + 0.5 * (sld.Value / 50));
            transfrom.Children.Add(rt);
            transfrom.Children.Add(sc);

            if (wpCurrent != null)
                wpCurrent.LayoutTransform = transfrom;
        }

        private void DrawArea_DragEnter(object sender, DragEventArgs e)
        {
            MessageBox.Show("drag");
        }
    }
    
}

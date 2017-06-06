using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using ObjectMovingConsole;
using System.Windows.Media.Imaging;
using System;
using System.Collections.Generic;
using System.Windows.Input;
using System.Windows.Threading;
using System.IO;

namespace ObjectMovingUI
{
    public partial class MainWindow : Window
    {
        #region staticVariable
        private static string title = "Hệ thống quản lý kho hàng"; // Tiêu đề chương trình
        private static string guideFile = "../../../Resources/Guide.txt"; // Đường dẫn file Hướng dẫn
        private static Uri iconUri = new Uri("../../../Resources/ico.png", UriKind.Relative); // Đường dẫn icon chương trình
        #endregion

        #region variable
        private static KhoHang khoHang; // Kho Hàng
        private const int buttonHeight = 20; // Chiều dài các Button
        private const int buttonWidth = 40; // Chiều rộng các Button

        private StackPanel stackPanel; // Khu vực chứa các khu hàng
        private KButton draggedButton; // Nút được kéo đi
        private KButton droppedButton; // Nút được thả xuống
        private WrapPanel currentWrapPanel = null; // Khu hàng hiện đang xử lý
        private float currentAngle = 0; // Góc xoay hiện tại
        private double[] posXkhuHang = new double[3] { -1, -1, -1 }; // Tọa độ X của khu hàng
        private double[] posYkhuHang = new double[3] { -1, -1, -1 }; // Tọa độ Y của khu hàng
        private const int drawAreaPaddingX = 20; // Padding của khu vực vẽ
        private const int drawAreaPaddingY = 20; // Padding của khu vực vẽ

        private const int refreshTime = 10; // Thời gian refreshData (giây)
        private DispatcherTimer dispatcherTimer = new DispatcherTimer(); // Bộ đếm giờ
        private TextBox selTb = null; // Textbox đang xử lý
        private List<List<List<KButton>>> buttonList; // Danh sách các Button

        private bool mouseDown = false; // Trạng thái đang giữ của chuột
        private Point mouseDownPos; // Điểm click chuột
        #endregion

        #region OverrideMethod
        protected override void OnClosed(EventArgs e)
        {
            // Upload file trước khi đóng chương trình
            if (isOnline.IsChecked == true)
                KhoHang.uploadFile();
            // Đóng các cửa sổ đang mở
            base.OnClosed(e);
            // Tắt chương trình
            App.Current.Shutdown();
        }
        #endregion
        
        public MainWindow()
        {
            #region initialize
            // Khởi tạo các tham số ứng dụng
            InitializeComponent();
            Title = title;
            Icon = BitmapFrame.Create(new BitmapImage(iconUri));
            #endregion

            #region timerStart
            // Bắt đầu bộ đếm giờ
            dispatcherTimer.Tick += new EventHandler(dispatcherTimer_Tick);
            dispatcherTimer.Interval = new TimeSpan(0, 0, refreshTime);
            dispatcherTimer.Start();
            #endregion

            #region captureMouseOnTextBox
            // Theo dõi chuột trên Textbox
            startPos.GotMouseCapture += StartPos_GotMouseCapture;
            endPos.GotMouseCapture += EndPos_GotMouseCapture;
            startPos.IsReadOnly = false;
            endPos.IsReadOnly = false;
            #endregion

            #region drawMap
            // Download dữ liệu nếu đang trực tuyến và vẽ Kho Hàng
            if (isOnline.IsChecked == true)
                KhoHang.downloadFile();
            GenerateMap();
            GenerateZoomMenu();
            #endregion
        }

        #region dragToSelectFeature
        // Event nhấn chuột
        private void Grid_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (currentWrapPanel != null)
                return;
            // Bắt đầu theo dõi chuột
            mouseDown = true;
            mouseDownPos = e.GetPosition(theGrid);
            theGrid.CaptureMouse();

            // Khởi tạo vùng chọn kéo thả         
            Canvas.SetLeft(selectionBox, mouseDownPos.X);
            Canvas.SetTop(selectionBox, mouseDownPos.Y);
            selectionBox.Width = 0;
            selectionBox.Height = 0;

            // Hiển thị vùng chọn của chuột
            selectionBox.Visibility = Visibility.Visible;
        }

        private double TinhKhoangCach(double x1 = 0, double y1 = 0, double x2 = 0, double y2 = 0)
        {
            return Math.Sqrt((x2 - x1) * (x2 - x1) + (y2 - y1) * (y2 - y1));
        }
        
        //Event thả chuột
        private void Grid_MouseUp(object sender, MouseButtonEventArgs e)
        {
            // Thả chuột và ngừng theo dõi
            mouseDown = false;
            theGrid.ReleaseMouseCapture();

            // Ẩn vùng chọn
            selectionBox.Visibility = Visibility.Collapsed;

            // Lấy tọa độ để xử lý
            Point mouseUpPos = e.GetPosition(theGrid);
            double left = Canvas.GetLeft(selectionBox);
            double top = Canvas.GetTop(selectionBox);
            double right = left + selectionBox.Width;
            double bottom = top + selectionBox.Height;

            // Bắt đầu xử lý, pos là số thứ tự khu hàng
            int pos = -1;
            bool ok = false;
            foreach (var lv1 in buttonList)
            {
                pos = pos + 1;
                foreach (var lv2 in lv1)
                {
                    foreach (KButton kk in lv2)
                    {
                        Point relativePoint = kk.TransformToAncestor(this).Transform(new Point(0, 0));
                        double x = relativePoint.X;
                        double y = relativePoint.Y;
                        if (left <= x && right >= x && top <= y && y <= bottom)
                        {
                            ok = true;
                            break;
                        }
                    }
                    if (ok) break;
                }
                if (ok) break;
            }

            // Nếu chọn được, ta vẽ map và thiết lập lại các thông số
            if (ok)
                GenerateMapZoom1Khu(pos + 1);
            Canvas.SetLeft(selectionBox, 0);
            Canvas.SetTop(selectionBox, 0);
            selectionBox.Width = 0;
            selectionBox.Height = 0;

        }

        // Event di chuyển thuột
        private void Grid_MouseMove(object sender, MouseEventArgs e)
        {
            if (mouseDown)
            {
                // Khi chuột vẫn đang giữ, vẽ lại vùng chọn
                Point mousePos = e.GetPosition(theGrid);

                if (mouseDownPos.X < mousePos.X)
                {
                    Canvas.SetLeft(selectionBox, mouseDownPos.X);
                    selectionBox.Width = mousePos.X - mouseDownPos.X;
                }
                else
                {
                    Canvas.SetLeft(selectionBox, mousePos.X);
                    selectionBox.Width = mouseDownPos.X - mousePos.X;
                }

                if (mouseDownPos.Y < mousePos.Y)
                {
                    Canvas.SetTop(selectionBox, mouseDownPos.Y);
                    selectionBox.Height = mousePos.Y - mouseDownPos.Y;
                }
                else
                {
                    Canvas.SetTop(selectionBox, mousePos.Y);
                    selectionBox.Height = mouseDownPos.Y - mousePos.Y;
                }
            }
        }
        #endregion

        #region generateMapFeature
        private void GenerateMap(string fileName = "../../../Resources/input.txt")
        {
            // Khởi tạo các biến số kho hàng, stackPanel, buttonList và drawArea
            drawArea.HorizontalScrollBarVisibility = ScrollBarVisibility.Auto;
            drawArea.VerticalScrollBarVisibility = ScrollBarVisibility.Auto;
            drawArea.Padding = new Thickness(drawAreaPaddingX, drawAreaPaddingY, drawAreaPaddingX, drawAreaPaddingY);
            khoHang = new KhoHang(fileName);
            stackPanel = new StackPanel();
            stackPanel.Orientation = Orientation.Horizontal;
            if (buttonList != null)
                buttonList.Clear();
            buttonList = new List<List<List<KButton>>>();
            int currentPosX = 0;
            int currentPosY = 0;

            for (int k = 0; k < khoHang.getSoLuongKhu(); k++)
            {
                // Khởi tạo các biến số khu hàng, wrapPanel
                KhuHang khuHang = khoHang.getKhu(k);
                WrapPanel wrapPanel = new WrapPanel();
                int row = khuHang.getNRow();
                int col = khuHang.getNCol();
                wrapPanel.Height = buttonHeight * (row + 1);
                wrapPanel.Width = buttonWidth * col;
                wrapPanel.VerticalAlignment = VerticalAlignment.Top;
                List<List<KButton>> button_list;

                button_list = new List<List<KButton>>();
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
                        btn.AllowDrop = true;
                        btn.PreviewMouseMove += Btn_PreviewMouseMove;
                        btn.Drop += Btn_Drop;
                        btn.MouseDown += Btn_MouseDown;
                        btn.GotMouseCapture += Btn_GotMouseCapture;
                        btn.MouseEnter += Btn_MouseEnter;
                        btn.MouseLeave += Btn_MouseLeave;
                        btn.upBackGround();
                        button_list[i].Add(btn);
                    }
                }

                // Thiết lập buttonList
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
                        wrapPanel.Children.Add(button_list[i][j]);
                    }
                }

                // Tạo hiệu ứng xoay khu hàng
                RotateTransform rotateTransform = new RotateTransform();
                rotateTransform.Angle = khuHang.getAngle();
                wrapPanel.LayoutTransform = rotateTransform;

                // Thiết đặt tọa độ cho khu hàng
                wrapPanel.Margin = new Thickness(khuHang.getPosX() * buttonWidth - currentPosX, khuHang.getPosY() * buttonHeight - currentPosY, 0, 0);
                currentPosX = (khuHang.getPosX() + col) * buttonWidth;
                currentPosX = (khuHang.getPosY() + row) * buttonHeight;
                stackPanel.Children.Add(wrapPanel);

                // Thêm tên khu hàng
                Label khuHangName = new Label();
                khuHangName.Height = buttonHeight;
                khuHangName.Width = wrapPanel.Width;
                khuHangName.Content = "Khu hàng " + (k + 1).ToString();
                khuHangName.HorizontalContentAlignment = HorizontalAlignment.Center;
                khuHangName.Padding = new Thickness(0);
                wrapPanel.Children.Add(khuHangName);

                buttonList.Add(button_list);
            }
            currentAngle = 0;
            sldZoom.Value = 50;
            sldZoom.IsEnabled = true;
            drawArea.Content = stackPanel;
            drawArea.Loaded += Page_Loaded;
        }

        private void GenerateMapZoom1Khu(int khu = 1, string fileName = "../../../Resources/input.txt")
        {
            // Khởi tạo các biến số kho hàng, stackPanel, buttonList và drawArea
            drawArea.HorizontalScrollBarVisibility = ScrollBarVisibility.Auto;
            drawArea.VerticalScrollBarVisibility = ScrollBarVisibility.Auto;
            drawArea.Padding = new Thickness(drawAreaPaddingX, drawAreaPaddingY, drawAreaPaddingX, drawAreaPaddingY);
            khoHang = new KhoHang(fileName);
            stackPanel = new StackPanel();
            stackPanel.Orientation = Orientation.Horizontal;
            if (buttonList != null)
                buttonList.Clear();
            buttonList = new List<List<List<KButton>>>();
            khu--;
            {
                // Khởi tạo các biến số khu hàng, wrapPanel
                KhuHang khuHang = khoHang.getKhu(khu);
                WrapPanel wrapPanel = new WrapPanel();
                int row = khuHang.getNRow();
                int col = khuHang.getNCol();
                wrapPanel.Height = buttonHeight * (row + 1);
                wrapPanel.Width = buttonWidth * col;
                wrapPanel.VerticalAlignment = VerticalAlignment.Top;
                List<List<KButton>> button_list;

                button_list = new List<List<KButton>>();
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
                        btn.AllowDrop = true;
                        btn.PreviewMouseMove += Btn_PreviewMouseMove;
                        btn.Drop += Btn_Drop;
                        btn.MouseDown += Btn_MouseDown;
                        btn.GotMouseCapture += Btn_GotMouseCapture;
                        btn.MouseEnter += Btn_MouseEnter;
                        btn.MouseLeave += Btn_MouseLeave;
                        btn.upBackGround();
                        button_list[i].Add(btn);
                    }
                }

                // Thiết lập buttonList
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
                        wrapPanel.Children.Add(button_list[i][j]);
                    }
                }

                // Tạo hiệu ứng xoay khu hàng
                RotateTransform rotateTransform = new RotateTransform();
                rotateTransform.Angle = khuHang.getAngle();
                wrapPanel.LayoutTransform = rotateTransform;

                // Thêm tên khu hàng
                Label khuHangName = new Label();
                khuHangName.Height = buttonHeight;
                khuHangName.Width = wrapPanel.Width;
                khuHangName.Content = "Khu hàng " + (khu + 1).ToString();
                khuHangName.HorizontalContentAlignment = HorizontalAlignment.Center;
                khuHangName.Padding = new Thickness(0);

                stackPanel.Children.Add(wrapPanel);
                stackPanel.HorizontalAlignment = HorizontalAlignment.Center;
                stackPanel.VerticalAlignment = VerticalAlignment.Center;
                buttonList.Add(button_list);
                currentWrapPanel = wrapPanel;
                currentAngle = khuHang.getAngle();
                sldZoom.Value = 50;
                wrapPanel.Children.Add(khuHangName);
            }
            drawArea.Content = stackPanel;
            drawArea.Loaded += Page_Loaded;
        }
        #endregion

        #region editKienHang
        // Event nhấn chuột
        private void Btn_MouseDown(object sender, MouseButtonEventArgs e)
        {
            KButton btn = e.Source as KButton;
            
            //Nếu nhấn chuột phải thì xử lý
            if (e.ChangedButton == MouseButton.Right)
            {
                KienHang k = btn.getKienHang();
                // Nếu tồn tại kiện hàng
                if (k.getWidth() != 0)
                {
                    // PopUp cho phép sửa kiện hàng
                    PopUp p = new PopUp(btn.getKienHang(), btn, khoHang, (bool)isOnline.IsChecked);
                    if (p != null)
                    {
                        p.kichThuocEdit.Visibility = Visibility.Collapsed;
                        p.Show();
                    }
                }
                // Nếu không tồn tại kiện hàng
                else
                {
                    // PopUp cho phép thêm kiện hàng
                    PopUp p = new PopUp(btn.getKienHang(), btn, khoHang, (bool)isOnline.IsChecked);
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
        #endregion
        
        #region showInfo&EffectHoverOnButton
        // Event lướt chuột qua Button
        private void Btn_MouseEnter(object sender, MouseEventArgs e)
        {
            KButton btn = e.Source as KButton;

            // Hiển thị tooltip là thông tin kho hàng
            btn.ToolTip = btn.getInfo();

            // Thay đổi màu của Button
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
        
        // Event dời chuột khỏi Button
        private void Btn_MouseLeave(object sender, MouseEventArgs e)
        {
            KButton btn = e.Source as KButton;

            // Thiết lập màu Button về mặc định
            btn.updateBackGround();
        }
        #endregion

        #region dragDropFeature
        // Event theo dõi di chuyển của chuột
        private void Btn_PreviewMouseMove(object sender, MouseEventArgs e)
        {
            // Nếu nhấn chuột
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                // Lưu lại nút bị kéo
                draggedButton = (KButton)sender;
                int x = 0;
                DragDrop.DoDragDrop((KButton)sender, x, DragDropEffects.All);
                e.Handled = true;
            }
        }

        // Event thả chuột
        private void Btn_Drop(object sender, DragEventArgs e)
        {
            // Nếu đã kéo một nút nào đó
            if (draggedButton != null)
            {
                // Bắt đầu di chuyển
                droppedButton = (KButton)sender;
                draggedButton.move(khoHang, droppedButton);
                draggedButton = null;
                khoHang.writeData();
                // Cập nhật thông tin nếu đang trực tuyến
                if (isOnline.IsChecked == true)
                    KhoHang.uploadFile();
            }
        }
        #endregion

        #region refreshData
        // Refresh dữ liệu khi load trang
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            RefreshData();
        }

        // Bộ đếm giờ làm việc
        protected void dispatcherTimer_Tick(object sender, EventArgs e)
        {
            // Nếu đang trực tuyến thì cập nhật
            if (isOnline.IsChecked == true)
            {
                KhoHang.downloadFile();
                khoHang.loadData();
            }
            foreach (var lv1 in buttonList)
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

        // Refresh data bằng cách cập nhật lại hình ảnh
        private void RefreshData()
        {
            drawArea.InvalidateVisual();
        }
        #endregion

        #region moveUsingTextBoxFeature
        // Reset text nếu bấm chuột vào các ô nhập tọa độ di chuyển
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

        // Lấy tọa độ và truyền command về kho hàng để xử lý
        private void move_Click(object sender, RoutedEventArgs e)
        {
            string command = "M " + startPos.Text + " " + endPos.Text;
            khoHang.moveByCommand(command);
            foreach (var lv1 in buttonList)
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
        #endregion

        #region fileMenuFeature
        // Mở file tùy chọn
        public void open_Click(object sender, RoutedEventArgs e)
        {
            // Mở hộp thoại chọn file và thiết lập chỉ chọn file txt, kiểm tra khả năng truy xuất
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            dlg.DefaultExt = ".txt";
            dlg.Filter = "Text Files (*.txt)|*.txt";
            bool? result = dlg.ShowDialog();

            // Nếu có khả năng truy xuất thì xử lý file và cập nhật dữ liệu
            if (result == true)
            {
                string filename = dlg.FileName;
                GenerateMap(filename);
                isOnline.Visibility = Visibility.Hidden;
                drawArea.InvalidateVisual();
            }
        }

        // Lưu file tùy chọn
        public void save_Click(object sender, RoutedEventArgs e)
        {
            // Mở hộp thoại chọn file và thiết lập chỉ chọn file txt, kiểm tra khả năng truy xuất
            Microsoft.Win32.SaveFileDialog dlg = new Microsoft.Win32.SaveFileDialog();
            dlg.DefaultExt = ".txt";
            dlg.Filter = "Text Files (*.txt)|*.txt|All Files (*.*)|*.*";
            bool? result = dlg.ShowDialog();

            // Nếu có khả năng truy xuất thì lưu file
            if (result == true)
            {
                string filename = dlg.FileName;
                khoHang.saveTo(filename);
            }
        }

        // Thoát chương trình
        public void quit_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
        #endregion

        #region guideMenuFeature
        // Mở file hướng dẫn sử dụng
        public void guide_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                StreamReader file = new StreamReader(guideFile);
                MessageBox.Show(file.ReadToEnd(), "Hướng dẫn sử dụng");
            }
            catch (IOException)
            {
                MessageBox.Show("Không tìm thấy tập tin Hướng dẫn", "Lỗi truy xuất tập tin");
                return;
            }
        }
        #endregion

        #region zoomMenuFeature
        // Tạo menu phóng to gồm các khu hàng và gán sự kiện cho các menu
        public void GenerateZoomMenu()
        {
            for (int i = 0; i < khoHang.getSoLuongKhu(); i++)
            {
                MenuItem zoom_khu_i = new MenuItem();
                zoom_khu_i.Header = "Khu hàng " + (i + 1);
                zoom_khu_i.Click += zoom_1kho;
                zoom_Menu.Items.Add(zoom_khu_i);
            }
        }

        public void zoom_1kho(object sender, RoutedEventArgs e)
        {
            MenuItem temp = (MenuItem)sender;
            string s = temp.Header as string;
            string s1 = s.Substring(9);
            GenerateMapZoom1Khu(int.Parse(s1));
        }

        public void zoom_All(object sender, RoutedEventArgs e)
        {
            currentWrapPanel = null;
            GenerateMap();
        }
        #endregion

        #region zoomSliderFeature
        // Lấy giá trị trong Slider và bắt đầu phóng to các phần cần thiết
        private void sldZoom_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            Slider sld = sender as Slider;
            TransformGroup transform = new TransformGroup();
            RotateTransform rt = new RotateTransform();
            ScaleTransform sc = new ScaleTransform(0.5 + 0.5 * (sld.Value / 50), 0.5 + 0.5 * (sld.Value / 50));
            rt.Angle = currentAngle;
   
            transform.Children.Add(rt);
            transform.Children.Add(sc);

            if (currentWrapPanel != null)
                currentWrapPanel.LayoutTransform = transform;
            else
                if (stackPanel != null)
                stackPanel.LayoutTransform = transform;
        }
        #endregion
    }

}

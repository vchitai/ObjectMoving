using ObjectMovingConsole;
using System.Windows;

namespace ObjectMovingUI
{
    public partial class PopUp : Window
    {
        #region variable
        private KhoHang khoHang;
        private KienHang k;
        private KButton b;
        private bool isOn;
        #endregion

        #region constructor
        // Khởi tạo các biến
        public PopUp(KienHang kienHang, KButton _b, KhoHang _khoHang, bool isOnline)
        {            
            InitializeComponent();
            k = kienHang;
            b = _b;
            khoHang = _khoHang;
            isOn = isOnline;
            this.kichThuoc.Content = kienHang.getWidthInfo();
            this.ngay.Text = kienHang.getNgayInfo();
            this.thang.Text = kienHang.getThangInfo();
            this.nam.Text = kienHang.getNamInfo();
            this.maKien.Text = kienHang.getMaKienInfo();
            this.donGia.Text = kienHang.getDonGiaInfo();
        }
        #endregion

        #region setVariable
        // Thiết lập các biến
        public void set(KienHang kienHang)
        {
            k = kienHang;
            this.kichThuoc.Content = kienHang.getWidthInfo();
            this.ngay.Text = kienHang.getNgayInfo();
            this.thang.Text = kienHang.getThangInfo();
            this.nam.Text = kienHang.getNamInfo();
            this.maKien.Text = kienHang.getMaKienInfo();
            this.donGia.Text = kienHang.getDonGiaInfo();
        }
        #endregion

        #region clickEvents
        // Event Click nút Save
        private void save_Click(object sender, RoutedEventArgs e)
        {
            // Nếu tồn tại kiện hàng, thì cập nhật thông tin kiện hàng
            if (k.getWidth() != 0)
            {
                int r = k.setInfo(this.ngay.Text, this.thang.Text, this.nam.Text, this.maKien.Text, this.donGia.Text);
                if (r < 0)
                {
                    if (r == -2)
                        MessageBox.Show("Ngày tháng năm nhập vào không hợp lệ.");
                    if (r == -3)
                        MessageBox.Show("Đơn giá nhập vào không hợp lệ.");
                }
                else
                {
                    // Cập nhật hiển thị và update dữ liệu nếu online
                    b.updateBackGround();
                    this.Close();
                    khoHang.writeData();
                    if (isOn == true)
                        KhoHang.uploadFile();
                }
            }
            // Nếu không tồn tại, ta tạo kiện hàng mới
            else
            {
                int r = k.setInfo2(this.kichThuocEdit.Text, this.ngay.Text, this.thang.Text, this.nam.Text, this.maKien.Text, this.donGia.Text);
                if (r < 0)
                {
                    if (r == -1)
                        MessageBox.Show("Kích thước nhập vào không hợp lệ (Kích thước từ 1-2).");
                    if (r == -2)
                        MessageBox.Show("Ngày tháng năm nhập vào không hợp lệ.");
                    if (r == -3)
                        MessageBox.Show("Đơn giá nhập vào không hợp lệ.");
                    if (r == -4)
                        MessageBox.Show("Kiện hàng thêm vào vị trí không hợp lệ.");
                }
                else
                {
                    // Cập nhật hiển thị và update dữ liệu nếu online
                    b.updateBackGround();
                    this.Close();
                    khoHang.writeData();
                    if (isOn == true)
                        KhoHang.uploadFile();
                }
            }
        }

        // Event Click nút Cancel
        private void cancel_Click(object sender, RoutedEventArgs e)
        {            
            // Đóng Popup
            this.Close();
        }

        // Event Click nút Delete
        private void delete_Click(object sender, RoutedEventArgs e)
        {
            // Xóa kiện hàng và cập nhật hiển thị và update dữ liệu nếu online
            k.refresh();
            b.updateBackGround();
            this.Close();
            khoHang.writeData();
            if (isOn == true)
                KhoHang.uploadFile();
        }
        #endregion
    }
}

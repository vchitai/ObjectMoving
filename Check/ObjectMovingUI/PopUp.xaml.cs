using Check;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace ObjectMovingUI
{
    /// <summary>
    /// Interaction logic for PopUp.xaml
    /// </summary>
    public partial class PopUp : Window
    {
        private KhoHang khoHang;
        private KienHang k;
        private KButton b;
        private bool isOn;

        public PopUp(KienHang kienHang, KButton _b, KhoHang _khoHang, bool isOnline)
        {            
            InitializeComponent();
            k = kienHang;
            b = _b;
            khoHang = _khoHang;
            isOn = isOnline;
            this.kichThuoc.Text = kienHang.getWidthInfo();
            this.ngay.Text = kienHang.getNgayInfo();
            this.thang.Text = kienHang.getThangInfo();
            this.nam.Text = kienHang.getNamInfo();
            this.maKien.Text = kienHang.getMaKienInfo();
            this.donGia.Text = kienHang.getDonGiaInfo();
        }

        public void set(KienHang kienHang)
        {
            k = kienHang;
            this.kichThuoc.Text = kienHang.getWidthInfo();
            this.ngay.Text = kienHang.getNgayInfo();
            this.thang.Text = kienHang.getThangInfo();
            this.nam.Text = kienHang.getNamInfo();
            this.maKien.Text = kienHang.getMaKienInfo();
            this.donGia.Text = kienHang.getDonGiaInfo();
        }

        private void save_Click(object sender, RoutedEventArgs e)
        {
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
                    b.updateBackGround();
                    this.Close();
                    khoHang.writeData();
                    if (isOn == true)
                        KhoHang.uploadFile();
                }
            }
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
                    b.updateBackGround();
                    this.Close();
                    khoHang.writeData();
                    if (isOn == true)
                        KhoHang.uploadFile();
                }
            }
        }

        private void cancel_Click(object sender, RoutedEventArgs e)
        {            
            this.Close();
        }       

        private void delete_Click(object sender, RoutedEventArgs e)
        {
            k.refresh();
            b.updateBackGround();
            this.Close();
            khoHang.writeData();
            if (isOn == true)
                KhoHang.uploadFile();
        }
    }
}

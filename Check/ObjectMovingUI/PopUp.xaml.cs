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
        private KienHang k;

        public PopUp(KienHang kienHang)
        {
            InitializeComponent();
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
            int r = k.setInfo(this.ngay.Text, this.thang.Text, this.nam.Text, this.maKien.Text, this.donGia.Text);
            if (r == -1)
            {
                MessageBox.Show("Thông tin nhập vào không hợp lệ");
            }
            else 
                this.Close();
        }

        private void cancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}

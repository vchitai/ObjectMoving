using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Check;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace ObjectMovingGUI
{
    public struct Rect
    {
        int x;
        int y;
    }

    public struct GoodMap
    {
        List<List<Rect>> coord;
        float angle;
    }

    public partial class MainForm : Form
    {
        private KhoHang khoHang;
        private List<GoodMap> goodMap;
        private const int kienHangHeight = 15;
        private const int kienHangWidth = 30;
        private const int offset = 2;

        public MainForm()
        {
            khoHang = new KhoHang();
            InitializeComponent();
        }

        private void khoHangGeneration()
        {
            int soKhu = khoHang.getSoLuongKhu();
            for (int i = 0; i < soKhu; i++)
            {
                khuHangGeneration(khoHang.getKhu(i));
            }
        }

        private void khuHangGeneration(KhuHang khu)
        {
        }

        private void menuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}

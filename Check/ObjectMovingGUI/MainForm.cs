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

namespace ObjectMovingGUI
{
    public partial class MainForm : Form
    {
        private KhoHang khoHang;
        private const int kienHangHeight = 15;
        private const int kienHangWidth = 30;

        public MainForm()
        {
            khoHang = new KhoHang();
            khoHangGUI = new List<TableLayoutPanel>();
            InitializeComponent();
            khoHangGeneration();
        }

        private void khoHangGeneration()
        {
            int soLuongKhu = khoHang.getSoLuongKhu();
            int prevRow = 0;
            for (int i = 0; i < soLuongKhu; i++)
            {
                khoHangGUI.Add(new System.Windows.Forms.TableLayoutPanel());
                khuHangGeneration(prevRow, khoHangGUI[i], khoHang.getKhu(i));
                prevRow = prevRow + khoHang.getKhu(i).getNRow();
                this.Controls.Add(khoHangGUI[i]);
            }
        }

        private void khuHangGeneration(int prevRow, TableLayoutPanel khuHangGUI, KhuHang khu)
        {
            int rowCount = khu.getNRow();
            int columnCount = khu.getNCol();
            khuHangGUI.Location = new System.Drawing.Point(20, 27 + prevRow * (kienHangHeight + 2));
            khuHangGUI.Size = new System.Drawing.Size(columnCount * kienHangWidth, rowCount * kienHangHeight);
            khuHangGUI.ColumnCount = columnCount;
            khuHangGUI.RowCount = rowCount;

            khuHangGUI.ColumnStyles.Clear();
            khuHangGUI.RowStyles.Clear();

            for (int i = 0; i < columnCount; i++)
                khuHangGUI.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100 / columnCount));

            for (int i = 0; i < rowCount; i++)
                khuHangGUI.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100 / rowCount));

            for (int i = 0; i < rowCount; i++)
            {
                for (int j = 0; j < columnCount; j++)
                {
                    KienHang k = khu.get(i, j);
                    var b = new Button();
                    b.Text = " ";
                    b.FlatStyle = FlatStyle.Flat;
                    b.Name = string.Format("b_{0}", i * j + 1);
                    b.Click += b_Click;
                    b.Dock = DockStyle.Fill;
                    b.Margin = new Padding(0);
                    if (k.getWidth() == 1)
                    {
                        b.BackgroundImage = ((System.Drawing.Image)(Properties.Resources.Box1));
                        b.BackgroundImageLayout = ImageLayout.Stretch;
                    }
                    else if (k.getWidth() == 2)
                    {
                        b.BackgroundImage = ((System.Drawing.Image)(Properties.Resources.BoxHead));
                        b.BackgroundImageLayout = ImageLayout.Stretch;
                    }
                    else if (k.getWidth() == -2)
                    {
                        b.BackgroundImage = ((System.Drawing.Image)(Properties.Resources.BoxTail));
                        b.BackgroundImageLayout = ImageLayout.Stretch;
                    }
                    khuHangGUI.Controls.Add(b);
                }
            }
        }

        void b_Click(object sender, EventArgs e)
        {
            var b = sender as Button;
            if (b != null)
                MessageBox.Show(string.Format("{0} Clicked", b.Text));
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

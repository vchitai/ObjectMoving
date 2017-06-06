using ObjectMovingConsole;
using System;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace ObjectMovingUI
{
    public class KButton : System.Windows.Controls.Button
    {
        #region variable
        private int khu;
        private int x;
        private int y;
        private KienHang kienHang;
        private KButton left, right;
        #endregion

        #region staticVariable
        // Lưu các file hình ảnh
        private static ImageBrush brush1;
        private static ImageBrush brush2;
        private static ImageBrush brush3;
        #endregion

        #region constructor
        static KButton()
        {
            brush1 = new ImageBrush(new BitmapImage(new Uri("../../../Resources/Box1.png", UriKind.Relative)));
            brush2 = new ImageBrush(new BitmapImage(new Uri("../../../Resources/BoxHead.png", UriKind.Relative)));
            brush3 = new ImageBrush(new BitmapImage(new Uri("../../../Resources/BoxTail.png", UriKind.Relative)));
        }

        public KButton(int k, int x_co, int y_co, KienHang _kienHang)
        {
            khu = k;
            x = x_co;
            y = y_co;
            kienHang = _kienHang;
        }
        #endregion

        #region getVariable
        // Lấy thông tin từ trong class
        public string getInfo()
        {
            return kienHang.getInfo();
        }

        public KienHang getKienHang()
        {
            return kienHang;
        }

        public KButton getLeft()
        {
            return left;
        }

        public KButton getRight()
        {
            return right;
        }

        public string getPos()
        {
            return String.Format("[{0}] ({1},{2})", khu, x, y);
        }
        #endregion

        #region setVariable
        // Thiết lập các biến
        public void setLeft(KButton b)
        {
            left = b;
        }

        public void setRight(KButton b)
        {
            right = b;
        }
        #endregion

        #region updateBackground
        // Cập nhật nền Button và các Button liên quan
        public void setBackGround(int id)
        {
            if (id == 0)
            {
                this.Background = Brushes.White;
            }
            else if (id == 1)
            {
                this.Background = brush1;
            }
            else if (id == 2)
            {
                this.Background = brush2;
            }
            else if (id == -2)
            {
                this.Background = brush3;
            }
        }

        public void upBackGround()
        {
            this.setBackGround(kienHang.getWidth());
        }

        public void updateBackGround()
        {
            this.upBackGround();
            if (this.left != null)
                this.left.upBackGround();
            if (this.right != null)
                this.right.upBackGround();
        }
        #endregion

        #region moveFunction
        // Di chuyển kiện hàng và cập nhật nền Button
        public void move(KhoHang k, KButton b)
        {
            k.Move(khu, x, y, b.khu, b.x, b.y);
            this.updateBackGround();
            b.updateBackGround();
        }
        #endregion
    }
       
}

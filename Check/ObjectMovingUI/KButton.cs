using Check;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace ObjectMovingUI
{
    public class KButton : System.Windows.Controls.Button
    {
        private int khu;
        private int x;
        private int y;
        private KienHang kienHang;
        private KButton left, right;

        private ImageBrush brush1 = new ImageBrush(new BitmapImage(new Uri("Resources/Box1.png", UriKind.Relative)));
        private ImageBrush brush2 = new ImageBrush(new BitmapImage(new Uri("Resources/BoxHead.png", UriKind.Relative)));
        private ImageBrush brush3 = new ImageBrush(new BitmapImage(new Uri("Resources/BoxTail.png", UriKind.Relative)));


        public KButton(int k, int x_co, int y_co, KienHang _kienHang)
        {
            khu = k;
            x = x_co;
            y = y_co;
            kienHang = _kienHang;
        }

        public string getInfo()
        {
            return kienHang.getInfo();
        }

        public KienHang getKienHang()
        {
            return kienHang;
        }

        public void setLeft(KButton b)
        {
            left = b;
        }

        public void setRight(KButton b)
        {
            right = b;
        }

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

        private void upBackGround()
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

        public void move(KhoHang k, KButton b)
        {
            k.Move(khu, x, y, b.khu, b.x, b.y);
            setBackGround(kienHang.getWidth());
            b.setBackGround(b.kienHang.getWidth());

            if (left != null)
                left.setBackGround(left.kienHang.getWidth());

            if (right != null)
                right.setBackGround(right.kienHang.getWidth());

            if (b.left != null)
                b.left.setBackGround(b.left.kienHang.getWidth());

            if (b.right != null)
                b.right.setBackGround(b.right.kienHang.getWidth());
        }

    }
       
}

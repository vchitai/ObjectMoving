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
    class KButton : System.Windows.Controls.Button
    {
        private KienHang kienHang;
        private KButton left, right;

        private ImageBrush brush1 = new ImageBrush(new BitmapImage(new Uri("Resources/Box1.png", UriKind.Relative)));
        private ImageBrush brush2 = new ImageBrush(new BitmapImage(new Uri("Resources/BoxHead.png", UriKind.Relative)));
        private ImageBrush brush3 = new ImageBrush(new BitmapImage(new Uri("Resources/BoxTail.png", UriKind.Relative)));


        public KButton(KienHang _kienHang)
        {
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

    }
       
}

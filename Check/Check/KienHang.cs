using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Check
{
    public class Date
    {
        private int ngay;
        private int thang;
        private int nam;

        private int toInt(string s)
        {
            int res = 0;

            for (int i = 0; i < s.Length; ++i)
            {
                if (s[i] <= '9' && s[i] >= '0')
                {
                    res = res * 10 + (s[i] - '0');
                }
                else
                {
                    return -1;
                }
            }

            return res;
        }

        public Date()
        {
            ngay = thang = nam = 0;
        }

        public Date(int _ngay, int _thang, int _nam)
        {
            ngay = _ngay;
            thang = _thang;
            nam = _nam;
        }

        public Date(string s)
        {
            ngay = toInt(""+ s[0] + s[1]);
            thang = toInt("" + s[3] + s[4]);
            nam = toInt("" + s[6] + s[7] + s[8] + s[9]);
        }

        public void capNhatNgay(int _ngay)
        {
            ngay = _ngay;
        }

        public void capNhatThang(int _thang)
        {
            thang = _thang;
        }

        public void capNhatNam(int _nam)
        {
            nam = _nam;
        }

        public int layNgay()
        {
            return ngay;
        }

        public int layThang()
        {
            return thang;
        }

        public int layNam()
        {
            return nam;
        }
    }

    public class KienHang
    {
        private int posX;
        private int posY;
        private int width;

        private string maKienHang;
        private int donGia;
        private Date ngayNhapKho;

        public KienHang()
        {
            posX = posY = width = 0;
            maKienHang = "";
            donGia = 0;
            ngayNhapKho = new Date(-1, -1, -1);
        }

        public KienHang(int x, int y, int w)
        {
            posX = x; posY = y; width = w;
            maKienHang = "";
            donGia = 0;
            ngayNhapKho = new Date(-1, -1, -1);
        }

        public KienHang(int x, int y, int w, string _maKienHang, int _donGia, Date _ngayNhapKho)
        {
            posX = x; posY = y; width = w;
            maKienHang = _maKienHang;
            donGia = _donGia;
            ngayNhapKho = _ngayNhapKho;
        }

        public char getChar()
        {
            return (width == 0 ? '-' : (width == 1 ? '1' : '2'));
        }

        public int getWidth()
        {
            return width;
        }
    }
}

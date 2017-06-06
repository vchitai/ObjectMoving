using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ObjectMovingConsole
{
    //class luu tru thong tin ngay thang nhap xuat kho
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

        //khoi tao ngay mac dinh
        public Date()
        {
            ngay = thang = nam = -1;
        }

        //khoi tao ngay voi thong tin cho truoc
        public Date(int _ngay, int _thang, int _nam)
        {
            ngay = _ngay;
            thang = _thang;
            nam = _nam;
        }

        //khoi tao ngay tu moi chuoi 
        public Date(string s)
        {
            ngay = toInt("" + s[0] + s[1]);
            thang = toInt("" + s[3] + s[4]);
            nam = toInt("" + s[6] + s[7] + s[8] + s[9]);
        }

        //cap nhat thong tin ngay
        public void capNhatNgay(int _ngay)
        {
            ngay = _ngay;
        }

        //cap nhat thong tin thang
        public void capNhatThang(int _thang)
        {
            thang = _thang;
        }

        //cap nhat thong tin nam
        public void capNhatNam(int _nam)
        {
            nam = _nam;
        }

        //lay ngay
        public int layNgay()
        {
            return ngay;
        }

        //lay thang
        public int layThang()
        {
            return thang;
        }

        //lay nam
        public int layNam()
        {
            return nam;
        }

        //lay thong tin ngay thang nam theo chuan DD/MM/YYYY
        public string getInfo()
        {
            string res = "";
            if (ngay == -1)
            {
                res = "--/--/----";
            }
            else
            {
                res = String.Format("{0:00}/{1:00}/{2:0000}", ngay, thang, nam);
            }
            return res;
        }

        //xuat ngay theo dinh dang string
        public string getDayInfo()
        {
            if (ngay == -1)
            {
                return "--";
            }
            return String.Format("{0}", ngay);
        }

        //xuat thang theo dinh dang string
        public string getThangInfo()
        {
            if (thang == -1)
            {
                return "--";
            }
            return String.Format("{0}", thang);
        }

        //xuat nam theo dinh dang string
        public string getNamInfo()
        {
            if (nam == -1)
            {
                return "--";
            }
            return String.Format("{0}", nam);
        }

        //kiem tra ngay thang nam co hop le khong
        private static bool check_date(int day, int month, int year)
        {
            int leap = 0;
            if ((year % 4) == 0 && (year % 100) != 100)
                leap = 1;
            if ((year % 400) == 0)
                leap = 1;

            int upper = 0;
            switch (month)
            {
                case 1:
                    upper = 31;
                    break;
                case 2:
                    if (leap == 1)
                        upper = 29;
                    else upper = 28;
                    break;
                case 3:
                    upper = 31;
                    break;
                case 4:
                    upper = 30;
                    break;
                case 5:
                    upper = 31;
                    break;
                case 6:
                    upper = 30;
                    break;
                case 7:
                    upper = 31;
                    break;
                case 8:
                    upper = 31;
                    break;
                case 9:
                    upper = 30;
                    break;
                case 10:
                    upper = 31;
                    break;
                case 11:
                    upper = 30;
                    break;
                case 12:
                    upper = 31;
                    break;
            }

            if (day < 1 || day > upper) return false;
            return true;
        }

        //chuyen ngay thang nam tu chuoi thanh Date
        public static Date convert(string _day, string _month, string _year)
        {
            int x;
            int day = 0;
            int month = 0;
            int year = 0;

            if (Int32.TryParse(_day, out x))
            {
                day = Int32.Parse(_day);
                if (Int32.TryParse(_month, out x))
                {
                    month = Int32.Parse(_month);
                    if (Int32.TryParse(_year, out x))
                    {
                        year = Int32.Parse(_year);

                        if (check_date(day, month, year))
                        {
                            return new Date(day, month, year);
                        }
                    }
                }

            }

            return new Date();
        }
    }

    public class KienHang
    {
        private int posX;
        private int posY;
        private int width;

        private KienHang left;
        private KienHang right;

        private string maKienHang;
        private int donGia;
        private Date ngayNhapKho;

        //khoi tao kien hang
        public KienHang()
        {
            posX = posY = width = 0;
            maKienHang = "";
            donGia = 0;
            ngayNhapKho = new Date(-1, -1, -1);
        }

        //khoi tao kien hang tu thong tin cho truoc
        public KienHang(int x, int y, int w)
        {
            posX = x; posY = y; width = w;
            maKienHang = "";
            donGia = 0;
            ngayNhapKho = new Date(-1, -1, -1);
        }

        //khoi tao kien hang tu thong tin cho truoc
        public KienHang(int x, int y, int w, string _maKienHang, int _donGia, Date _ngayNhapKho)
        {
            posX = x; posY = y; width = w;
            maKienHang = _maKienHang;
            donGia = _donGia;
            ngayNhapKho = _ngayNhapKho;
        }

        //lay thong tin ve kien hang       
        public string getInfo()
        {
            string res = "";
            if (width == 0)
            {
                res = "Chưa có kiện hàng ở ô hàng này.\n\nClick chuột phải để thêm kiện hàng.";
            }
            else
            {
                res = String.Format("Độ dài:                  {0}.\n", Math.Abs(width));
                res = res + "Ngày nhập kho:    " + ngayNhapKho.getInfo() + ".\n";
                res = res + "Mã kiện hàng:       " + maKienHang + ".\n";
                res = res + String.Format("Đơn giá:                {0}.", donGia);
                res = res + "\n\nClick chuột phải để chỉnh sửa thông tin kiện hàng.";
            }
            return res;
        }

        //thiet lap thong tin moi cho kien hang
        internal void set(int _x, int _y, int _w, string _maKienHang, int _donGia, Date _ngayNhapKho)
        {
            posX = _x;
            posY = _y;
            width = _w;
            maKienHang = _maKienHang;
            donGia = _donGia;
            ngayNhapKho = _ngayNhapKho;
        }

        //lay thong tin do dai
        public string getWidthInfo()
        {
            return String.Format("{0}", Math.Abs(width));
        }

        //lay thong tin ngay nhap kho theo dinh dang chuoi
        public string getNgayInfo()
        {
            return ngayNhapKho.getDayInfo();
        }

        //lay thong tin thang nhap kho theo dinh dang chuoi
        public string getThangInfo()
        {
            return ngayNhapKho.getThangInfo();
        }

        //ghi thong tin kien hang vao file
        internal void writeData(StreamWriter file)
        {
            file.Write(posX + " " + posY + " " + width + " " + maKienHang + " " + donGia + " " + ngayNhapKho.getInfo());
        }

        //lay thong tin nam nhap kho theo dinh dang chuoi
        public string getNamInfo()
        {
            return ngayNhapKho.getNamInfo();
        }

        //lay thong tin ngay nhap kho theo dinh dang chuoi
        public string getDayInfo()
        {
            return ngayNhapKho.getDayInfo();
        }

        //lay thong tin ma kien hang
        public string getMaKienInfo()
        {
            return maKienHang;
        }

        //lay thong tin don gia
        public string getDonGiaInfo()
        {
            return String.Format("{0}", donGia);
        }

        //lay ki tu bieu tuong cho kien hang
        public char getChar()
        {
            return (width == 0 ? '-' : (width == 1 ? '1' : '2'));
        }

        //lay kich thuoc ngang
        public int getWidth()
        {
            return width;
        }

        //cap nhat thong tin kien hang
        private void updateInfo(int w, Date d, string maKien, int gia)
        {
            width = -2;
            ngayNhapKho = d;
            maKienHang = maKien;
            donGia = gia;
        }

        //thiet lap kien hang phai
        public void setKienHangRight(KienHang k)
        {
            right = k;
        }

        //thiet lap kien hang trai
        public void setKienHangLeft(KienHang k)
        {
            left = k;
        }

        //thiet lap thong tin kien hang
        public int setInfo(string ngay, string thang, string nam, string maKien, string _donGia)
        {
            Date d = Date.convert(ngay, thang, nam);
            if (d.layNam() == -1) return -2;
            int x = 0;
            int gia = 0;
            if (Int32.TryParse(_donGia, out x))
            {
                gia = Int32.Parse(_donGia);
            }
            else return -3;

            ngayNhapKho = d;
            maKienHang = maKien;
            donGia = gia;

            return 1;
        }

        //thiet lap thong tin kien hang
        public int setInfo2(string _kichThuoc, string ngay, string thang, string nam, string maKien, string _donGia)
        {
            int x = 0;
            int kk = 0;
            if (Int32.TryParse(_kichThuoc, out x))
            {
                kk = Int32.Parse(_kichThuoc);
                if (kk < 1 || kk > 2)
                    return -1;
            }
            else
            {
                return -1;
            }
            Date d = Date.convert(ngay, thang, nam);
            if (d.layNam() == -1) return -2;
            int gia = 0;
            if (Int32.TryParse(_donGia, out x))
            {
                gia = Int32.Parse(_donGia);
            }
            else return -3;

            if (kk == 2 && (right == null || right.getWidth() != 0))
                return -4;
            else
                if (kk == 2)
                right.updateInfo(-2, d, maKien, gia);

            width = kk;
            ngayNhapKho = d;
            maKienHang = maKien;
            donGia = gia;

            return 1;
        }

        //thiet lap trang thai rong cho kien hang
        private void setEmpty()
        {
            width = 0;
            maKienHang = "";
            donGia = 0;
            ngayNhapKho = new Date(-1, -1, -1);
        }

        //copy thong tin tu KienHang k
        public void copy(KienHang k)
        {
            width = k.width;
            maKienHang = k.maKienHang;
            donGia = k.donGia;
            ngayNhapKho = k.ngayNhapKho;
        }

        //cap nhat lai trang thai kien hang (ke ca kien hang 2 o)
        public void refresh()
        {
            if (width == 2)
            {
                right.setEmpty();
            }
            if (width == -2)
            {
                left.setEmpty();
            }
            this.setEmpty();
        }

        //thiet lap toa do X, Y cua kien hang
        public void setXY(int x, int y)
        {
            posX = x;
            posY = y;
        }
    }
}

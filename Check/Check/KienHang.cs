﻿using System;
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
            ngay = thang = nam = -1;
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

        public string getInfo()
        {
            string res = "";
            if (ngay == -1)
            {
                res = "--/--/----";
            }
            else
            {
                res = String.Format("{0}/{1}/{2}", ngay, thang, nam);
            }
            return res;
        }

        public string getDayInfo()
        {
            if (ngay == -1)
            {
                return "--";
            }
            return String.Format("{0}", ngay);
        }

        public string getThangInfo()
        {
            if (thang == -1)
            {
                return "--";
            }
            return String.Format("{0}", thang);
        }

        public string getNamInfo()
        {
            if (nam == -1)
            {
                return "--";
            }
            return String.Format("{0}", nam);
        }

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
                    upper =  31;
                    break;
                case 2:
                    if (leap == 1)
                        upper =  29;
                    else upper =  28;
                    break;
                case 3:
                    upper =  31;
                    break;
                case 4:
                    upper =  30;
                    break;
                case 5:
                    upper =  31;
                    break;
                case 6:
                    upper =  30;
                    break;
                case 7:
                    upper =  31;
                    break;
                case 8:
                    upper =  31;
                    break;
                case 9:
                    upper =  30;
                    break;
                case 10:
                    upper =  31;
                    break;
                case 11:
                    upper =  30;
                    break;
                case 12:
                    upper =  31;
                    break;                
            }

            if (day < 1 || day > upper) return false;
            return true;
        }

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

            return new Check.Date();
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

        public string getInfo()
        {
            string res = "";
            if (width == 0)
            {
                res = "Chưa có kiện hàng ở ô hàng này";
            }
            else
            {
                res = String.Format("Độ dài: {0}\n", Math.Abs(width));
                res = res + "Ngày nhập kho: " + ngayNhapKho.getInfo() + "\n";
                res = res + "Mã kiện hàng: " + maKienHang + "\n";
                res = res + String.Format("Đơn giá: {0}", donGia);
            }
            return res;             
        }

        public string getWidthInfo()
        {
            return String.Format("{0}",  Math.Abs(width));
        }

        public string getNgayInfo()
        {
            return ngayNhapKho.getDayInfo();
        }

        public string getThangInfo()
        {
            return ngayNhapKho.getThangInfo();
        }

        public string getNamInfo()
        {
            return ngayNhapKho.getNamInfo();
        }

        public string getDayInfo()
        {
            return ngayNhapKho.getDayInfo();
        }

        public string getMaKienInfo()
        {
            return maKienHang;
        }

        public string getDonGiaInfo()
        {
            return String.Format("{0}", donGia);
        }

        public char getChar()
        {
            return (width == 0 ? '-' : (width == 1 ? '1' : '2'));
        }

        public int getWidth()
        {
            return width;
        }

        public int setInfo(string ngay, string thang, string nam, string maKien, string _donGia)
        {
            Date d = Date.convert(ngay, thang, nam);
            if (d.layNam() == -1) return -1;
            int x = 0;
            int gia = 0;
            if (Int32.TryParse(_donGia, out x))
            {
                gia = Int32.Parse(_donGia);
            }
            else return -1;

            ngayNhapKho = d;
            maKienHang = maKien;
            donGia = gia;

            return 1;               
        }
    }
}

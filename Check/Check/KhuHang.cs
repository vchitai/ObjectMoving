using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Check
{
    public class KhuHang
    {
        public int posX;
        public int posY;
        private int nRow;
        private int nCol;
        private float gocNghieng;

        private List<List<KienHang>> kienHang;

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
        
        public KhuHang()
        {
            nRow = nCol = 0;
            kienHang = new List<List<KienHang>>();
        }

        public KhuHang(int nRow, int nCol)
        {
            kienHang = new List<List<KienHang>>();
            for (int i = 0; i < nRow; ++i)
            {
                kienHang.Add(new List<KienHang>());
                for (int j = 0; j < nCol; ++j)
                {
                    kienHang[i].Add(new KienHang(1, 1, 0));
                }
            }
        }

        public void docFile(System.IO.StreamReader file)
        {
            string line = file.ReadLine();
            string[] numberArray = line.Split(' ');

            posX = toInt(numberArray[0]);
            posY = toInt(numberArray[1]);
            gocNghieng = float.Parse(numberArray[2], System.Globalization.CultureInfo.InvariantCulture.NumberFormat);
            nCol = toInt(numberArray[3]);
            nRow = toInt(numberArray[4]);
            int sl = toInt(numberArray[5]);

            kienHang = new List<List<KienHang>>();
            for (int i = 0; i < nRow; ++i)
            {
                kienHang.Add(new List<KienHang>());
                for (int j = 0; j < nCol; ++j)
                {
                    kienHang[i].Add(new KienHang());
                    kienHang[i][j].setXY(i, j);
                }
            }

            for (int i = 0; i < sl; ++i)
            {
                line = file.ReadLine();
                string[] dataInfo = line.Split(' ');
                int temp_n = dataInfo.Length;

                int x = toInt(dataInfo[0]);
                int y = toInt(dataInfo[1]);
                int w = toInt(dataInfo[2]);
                string maKienHang = "";
                int donGia = 0;
                Date ngayNhapKho = new Check.Date();

                if (temp_n >= 4)
                {
                    maKienHang = dataInfo[3];
                }

                if (temp_n >= 5)
                {                    
                    donGia = toInt(dataInfo[4]);
                }
                if (temp_n >= 6)
                {
                    ngayNhapKho = new Check.Date(dataInfo[5]);
                }

                //KienHang(x, y, ...): (x, y) la vi tri ve len man hinh
                //luc tinh toan can luu y lai
                kienHang[x][y] = new Check.KienHang(x, y, w, maKienHang, donGia, ngayNhapKho);
                if (w == 2)
                    kienHang[x][y + 1] = new Check.KienHang(x, y, -w, maKienHang, donGia, ngayNhapKho);
            }

            for(int i = 0; i < nRow; ++i)
            {
                for(int j = 0; j < nCol; ++j)
                {
                    if (j == nCol - 1)
                        kienHang[i][j].setKienHangRight(null);
                    else
                        kienHang[i][j].setKienHangRight(kienHang[i][j + 1]);
                }
            }

            for (int i = 0; i < nRow; ++i)
            {
                for (int j = 0; j < nCol; ++j)
                {
                    if (j == 0)
                        kienHang[i][j].setKienHangLeft(null);
                    else
                        kienHang[i][j].setKienHangLeft(kienHang[i][j - 1]);
                }
            }
        }

        public void printOnConsole()
        {
            for(int i = 0; i < nRow; ++i)
            {
                for(int j = 0; j < nCol; ++j)
                {
                    char t = kienHang[i][j].getChar();
                    Console.Write(t);
                    Console.Write(' ');
                }
                Console.Write('\n');
            }
        }

        public void set(int x, int y, KienHang k)
        {
            kienHang[x][y].copy(k);
        }

        public KienHang get(int x, int y)
        {
            return kienHang[x][y];
        }
        
        public int getNRow()
        {
            return nRow;
        }        

        public int getNCol()
        {
            return nCol;
        }

        public float getAngle()
        {
            return gocNghieng;
        }

        public void loadData(System.IO.StreamReader file)
        {
            string line = file.ReadLine();
            string[] numberArray = line.Split(' ');

            posX = toInt(numberArray[0]);
            posY = toInt(numberArray[1]);
            gocNghieng = float.Parse(numberArray[2], System.Globalization.CultureInfo.InvariantCulture.NumberFormat);
            nCol = toInt(numberArray[3]);
            nRow = toInt(numberArray[4]);
            int sl = toInt(numberArray[5]);

            for (int i = 0; i < nRow; ++i)
                for (int j = 0; j < nCol; ++j)
                {
                    kienHang[i][j].refresh();
                    kienHang[i][j].setXY(i, j);
                }  

            for (int i = 0; i < sl; ++i)
            {
                line = file.ReadLine();
                string[] dataInfo = line.Split(' ');
                int temp_n = dataInfo.Length;

                int x = toInt(dataInfo[0]);
                int y = toInt(dataInfo[1]);
                int w = toInt(dataInfo[2]);
                string maKienHang = "";
                int donGia = 0;
                Date ngayNhapKho = new Check.Date();

                if (temp_n >= 4)
                {
                    maKienHang = dataInfo[3];
                }

                if (temp_n >= 5)
                {
                    donGia = toInt(dataInfo[4]);
                }
                if (temp_n >= 6)
                {
                    ngayNhapKho = new Check.Date(dataInfo[5]);
                }

                //KienHang(x, y, ...): (x, y) la vi tri ve len man hinh
                //luc tinh toan can luu y lai
                kienHang[x][y].set(x, y, w, maKienHang, donGia, ngayNhapKho);
                if (w == 2)
                    kienHang[x][y + 1].set(x, y, -w, maKienHang, donGia, ngayNhapKho);
            }            
        }

        public void writeData(System.IO.StreamWriter file)
        {
            file.Write(posX + " " + posY + " " + gocNghieng + " " + nCol + " " + nRow);
            int cnt = 0;
            for (int i = 0; i < nRow; ++i)
                for (int j = 0; j < nCol; ++j)
                    if (kienHang[i][j].getWidth() > 0) cnt = cnt + 1;
            file.Write(" " + cnt);
            file.WriteLine("");
            for (int i = 0; i < nRow; ++i)
                for(int j = 0; j < nCol; ++j)
                {
                    if (kienHang[i][j].getWidth() > 0)
                    {
                        kienHang[i][j].writeData(file);
                        file.WriteLine();
                    }
                }                
        }
    }
}

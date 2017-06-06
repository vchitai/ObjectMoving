using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ObjectMovingConsole
{
    class Program
    {
        #region handleString
        static private int toInt(string s)
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

        static public string Remove(string s, char ch)
        {
            while (s.Length > 0 && s[0] != ch)
            {
                s = s.Remove(0, 1);
            }
            if (s.Length > 0) s = s.Remove(0, 1);
            return s;
        }
        #endregion

        static void Main()
        {
            KhoHang kho = new KhoHang();
            kho.printOnConsole();

            do
            {
                #region inputCommand
                Console.Write("Nhap lenh di chuyen theo format sau: \"M [p] (x,y) [k] (u,v)\"\n");
                string temp_line = Console.ReadLine();
                string line = "";
                for (int i = 0; i < temp_line.Length; ++i)
                {
                    if (temp_line[i] == ' ') continue;
                    line = line + temp_line[i];
                }
                #endregion

                #region checkCommand
                bool ok = false;

                int p = 0;
                int x = 0;
                int y = 0;
                int k = 0;
                int u = 0;
                int v = 0;

                do
                {
                    if (line[0] != 'M') break;

                    line = Remove(line, '[');
                    if (line.Length == 0) break;

                    string temp = "";
                    while (line.Length > 0 && line[0] != ']')
                    {
                        temp = temp + line[0];
                        line = line.Remove(0, 1);
                    }
                    if (line.Length > 0) line = line.Remove(0, 1);
                    p = toInt(temp);
                    if (p == -1 || line.Length == 0) break;

                    line = Remove(line, '(');
                    temp = "";
                    while (line.Length > 0 && line[0] != ',')
                    {
                        temp = temp + line[0];
                        line = line.Remove(0, 1);
                    }
                    if (line.Length > 0) line = line.Remove(0, 1);
                    x = toInt(temp);
                    if (x == -1 || line.Length == 0) break;

                    temp = "";
                    while (line.Length > 0 && line[0] != ')')
                    {
                        temp = temp + line[0];
                        line = line.Remove(0, 1);
                    }
                    if (line.Length > 0) line = line.Remove(0, 1);
                    y = toInt(temp);
                    if (y == -1 || line.Length == 0) break;

                    line = Remove(line, '[');
                    if (line.Length == 0) break;

                    temp = "";
                    while (line.Length > 0 && line[0] != ']')
                    {
                        temp = temp + line[0];
                        line = line.Remove(0, 1);
                    }
                    if (line.Length > 0) line = line.Remove(0, 1);
                    k = toInt(temp);
                    if (p == -1 || line.Length == 0) break;

                    line = Remove(line, '(');
                    temp = "";
                    while (line.Length > 0 && line[0] != ',')
                    {
                        temp = temp + line[0];
                        line = line.Remove(0, 1);
                    }
                    if (line.Length > 0) line = line.Remove(0, 1);
                    u = toInt(temp);
                    if (u == -1 || line.Length == 0) break;

                    temp = "";
                    while (line.Length > 0 && line[0] != ')')
                    {
                        temp = temp + line[0];
                        line = line.Remove(0, 1);
                    }
                    if (line.Length > 0) line = line.Remove(0, 1);
                    v = toInt(temp);
                    if (v == -1) break;

                    ok = true;
                    break;
                } while (true);
                #endregion

                #region executeCommand
                if (!ok)
                {
                    Console.Write("Nhap sai cu phap\n");
                }
                else
                {
                    int kq = kho.Move(p, x, y, k, u, v);
                    if (kq == -1)
                        Console.Write("Dich den khong hop le (da ton tai hang o dich den)\n");
                    else
                        if (kq == -2)
                        Console.Write("Khong co hang tai o xuat phat\n");
                    else
                        if (kq == -3)
                        Console.Write("Toa do vuot gioi han\n");
                    else
                    {
                        Console.Clear();
                        kho.printOnConsole();
                        Console.Write("Ban do nha kho da duoc cap nhat\n");
                    }
                }
                #endregion

                #region checkContinue
                Console.Write("Nhap Q de thoat, C de tiep tuc: \n");
                string key = Console.ReadLine();
                if (key == "Q") break;
                #endregion
            } while (true);
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Check
{
    public class KhoHang
    {
        private int soLuongKhu;
        private List<KhuHang> khu;

        public int getSoLuongKhu()
        {
            return soLuongKhu;
        }

        public KhuHang getKhu(int i)
        {
            return khu[i];
        }

        private int toInt(string s)
        {
            int res = 0;

            for(int i = 0; i < s.Length; ++i)
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

        public KhoHang()
        {
            System.IO.StreamReader file = new System.IO.StreamReader("../../input.txt");
            string line = file.ReadLine();
            soLuongKhu = toInt(line);

            khu = new List<KhuHang>();
            for(int i = 0; i < soLuongKhu; ++i)
            {
                KhuHang k = new KhuHang();
                k.docFile(file);
                khu.Add(k);
            }
        }

        public void printOnConsole()
        {
            for (int i = 0; i < soLuongKhu; ++i)
            {
                Console.Write("["); Console.Write(i + 1); Console.Write("]\n");
                khu[i].printOnConsole();
            }
                
        }

        //-1: Dich den khong hop le
        //-2: Khong co hang o (pre_x, pre_y)
        //-3: Toa do qua gioi han        
        public int Move(int pre_k, int pre_x, int pre_y, int new_k, int new_x, int new_y)
        {
            pre_k--; new_k--;

            if (pre_x < 0 || pre_x >= khu[pre_k].getNRow()) return -3;
            if (new_x < 0 || new_x >= khu[new_k].getNRow()) return -3;
            if (pre_y < 0 || pre_y >= khu[pre_k].getNCol()) return -3;
            if (new_y < 0 || new_y >= khu[new_k].getNCol()) return -3;

            int w1 = khu[pre_k].get(pre_x, pre_y).getWidth();            
            
            if (w1 == 0) return -2;
            
            if (w1 == 2)
            {
                if (new_y + 1 >= khu[new_k].getNCol()) return -1;
                KienHang temp1 = new KienHang();
                KienHang temp2 = new Check.KienHang();
                temp1.copy(khu[pre_k].get(pre_x, pre_y));
                temp2.copy(khu[pre_k].get(pre_x, pre_y + 1));
                khu[pre_k].get(pre_x, pre_y).refresh();
                khu[pre_k].get(pre_x, pre_y + 1).refresh();
                if (khu[new_k].get(new_x, new_y + 1).getWidth() != 0 || khu[new_k].get(new_x, new_y).getWidth() != 0)
                {
                    khu[pre_k].set(pre_x, pre_y, temp1);
                    khu[pre_k].set(pre_x, pre_y + 1, temp2);
                    return -1;
                }
                khu[new_k].set(new_x, new_y, temp1);
                khu[new_k].set(new_x, new_y + 1, temp2);                
            }
            if (w1 == -2)
            {
                if (new_y - 1 < 0) return -1;
                KienHang temp1 = new KienHang();
                KienHang temp2 = new Check.KienHang();
                temp1.copy(khu[pre_k].get(pre_x, pre_y));
                temp2.copy(khu[pre_k].get(pre_x, pre_y - 1));
                khu[pre_k].get(pre_x, pre_y).refresh();
                khu[pre_k].get(pre_x, pre_y - 1).refresh();
                if (khu[new_k].get(new_x, new_y - 1).getWidth() != 0 || khu[new_k].get(new_x, new_y).getWidth() != 0)
                {
                    khu[pre_k].set(pre_x, pre_y, temp1);
                    khu[pre_k].set(pre_x, pre_y - 1, temp2);
                    return -1;
                }
                khu[new_k].set(new_x, new_y, temp1);
                khu[new_k].set(new_x, new_y - 1, temp2);                
            }
            if (w1 == 1)
            {
                if (khu[new_k].get(new_x, new_y).getWidth() != 0) return -1;
                khu[new_k].set(new_x, new_y, khu[pre_k].get(pre_x, pre_y));
                khu[pre_k].get(pre_x, pre_y).refresh();
            }            

            return 1;
        }        
    }
}

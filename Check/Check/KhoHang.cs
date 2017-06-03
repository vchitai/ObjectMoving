using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Net;
using System.IO;
using System.Text;

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
            System.IO.StreamReader file = new System.IO.StreamReader("../../Resources/input.txt");
            string line = file.ReadLine();
            soLuongKhu = toInt(line);

            khu = new List<KhuHang>();
            for(int i = 0; i < soLuongKhu; ++i)
            {
                KhuHang k = new KhuHang();
                k.docFile(file);
                khu.Add(k);
            }
            file.Close();
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

        public void moveByCommand(string temp_line)
        {
            string line = "";
            for (int i = 0; i < temp_line.Length; ++i)
            {
                if (temp_line[i] == ' ') continue;
                line = line + temp_line[i];
            }
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

            if (!ok)
            {
                MessageBox.Show("Nhập sai cú pháp (đề nghị chọn ô xuất phát và ô kết thúc.");
            }
            else
            {
                if (p < 1 || p > soLuongKhu)
                {
                    MessageBox.Show("Khu hàng nguồn không tồn tại.");
                    return;
                }
                if (k < 1 || k > soLuongKhu)
                {
                    MessageBox.Show("Khu hàng đích không tồn tại.");
                    return;
                }
                int kq = this.Move(p, x, y, k, u, v);
                if (kq == -1)
                    MessageBox.Show("Tồn tại hàng ở đích đến.");
                else
                    if (kq == -2)
                    MessageBox.Show("Không có hàng ở ô xuất phát.");
                else
                    if (kq == -3)
                    MessageBox.Show("Tọa độ vượt giới hạn của khu hàng.");
                else
                {
                }
            }
        }


        public void loadData()
        {
            System.IO.StreamReader file;
            try
            {
                file = new System.IO.StreamReader("../../Resources/input.txt");
            }
            catch (IOException)
            {
                //the file is unavailable because it is:
                //still being written to
                //or being processed by another thread
                //or does not exist (has already been processed)
                return;
            }
            string line = file.ReadLine();
            soLuongKhu = toInt(line);
            
            for (int i = 0; i < soLuongKhu; ++i)
            {
                khu[i].loadData(file);
            }
            file.Close();
        }

        public void writeData()
        {
            System.IO.StreamWriter file;
            try
            {
                file = new System.IO.StreamWriter("../../Resources/input.txt");
            }                        
            catch (IOException)
            {
                //the file is unavailable because it is:
                //still being written to
                //or being processed by another thread
                //or does not exist (has already been processed)
                return;
            }
            file.Write(soLuongKhu);
            file.WriteLine("");
            for(int i = 0; i < soLuongKhu; ++i)
            {
                khu[i].writeData(file);
            }
            file.Close();
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

        public static void downloadFile()
        {
            // Get the object used to communicate with the server.  
            FtpWebRequest request = (FtpWebRequest)WebRequest.Create("ftp://b9_20183079:3781159@ftp.byethost9.com/htdocs/input.txt");
            request.Method = WebRequestMethods.Ftp.DownloadFile;

            // This example assumes the FTP site uses anonymous logon.  
            request.Credentials = new NetworkCredential("b9_20183079", "3781159");

            FtpWebResponse response = (FtpWebResponse)request.GetResponse();

            Stream responseStream = response.GetResponseStream();
            StreamReader reader = new StreamReader(responseStream);
            StreamWriter file = new StreamWriter("../../Resources/input.txt");
            file.Write(reader.ReadToEnd());

            file.Close();
            reader.Close();
            response.Close();
        }

        public static void uploadFile()
        {
            // Get the object used to communicate with the server.  
            FtpWebRequest request = (FtpWebRequest)WebRequest.Create("ftp://b9_20183079:3781159@ftp.byethost9.com/htdocs/input.txt");
            request.Method = WebRequestMethods.Ftp.Rename;
            request.RenameTo = "/htdocs/input2.txt";
            request.GetResponse();

            request = (FtpWebRequest)WebRequest.Create("ftp://b9_20183079:3781159@ftp.byethost9.com/htdocs/input.txt");
            request.Method = WebRequestMethods.Ftp.UploadFile;

            // This example assumes the FTP site uses anonymous logon.  
            request.Credentials = new NetworkCredential("b9_20183079", "3781159");

            // Copy the contents of the file to the request stream.  
            StreamReader sourceStream = new StreamReader("../../Resources/input.txt");
            byte[] fileContents = Encoding.UTF8.GetBytes(sourceStream.ReadToEnd());
            sourceStream.Close();
            request.ContentLength = fileContents.Length;

            Stream requestStream = request.GetRequestStream();
            requestStream.Write(fileContents, 0, fileContents.Length);
            requestStream.Close();

            FtpWebResponse response = (FtpWebResponse)request.GetResponse();

            response.Close();
        }
    }
}

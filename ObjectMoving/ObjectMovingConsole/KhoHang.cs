using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Net;
using System.IO;

namespace ObjectMovingConsole
{
    public class KhoHang
    {
        private static WebClient webClient;
        private static FtpWebRequest ftpWebRequest;
        private static string defaultInputFile;
        private static string fileLink;
        private static string httpLink;
        private static string ftpLink;
        private static string userName;
        private static string password;
        private static NetworkCredential nc;

        private int soLuongKhu;
        private List<KhuHang> khu;

        //khoi tao kho hang
        static KhoHang()
        {
            defaultInputFile = "../../../Resources/input.txt";
            fileLink = "objectmoving.esy.es/input.txt";
            httpLink = "http://" + fileLink;
            ftpLink = "ftp://" + fileLink;
            userName = "u288570171";
            password = "3781159";
            webClient = new WebClient();
            nc = new NetworkCredential(userName, password);
        }

        //lay so luong khu hang trong kho
        public int getSoLuongKhu()
        {
            return soLuongKhu;
        }

        //lay thong tin cua mot khu
        public KhuHang getKhu(int i)
        {
            return khu[i];
        }

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

        //khoi tao kho hang tu moi file txt cho truoc
        public KhoHang(string fileName = "../../../Resources/input.txt")
        {
            defaultInputFile = fileName;
            StreamReader file = new StreamReader(fileName);
            string line = file.ReadLine();
            soLuongKhu = toInt(line);

            khu = new List<KhuHang>();
            for (int i = 0; i < soLuongKhu; ++i)
            {
                KhuHang k = new KhuHang();
                k.docFile(file);
                khu.Add(k);
            }
            file.Close();
        }

        //ham ho tro xu ly chuoi
        static public string Remove(string s, char ch)
        {
            while (s.Length > 0 && s[0] != ch)
            {
                s = s.Remove(0, 1);
            }
            if (s.Length > 0) s = s.Remove(0, 1);
            return s;
        }

        //ham di chuyen kien ham tu lenh temp_line
        //format lenh: M [k0] (x0,y0) [k1] (x1,y1)
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

        //ham cap nhat du lieu thu file cho truoc
        //dung trong dong bo hoa du lieu
        public void loadData()
        {
            try
            {
                StreamReader file = new StreamReader(defaultInputFile);
                string line = file.ReadLine();

                if (line == null) return;

                soLuongKhu = toInt(line);

                for (int i = 0; i < soLuongKhu; ++i)
                {
                    khu[i].loadData(file);
                }
                file.Close();
            }
            catch (IOException)
            {
                //the file is unavailable because it is:
                //still being written to
                //or being processed by another thread
                //or does not exist (has already been processed)
                MessageBox.Show("Không thể truy cập được file.", "Lỗi truy cập file");
                return;
            }
        }

        //ham cap nhat du lieu vao file cho truoc
        //dung trong dong bo hoa du lieu
        public void writeData()
        {
            try
            {
                StreamWriter file = new StreamWriter(defaultInputFile);
                file.Write(soLuongKhu);
                file.WriteLine("");
                for (int i = 0; i < soLuongKhu; ++i)
                {
                    khu[i].writeData(file);
                }
                file.Close();
            }
            catch (IOException)
            {
                //the file is unavailable because it is:
                //still being written to
                //or being processed by another thread
                //or does not exist (has already been processed)
                MessageBox.Show("Không thể truy cập được file.", "Lỗi truy cập file");
                return;
            }
        }

        //ham xuat thong tin kho hang len man hinh console
        public void printOnConsole()
        {
            for (int i = 0; i < soLuongKhu; ++i)
            {
                Console.Write("["); Console.Write(i + 1); Console.Write("]\n");
                khu[i].printOnConsole();
            }

        }

        //ham di chuyen kien hang [pre_k] (pre_x,pre_y) den [new_k] (new_x,new_y)
        //cac ket qua tra ve
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
                KienHang temp2 = new KienHang();
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
                KienHang temp2 = new KienHang();
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


        //cap nhat file tu server
        public static void downloadFile()
        {
            try
            {
                string content = webClient.DownloadString(httpLink);
                StreamWriter file = new StreamWriter(defaultInputFile);
                file.Write(content);
                file.Close();
            }
            catch (Exception)
            {
                MessageBox.Show("Không thể kết nối với máy chủ.", "Lỗi kết nối");
                return;
            }
        }

        //upload file len server
        public static void uploadFile()
        {
            try
            {
                // Rename
                ftpWebRequest = (FtpWebRequest)WebRequest.Create(ftpLink);
                ftpWebRequest.Credentials = nc;
                ftpWebRequest.Method = WebRequestMethods.Ftp.Rename;
                ftpWebRequest.RenameTo = "input2.txt";
                ftpWebRequest.UseBinary = true;
                ftpWebRequest.UsePassive = true;
                ftpWebRequest.KeepAlive = true;
                ftpWebRequest.GetResponse();

                //Upload
                ftpWebRequest = (FtpWebRequest)WebRequest.Create(ftpLink);
                ftpWebRequest.Credentials = nc;
                ftpWebRequest.Method = WebRequestMethods.Ftp.UploadFile;
                ftpWebRequest.UseBinary = true;
                ftpWebRequest.UsePassive = true;
                ftpWebRequest.KeepAlive = true;

                // Copy the contents of the file to the request stream.  
                StreamReader sourceStream = new StreamReader(defaultInputFile);
                byte[] fileContents = Encoding.UTF8.GetBytes(sourceStream.ReadToEnd());
                sourceStream.Close();

                ftpWebRequest.ContentLength = fileContents.Length;

                Stream requestStream = ftpWebRequest.GetRequestStream();
                requestStream.Write(fileContents, 0, fileContents.Length);
                requestStream.Close();
                ftpWebRequest.GetResponse();
            }
            catch (Exception)
            {
                MessageBox.Show("Không thể kết nối với máy chủ.", "Lỗi kết nối");
                return;
            }
        }

        //truy xuat tap tin de luu tru thong tin cua kho hang
        public void saveTo(string fileName)
        {
            StreamWriter file;
            try
            {
                file = new StreamWriter(fileName);
                file.Write(soLuongKhu);
                file.WriteLine("");
                for (int i = 0; i < soLuongKhu; ++i)
                {
                    khu[i].writeData(file);
                }
                file.Close();
            }
            catch (IOException)
            {
                //the file is unavailable because it is:
                //still being written to
                //or being processed by another thread
                //or does not exist (has already been processed)
                MessageBox.Show("Không thể truy xuất tập tin.", "Lỗi đường dẫn");
                return;
            }
        }
    }
}

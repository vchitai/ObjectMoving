using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Check;
using System.Drawing;
using System.Windows.Forms;

namespace ObjectMovingGUI
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            KhoHang kho = new Check.KhoHang();
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm());
        }


    }
}

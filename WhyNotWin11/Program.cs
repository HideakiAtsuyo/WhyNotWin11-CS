using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WhyNotWin11
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        public static Form1 Form1 = new Form1();
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            //Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(Form1);
        }
    }
}

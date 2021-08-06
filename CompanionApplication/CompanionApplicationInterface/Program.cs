using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using CompanionApplication.Core;

namespace CompanionApplication.Interface
{
    class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // Get list of processes with same name
            string applicationName = System.Reflection.Assembly.GetExecutingAssembly().GetName().Name;
            System.Diagnostics.Process[] processes = System.Diagnostics.Process.GetProcessesByName(applicationName);

            // Exits application if more than one exists
            if (processes.Count() > 1)
            {
                Application.Exit();
            }
            else
            {
                Application.Run(new TrayIcon());
            }
        }
    }
}

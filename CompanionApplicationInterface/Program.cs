using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using CompanionApplication;

namespace CompanionApplication.Interface
{
    class Program
    {
        static RemoteManager _remoteManager;

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            //Application.Run(new Form1());

            _remoteManager = new RemoteManager();
            var mediaInterface = _remoteManager.GetMediaApplicationInterface("iTunes");

            mediaInterface.Next();
        }
    }
}

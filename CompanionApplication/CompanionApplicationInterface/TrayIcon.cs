using System;
using System.Windows.Forms;
using CompanionApplication.Core;

namespace CompanionApplication.Interface
{
    class TrayIcon : ApplicationContext
    {
        static RemoteManager _remoteManager;

        private NotifyIcon _notifyIcon;

        internal TrayIcon()
        {
            // Instantiate notification tray icon
            _notifyIcon = new NotifyIcon()
            {
                Icon = Properties.Resources.DisplayIcon,
                ContextMenu = new ContextMenu(new MenuItem[]
                {
                    new MenuItem("Test", Test),
                    new MenuItem("-"), // Separator
                    new MenuItem("Quit", Quit)
                }),
                Visible = true,
                Text = "Media Remote Companion",
            };

            _remoteManager = new RemoteManager();
            var mediaInterface = _remoteManager.SetMediaApplicationInterface("iTunes");
        }

        /// <summary>
        /// To run when user elects to quit from context menu
        /// </summary>
        private void Quit(object sender, EventArgs e)
        {
            // Dispose of interface
            _remoteManager.ApplicationInterface.Dispose();

            // Hide and dispose of notification tray icon
            _notifyIcon.Visible = false;
            _notifyIcon.Dispose();

            // Close the application
            Application.Exit();
        }

        /// <summary>
        /// For use during testing
        /// </summary>
        private void Test(object sender, EventArgs e)
        {
            _remoteManager.ApplicationInterface?.Next();
        }
    }
}

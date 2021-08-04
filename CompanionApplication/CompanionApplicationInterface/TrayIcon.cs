using System;
using System.Collections.Generic;
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
            // Instantiate remote manager
            _remoteManager = new RemoteManager();

            // Instantiate notification tray icon
            _notifyIcon = new NotifyIcon()
            {
                Icon = Properties.Resources.DisplayIcon,
                ContextMenu = new ContextMenu(),
                Visible = true,
                Text = "Media Remote Companion",
            };

            // Populate context menu
            _notifyIcon.ContextMenu.MenuItems.AddRange(new MenuItem[]
            {
                new MenuItem("Test", Test),
                new MenuItem("-"), // Separator
            });
            _notifyIcon.ContextMenu.MenuItems.AddRange(LoadInterfaces());
            _notifyIcon.ContextMenu.MenuItems.AddRange(new MenuItem[]
            {
                new MenuItem("-"), // Separator
                new MenuItem("Quit", Quit)
            });

            var mediaInterface = _remoteManager.SetMediaApplicationInterface("iTunes");
        }

        /// <summary>
        /// Automatically create context menu items to select interface
        /// </summary>
        private MenuItem[] LoadInterfaces()
        {
            MenuItem[] menuItems = new MenuItem[_remoteManager.AvailableMediaApplicationInterfaces.Count];

            for (int i = 0; i < menuItems.Length; i++)
            {
                string name = _remoteManager.AvailableMediaApplicationInterfaces[i];
                menuItems[i] = new MenuItem(name, InterfaceSelected)
                {
                    RadioCheck = true,
                };
            }

            return menuItems;
        }

        /// <summary>
        /// Triggered when interface is selected from context menu
        /// </summary>
        private void InterfaceSelected(object sender, EventArgs e)
        {

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

using System;
using System.Collections.Generic;
using System.Windows.Forms;
using CompanionApplication.Core;
using System.Linq;

namespace CompanionApplication.Interface
{
    internal enum MenuItemType
    {
        ApplicationInterface,
        Other
    }

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

            // For testing purposes, set interface to iTunes
            //var mediaInterface = _remoteManager.SetMediaApplicationInterface("iTunes");
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
                    Tag = MenuItemType.ApplicationInterface,
                };
            }

            return menuItems;
        }

        /// <summary>
        /// Triggered when interface is selected from context menu
        /// Sets interface and checks appropriate menu item
        /// </summary>
        private void InterfaceSelected(object sender, EventArgs e)
        {
            // Uncheck other applicaiton interface items
            foreach (MenuItem menuItem in _notifyIcon.ContextMenu.MenuItems)
            {
                if (Equals(menuItem.Tag, MenuItemType.ApplicationInterface) 
                    && menuItem != (MenuItem)sender)
                {
                    menuItem.Checked = false;
                }
            }

            // Check this item
            ((MenuItem)sender).Checked = true;

            // If the interface is different to the one already selected
            string selectedName = ((MenuItem)sender).Text;
            string currentName = _remoteManager.ApplicationInterface?.Name;
            if (currentName == null || selectedName != currentName)
            {
                // Dispose of current interface
                _remoteManager.ApplicationInterface?.Dispose();

                // Set interface to selected
                _remoteManager.SetMediaApplicationInterface(selectedName);
            }
        }

        /// <summary>
        /// To run when user elects to quit from context menu
        /// </summary>
        private void Quit(object sender, EventArgs e)
        {
            // Dispose of interface
            _remoteManager.ApplicationInterface?.Dispose();

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

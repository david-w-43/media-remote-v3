using CompanionApplication.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CompanionApplication.Interface
{
    public partial class SettingsForm : Form
    {
        static RemoteManager _remoteManager;

        public SettingsForm(RemoteManager remoteManager)
        {
            _remoteManager = remoteManager;
            InitializeComponent();
        }

        /// <summary>
        /// Open settings file for each loaded MEF plugin
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SettingsForm_Load(object sender, EventArgs e)
        {
            // Load settings from loaded parts
            foreach (string partName in _remoteManager.AvailableMediaApplicationInterfaces)
            {
                
            }
        }
    }
}

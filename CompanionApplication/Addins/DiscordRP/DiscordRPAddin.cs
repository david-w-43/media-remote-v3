using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CompanionApplication.Core;
using DiscordRPC;

namespace CompanionApplication.Addins.DiscordRP
{
    /// <summary>
    /// Discord RP addin
    /// </summary>
    [Export(typeof(IGetAddin))]
    [ExportMetadata("Name", "Discord RP")]
    public class AddinGetter : IGetAddin
    {
        internal static DiscordRPAddin _addin;

        /// <summary>
        /// Get the addin, creating one if it does not exist
        /// </summary>
        public IAddin Addin
        {
            get
            {
                if (Equals(_addin, null))
                    _addin = new DiscordRPAddin();
                return _addin;
            }
        }
    }

    public class DiscordRPAddin : IAddin
    {
        public string Name { get => "Discord RP"; }

        private DiscordRpcClient _client;

        private RichPresence _richPresence;

        private const string key = "697575193358368899";

        public DiscordRPAddin()
        {
            _client = new DiscordRpcClient(key);
            _client.Initialize();

            _richPresence = new RichPresence();
        }

        private void UpdateRP()
        {
            // Set RP
            _client.SetPresence(_richPresence);
        }

        public void UpdatePosition(int position)
        {
            throw new NotImplementedException();
        }

        public void UpdateSettings(PlaybackSettings playbackSettings)
        {
            throw new NotImplementedException();
        }

        public void UpdateTrack(TrackInformation trackInformation)
        {
            throw new NotImplementedException();
        }

        public void UpdateVolume(int volume)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            _client.ClearPresence();
            _client.Dispose();
            _client = null;
        }
    }
}

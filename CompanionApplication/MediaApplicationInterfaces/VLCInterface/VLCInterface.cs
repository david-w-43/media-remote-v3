using System;
using System.Timers;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.Composition;
using CompanionApplication.Core;

namespace CompanionApplication.MediaApplicationInterfaces.VLC
{
    /// <summary>
    /// VLC interface, placeholder
    /// </summary>
    [Export(typeof(IGetMediaApplicationInterface))]
    [ExportMetadata("Name", "VLC")]
    public class InterfaceGetter : IGetMediaApplicationInterface
    {
        internal static VLCInterface _interface;

        /// <summary>
        /// Get the interface, creating one if it does not exist
        /// </summary>
        public IMediaApplicationInterface Interface
        {
            get
            {
                if (Equals(_interface, null))
                    _interface = new VLCInterface();
                return _interface;
            }
        }
    }

    public class VLCInterface : IMediaApplicationInterface
    {
        public string Name { get => "VLC"; }

        public TrackInformation TrackInformation { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public int PlaybackPosition => throw new NotImplementedException();

        public int Volume { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public PlaybackSettings PlaybackSettings { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public event EventHandler<TrackUpdateEventArgs> TrackChanged;
        public event EventHandler<VolumeUpdateEventArgs> VolumeChanged;
        public event EventHandler<PlaybackSettingsUpdateEventArgs> PlaybackSettingsChanged;
        public event EventHandler<PlaybackPositionUpdateEventArgs> PlaybackPositionChanged;

        public VLCInterface()
        {
            // Do nothing (yet)
        }

        public void Backtrack()
        {
            throw new NotImplementedException();
        }

        public void Disconnect()
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public void FastForward()
        {
            throw new NotImplementedException();
        }

        public void IncrementRepeat()
        {
            throw new NotImplementedException();
        }

        public void Next()
        {
            throw new NotImplementedException();
        }

        public void Pause()
        {
            throw new NotImplementedException();
        }

        public void Play()
        {
            throw new NotImplementedException();
        }

        public void PlayPause()
        {
            throw new NotImplementedException();
        }

        public void Prev()
        {
            throw new NotImplementedException();
        }

        public void Rewind()
        {
            throw new NotImplementedException();
        }

        public void Stop()
        {
            throw new NotImplementedException();
        }

        public void ToggleShuffle()
        {
            throw new NotImplementedException();
        }
    }
}

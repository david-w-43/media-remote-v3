using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.Composition;
using CompanionApplication;
using iTunesLib;

namespace CompanionApplication.MediaApplicationInterfaces.iTunes
{
    /// <summary>
    /// iTunes interface, currently containing nothing
    /// </summary>
    [Export(typeof(IGetMediaApplicationInterface))]
    [ExportMetadata("Name", "iTunes")]
    public class InterfaceGetter : IGetMediaApplicationInterface
    {
        internal static iTunesInterface _interface;

        /// <summary>
        /// Get the interface, creating one if it does not exist
        /// </summary>
        public IMediaApplicationInterface Interface {
            get
            {
                if (Equals(_interface, null))
                    _interface = new iTunesInterface();
                return _interface;
            }
        }
    }

    public class iTunesInterface : IMediaApplicationInterface
    {
        private iTunesApp _application; 

        /// <summary>
        /// Constructor
        /// </summary>
        public iTunesInterface()
        {
            _application = new iTunesApp();
        }

        public string Title => throw new NotImplementedException();

        public string Artist => throw new NotImplementedException();

        public string Album => throw new NotImplementedException();

        public int TrackLength => throw new NotImplementedException();

        public int PlaybackPosition => throw new NotImplementedException();

        public int Volume { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public PlayStatus PlayStatus { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public RepeatMode RepeatMode { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public bool Shuffle { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public MediaType MediaType => throw new NotImplementedException();

        public void Disconnect()
        {
            throw new NotImplementedException();
        }

        public void IncrementRepeat()
        {
            throw new NotImplementedException();
        }

        public void Next()
        {
            _application.NextTrack();
        }

        public void PlayPause()
        {
            _application.PlayPause();
        }

        public void Prev()
        {
            _application.BackTrack();
        }

        public void ToggleShuffle()
        {
            throw new NotImplementedException();
        }
    }
}

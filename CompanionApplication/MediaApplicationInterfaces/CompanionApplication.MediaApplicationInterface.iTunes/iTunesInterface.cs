using System;
using System.Timers;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.Composition;
using CompanionApplication.Core;
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
        public IMediaApplicationInterface Interface
        {
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

        private Timer _timer;

        public event EventHandler<TrackUpdateEventArgs> TrackChanged;
        public event EventHandler<VolumeUpdateEventArgs> VolumeChanged;
        public event EventHandler<PlaybackSettingsUpdateEventArgs> PlaybackSettingsChanged;
        public event EventHandler<PlaybackPositionUpdateEventArgs> PlaybackPositionChanged;

        /// <summary>
        /// Constructor
        /// </summary>
        public iTunesInterface()
        {
            _application = new iTunesApp();

            // Start timer to monitor changes that are not subscribable
            _timer = new Timer()
            {
                Enabled = true,
                AutoReset = true,
                Interval = 50,
            };

            // Subscribe to events
            _timer.Elapsed += _timer_Elapsed;
            _application.OnPlayerPlayEvent += _application_OnPlayerPlayEvent;
            _application.OnSoundVolumeChangedEvent += _application_OnSoundVolumeChangedEvent;
            _application.OnQuittingEvent += _application_OnQuittingEvent;
        }

        /// <summary>
        /// Monitors non-subscribable changes
        /// </summary>
        private void _timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            // Get updated playback settings
            if (GetPlaybackSettings(out PlaybackSettings updatedSettings))
            {
                // Update property
                PlaybackSettings = updatedSettings;

                PlaybackSettingsChanged?.Invoke(this, new PlaybackSettingsUpdateEventArgs()
                {
                    PlaybackSettings = updatedSettings
                });
            }

            // Get updated playback position
            if (GetPlaybackPosition(out int updatedPosition))
            {
                PlaybackPosition = updatedPosition;

                PlaybackPositionChanged?.Invoke(this, new PlaybackPositionUpdateEventArgs()
                {
                    PlaybackPosition = updatedPosition
                });
            }
        }

        /// <summary>
        /// Gets updated playback settings
        /// </summary>
        /// <returns>True if changed</returns>
        private bool GetPlaybackSettings(out PlaybackSettings updatedPlaybackSettings)
        {
            updatedPlaybackSettings = new PlaybackSettings()
            {
                // Include check to see if paused
                PlayStatus = /*isPaused ? PlayStatus.Paused */ (PlayStatus)_application.PlayerState,

                // Set repeat mode, taking into account whether control is enabled
                RepeatMode = _application.CanSetSongRepeat[_application.CurrentPlaylist] ?
                    (RepeatMode)_application.CurrentPlaylist.SongRepeat : RepeatMode.Off,

                // Set shuffle, taking into account whether or not control is enabled
                Shuffle = _application.CanSetShuffle[_application.CurrentPlaylist] ?
                    _application.CurrentPlaylist.Shuffle : false,
            };

            return !Equals(updatedPlaybackSettings, PlaybackSettings);
        }

        /// <summary>
        /// Gets updated playback position
        /// </summary>
        /// <returns>True if changed</returns>
        private bool GetPlaybackPosition(out int updatedPosition)
        {
            updatedPosition = _application.PlayerPosition;

            return updatedPosition != PlaybackPosition;
        }

        private void _application_OnQuittingEvent()
        {
            Dispose();
        }

        /// <summary>
        /// Triggered when track resumed, started
        /// </summary>
        private void _application_OnPlayerPlayEvent(object iTrack)
        {
            IITTrack track = (IITTrack)iTrack;

            Console.WriteLine(track.Name);
            OnTrackChanged(new TrackUpdateEventArgs()
            {
                Title = track.Name,
                Artist = track.Artist,
                Album = track.Album,
                Length = track.Duration,
                MediaType = MediaType.Audio,
            });
        }

        /// <summary>
        /// Triggered when volume is changed
        /// </summary>
        private void _application_OnSoundVolumeChangedEvent(int newVolume)
        {
            VolumeChanged?.Invoke(this, new VolumeUpdateEventArgs()
            {
                Volume = newVolume
            });
        }

        public string Title => throw new NotImplementedException();

        public string Artist => throw new NotImplementedException();

        public string Album => throw new NotImplementedException();

        public int TrackLength => throw new NotImplementedException();

        public int PlaybackPosition { get; set; }

        public int Volume { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        //public PlayStatus PlayStatus { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        //public RepeatMode RepeatMode { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        //public bool Shuffle { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public MediaType MediaType => throw new NotImplementedException();

        public PlaybackSettings PlaybackSettings { get; set; }

        protected virtual void OnTrackChanged(TrackUpdateEventArgs e)
        {
            TrackChanged?.Invoke(this, e);
        }

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

        public void Dispose()
        {
            // Unsubscribe from events
            _application.OnPlayerPlayEvent -= _application_OnPlayerPlayEvent;
            _application.OnSoundVolumeChangedEvent -= _application_OnSoundVolumeChangedEvent;
            _application.OnQuittingEvent -= _application_OnQuittingEvent;

            // Set to null
            _application = null;
        }
    }
}

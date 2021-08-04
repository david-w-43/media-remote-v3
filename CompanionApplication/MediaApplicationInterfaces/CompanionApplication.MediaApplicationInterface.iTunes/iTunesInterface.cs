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
            try
            {
                if (_application.CurrentTrack != null)
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
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
                //throw;
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

            TrackInformation = new TrackInformation()
            {
                Title = track.Name,
                Artist = track.Artist,
                Album = track.Album,
                TrackLength = track.Duration,
                MediaType = MediaType.Audio,
            };

            //Console.WriteLine(track.Name);
            OnTrackChanged(new TrackUpdateEventArgs()
            {
                TrackInformation = TrackInformation
            }); ;
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

        public int PlaybackPosition
        {
            get => _application.PlayerPosition;
            set => _application.PlayerPosition = value;
        }

        public int Volume 
        { 
            get => _application.SoundVolume; 
            set => _application.SoundVolume = value; 
        }

        public PlaybackSettings PlaybackSettings { get; set; }
        public TrackInformation TrackInformation { get; set; }

        protected virtual void OnTrackChanged(TrackUpdateEventArgs e)
        {
            TrackChanged?.Invoke(this, e);
        }

        public void Next() => _application.NextTrack();

        public void Prev() => _application.PreviousTrack();

        public void Backtrack() => _application.BackTrack();
        
        public void PlayPause() => _application.PlayPause();

        public void Play()
        {
            // Resume playback (unpause or cancel ffw /rewind) of current track, 
            // Play() if nothing currently playing
            if (_application.CurrentTrack != null)
                _application.Resume();
            else
                _application.Play();
        }

        public void Pause() => _application.Pause();

        public void Stop() => _application.Stop();

        public void FastForward() => _application.FastForward();

        public void Rewind() => _application.Rewind();

        public void ToggleShuffle() =>
            _application.CurrentPlaylist.Shuffle = !_application.CurrentPlaylist.Shuffle;

        public void IncrementRepeat()
        {
            // I think it should be Off -> All -> One, so I am decrementing it
            ITPlaylistRepeatMode songRepeat = _application.CurrentPlaylist.SongRepeat;

            if (songRepeat == 0)
                songRepeat = ITPlaylistRepeatMode.ITPlaylistRepeatModeAll;
            else
                songRepeat--;
        }

        public void Disconnect()
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            // Unsubscribe from events
            _timer.Elapsed -= _timer_Elapsed;
            _application.OnPlayerPlayEvent -= _application_OnPlayerPlayEvent;
            _application.OnSoundVolumeChangedEvent -= _application_OnSoundVolumeChangedEvent;
            _application.OnQuittingEvent -= _application_OnQuittingEvent;

            _timer.Enabled = false;

            // Attempt to release resources and perform garbage collection
            // Prevents high resource usage (due to multiple connections?)
            try
            {
                System.Runtime.InteropServices.Marshal.ReleaseComObject(_application);
                GC.Collect();
            }
            catch (System.Runtime.InteropServices.InvalidComObjectException ex)
            {
                Console.WriteLine(ex.Message);
            }

            // Set to null
            _application = null;
        }


    }
}

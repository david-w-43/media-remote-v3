using System;
using System.Collections.Generic;
using System.Text;

namespace CompanionApplication.Core
{
    /// <summary>
    /// Based on iTunes
    /// </summary>
    public enum PlayStatus
    {
        Stopped = 0,
        Playing = 1,
        FastForward = 2,
        Rewind = 3,
        Paused = 4,
    }

    /// <summary>
    /// Based on iTunes
    /// </summary>
    public enum RepeatMode
    {
        Off = 0,
        One = 1,
        All = 2,
    }

    public enum MediaType
    {
        Audio,
        Video
    }

    public class TrackUpdateEventArgs : EventArgs
    {
        public TrackInformation TrackInformation { get; set; }
    }

    public class PlaybackSettingsUpdateEventArgs : EventArgs
    {
        public PlaybackSettings PlaybackSettings { get; set; }
    }

    public class PlaybackPositionUpdateEventArgs : EventArgs
    {
        public int PlaybackPosition { get; set; }
    }

    public class VolumeUpdateEventArgs : EventArgs
    {
        public int Volume { get; set; }
    }

    /// <summary>
    /// Struct for storing playback settings
    /// </summary>
    public struct PlaybackSettings
    {
        // Could have bit flag enum showing changed properties?

        public PlayStatus PlayStatus { get; set; }
        public RepeatMode RepeatMode { get; set; }
        public bool Shuffle { get; set; }
    }

    /// <summary>
    /// Struct for storing track information
    /// </summary>
    public struct TrackInformation
    {
        public string Title { get; set; }
        public string Artist { get; set; }
        public string Album { get; set; }
        public int TrackLength { get; set; }
        public MediaType MediaType { get; set; }
    }

    /// <summary>
    /// ???
    /// </summary>
    //public delegate bool TrackUpdateDelegate(TrackUpdateEventArgs e);

    /// <summary>
    /// Interface for all media applications
    /// </summary>
    public interface IMediaApplicationInterface : IDisposable
    {
        /// <summary>
        /// Current track information
        /// </summary>
        TrackInformation TrackInformation { get; set; }

        /// <summary>
        /// Playback position in seconds
        /// </summary>
        int PlaybackPosition { get; }

        /// <summary>
        /// Volume, application-defined
        /// </summary>
        int Volume { get; set; }

        /// <summary>
        /// Playback settings (status, repeat mode, shuffle)
        /// </summary>
        PlaybackSettings PlaybackSettings { get; set; }

        /// <summary>
        /// Skip to next track
        /// </summary>
        void Next();

        /// <summary>
        /// Play previous track
        /// </summary>
        void Prev();

        /// <summary>
        /// Skip to beginning of current track,
        /// play previous if already at start
        /// </summary>
        void Backtrack();

        /// <summary>
        /// Toggle play/pause
        /// </summary>
        void PlayPause();

        /// <summary>
        /// Play (application defined)
        /// </summary>
        void Play();

        /// <summary>
        /// Pause current track
        /// </summary>
        void Pause();

        /// <summary>
        /// Stop playback
        /// </summary>
        void Stop();

        /// <summary>
        /// Fast forward current track
        /// </summary>
        void FastForward();

        /// <summary>
        /// Rewind playback
        /// </summary>
        void Rewind();

        /// <summary>
        /// Toggle playlist shuffle
        /// </summary>
        void ToggleShuffle();

        /// <summary>
        /// Cycles through repeat modes
        /// </summary>
        void IncrementRepeat();

        /// <summary>
        /// Disconnect from the application
        /// </summary>
        void Disconnect();

        // Should have events in here too
        /// <summary>
        /// Raised when a track is changed
        /// </summary>
        event EventHandler<TrackUpdateEventArgs> TrackChanged;

        /// <summary>
        /// Raised when the volume is changed
        /// </summary>
        event EventHandler<VolumeUpdateEventArgs> VolumeChanged;

        /// <summary>
        /// Raised when the playback settings (play status, repeat, shuffle) change
        /// </summary>
        event EventHandler<PlaybackSettingsUpdateEventArgs> PlaybackSettingsChanged;

        /// <summary>
        /// Raised when the playback position changes
        /// </summary>
        event EventHandler<PlaybackPositionUpdateEventArgs> PlaybackPositionChanged;
    }

    public interface IGetMediaApplicationInterface
    {
        IMediaApplicationInterface Interface { get; }
    }

    /// <summary>
    /// Metadata for MEF
    /// </summary>
    public interface IGetMediaApplicationInterfaceData
    {
        string Name { get; }
    }
}

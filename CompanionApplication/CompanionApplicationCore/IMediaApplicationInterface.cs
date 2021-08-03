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
        // Basic properties
        public string Title { get; set; }
        public string Artist { get; set; }
        public string Album { get; set; }
        public int Length { get; set; }
        public MediaType MediaType { get; set; }
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
    /// ???
    /// </summary>
    public delegate bool TrackUpdateDelegate(TrackUpdateEventArgs e);

    /// <summary>
    /// Interface for all media applications
    /// </summary>
    public interface IMediaApplicationInterface : IDisposable
    {
        //// Basic properties
        string Title { get; }
        string Artist { get; }
        string Album { get; }

        int TrackLength { get; }
        int PlaybackPosition { get; }

        int Volume { get; set; }

        //PlayStatus PlayStatus { get => PlaybackSettings.; }
        //RepeatMode RepeatMode { get; }
        //bool Shuffle { get;}

        PlaybackSettings PlaybackSettings { get; set; }

        MediaType MediaType { get; }

        // Control methods, make these async?
        void Next();
        void Prev();
        void PlayPause();

        void ToggleShuffle();
        void IncrementRepeat();

        void Disconnect();

        // Should have events in here too
        event EventHandler<TrackUpdateEventArgs> TrackChanged;
        event EventHandler<VolumeUpdateEventArgs> VolumeChanged;
        event EventHandler<PlaybackSettingsUpdateEventArgs> PlaybackSettingsChanged;
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

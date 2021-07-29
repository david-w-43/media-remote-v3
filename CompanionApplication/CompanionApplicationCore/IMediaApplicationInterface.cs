using System;
using System.Collections.Generic;
using System.Text;

namespace CompanionApplication
{
    public enum PlayStatus
    {
        Playing,
        Paused,
        Stopped,
        FastForward,
        Rewind
    }

    public enum RepeatMode
    {
        Off,
        All,
        One
    }

    public enum MediaType
    {
        Audio,
        Video
    }

    /// <summary>
    /// Interface for all media applications
    /// </summary>
    public interface IMediaApplicationInterface
    {
        // Basic properties
        string Title { get; }
        string Artist { get; }
        string Album { get; }

        int TrackLength { get; }
        int PlaybackPosition { get; }

        int Volume { get; set; }

        PlayStatus PlayStatus { get; set; }
        RepeatMode RepeatMode { get; set; }
        bool Shuffle { get; set; }
        
        MediaType MediaType { get; }

        // Control methods, make these async?
        void Next();
        void Prev();
        void PlayPause();

        void ToggleShuffle();
        void IncrementRepeat();

        void Disconnect();

        // Should have events in here too
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

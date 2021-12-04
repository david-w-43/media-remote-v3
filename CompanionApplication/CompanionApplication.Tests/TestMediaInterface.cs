using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CompanionApplication.Core;


namespace CompanionApplication.Tests
{
    class TestMediaInterface : IMediaApplicationInterface
    {
        public string Name => throw new NotImplementedException();

        public TrackInformation TrackInformation { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public int PlaybackPosition => throw new NotImplementedException();

        public int Volume { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public PlaybackSettings PlaybackSettings { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public event EventHandler<TrackUpdateEventArgs> TrackChanged;
        public event EventHandler<VolumeUpdateEventArgs> VolumeChanged;
        public event EventHandler<PlaybackSettingsUpdateEventArgs> PlaybackSettingsChanged;
        public event EventHandler<PlaybackPositionUpdateEventArgs> PlaybackPositionChanged;

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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompanionApplication.Core
{
    /// <summary>
    /// Responsible for add-ins that do not affect the rest of the
    /// functionality, such as Discord Rich Presence
    /// </summary>
    public interface IAddin : IDisposable
    {
        /// <summary>
        /// Retrieves name of the add-in
        /// </summary>
        string Name { get; }
        
        /// <summary>
        /// Updates the track information
        /// </summary>
        void UpdateTrack(TrackInformation trackInformation);

        /// <summary>
        /// Updates the playback settings
        /// </summary>
        void UpdateSettings(PlaybackSettings playbackSettings);

        /// <summary>
        /// Updates the playback position
        /// </summary>
        void UpdatePosition(int position);

        /// <summary>
        /// Updates the volume
        /// </summary>
        void UpdateVolume(int volume);
    }

    public interface IGetAddin
    {
        /// <summary>
        /// Gets addin object, creates new if not present
        /// </summary>
        IAddin Addin { get; }
    }

    /// <summary>
    /// Metadata for MEF
    /// </summary>
    public interface IGetAddinData
    {
        string Name { get; }
    }
}

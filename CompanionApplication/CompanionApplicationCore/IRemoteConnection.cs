using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompanionApplication.Core
{
    /// <summary>
    /// Exception that indicates remote is not connected
    /// </summary>
    internal class RemoteNotPresentException : Exception
    {

    }

    internal class TransmissionReceivedEventArgs : EventArgs
    {
        internal Queue<string> Transmissions { get; set; }
    }


    internal interface IRemoteConnection : IDisposable
    {
        /// <summary>
        /// Disconnect from the remote
        /// </summary>
        void Disconnect();

        /// <summary>
        /// Send a command to the remote
        /// </summary>
        /// <param name="command"></param>
        void Send(Command command);

        /// <summary>
        /// Send a queue of commands to the remote
        /// </summary>
        /// <param name="commands"></param>
        void Send(Queue<Command> commands);

        //void DataReceived(object sender, EventArgs e);

        /// <summary>
        /// Transmission received by remote
        /// </summary>
        event EventHandler<TransmissionReceivedEventArgs> TransmissionReceived;

        /// <summary>
        /// Pushes updated track information to remote
        /// </summary>
        void TrackChanged(object sender, TrackUpdateEventArgs e);

        /// <summary>
        /// Pushes updated playback position to remote
        /// </summary>
        void PositionChanged(object sender, PlaybackPositionUpdateEventArgs e);
    }
}

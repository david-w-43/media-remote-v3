using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CompanionApplication.Core;


namespace CompanionApplication.Tests
{
    class TestRemoteInterface : IRemoteConnection
    {
        public event EventHandler<TransmissionReceivedEventArgs> TransmissionReceived;

        public void Disconnect()
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public void PositionChanged(object sender, PlaybackPositionUpdateEventArgs e)
        {
            throw new NotImplementedException();
        }

        public void Send(Command command)
        {
            throw new NotImplementedException();
        }

        public void Send(Queue<Command> commands)
        {
            throw new NotImplementedException();
        }

        public void TrackChanged(object sender, TrackUpdateEventArgs e)
        {
            throw new NotImplementedException();
        }
    }
}

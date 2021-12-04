using System;
using System.Collections.Generic;
using System.Text;
using System.IO.Ports;


namespace CompanionApplication.Core
{
    /// <summary>
    /// Shorthand for control characters used in communication with remote
    /// </summary>
    internal class ControlCharacters
    {
        internal static char StartOfText { get => '\u0002'; }
        internal static char EndOfText { get => '\u0003'; }
        internal static char EndOfTransmission { get => '\u0004'; }
        internal static char Enquiry { get => '\u0005'; }
        internal static char Acknowledge { get => '\u0006'; }
        internal static char NegativeAcknowledge { get => '\u0021'; }
        internal static char Synchronise { get => '\u0022'; }
        internal static char Cancel { get => '\u0024'; }
        internal static char RecordSeparator { get => '\u0030'; }
    }

    /// <summary>
    /// Command IDs
    /// </summary>
    internal enum CommandID : byte
    {
        SYSTEM = 0x00,
        Handshake,
        SetRTC,
        SetState,

        DISPLAY = 0x10,
        SetBacklight,
        SetContrast,

        MEDIA = 0x20,
        SetTrack,
        SetPosition,
        SetVolume,
        SetPlaybackSettings,
    }

    /// <summary>
    /// Contains data and formatting capabilities for commands to be 
    /// issued to remote
    /// </summary>
    internal class Command
    {
        internal CommandID CommandID { get; }
        internal string Text { get; }

        /// <summary>
        /// Create command with specified ID and text
        /// </summary>
        /// <param name="commandID"></param>
        /// <param name="text"></param>
        internal Command(CommandID commandID, byte fieldID, string text)
        {
            CommandID = commandID;
            Text = WrapText(fieldID, text);
        }

        /// <summary>
        /// Concatenate records into string
        /// </summary>
        /// <param name="commandID"></param>
        /// <param name="records"></param>
        internal Command(CommandID commandID, byte[] fieldIDs, string[] fields)
        {
            string text = "";
            for (int i = 0; i < fields.Length; i++)
            {
                // Add record to text
                text += WrapText(fieldIDs[i], fields[i]);

                // Add record separator
                if (i < fields.Length)
                    text += ControlCharacters.RecordSeparator;
            }

            CommandID = commandID;
            Text = text;
        }

        /// <summary>
        /// Wrap text with field, start and end control characters
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        private string WrapText(byte fieldID, string text)
        {
            return fieldID + ControlCharacters.StartOfText + text + ControlCharacters.EndOfText;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns>Command text as a byte array</returns>
        internal byte[] ToByteArray()
        {
            // Get text as array of bytes
            byte[] textBytes = RemoteConnection.Encoding.GetBytes(Text);
            int length = 1 + textBytes.Length;

            byte[] toReturn =  new byte[length];
            
            // Place data in array to return
            toReturn[0] = (byte)CommandID;
            textBytes.CopyTo(toReturn, 1);

            return toReturn;
        }
    }

    /// <summary>
    /// Handles serial communication with the remote
    /// </summary>
    internal class RemoteConnection : IRemoteConnection
    {
        // Constant definitions
        private const int _baudRate = 115200;
        internal static Encoding Encoding { get => Encoding.UTF8;} 

        // Serial port to communicate with
        SerialPort _port;

        // Queue in which to store received transmissions
        Queue<string> _transmissions = new Queue<string>();

        /// <summary>
        /// Instantiates a RemoteConnection
        /// </summary>
        internal RemoteConnection()
        {
            // Get names of ports connected
            Stack<string> portNames = new Stack<string>(SerialPort.GetPortNames());

            // Successful connection flag
            bool connected = false;
            
            // TODO: Push most recently successful to stack
            
            // Select serial port
            while (portNames.Count > 0)
            {
                // Define new port
                string name = portNames.Pop();
                _port = new SerialPort()
                {
                    PortName = name,
                    BaudRate = _baudRate,
                    Encoding = Encoding,
                };

                // Attempt to open port and handshake with remote
                try
                {
                    _port.Open();

                    // Write name to serial
                    Send(new Command(0, 0, name));

                    // Wait for response
                    string response = Read();

                    // Expect remote to echo port name
                    if (response != name)
                        continue;
                    else
                    {
                        // Remote has been found
                        connected = true;
                        break;
                    }
                }
                catch (UnauthorizedAccessException)
                {
                    // Port is likely already in use
                    continue;
                }
                catch (TimeoutException)
                {
                    // Either device is unresponsive or it is not the correct port
                    continue;
                }
            }

            // Throw exception if not connected
            if (!connected)
                throw new RemoteNotPresentException();
            else
            {
                // Subscribe to DataRecieved event
                _port.DataReceived += DataReceived;
            }
            
        }

        
        public void Send(Command command)
        {
            byte[] buffer = command.ToByteArray();
            _port.Write(buffer, 0, buffer.Length);
        }

        string Read()
        {
            // Read up to end of transmission
            string received = _port.ReadTo(ControlCharacters.EndOfTransmission.ToString());

            return received;
        }

        public void Disconnect()
        {
            _port.Dispose();
        }

        /// <summary>
        /// Send a queue of commands
        /// </summary>
        /// <param name="commands"></param>
        public void Send(Queue<Command> commands)
        {
            while (commands.Count > 0)
                Send(commands.Dequeue());
        }

        public void Dispose()
        {
            Disconnect();
        }

        /// <summary>
        /// Called when data received by serial port
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void DataReceived(object sender, EventArgs e)
        {
            // While there are bytes to read
            while (_port.BytesToRead > 0)
            {
                // Add full messages to queue
                _transmissions.Enqueue(_port.ReadTo(ControlCharacters.EndOfTransmission.ToString()));
            }
        }

        public void TrackChanged(object sender, TrackUpdateEventArgs e)
        {
            Console.WriteLine($"Title: {e.TrackInformation.Title}\n" +
                $"Artist: {e.TrackInformation.Artist}\n" +
                $"Album: {e.TrackInformation.Album}\n" +
                $"Length: {e.TrackInformation.TrackLength}");
        }

        public void PositionChanged(object sender, PlaybackPositionUpdateEventArgs e)
        {
            Console.WriteLine($"Position: {e.PlaybackPosition} s");
        }
    }
}

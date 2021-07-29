using System;
using System.Collections.Generic;
using System.Text;
using System.IO.Ports;


namespace CompanionApplication
{
    /// <summary>
    /// Stores useful control characters
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

    internal class Command
    {
        internal byte CommandID { get; }
        internal string Text { get; }

        internal Command(byte commandID, string text)
        {
            CommandID = commandID;
            Text = text;
        }

        /// <summary>
        /// Concatenate records into string
        /// </summary>
        /// <param name="commandID"></param>
        /// <param name="records"></param>
        internal Command(byte commandID, string[] records)
        {
            string text = "";
            for (int i = 0; i < records.Length; i++)
            {
                // Add record to text
                text += records[i];

                // Add record separator
                if (i < records.Length)
                    text += ControlCharacters.RecordSeparator;
            }

            CommandID = commandID;
            Text = text;
        }

        internal byte[] ToByteArray()
        {
            // Get text as array of bytes
            byte[] textBytes = RemoteConnection.Encoding.GetBytes(Text);
            int length = 1 + textBytes.Length;

            byte[] toReturn =  new byte[length];
            
            // Place data in array to return
            toReturn[0] = CommandID;
            textBytes.CopyTo(toReturn, 1);

            return toReturn;
        }
    }

    /// <summary>
    /// Handles serial communication with the remote
    /// </summary>
    internal class RemoteConnection
    {
        // Constant definitions
        private const int _baudRate = 115200;
        internal static Encoding Encoding { get => Encoding.UTF8;} 

        // Serial port to communicate with
        SerialPort _port;

        /// <summary>
        /// Instantiates a RemoteConnection
        /// </summary>
        internal RemoteConnection()
        {
            // Get names of ports connected
            Stack<string> portNames = new Stack<string>(SerialPort.GetPortNames());
            
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
                    Send(new Command(0, name));

                    // Wait for response
                    string response = Read();

                    // Expect remote to echo port name
                    if (response != name)
                        continue;
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

            
        }

        
        void Send(Command command)
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
    }
}

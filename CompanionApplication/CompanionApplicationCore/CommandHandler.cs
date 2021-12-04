using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompanionApplication.Core
{
    

    internal class CommandHandler
    {
        /// <summary>
        /// Media application interface
        /// </summary>
        internal IMediaApplicationInterface ApplicationInterface { get; set; }

        /// <summary>
        /// Remote connection interface
        /// </summary>
        internal IRemoteConnection RemoteConnection { get; set; }

        internal CommandHandler(IMediaApplicationInterface mediaApplicationInterface, IRemoteConnection remoteConnection)
        {
            ApplicationInterface = mediaApplicationInterface;
            RemoteConnection = remoteConnection;
        }


    }
}

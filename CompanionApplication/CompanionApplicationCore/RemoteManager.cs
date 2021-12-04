using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;

namespace CompanionApplication.Core
{
    /// <summary>
    /// Manages MEF and 
    /// </summary>
    public class RemoteManager
    {
        // Used for MEF, contains parts
        private CompositionContainer _container;

#pragma warning disable CS0649
        // Import all available Media Application Interfaces
        [ImportMany]
        internal IEnumerable<Lazy<IGetMediaApplicationInterface, IGetMediaApplicationInterfaceData>> MediaApplicationInterfaces;

        // Import all available addins
        [ImportMany]
        internal IEnumerable<Lazy<IGetAddin, IGetAddinData>> Addins;
#pragma warning restore

        /// <summary>
        /// Current application interface - only one can be loaded
        /// </summary>
        private IMediaApplicationInterface _applicationInterface;

        /// <summary>
        /// Current remote connection
        /// </summary>
        private IRemoteConnection _remoteConnection;

        /// <summary>
        /// Command handler
        /// </summary>
        private CommandHandler _commandHandler;

        /// <summary>
        /// Currently loaded addins - multiple can be loaded or unloaded
        /// </summary>
        private List<IAddin> _addins = new List<IAddin>();

        /// <summary>
        /// Active application interface
        /// </summary>
        public IMediaApplicationInterface ApplicationInterface { get => _applicationInterface; }

        /// <summary>
        /// List of names of available media application interface
        /// </summary>
        public List<string> AvailableMediaApplicationInterfaces 
        {
            get
            {
                List<string> list = new List<string>();

                foreach (var part in MediaApplicationInterfaces)
                    list.Add(part.Metadata.Name);

                return list;
            }
        }

        /// <summary>
        /// List of names of loaded addins
        /// </summary>
        public List<string> LoadedAddins 
        {
            get
            {
                List<string> list = new List<string>();

                foreach (var addin in Addins)
                    list.Add(addin.Metadata.Name);

                return list;
            }
        }

        /// <summary>
        /// Instantiate a RemoteManager object
        /// </summary>
        public RemoteManager()
        {
            try
            {
                // Aggregate catalog that combines multiple catalogs
                AggregateCatalog catalog = new AggregateCatalog();

                // Add all parts from this assembly
                catalog.Catalogs.Add(new AssemblyCatalog(typeof(RemoteManager).Assembly));

                // Also add parts in base directory
                catalog.Catalogs.Add(new DirectoryCatalog(AppContext.BaseDirectory));

                // Create the CompositionContainer with the catalog parts
                _container = new CompositionContainer(catalog);
                _container.ComposeParts(this);
            }
            catch (CompositionException e)
            {
                // If composition exception occurs, display in console
                Console.WriteLine(e.ToString());
                throw e;
            }

            // Load all addins (use settings to select?)
            LoadAddins();

            // Connect to remote
            _remoteConnection = new RemoteConnection();

            // Create command handler
            _commandHandler = new CommandHandler(_applicationInterface, _remoteConnection);
        }

        private void LoadAddins()
        {
            foreach (var addin in Addins)
            {
                _addins.Add(addin.Value.Addin);
            }
        }


        /// <summary>
        /// May not work
        /// Gets the named media application interface
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public bool SetMediaApplicationInterface(string name)
        {
            foreach (var mediaApplicationInterface in MediaApplicationInterfaces)
            {
                if (mediaApplicationInterface.Metadata.Name == name)
                {
                    //return mediaApplicationInterface.Value.Interface;
                    _applicationInterface = mediaApplicationInterface.Value.Interface;

                    // Subscribe to events
                    _applicationInterface.PlaybackPositionChanged += _remoteConnection.PositionChanged;
                    _applicationInterface.TrackChanged += _remoteConnection.TrackChanged;

                    // Change command handler interface?
                    //_commandHandler.ApplicationInterface = _applicationInterface;

                    return true;
                }
            }
            return false;
            //return null;
        }

        
    }
}

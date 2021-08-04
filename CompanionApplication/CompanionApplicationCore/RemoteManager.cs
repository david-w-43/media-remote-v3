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
#pragma warning restore

        private IMediaApplicationInterface _applicationInterface;

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
                    _applicationInterface.PlaybackPositionChanged += TestPositionChanged;
                    _applicationInterface.TrackChanged += TestTrackChanged;

                    return true;
                }
            }
            return false;
            //return null;
        }

        /// <summary>
        /// For testing purposes
        /// </summary>
        private void TestTrackChanged(object sender, TrackUpdateEventArgs e)
        {
            Console.WriteLine($"Title: {e.TrackInformation.Title}\n" +
                $"Artist: {e.TrackInformation.Artist}\n" +
                $"Album: {e.TrackInformation.Album}\n" +
                $"Length: {e.TrackInformation.TrackLength}");
        }

        /// <summary>
        /// For testing purposes
        /// </summary>
        private void TestPositionChanged(object sender, PlaybackPositionUpdateEventArgs e)
        {
            Console.WriteLine($"Position: {e.PlaybackPosition} s");
        }

        
    }
}

﻿using System;
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

        IMediaApplicationInterface _applicationInterface;
        public IMediaApplicationInterface ApplicationInterface { get => _applicationInterface; }

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
        /// For testing purposes
        /// </summary>
        private void Test(object sender, PlaybackPositionUpdateEventArgs e)
        {
            Console.WriteLine($"Position: {e.PlaybackPosition} s");
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
                    _applicationInterface.PlaybackPositionChanged += Test;

                    return true;
                }
            }
            return false;
            //return null;
        }
    }
}

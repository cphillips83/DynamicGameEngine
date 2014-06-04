using Disseminate.Core;
using Disseminate.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Disseminate.FileSystem
{
    public sealed class ArchiveManager : Singleton<ArchiveManager>
    {
        #region Fields and Properties

        /// <summary>
        /// The list of factories
        /// </summary>
        private Dictionary<string, ArchiveFactory> _factories = new Dictionary<string, ArchiveFactory>();

        private Dictionary<string, Archive> _archives = new Dictionary<string, Archive>();

        #endregion

        #region Constructor

        /// <summary>
        /// Internal constructor.  This class cannot be instantiated externally.
        /// </summary>
        public ArchiveManager() { }

        #endregion Constructor

        #region Methods

        /// <summary>
        /// Opens an archive for file reading.
        /// </summary>
        /// <remarks>
        /// The archives are created using class factories within
        /// extension libraries.
        /// </remarks>
        /// <param name="filename">The filename that will be opened</param>
        /// <param name="archiveType">The library that contains the data-handling code</param>
        /// <returns>
        /// If the function succeeds, a valid pointer to an Archive object is returned.
        /// <para/>
        /// If the function fails, an exception is thrown.
        /// </returns>
        public Archive Load(string filename, string archiveType)
        {
            Archive arch = null;
            if (!_archives.TryGetValue(filename, out arch))
            {
                // Search factories
                ArchiveFactory fac = null;
                if (!_factories.TryGetValue(archiveType, out fac))
                {
                    throw ExceptionFactory.CreateFatalException("Cannot find an archive factory to deal with archive of type {0}", archiveType);
                }

                arch = fac.CreateInstance(filename);
                arch.Load();
                _archives.Add(filename, arch);
            }
            return arch;
        }

        #region Unload Method

        /// <summary>
        ///  Unloads an archive.
        ///  </summary>
        ///  <remarks>
        /// You must ensure that this archive is not being used before removing it.
        ///  </remarks>
        /// <param name="arch">The Archive to unload</param>
        public void Unload(Archive arch)
        {
            Unload(arch.Name);
        }

        /// <summary>
        ///  Unloads an archive.
        ///  </summary>
        ///  <remarks>
        /// You must ensure that this archive is not being used before removing it.
        ///  </remarks>
        /// <param name="arch">The Archive to unload</param>
        public void Unload(string filename)
        {
            Archive arch = _archives[filename];

            if (arch != null)
            {
                arch.Unload();

                ArchiveFactory fac = _factories[arch.Type];
                if (fac == null)
                {
                    throw ExceptionFactory.CreateFatalException("Cannot find an archive factory to deal with archive of type {0}", arch.Type);
                }
                _archives.Remove(arch.Name);
                fac.DestroyInstance(ref arch);
            }
        }

        #endregion Unload Method

        /// <summary>
        /// Add an archive factory to the list
        /// </summary>
        /// <param name="type">The type of the factory (zip, file, etc.)</param>
        /// <param name="factory">The factory itself</param>
        public void AddArchiveFactory(ArchiveFactory factory)
        {
            if (_factories.ContainsKey(factory.Type) == true)
            {
                throw ExceptionFactory.CreateFatalException("Attempted to add the {0} factory to ArchiveManager more than once.", factory.Type);
            }

            _factories.Add(factory.Type, factory);
            LogManager.Instance.Write("ArchiveFactory for archive type {0} registered.", factory.Type);
        }

        #endregion Methods

        #region Singleton<ArchiveManager> Implementation

        /// <summary>
        ///     Called when the engine is shutting down.
        /// </summary>
        protected override void dispose(bool disposeManagedResources)
        {
            if (!isDisposed)
            {
                if (disposeManagedResources)
                {
                    // Unload & delete resources in turn
                    foreach (KeyValuePair<string, Archive> arch in _archives)
                    {
                        // Unload
                        arch.Value.Unload();

                        // Find factory to destroy
                        ArchiveFactory fac = _factories[arch.Value.Type];
                        if (fac == null)
                        {
                            // Factory not found
                            throw ExceptionFactory.CreateFatalException("Cannot find an archive factory to deal with archive of type {0}", arch.Value.Type);
                        }
                        Archive tmp = arch.Value;
                        fac.DestroyInstance(ref tmp);
                    }

                    // Empty the list
                    _archives.Clear();
                }

                // There are no unmanaged resources to release, but
                // if we add them, they need to be released here.
            }

            // If it is available, make the call to the
            // base class's Dispose(Boolean) method
            base.dispose(disposeManagedResources);
        }

        #endregion Singleton<ArchiveManager> Implementation
    }

}

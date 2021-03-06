﻿using Disseminate.Core;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Disseminate.FileSystem
{
    public class FileSystemArchive : Archive
    {
        #region Fields and Properties

        /// <summary>Base path; actually the same as Name, but for clarity </summary>
        private string _basePath;

        /// <summary>Directory stack of previous directories </summary>
        private Stack<string> _directoryStack = new Stack<string>();

        ///// <summary>
        ///// Is this archive capable of being monitored for additions, changes and deletions
        ///// </summary>
        //public override bool IsMonitorable { get { return true; } }

        #endregion Fields and Properties

        #region Utility Methods

        protected delegate void Action();

        protected void SafeDirectoryChange(string directory, Action action)
        {
            if (Directory.Exists(directory))
            {
                // Check we can change to it
                pushDirectory(directory);

                try
                {
                    action();
                }
                catch (Exception ex)
                {
                    LogManager.Instance.Write(LogManager.BuildExceptionString(ex));
                }
                finally
                {
                    // return to previous
                    popDirectory();
                }
            }
        }

        /// <overloads>
        /// <summary>
        /// Utility method to retrieve all files in a directory matching pattern.
        /// </summary>
        /// <param name="pattern">File pattern</param>
        /// <param name="recursive">Whether to cascade down directories</param>
        /// <param name="simpleList">Populated if retrieving a simple list</param>
        /// <param name="detailList">Populated if retrieving a detailed list</param>
        /// </overloads>
        protected void findFiles(string pattern, bool recursive, List<string> simpleList, FileInfoList detailList)
        {
            findFiles(pattern, recursive, simpleList, detailList, "");
        }

        /// <param name="currentDir">The current directory relative to the base of the archive, for file naming</param>
        protected void findFiles(string pattern, bool recursive, List<string> simpleList, FileInfoList detailList, string currentDir)
        {
            if (pattern == "")
            {
                pattern = "*";
            }
            if (currentDir == "")
            {
                currentDir = _basePath;
            }

            string[] files;

#if !( XBOX || XBOX360 )
            files = Directory.GetFiles(currentDir, pattern, recursive ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly);
#else
			files = recursive ? this.getFilesRecursively(currentDir, pattern) : Directory.GetFiles(currentDir, pattern);
#endif
            foreach (string file in files)
            {
                System.IO.FileInfo fi = new System.IO.FileInfo(file);
                if (simpleList != null)
                {
                    simpleList.Add(fi.Name);
                }
                if (detailList != null)
                {
                    FileInfo fileInfo;
                    fileInfo.Archive = this;
                    fileInfo.Filename = fi.FullName;
                    fileInfo.Basename = fi.FullName.Substring(Path.GetFullPath(currentDir).Length);
                    fileInfo.Path = currentDir;
                    fileInfo.CompressedSize = fi.Length;
                    fileInfo.UncompressedSize = fi.Length;
                    fileInfo.ModifiedTime = fi.LastWriteTime;
                    detailList.Add(fileInfo);
                }
            }
        }

#if ( XBOX || XBOX360 )
	/// <summary>
	/// Returns the names of all files in the specified directory that match the specified search pattern, performing a recursive search
	/// </summary>
	/// <param name="dir">The directory to search.</param>
	/// <param name="pattern">The search string to match against the names of files in path.</param>
		private string[] getFilesRecursively( string dir, string pattern )
		{
			List<string> searchResults = new List<string>();
			string[] folders = Directory.GetDirectories( dir );
			string[] files = Directory.GetFiles( dir );

			foreach ( string folder in folders )
			{
				searchResults.AddRange( this.getFilesRecursively( dir + Path.GetFileName( folder ) + "\\", pattern ) );
			}

			foreach ( string file in files )
			{
				string ext = Path.GetExtension( file );

				if ( pattern == "*" || pattern.Contains( ext ) )
					searchResults.Add( file );
			}

			return searchResults.ToArray();
		}
#endif

        /// <summary>Utility method to change the current directory </summary>
        protected void changeDirectory(string dir)
        {
            Directory.SetCurrentDirectory(dir);
        }

        /// <summary>Utility method to change directory and push the current directory onto a stack </summary>
        private void pushDirectory(string dir)
        {
            // get current directory and push it onto the stack
#if !( XBOX || XBOX360 )
            string cwd = Directory.GetCurrentDirectory();
            _directoryStack.Push(cwd);
#endif
            changeDirectory(dir);
        }

        /// <summary>Utility method to pop a previous directory off the stack and change to it </summary>
        private void popDirectory()
        {
            if (_directoryStack.Count == 0)
            {
#if !( XBOX || XBOX360 )
                throw new AxiomException("No directories left in the stack.");
#else
				return;
#endif
            }
            string cwd = _directoryStack.Pop();
            changeDirectory(cwd);
        }

        #endregion Utility Methods

        #region Constructors and Destructors

        public FileSystemArchive(string name, string archType)
            : base(name, archType) { }

        ~FileSystemArchive()
        {
            Unload();
        }

        #endregion Constructors and Destructors

        #region Archive Implementation

        public override bool IsCaseSensitive { get { return true; } }

        public override void Load()
        {
            _basePath = Path.GetFullPath(Name) + Path.DirectorySeparatorChar;
            IsReadOnly = false;

            SafeDirectoryChange(_basePath, () =>
            {
                try
                {
#if !( XBOX || XBOX360 )
                    File.Create(_basePath + @"__testWrite.Axiom", 1, FileOptions.DeleteOnClose);
#else
													File.Create(_basePath + @"__testWrite.Axiom", 1 );
#endif
                }
                catch //(Exception ex)
                {
                    IsReadOnly = true;
                }
            });
        }

        public override Stream Create(string filename, bool overwrite)
        {
            if (IsReadOnly)
            {
                throw new AxiomException("Cannot create a file in a read-only archive.");
            }

            Stream stream = null;
            string fullPath = _basePath + Path.DirectorySeparatorChar + filename;
            bool exists = File.Exists(fullPath);
            if (!exists || overwrite)
            {
                try
                {
#if !( XBOX || XBOX360 )
                    stream = File.Create(fullPath, 1, FileOptions.RandomAccess);
#else
					stream = File.Create( fullPath, 1 );
#endif
                }
                catch (Exception ex)
                {
                    throw new AxiomException("Failed to open file : " + filename, ex);
                }
            }
            else
            {
                stream = Open(fullPath, false);
            }

            return stream;
        }

        public override void Unload()
        {
            // Nothing to do here.
        }

        public override System.IO.Stream Open(string filename, bool readOnly)
        {
            Stream strm = null;

            SafeDirectoryChange(_basePath, () =>
            {
                if (File.Exists(_basePath + filename))
                {
                    System.IO.FileInfo fi = new System.IO.FileInfo(_basePath + filename);
                    strm = (Stream)fi.Open(FileMode.Open, readOnly ? FileAccess.Read : FileAccess.ReadWrite);
                }
            });

            return strm;
        }

        public override List<string> List(bool recursive)
        {
            return Find("*", recursive);
        }

        public override FileInfoList ListFileInfo(bool recursive)
        {
            return FindFileInfo("*", recursive);
        }

        public override List<string> Find(string pattern, bool recursive)
        {
            List<string> ret = new List<string>();

            SafeDirectoryChange(_basePath, () => findFiles(pattern, recursive, ret, null));

            return ret;
        }

        public override FileInfoList FindFileInfo(string pattern, bool recursive)
        {
            FileInfoList ret = new FileInfoList();

            SafeDirectoryChange(_basePath, () => findFiles(pattern, recursive, null, ret));

            return ret;
        }

        public override bool Exists(string fileName)
        {
            return File.Exists(_basePath + fileName);
        }

        #endregion Archive Implementation
    }

    public class FileSystemArchiveFactory : ArchiveFactory
    {
        private const string _type = "Folder";

        #region ArchiveFactory Implementation

        public override string Type { get { return _type; } }

        public override Archive CreateInstance(string name)
        {
            return new FileSystemArchive(name, _type);
        }

        public override void DestroyInstance(ref Archive obj)
        {
            obj.Dispose();
        }

        #endregion ArchiveFactory Implementation
    };
}

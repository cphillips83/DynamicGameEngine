using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Disseminate.Core
{
    public abstract class AbstractResourceFactory
    {
        private List<string> searchPathes = new List<string>();

        public AbstractResourceFactory(string rootDirectory)
        {
            searchPathes.Add(rootDirectory);
        }

        public void add(string path)
        {
            searchPathes.Add(path);
        }

        public string file(string file)
        {
            file = file.Replace('/', '\\');
            if (System.IO.File.Exists(file))
                return file;

            foreach (var path in searchPathes)
            {
                var newfile = System.IO.Path.Combine(path, file);
                if (System.IO.File.Exists(newfile))
                    return newfile;
            }

            return null;
        }

        public IEnumerable<string> files(string folder)
        {
            var path = string.Empty;
            folder = this.folder(folder, out path);
            
            var files = System.IO.Directory.GetFiles(folder);
            foreach (var file in files)
                yield return file.Replace(path, string.Empty);
        }

        public IEnumerable<string> folders(string folder)
        {
            var path = string.Empty;
            folder = this.folder(folder, out path);

            var folders = System.IO.Directory.GetDirectories(folder);
            foreach (var f in folders)
                yield return f.Replace(path, string.Empty);
        }

        protected string folder(string folder, out string path)
        {
            path = null;
            folder = folder.Replace('/', '\\');
            //if (System.IO.Directory.Exists(folder))
            //    return folder;

            foreach (var _path in searchPathes)
            {
                path = _path;
                var newFolder = System.IO.Path.Combine(path, folder);
                if (System.IO.Directory.Exists(newFolder))
                    return newFolder;
            }

            return null;

        }

        public string folder(string folder)
        {
            var path = string.Empty;
            return this.folder(folder, out path);
        }
    }
}

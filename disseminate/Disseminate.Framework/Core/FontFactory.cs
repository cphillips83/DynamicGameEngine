//using Disseminate.Graphics;
//using System;
//using System.Collections.Generic;
//using System.IO;
//using System.Linq;
//using System.Text;

//namespace Disseminate.Core
//{
//    public class FontFactory<T, M>
//        where T : GLFont
//        where M : GLMaterial
//    {
//        protected Dictionary<string, T> _fonts = new Dictionary<string, T>();
//        protected AbstractMaterialFactory<M> _materials;
//        protected AbstractResourceFactory _resources;
//        private ushort[] _freeIds = new ushort[ushort.MaxValue];
//        private int _freeIndex = ushort.MaxValue - 1;

//        public FontFactory(AbstractMaterialFactory<M> mf, AbstractResourceFactory rf)
//        {
//            _resources = rf;
//            for (var i = 0; i < _freeIds.Length; i++)
//                _freeIds[ushort.MaxValue - i - 1] = (ushort)i;
//        }

//        private ushort getNextId()
//        {
//            if (_freeIndex == 0)
//                throw new Exception("No more entities free!");

//            _freeIndex--;
//            var id = _freeIds[_freeIndex];

//            return id;
//        }

//        private void freeId(ushort id)
//        {
//            _freeIds[_freeIndex] = id;
//            _freeIndex++;
//        }

//        public T get(string name)
//        {
//            name = name.ToLower();
//            return _fonts[name];
//        }

//        public T load(string name, string file)
//        {
//            var t = default(T);
//            if (!_fonts.TryGetValue(name, out t))
//            {
//                var location = _resources.file(file);
//                if (location == null)
//                {
//                    return null;
//                }
//                else
//                {
//                    var id = getNextId();
//                    t = loadFont(id, name, location);
//                    if (t == null)
//                    {
//                        return null;
//                    }
//                    else
//                    {
//                        _fonts.Add(name, t);
//                    }
//                }
//            }

//            return t;
//        }

//        public void unload(string name)
//        {
//            T t;
//            if (_fonts.TryGetValue(name, out t))
//            {
//                freeId(t.id);
//                _fonts.Remove(name);
//                t.Dispose();
//            }
//        }

//        public void clear()
//        {
//            var fonts = _fonts.Keys.ToArray();
//            foreach (var f in fonts)
//                unload(f);
//        }

//        protected virtual T loadFont(ushort id, string name, string file)
//        {
//            var fontFilePath = _resources.file(file);
//            var fontFile = FontLoader.Load(fontFilePath);
//            var fontPng = _resources.file(System.IO.Path.Combine(System.IO.Path.GetDirectoryName(file), fontFile.Pages[0].File));
//            var fontMaterial = _materials.load(name, fontPng);
            
//            return new GLFont(id, fontFile, fontMaterial);
//        }

//    }
//}

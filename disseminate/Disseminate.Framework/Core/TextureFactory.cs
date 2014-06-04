using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Disseminate.Graphics;
using Disseminate.Math;

namespace Disseminate.Core
{
    public abstract class AbstractTextureFactory<T>
        where T : GLTexture2D
    {
        protected static int id = 0;
        protected Dictionary<string, T> _textures = new Dictionary<string, T>();

        private ushort[] _freeIds = new ushort[ushort.MaxValue];
        private int _freeIndex = ushort.MaxValue - 1;

        protected AbstractResourceFactory _resources;

        public AbstractTextureFactory(AbstractResourceFactory rf)
        {
            _resources = rf;
            for (var i = 0; i < _freeIds.Length; i++)
                _freeIds[ushort.MaxValue - i - 1] = (ushort)i;

        }

        private ushort getNextId()
        {
            if (_freeIndex == 0)
                throw new Exception("No more entities free!");

            _freeIndex--;
            var id = _freeIds[_freeIndex];

            return id;
        }

        private void freeId(ushort id)
        {
            _freeIds[_freeIndex] = id;
            _freeIndex++;
        }

        public T create(int width, int height)
        {
            return create(null, width, height);
        }

        public T create(string name, int width, int height)
        {
            var id = getNextId();
            if (string.IsNullOrEmpty(name))
                name = string.Format("_texture_{0}_", id);

            var t = createTexture(id, name, width, height);
            _textures.Add(name, t);
            return t;
        }

        public T get(string name)
        {
            name = name.ToLower();
            return _textures[name];
        }

        //public T find(string name)
        //{
        //    name = name.ToLower();

        //    T t;
        //    if (_textures.TryGetValue(name, out t))
        //        return t;

        //    return load(name);
        //}

        public T load(string name, string file)
        {
            //name = name.ToLower();

            var t = default(T);
            if (!_textures.TryGetValue(name, out t))
            {
                var location = _resources.file(file);
                if (location == null)
                {
                    t = create(1, 1);
                    t.SetData(new Color[] { Color.HotPink });
                }
                else
                {
                    var id = getNextId();
                    t = loadTexture(id, name, location);
                    _textures.Add(name, t);
                }
            }

            return t;
        }

        public void unload(string file)
        {
            file = file.ToLower();

            T t;
            if (_textures.TryGetValue(file, out t))
            {
                _textures.Remove(file);
                t.Dispose();
            }
        }

        public void clear()
        {
            var textures = _textures.Keys.ToArray();
            foreach (var t in textures)
                unload(t);
        }

        protected abstract T loadTexture(ushort id, string name, string file);
        protected abstract T createTexture(ushort id, string name, int width, int height);
    }
}

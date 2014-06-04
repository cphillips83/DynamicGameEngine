using Disseminate.Math;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Disseminate.Graphics
{
    //public interface ITexture2D : IDisposable
    //{
    //    int width { get; }
    //    int height { get; }
    //    void GetData<T>(ref T[] data) where T : struct;
    //    void GetData<T>(ref T[] data, int startIndex, int elementCount) where T : struct;
    //    void SetData<T>(T[] data) where T : struct;
    //    void SetData<T>(T[] data, int startIndex, int elementCount) where T : struct;

    //    void draw(GLRenderable item);
    //}

    public abstract class GLTexture2D : IDisposable
    {
        private bool isDisposed = false;

        public ushort id { get; private set; }
        public string name { get; private set; }

        public GLTexture2D(ushort id, string name)
        {
            this.id = id;
            this.name = name;
        }

        public int width { get { checkDisposed(); return getWidth(); } }
        public int height { get { checkDisposed(); return getHeight(); } }
        public void GetData<T>(ref T[] data) where T : struct { checkDisposed(); getData(ref data); }
        public void GetData<T>(ref T[] data, int startIndex, int elementCount) where T : struct { checkDisposed(); getData(ref data, startIndex, elementCount); }
        public void SetData<T>(T[] data) where T : struct { checkDisposed(); setData(data); }
        public void SetData<T>(T[] data, int startIndex, int elementCount) where T : struct { checkDisposed(); setData(data, startIndex, elementCount); }

        public void draw(GLRenderable item) { checkDisposed(); ondraw(item); }

        protected abstract int getWidth();
        protected abstract int getHeight();
        protected abstract void getData<T>(ref T[] data) where T : struct;
        protected abstract void getData<T>(ref T[] data, int startIndex, int elementCount) where T : struct;
        protected abstract void setData<T>(T[] data) where T : struct;
        protected abstract void setData<T>(T[] data, int startIndex, int elementCount) where T : struct;

        protected abstract void ondraw(GLRenderable item);
        protected abstract void ondispose();


        protected void checkDisposed()
        {
            if (isDisposed)
                throw new ObjectDisposedException("Texture2D");
        }

        public void Dispose()
        {
            if (!isDisposed)
            {
                isDisposed = true;
                ondispose();
            }
        }

    }
}

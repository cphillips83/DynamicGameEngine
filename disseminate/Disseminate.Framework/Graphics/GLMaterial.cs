using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Disseminate.Graphics
{
    //public interface IMaterial : IDisposable
    //{
    //    //GLTexture2D texture { get; }
    //    //int getSortKey(GLSortMode mode);
    //    int ID { get; }
    //    void enable();

    //}

    public abstract class GLMaterial : GPUState, IDisposable
    {
        //private static int nextid = 0;

        private bool isDisposed = false;

        public string name { get; private set; }
        public ushort id { get; private set; }

        public GLMaterial(ushort id, string name)
        {
            this.id = id;
            this.name = name;
        }

        public void Dispose()
        {
            if (!isDisposed)
            {
                isDisposed = true;
                ondispose();
            }
        }

        protected void checkDispose()
        {
            if (isDisposed)
                throw new ObjectDisposedException("Material");
        }

        protected virtual void ondispose() { }

        public abstract void enable();


    }
}

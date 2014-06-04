using Disseminate.Math;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Disseminate.Graphics
{
    public struct GLRenderable : IRadixKey
    {
        //public long id;
        public int key;
        public float depth;
        public float rotation;
        public Vector2 pivot;
        public Vector2 position;
        public Vector2 scale;
        public Color color;
        public AxisAlignedBox2 scissorRect;
        public AxisAlignedBox2 sourceRectangle;
        public GLRenderableType type;
        public GLMaterial material;
        public GLTexture2D texture;
        public IEffect effect;
        //public SpriteEffects effect;
        public bool applyScissor;

        public int Key { get { return key; } }
        //public TextureRef texture;
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Disseminate.Graphics;
using MGTexture = Microsoft.Xna.Framework.Graphics.Texture2D;
using XnaMath = Microsoft.Xna.Framework;
using Disseminate.Math;

namespace Disseminate.MonoGame.Graphics
{
    public class Texture2D : GLTexture2D, IDisposable
    {
        public static MGTexture baseWhite { get; private set; }

        public MGTexture texture { get; private set; }

        public Texture2D(ushort id, string name, MGTexture texture)
            : base(id, name)
        {
            this.texture = texture;
        }

        protected override int getWidth() { return texture.Width; }

        protected override int getHeight() { return texture.Height; }

        protected override void getData<T>(ref T[] data)
        {
            texture.GetData(data);
        }

        protected override void getData<T>(ref T[] data, int startIndex, int elementCount)
        {
            texture.GetData(data, startIndex, elementCount);
        }

        protected override void setData<T>(T[] data)
        {
            texture.SetData(data);
        }

        protected override void setData<T>(T[] data, int startIndex, int elementCount)
        {
            texture.SetData(data, startIndex, elementCount);
        }

        protected override void ondispose()
        {
            if (texture != null)
                texture.Dispose();

            texture = null;
        }

        protected override void ondraw(GLRenderable item)
        {
            var mgl = MonoGL.instance;

            XnaMath.Rectangle srcRectangle;
            if (item.sourceRectangle.IsNull)
                srcRectangle = new XnaMath.Rectangle(0, 0, texture.Width, texture.Height);
            else
                srcRectangle = item.sourceRectangle.ToRect();

            var _origin = (new Vector2(srcRectangle.Width, srcRectangle.Height) * item.pivot);// +new Vector2(srcRectangle.Value.X, srcRectangle.Value.Y);
            var origin = new XnaMath.Vector2(_origin.X, _origin.Y);

            //var p = item.position + item.scale * _origin;
            var p = item.position;

            //if (item.type == GLRenderableType.Quad)
            p += item.scale * item.pivot;

            var s = item.scale;
            var destRectangle = p.ToRectangle(s);

            var depth = 0f;
            if (mgl.depthRange > 0f)
                depth = (item.depth - mgl._minDepth) / mgl.depthRange;

            var color = new XnaMath.Color(item.color.r, item.color.g, item.color.b, item.color.a);
            mgl.batch.Draw(texture,
                destRectangle,
                srcRectangle, color, item.rotation, origin,
                Microsoft.Xna.Framework.Graphics.SpriteEffects.None, 1f - depth);

        }
    }
}

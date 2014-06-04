using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Disseminate.Core;
using Disseminate.MonoGame.Graphics;
using XnaContentManager = Microsoft.Xna.Framework.Content.ContentManager;
using XnaTexture2D = Microsoft.Xna.Framework.Graphics.Texture2D;
using XnaGraphics = Microsoft.Xna.Framework.Graphics.GraphicsDevice;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

namespace Disseminate.MonoGame
{
    public class TextureFactory : AbstractTextureFactory<Texture2D>
    {
        protected XnaContentManager _content;
        protected XnaGraphics _graphics;

        public TextureFactory(ResourceFactory rf, XnaContentManager xcm, XnaGraphics graphics)
            : base(rf)
        {
            _content = xcm;
            _graphics = graphics;
        }

        protected override Texture2D loadTexture(ushort id, string name, string file)
        {
            using (var fs = new FileStream(file, FileMode.Open))
            using (var image = (Bitmap)Bitmap.FromStream(fs))
            {
                // Fix up the Image to match the expected format
                image.RGBToBGR();

                var data = new byte[image.Width * image.Height * 4];

                BitmapData bitmapData = image.LockBits(new System.Drawing.Rectangle(0, 0, image.Width, image.Height),
                    ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
                if (bitmapData.Stride != image.Width * 4)
                    throw new NotImplementedException();
                Marshal.Copy(bitmapData.Scan0, data, 0, data.Length);
                image.UnlockBits(bitmapData);

                var texture = new XnaTexture2D(_graphics, image.Width, image.Height);
                texture.SetData(data);

                return new Texture2D(id, name, texture);
            }
            //return new Texture2D(id, name, new XnaTexture2D(_graphics, 1, 1));
        }

        protected override Texture2D createTexture(ushort id, string name, int width, int height)
        {
            return new Texture2D(id, name, new XnaTexture2D(_graphics, width, height));
        }
    }
}

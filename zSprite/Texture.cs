//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using Microsoft.Xna.Framework.Graphics;

//namespace zSprite
//{
//    public class TextureRef : IDisposable
//    {
//        private static int index = 0;
//        public readonly int id = index++;
//        public readonly string name;

//        public TextureRef(string name)
//        {
//            this.name = name;
//        }

//        private Texture2D _texture;
//        public Texture2D texture
//        {
//            get
//            {
//                touch();
//                return _texture;
//            }
//            internal set
//            {
//                _texture = value;
//            }
//        }

//        public int width { get { return texture.Width; } }
//        public int height { get { return texture.Height; } } 

//        public void touch()
//        {
//            if (_texture == null)
//                _texture = Root.instance.content.Load<Texture2D>(name);

//        }

//        public void Dispose()
//        {
//            if (_texture != null)
//                _texture.Dispose();

//            _texture = null;
//        }
//    }
//}

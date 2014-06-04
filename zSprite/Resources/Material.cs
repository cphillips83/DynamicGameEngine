using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace zSprite.Resources
{
    public class Material : GraphicsState
    {
        private static int nextid = 0;
        public int id { get; internal set; }
        public string name { get; internal set; }
        
        public Material(string name) : base()
        {
            id = nextid++;
            this.name = name;
        }

        public Color transparency { get { return _textureRef.transparency; } set { _textureRef.transparency = value; } }

        private TextureRef _textureRef;
        private bool _sharedTexture = false;

        private string _textureName = null;
        public string textureName
        {
            get { return _textureName; }
            set
            {
                if (value != null)
                {
                    setupTexture(value);
                }
            }
        }

        private void setupTexture(string name)
        {
            if (name == null)
            {
                _sharedTexture = false;
                _textureRef = null;
            }
            else
            {
                if (_textureRef == null)
                    _textureRef = new TextureRef(name);

                _sharedTexture = true;
                _textureName = name;
            }
        }

        public Texture2D texture
        {
            get
            {
                if (_textureRef == null)
                    return null;

                _textureRef.touch();
                return _textureRef.texture;
            }
        }

        public int textureWidth
        {
            get
            {
                if (_textureRef == null)
                    return 0;

                _textureRef.touch();
                return _textureRef.width;
            }
        }

        public int textureHeight
        {
            get
            {
                if (_textureRef == null)
                    return 0;
                _textureRef.touch();
                return _textureRef.height;
            }
        }

        public Vector2 textureSize { get { return new Vector2(textureWidth, textureHeight); } }


    }
}
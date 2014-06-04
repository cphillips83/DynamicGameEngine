using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Disseminate.Math;
using Microsoft.Xna.Framework.Graphics;

namespace Disseminate.MonoGame.Graphics
{
    //public class TextureRef : IDisposable
    //{
    //    private bool isDisposed = false;
    //    private static int index = 0;
    //    public readonly int id = index++;
    //    public readonly string name;

    //    private Color? _transparency;
    //    public Color transparency
    //    {
    //        get
    //        {
    //            if (!_transparency.HasValue)
    //                throw new Exception("color key is not set");
    //            return _transparency.Value;
    //        }
    //        set
    //        {
    //            if (_texture != null)
    //                throw new Exception("texture was already loaded in to memory");

    //            _transparency = value;
    //        }
    //    }

    //    public TextureRef(string name)
    //    {
    //        this.name = name;
    //    }

    //    private Texture2D _texture;
    //    public Texture2D texture
    //    {
    //        get
    //        {
    //            touch();
    //            return _texture;
    //        }
    //        internal set
    //        {
    //            _texture = value;
    //        }
    //    }

    //    public int width { get { return texture.Width; } }
    //    public int height { get { return texture.Height; } }

    //    public void touch()
    //    {
    //        if (_texture == null)
    //        {
    //            if (_transparency.HasValue)
    //                _texture = Root.instance.resources.loadTexture(name, _transparency.Value);
    //            else
    //                _texture = Root.instance.resources.loadTexture(name);
    //        }
    //    }

    //    public void Dispose()
    //    {
    //        if (_texture != null)
    //            _texture.Dispose();

    //        _texture = null;
    //        isDisposed = true;
    //    }
    //}

}

using Disseminate.Math;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace Disseminate.Graphics
{
    public class GLFont : IDisposable
    {
        private Dictionary<char, FontChar> _characterMap;
        private FontFile _fontFile;
        private GLMaterial _material;

        public ushort id { get; protected set; }
        //private TextureRef _texture;

        public GLFont(ushort id, FontFile fontFile, GLMaterial material)
        {
            this.id = id;

            _fontFile = fontFile;
            _material = material;
            //_material = Root.instance.resources.createMaterialFromTexture(fontTexture, fontTexture);
            //_material.SetBlendState( BlendState.NonPremultiplied);
            //_material.textureName = fontTexture;

            //_texture = Root.instance.resources.findTexture(fontTexture);

            _characterMap = new Dictionary<char, FontChar>();

            foreach (var fontCharacter in _fontFile.Chars)
            {
                char c = (char)fontCharacter.ID;
                _characterMap.Add(c, fontCharacter);
                if (fontCharacter.Height + fontCharacter.YOffset > MaxLineHeight)
                    MaxLineHeight = fontCharacter.Height + fontCharacter.YOffset;
            }
        }

        public int MaxLineHeight { get; private set; }

        //internal void DrawText(int renderQueue, Vector2 pos, float scale, string text, Color color)
        //{
        //    DrawText(renderQueue, pos, scale, text, color, 0);
        //}

        public bool CanFit(string text, float width)
        {
            return false;
        }

        public Vector2 MeasureString(string text, Vector2 maxSize)
        {
            var size = Vector2.Zero;
            var currentSize = Vector2.Zero;
            var current = 0;

            while (current != -1 && current < text.Length)
            {
                current = FindWordIndexFromBounds(current, maxSize.X, text, out currentSize);

                if (size.X < currentSize.X)
                    size.X = currentSize.X;

                size.Y += currentSize.Y;
            }

            return size;
        }

        public Vector2 MeasureMinWrappedString(string text)
        {
            var size = Vector2.Zero;
            var width = 0f;

            for (int i = 0; i < text.Length; i++)
            {
                var c = text[i];

                FontChar fc;
                if (_characterMap.TryGetValue(c, out fc))
                {
                    width += fc.XAdvance;

                    if (c == ' ')
                    {
                        size.Y = MaxLineHeight;
                        if (width > size.X)
                            size.X = width;
                    }
                }
            }

            return size;
        }

        public Vector2 MeasureString(string text)
        {
            float dx = 0;
            float dy = 0;
            foreach (char c in text)
            {
                FontChar fc;
                if (_characterMap.TryGetValue(c, out fc))
                {
                    dx += fc.XAdvance;
                    if (fc.Height + fc.YOffset > dy)
                        dy = fc.Height + fc.YOffset;
                }
            }
            return new Vector2(dx, dy);
        }

        public int FindWordIndexFromBounds(int start, float width, string text, out Vector2 size)
        {
            var index = -1;
            size = Vector2.Zero;

            for (int i = start; i < text.Length; i++)
            {
                var c = text[i];

                FontChar fc;
                if (_characterMap.TryGetValue(c, out fc))
                {
                    if (size.X + fc.XAdvance > width)
                        return index;

                    size.X += fc.XAdvance;

                    if (c == ' ')
                        index = i + 1;

                    if (fc.Height + fc.YOffset > size.Y)
                        size.Y = fc.Height + fc.YOffset;
                }
            }

            index = text.Length;

            return index;
        }

        internal void DrawText(GL gl, string text)
        {
            DrawText(gl, text);
        }

        internal void DrawText(GL gl, string text, float? width)
        {
            var dx = 0f;// Utility.Floor(pos.X);
            //var dy = Utility.Floor(pos.Y);
            foreach (char c in text)
            {
                FontChar fc;
                if (_characterMap.TryGetValue(c, out fc))
                {
                    if (width.HasValue && dx + fc.XAdvance > width.Value)
                        break;

                    var sourceRectangle = AxisAlignedBox2.FromRect(fc.X, fc.Y, fc.Width, fc.Height);
                    var destRectangle = AxisAlignedBox2.FromRect(fc.XOffset, fc.YOffset, fc.Width, fc.Height);

                    gl.source(sourceRectangle);
                    gl.quad(destRectangle);
                    gl.translate(new Vector2(fc.XAdvance, 0));

                    dx += fc.XAdvance;
                }
            }
        }

        internal void DrawWrappedOnWordText(GL gl, string text, Vector2 size)
        {
            var currentSize = Vector2.Zero;
            var current = 0;

            while (current != -1 && current < text.Length)
            {
                var start = current;
                current = FindWordIndexFromBounds(current, size.X, text, out currentSize);

                if (current > 0)
                {
                    var p = gl.currentPosition;
                    for (int i = start; i < current; i++)
                    {
                        var c = text[i];
                        FontChar fc;
                        if (_characterMap.TryGetValue(c, out fc))
                        {
                            var sourceRectangle = AxisAlignedBox2.FromRect(fc.X, fc.Y, fc.Width, fc.Height);
                            var destRectangle = AxisAlignedBox2.FromRect(fc.XOffset,fc.YOffset, fc.Width, fc.Height);
                            gl.source(sourceRectangle);
                            gl.quad(destRectangle);
                            gl.translate(new Vector2(fc.XAdvance, 0));

                            //dx += fc.XAdvance;
                        }
                    }
                    gl.position(p);
                    gl.translate(new Vector2(0, MaxLineHeight));
                }

                size.Y += currentSize.Y;
            }
        }

        public void Dispose()
        {
            _characterMap.Clear();
        }
    }

    [Serializable]
    public class FontChar
    {
        [XmlAttribute("chnl")]
        public Int32 Channel
        {
            get;
            set;
        }

        [XmlAttribute("height")]
        public Int32 Height
        {
            get;
            set;
        }

        [XmlAttribute("id")]
        public Int32 ID
        {
            get;
            set;
        }

        [XmlAttribute("page")]
        public Int32 Page
        {
            get;
            set;
        }

        [XmlAttribute("width")]
        public Int32 Width
        {
            get;
            set;
        }

        [XmlAttribute("x")]
        public Int32 X
        {
            get;
            set;
        }

        [XmlAttribute("xadvance")]
        public Int32 XAdvance
        {
            get;
            set;
        }

        [XmlAttribute("xoffset")]
        public Int32 XOffset
        {
            get;
            set;
        }

        [XmlAttribute("y")]
        public Int32 Y
        {
            get;
            set;
        }

        [XmlAttribute("yoffset")]
        public Int32 YOffset
        {
            get;
            set;
        }
    }

    [Serializable]
    public class FontCommon
    {
        [XmlAttribute("alphaChnl")]
        public Int32 AlphaChannel
        {
            get;
            set;
        }

        [XmlAttribute("base")]
        public Int32 Base
        {
            get;
            set;
        }

        [XmlAttribute("blueChnl")]
        public Int32 BlueChannel
        {
            get;
            set;
        }

        [XmlAttribute("greenChnl")]
        public Int32 GreenChannel
        {
            get;
            set;
        }

        [XmlAttribute("lineHeight")]
        public Int32 LineHeight
        {
            get;
            set;
        }

        [XmlAttribute("packed")]
        public Int32 Packed
        {
            get;
            set;
        }

        [XmlAttribute("pages")]
        public Int32 Pages
        {
            get;
            set;
        }

        [XmlAttribute("redChnl")]
        public Int32 RedChannel
        {
            get;
            set;
        }

        [XmlAttribute("scaleH")]
        public Int32 ScaleH
        {
            get;
            set;
        }

        [XmlAttribute("scaleW")]
        public Int32 ScaleW
        {
            get;
            set;
        }
    }

    [Serializable]
    [XmlRoot("font")]
    public class FontFile
    {
        [XmlArray("chars")]
        [XmlArrayItem("char")]
        public List<FontChar> Chars
        {
            get;
            set;
        }

        [XmlElement("common")]
        public FontCommon Common
        {
            get;
            set;
        }

        [XmlElement("info")]
        public FontInfo Info
        {
            get;
            set;
        }

        [XmlArray("kernings")]
        [XmlArrayItem("kerning")]
        public List<FontKerning> Kernings
        {
            get;
            set;
        }

        [XmlArray("pages")]
        [XmlArrayItem("page")]
        public List<FontPage> Pages
        {
            get;
            set;
        }
    }

    [Serializable]
    public class FontInfo
    {
        private Rectangle _Padding;

        private Point _Spacing;

        [XmlAttribute("bold")]
        public Int32 Bold
        {
            get;
            set;
        }

        [XmlAttribute("charset")]
        public String CharSet
        {
            get;
            set;
        }

        [XmlAttribute("face")]
        public String Face
        {
            get;
            set;
        }

        [XmlAttribute("italic")]
        public Int32 Italic
        {
            get;
            set;
        }

        [XmlAttribute("outline")]
        public Int32 OutLine
        {
            get;
            set;
        }

        [XmlAttribute("padding")]
        public String Padding
        {
            get
            {
                return _Padding.X + "," + _Padding.Y + "," + _Padding.Width + "," + _Padding.Height;
            }
            set
            {
                String[] padding = value.Split(',');
                _Padding = new Rectangle(Convert.ToInt32(padding[0]), Convert.ToInt32(padding[1]), Convert.ToInt32(padding[2]), Convert.ToInt32(padding[3]));
            }
        }

        [XmlAttribute("size")]
        public Int32 Size
        {
            get;
            set;
        }

        [XmlAttribute("smooth")]
        public Int32 Smooth
        {
            get;
            set;
        }

        [XmlAttribute("spacing")]
        public String Spacing
        {
            get
            {
                return _Spacing.X + "," + _Spacing.Y;
            }
            set
            {
                String[] spacing = value.Split(',');
                _Spacing = new Point(Convert.ToInt32(spacing[0]), Convert.ToInt32(spacing[1]));
            }
        }

        [XmlAttribute("stretchH")]
        public Int32 StretchHeight
        {
            get;
            set;
        }

        [XmlAttribute("aa")]
        public Int32 SuperSampling
        {
            get;
            set;
        }

        [XmlAttribute("unicode")]
        public Int32 Unicode
        {
            get;
            set;
        }
    }

    [Serializable]
    public class FontKerning
    {
        [XmlAttribute("amount")]
        public Int32 Amount
        {
            get;
            set;
        }

        [XmlAttribute("first")]
        public Int32 First
        {
            get;
            set;
        }

        [XmlAttribute("second")]
        public Int32 Second
        {
            get;
            set;
        }
    }

    public class FontLoader
    {
        public static FontFile Load(String filename)
        {
            XmlSerializer deserializer = new XmlSerializer(typeof(FontFile));
            TextReader textReader = new StreamReader(filename);
            FontFile file = (FontFile)deserializer.Deserialize(textReader);
            textReader.Close();
            return file;
        }

        public static FontFile Load(Stream stream)
        {
            XmlSerializer deserializer = new XmlSerializer(typeof(FontFile));
            FontFile file = (FontFile)deserializer.Deserialize(stream);
            return file;
        }
    }

    [Serializable]
    public class FontPage
    {
        [XmlAttribute("file")]
        public String File
        {
            get;
            set;
        }

        [XmlAttribute("id")]
        public Int32 ID
        {
            get;
            set;
        }
    }
}

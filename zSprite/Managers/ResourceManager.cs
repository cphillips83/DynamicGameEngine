using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using zSprite.Json;
using zSprite.Resources;


namespace zSprite.Managers
{
    public sealed class ResourceManager : AbstractManager
    {
        public Material defaultMaterial { get; private set; }
        public TextureRef basewhite { get; private set; }

        private int _nameIndex = 0;
        private Dictionary<string, Song> _music = new Dictionary<string, Song>();
        private Dictionary<string, SoundEffect> _sounds = new Dictionary<string, SoundEffect>();
        private Dictionary<string, Material> _materials = new Dictionary<string, Material>();
        private Dictionary<string, TextureRef> textures = new Dictionary<string, TextureRef>();
        private Dictionary<string, BmFont> fonts = new Dictionary<string, BmFont>();
        private Dictionary<string, JsonObject> jsonobjects = new Dictionary<string, JsonObject>();

        private List<string> searchPathes = new List<string>();

        internal ResourceManager()
        {

        }

        internal void init()
        {
            //var m = Create("basewhite");
            //m.textureName = "basewhite";
            //releaseTextures();

        }

        public SoundEffect createSoundFromFile(string file)
        {
            file = file.ToLower();

            SoundEffect soundFile;
            if (!_sounds.TryGetValue(file, out soundFile))
            {
                soundFile = Root.instance.content.Load<SoundEffect>(findFile(file));
                _sounds.Add(file, soundFile);
            }
            
            return soundFile;
        }

        public Song createMusicFromFile(string file)
        {
            file = file.ToLower();

            Song songFile;
            if (!_music.TryGetValue(file, out songFile))
            {
                songFile = Root.instance.content.Load<Song>(findFile(file));
                _music.Add(file, songFile);
            }

            return songFile;
        }

        public Material createEmptyMaterial()
        {
            return createMaterial(string.Format("_material_{0}_", _nameIndex++));
        }

        public Material createMaterial(string name)
        {
            var m = new Material(name);
            _materials.Add(name, m);
            return m;
        }

        public Material createMaterialFromTexture(string textureName)
        {
            Material material;
            if (!_materials.TryGetValue(textureName, out material))
            {
                material = createMaterial(textureName);
                material.textureName = textureName;
            }
            return material;
        }

        public Material createMaterialFromTexture(string materialName, string textureName)
        {
            Material material;
            if (!_materials.TryGetValue(materialName, out material))
            {
                material = createMaterial(materialName);
                material.textureName = textureName;
            }
            return material;
        }

        //public Material CreateFromTexture(string texture)
        //{
        //    var m = Create();
        //    m.textureName = texture;
        //    return m;
        //}

        public Material findMaterial(string name)
        {
            return _materials[name];
        }

        public TextureRef createTexture(string name, int width, int height)
        {
            var texture = new TextureRef(name);
            texture.texture = new Texture2D(Root.instance.graphics.graphicsDevice, width, height);
            textures.Add(name, texture);
            return texture;
        }

        public Texture2D loadTexture(string name, Color transparentKey)
        {
            var texture = Root.instance.content.Load<Texture2D>(findFile(name));
            var data = new Color[texture.Width * texture.Height];
            texture.GetData(data);

            for (int x = 0; x < texture.Width; x++)
            {
                for (int y = 0; y < texture.Height; y++)
                {
                    if (data[x + y * texture.Width] == transparentKey)
                        data[x + y * texture.Width] = Color.Transparent;
                }
            }

            texture.SetData(data);
            return texture;
        }

        public Texture2D loadTexture(string name)
        {
            var texture = Root.instance.content.Load<Texture2D>(findFile(name));
            var data = new Color[texture.Width * texture.Height];
            texture.GetData(data);

            if (data[0] == Color.Magenta)
            {
                //var data = new Color[texture.Width * texture.Height];
                texture.GetData(data);

                for (int x = 0; x < texture.Width; x++)
                {
                    for (int y = 0; y < texture.Height; y++)
                    {
                        if (data[x + y * texture.Width] == Color.Magenta)
                            data[x + y * texture.Width] = Color.Transparent;
                    }
                }

                texture.SetData(data);
            }

            return texture;
        }

        public void touchTexture(string name)
        {
            findTexture(name);
        }

        public TextureRef findTexture(string name)
        {
            TextureRef texture = null;
            if (!textures.TryGetValue(name, out texture))
            {
                texture = new TextureRef(name);
                textures.Add(name, texture);
            }
            return texture;
        }

        internal void releaseTextures()
        {
            releaseTextures(true);
        }

        internal void releaseTextures(bool reload)
        {
            foreach (var texture in textures.Values)
                texture.Dispose();

            textures.Clear();
            if (reload)
            {
                basewhite = new TextureRef("basewhite");
                basewhite.texture = new Texture2D(Root.instance.graphics.graphicsDevice, 1, 1);
                basewhite.texture.SetData(new Color[] { Color.White });
                textures.Add("basewhite", basewhite);
            }
        }

        internal void reload()
        {
            releaseTextures(true);
            clearFonts();
            _materials.Clear();
            defaultMaterial = new Material("default");
        }

        internal void cleanup()
        {
            releaseTextures(false);
            clearFonts();
            _materials.Clear();
            defaultMaterial = new Material("default");
        }

        public BmFont findFont(string name)
        {
            BmFont font;
            if (!fonts.TryGetValue(name, out font))
            {
                font = loadFont(name);
                fonts.Add(name, font);
            }
            return fonts[name];
        }

        private BmFont loadFont(string path)
        {
            var fontFilePath = findFile(path);
            using (var stream = TitleContainer.OpenStream(fontFilePath))
            {
                var fontFile = FontLoader.Load(stream);
                var fontPng = System.IO.Path.Combine(System.IO.Path.GetDirectoryName(path), fontFile.Pages[0].File);

                // textRenderer initialization will go here
                stream.Close();
                return new BmFont(fontFile, fontPng);
            }
        }

        public Effect loadEffect(string file)
        {
            var effectFilePath = findFile(file);
            return Root.instance.content.Load<Effect>(effectFilePath);
            
        }

        private void clearFonts()
        {
            fonts.Clear();
        }

        public JsonObject findJson(string path)
        {
            JsonObject jo;
            if (!jsonobjects.TryGetValue(path, out jo))
            {
                var filepath = findFile(path);
                using (var stream = TitleContainer.OpenStream(filepath))
                {
                    var sr = new StreamReader(stream);
                    var text = sr.ReadToEnd();

                    jo = JsonReader.ParseObject(text);
                    jsonobjects.Add(path, jo);
                }
            }

            return jo;
        }

        private void clearJson()
        {
            jsonobjects.Clear();
        }

        public void addSearchPath(string path)
        {
            searchPathes.Add(path);
        }

        internal string findFile(string file)
        {
            if (System.IO.File.Exists(file))
                return file;

            var contentFile = System.IO.Path.Combine(Root.instance.content.RootDirectory, file);
            if (System.IO.File.Exists(contentFile))
                return contentFile;

            foreach (var path in searchPathes)
            {
                var newfile = System.IO.Path.Combine(path, file);
                if (System.IO.File.Exists(newfile))
                    return newfile;
            }

            throw new FileNotFoundException(file);
        }


    }
}

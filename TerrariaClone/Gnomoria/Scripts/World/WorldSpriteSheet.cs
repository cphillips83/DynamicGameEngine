using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameName1.Gnomoria.Scripts.World
{
    public class WorldSpriteSheet
    {
        private Dictionary<string, WorldSprite> sprites = new Dictionary<string, WorldSprite>();

        public void addSprite(string name, WorldSprite sprite)
        {
            sprites.Add(name, sprite);
        }

        public WorldSprite this[string name]
        {
            get
            {
                WorldSprite sprite;
                if (sprites.TryGetValue(name, out sprite))
                    return sprite;
                
                return null;
            }
        }
    }
}

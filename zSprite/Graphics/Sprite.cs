using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using zSprite.Managers;
using zSprite.Resources;

namespace zSprite.Graphics
{
    public class Sprite
    {
        public AxisAlignedBox source;
        public AxisAlignedBox destination;
        //public Color color;
        //public SpriteEffects effect;

        public void render(MonoRenderer renderer, Material material, Vector2 offset, float rotation, float depth, Color color, SpriteEffects effect )
        {
            var scale = new Vector2(destination.Width, destination.Height);
            var origin = new Vector2(0.5f, 0.5f);
            var position = destination.Center + offset;

            renderer.DrawInternal(material, position, scale, source, color, rotation, origin, effect, depth);
        }
    }
}

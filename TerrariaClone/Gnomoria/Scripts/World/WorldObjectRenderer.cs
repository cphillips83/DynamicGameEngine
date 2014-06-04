using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameName1.Gnomoria.Scripts.World
{
    public class SpriteRenderer : WorldObjectRenderer
    {
        public string spriteName;
        public Color color = Color.White;
        protected WorldSprite sprite;

        public SpriteRenderer(WorldRenderer renderer, string spriteName, Color color)
        {

            this.sprite = renderer.spriteSheet[spriteName];
            this.color = color;
            this.spriteName = spriteName;
        }

        public override void Render(WorldRenderer worldRenderer, Vector2 p, int z,  float alpha)
        {
            if (sprite != null)
            {
                p += new Vector2(sprite.offsetX, sprite.offsetY);

                var tempColor = new Color(color, alpha);
                var dest = p.ToAABB(sprite.width, sprite.height);
                worldRenderer.renderItems[z].Add(new WorldRenderItem() { color = tempColor, dstRect = dest, srcRect = sprite.textureSource, material = worldRenderer.spriteSheetMaterial });
                //worldRenderer.graphics.Draw(worldRenderer.spriteSheetMaterial, dest, sprite.textureSource, tempColor);
            }
        }
    }


    public abstract class WorldObjectRenderer
    {
        public abstract void Render(WorldRenderer worldRenderer, Vector2 p, int z, float alpha);
    }
}

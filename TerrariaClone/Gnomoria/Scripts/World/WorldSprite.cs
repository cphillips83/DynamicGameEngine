using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameName1.Gnomoria.Scripts.World
{
    public class WorldSprite
    {
        public AxisAlignedBox textureSource;
        public int width, height, offsetX, offsetY;

        public WorldSprite(AxisAlignedBox texSource, int width, int height, int offsetX, int offsetY)
        {
            this.textureSource = texSource;
            this.width = width;
            this.height = height;
            this.offsetX = offsetX;
            this.offsetY = offsetY;
        }

        public AxisAlignedBox getDrawRect(Vector2 p)
        {
            return AxisAlignedBox.FromRect(p + new Vector2(offsetX, offsetY), new Vector2(width, height));
        }
    }
}

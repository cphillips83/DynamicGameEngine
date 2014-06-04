using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameName1.Gnomoria.Scripts.World.GameObjects.Tiles.Floors
{
    public class BaseFloor : SpriteRenderer
    {
        public BaseFloor(WorldRenderer renderer, string spriteName, Color color)
            : base(renderer, spriteName, color)
        {

        }
    }
}

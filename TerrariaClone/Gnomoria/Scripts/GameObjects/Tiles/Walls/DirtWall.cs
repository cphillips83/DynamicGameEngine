using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace GameName1.Gnomoria.Scripts.World.GameObjects.Tiles.Walls
{
    public class DirtWall : SpriteRenderer
    {
        public DirtWall(WorldRenderer renderer) : base(renderer, "dirtwall", new Color(0x96, 0x63, 0x30)) { }
    }
}

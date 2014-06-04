using GameName1.Gnomoria.Scripts.World;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameName1.Gnomoria.Scripts.Tiles
{
    public abstract class WorldObject
    {
        public Color color = Color.White;
        public WorldObjectType worldType;

        protected WorldObject(WorldObjectType type)
        {
            worldType = type;
        }

        public virtual void Render(WorldRenderer renderer, int x, int y, int z, float alpha)
        {
            renderer.RenderObject(worldType, x, y, z, alpha);
        }
    }
}

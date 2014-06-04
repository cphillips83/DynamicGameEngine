using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameName1.Gnomoria.Scripts.World.GameObjects.Placeable
{
    public abstract class PlaceableObject
    {
        public const byte NOTHING = 0;
        public const byte DIRT_WALL = 1;
        public const byte DIRT_RAMP = 2;

        public byte id;
        public bool allowsMovementUp;
        public bool allowsMovementDown;
        public bool isSolid;

        protected WorldManager world;

        protected PlaceableObject(WorldManager world, byte id)
        {
            this.world = world;
            this.id = id;
        }

        public virtual void place(int x, int y, int z)
        {
            var block = world.getTile(x, y, z);
            if (block.hasPlaceable)
                throw new Exception("tile already has a placeable object");

            block.placedId = id;
            world.setTile(x, y, z, block);
        }

        public virtual void render(WorldRenderer renderer, MapTile block, int x, int y, int z, float alpha)
        {
            renderer.RenderObject(WorldObjectType.DirtWall, x, y, z, alpha);
        }

        public virtual void queuedplacing()
        {

        }

        public virtual void queueddestroying()
        {

        }

        public virtual void renderplacing()
        {

        }

        public virtual void renderdestroying()
        {

        }

        public virtual void destroy()
        {

        }
    }
}

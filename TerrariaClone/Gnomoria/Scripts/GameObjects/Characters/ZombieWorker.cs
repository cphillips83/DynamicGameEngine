using GameName1.Gnomoria.Scripts.Tiles;
using GameName1.Gnomoria.Scripts.World;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameName1.Gnomoria.Scripts.GameObjects.Characters
{
    public class ZombieWorker : WorldObject
    {
        public int x, y, z;
        public bool isIdle = true;

        public ZombieWorker(int x, int y, int z)
            : base(WorldObjectType.ZombieWorker)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        public override void Render(WorldRenderer renderer, int x, int y, int z, float alpha)
        {
            renderer.RenderObject(WorldObjectType.ZombieWorker, x, y, z, alpha);
        }

    }
}

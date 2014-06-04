using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using zSprite;
using zSprite.Scripts;

namespace GameName1.Digger.World
{
    public class TerrainCollider : Collider2
    {
        private CompoundShape _shape;

        private Terrain _terrain;
        public int x;
        public int y;
        public int width;
        public int height;

        protected override ICollidable shape
        {
            get { return _shape; }
        }

        protected override void onInit()
        {
            _terrain = Root.instance.RootObject.getScript<Terrain>();
            _shape = new CompoundShape();

            var p = _transform.DerivedPosition;
            var aabb = new AxisAlignedBox(p, p + Settings.TileSizeV);
            for (var xx = 0; xx < width; xx++)
            {
                for (var yy = 0; yy < width; yy++)
                {
                    var block = _terrain.GetForeground(x + xx, y + yy);
                    if (block > 0)
                    {
                        aabb.Center = new Vector2(xx, yy) * Settings.TileSizeV;
                        var shape = new Shape(aabb);
                        _shape.addShape(shape);
                    }
                }
            }
        }
    }
}

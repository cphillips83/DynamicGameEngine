using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using zSprite;

namespace GameName1.Roguelike.Scripts
{
    public class Collider : Script
    {
        public Vector2 size;
        public Vector2 origin = Vector2.Zero;
        public int flags = 0;
        public int collidesWith = 0;
        public GameObject notifyObject = null;
        public string eventName = "collision";


        private AxisAlignedBox aabb = AxisAlignedBox.Null;
        private List<Collision> collisions = new List<Collision>();

        private void init()
        {
            transformupdate();
        }

        private void transformupdate()
        {
            var p = gameObject.transform.DerivedPosition;
            var min = size * -origin + p;
            var max = size * (-origin + Vector2.One) + p;
            var pivot = p;

            aabb.SetExtents(min, max);
            aabb.RotateAndContain(pivot, gameObject.transform.DerivedOrientation);
        }

        private void fixedupdate()
        {
            if (!aabb.IsNull)
            {
                Root.instance.RootObject.broadcast("queryBroadphaseAABB",new CollisionQuery(collidesWith, this.gameObject, aabb), collisions);
                if (collisions.Count > 0)
                {
                    var target = notifyObject ?? this.gameObject;
                    target.sendMessage(eventName, collisions);
                    collisions.Clear();
                }
            }
        }

        private void queryBroadphaseAABB(CollisionQuery query, List<Collision> hits)
        {
            if (query.flag.CheckFlags(flags) && !aabb.IsNull && !query.aabb.IsNull && query.aabb.Intersects(aabb) && query.gameObject != gameObject)
            {
                hits.Add(new Collision(flags, notifyObject ?? gameObject, aabb));
            }
        }

        private void render()
        {
            if (!aabb.IsNull)
            {
                Root.instance.graphics.DrawRect(19, aabb, Color.Blue);
            }
        }
    }
}

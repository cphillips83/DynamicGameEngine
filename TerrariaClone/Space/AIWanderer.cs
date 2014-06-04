using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using zSprite;

namespace GameName1.Space
{
    public class AIWanderer : Script
    {
        public float distance = 6f;
        public float radius = 8f;
        public float change = 1f;
        public Vector2 around;
        public float aroundRadius = 0;

        private Ship _ship;

        private void init()
        {
            if (aroundRadius != 0 && around == Vector2.Zero)
                around = transform.DerivedPosition;

            _ship = gameObject.getScript<Ship>();
        }

        private void processlogic()
        {
            if (_ship != null)
            {
                var from = transform.DerivedPosition;
                if (aroundRadius > 0f)
                {
                    var arSQ = aroundRadius * aroundRadius;
                    var dstSQ = (around - from).LengthSquared();

                    _ship.steering.seek(from, Vector2.Zero, dstSQ / arSQ);
                    _ship.steering.wander(from, distance, radius, change);
                }
                else
                {
                    _ship.steering.wander(from, distance, radius, change);
                }
            }
        }
    }
}

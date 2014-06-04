using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameName1.TopDown.Components.World;
using Microsoft.Xna.Framework;
using zSprite;

namespace GameName1.TopDown.Components
{
    public class MovementComponent : Script
    {
        private int height = 1;
    
        private Transform _transform;
        private VelocityComponent _velocity;
        private MapRenderer _map;
        private void init()
        {
            _transform = this.gameObject.transform2();
            _transform.Position = new Vector2(100, 100);
            _velocity = this.gameObject.getScript<VelocityComponent>();
            _map = Root.instance.RootObject.getScript<MapRenderer>();
        }

        private void update()
        {
            var vd = Vector2.Zero;

            if (_velocity.value.X > 0) vd.X += 1;
            if (_velocity.value.X < 0) vd.X -= 1;
            if (_velocity.value.Y < 0) vd.Y -= 1;
            if (_velocity.value.Y > 0) vd.Y += 1;

            if (vd != Vector2.Zero)
            {
                vd.Normalize();
                var p = _transform.DerivedPosition;

                var dist = new Vector2(Math.Abs(_velocity.value.X), Math.Abs(_velocity.value.Y));
                dist *= Root.instance.time.deltaTime;

                while (dist.LengthSquared() != 0)
                {
                    if (dist.X > 0)
                    {
                        if (!move(ref p, new Vector2(vd.X, 0)))
                            dist.X = 0f;

                        dist.X -= 1f;
                        if (dist.X <= 0)
                            dist.X = 0f;
                    }

                    if (dist.Y > 0)
                    {
                        if (!move(ref p, new Vector2(0, vd.Y)))
                            dist.Y = 0f;

                        dist.Y -= 1f;
                        if (dist.Y <= 0)
                            dist.Y = 0f;
                    }
                }

                _transform.Position = p;
                _transform.Depth = height;
            }
        }

        public bool move(ref Vector2 p, Vector2 d)
        {
            var tp = p + d;
            if (!_map.checkCollisionByWorldPos(tp, height))
            {
                p = tp;
                return true;
            }
            return false;
        }        
    }
}

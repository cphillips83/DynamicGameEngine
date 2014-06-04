using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using zSprite;

namespace GameName1.Space
{
    public class LinearVelocity : Script
    {
        private Transform _transform;
        private Vector2 velocityY;
        public float maxYSpeed = 75f;
        public float accY = 0.5f;

        private void init()
        {
            _transform = this.gameObject.transform2();
        }

        private void fixedupdate()
        {
            var dirY = _transform.DerivedForward;

            if (dirY != Vector2.Zero)
            {
                dirY.Normalize();
                velocityY += dirY * accY * Root.instance.time.deltaTime;

                if (velocityY.LengthSquared() > 1)
                {
                    //acquires new heading faster
                    velocityY += dirY * accY * Root.instance.time.deltaTime * 10f;
                    velocityY.Normalize();
                }
            }

            _transform.Position += velocityY * maxYSpeed * Root.instance.time.deltaTime;
        }
    }
}

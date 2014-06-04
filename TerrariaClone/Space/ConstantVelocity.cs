using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using zSprite;

namespace GameName1.Space
{
    public class ConstantVelocity : Script
    {
        //public Vector2 direction;
        public float speed;

        private void fixedupdate()
        {
            transform.Position += transform.DerivedForward * speed;
        }
    }
}

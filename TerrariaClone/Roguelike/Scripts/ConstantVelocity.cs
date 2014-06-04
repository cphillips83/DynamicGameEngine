using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using zSprite;

namespace GameName1.Roguelike.Scripts
{
    public class ConstantVelocity : Script
    {
        public Vector2 speed = Vector2.Zero;

        private void fixedupdate()
        {
            gameObject.transform.Position += speed;
        }
    }
}

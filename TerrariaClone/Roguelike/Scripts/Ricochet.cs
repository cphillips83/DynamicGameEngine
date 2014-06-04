using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using zSprite;

namespace GameName1.Roguelike.Scripts
{
    public class Ricochet : Script
    {
        public int flags = 0;

        private bool ricocheted = false;
        private ConstantVelocity constantVelocity;
        private ApplyDamage damage;
        private Collider collider;

        private void init()
        {
            constantVelocity = gameObject.getScript<ConstantVelocity>();
            damage = gameObject.getScript<ApplyDamage>();
            collider = gameObject.getScript<Collider>();
        }

        private void collision(List<Collision> collisions)
        {
            if (!ricocheted && collisions.Any(x => x.flag.CheckFlags(flags)))
            {
                collider.flags = Physics.QUERY_PLAYER;
                collider.collidesWith = Physics.QUERY_ENEMIES;
                damage.flags = Physics.QUERY_ENEMIES;

                constantVelocity.speed = -constantVelocity.speed;
                ricocheted = true;
            }
        }
    }
}

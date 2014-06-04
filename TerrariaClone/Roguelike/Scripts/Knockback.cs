using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using zSprite;

namespace GameName1.Roguelike.Scripts
{
    public class Knockback : Script
    {
        public float knockbackDelay = 0.25f;
        public float knockbackSpeed = 200f;
        private float knockbackTimer = 0f;
        private Vector2 dir;

        private void applydamage(GameObject go, float amount)
        {
            if (knockbackTimer <= 0f)
            {
                var p = gameObject.transform.DerivedPosition;

                dir = p - go.transform.DerivedPosition;
                if (dir == Vector2.Zero)
                    dir = Vector2.One;
                dir.Normalize();
                knockbackTimer = knockbackDelay;
                gameObject.sendMessage("lockmovement", knockbackDelay);
            }
        }

        private void fixedupdate()
        {
            if (knockbackTimer > 0f)
            {
                knockbackTimer -= Root.instance.time.deltaTime;
                if (knockbackTimer <= 0)
                    gameObject.sendMessage("velocity", Vector2.Zero);
                else
                    gameObject.sendMessage("velocity", dir * knockbackSpeed);
            }
        }
    }
}

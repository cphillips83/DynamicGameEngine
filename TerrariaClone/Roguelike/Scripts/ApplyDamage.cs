using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using zSprite;

namespace GameName1.Roguelike.Scripts
{
    public class ApplyDamage : Script
    {
        public int flags = 0;
        public float amount = 0f;

        private void collision(List<Collision> collisions)
        {
            foreach (var c in collisions.Where(x => x.flag.CheckFlags(flags)))
            {
                c.gameObject.sendMessage("applydamage", gameObject, amount);
            }
        }
    }
}

using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using zSprite;

namespace GameName1.Roguelike.Scripts
{
    public class MeleeSwing : Script
    {
        public float arcStart = 0f;
        public float arcEnd = 0f;

        public float swingSpeed = 0.1f;
        protected float swingTimer = 0f;

        private void init()
        {
            swingTimer = swingSpeed;
        }

        private void fixedupdate()
        {
            if (swingTimer > 0f)
            {
                swingTimer -= Root.instance.time.deltaTime;

                var lerp = swingTimer / swingSpeed;
                var rot = arcStart.Lerp(arcEnd, lerp);

                gameObject.transform.Orientation = rot;
            }
        }
    }
}

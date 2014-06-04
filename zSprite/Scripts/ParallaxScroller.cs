using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using zSprite.Resources;

namespace zSprite
{
    public class ParallaxScroller : Script
    {
        public float speed = 1f;

        private void render()
        {
            transform.Position = (mainCamera.transform.DerivedPosition * speed);
        }
    }
}

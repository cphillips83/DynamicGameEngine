using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using zSprite;

namespace GameName1.Roguelike.Scripts
{
    public class DestroyTimer : Script
    {
        public float duration = 1f;
        
        private void fixedupdate()
        {
            duration -= Root.instance.time.deltaTime;
            if (duration <= 0f)
                gameObject.destroy();
        }
    }
}

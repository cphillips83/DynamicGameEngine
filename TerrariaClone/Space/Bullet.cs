using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using zSprite;

namespace GameName1.Space
{
    public class Bullet : Script
    {
        public TargetFilters targetFilters;

        public float damage = 1;

        private TargetableObjects _targets;

        private void init()
        {
            _targets = rootObject.getScript<TargetableObjects>();
        }

        private void fixedupdate()
        {
            var targets = _targets.get(transform.DerivedPosition, 40, targetFilters);

            foreach (var target in targets)
            {
                if (target != null)
                {
                    var scrolling = rootObject.getScript<ScrollingText>();
                    scrolling.emit(transform.DerivedPosition, damage.ToString());
                    target.broadcast("takedamage", damage);

                    //target.sendMessage("takedamage", damage);
                    gameObject.destroy();
                    break;
                }
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using zSprite;

namespace GameName1.Roguelike.Scripts
{
    public class Stats: Script
    {
        public float hp;
        public float maxhp;
        public float xp;

        private void applydamage(int amt)
        {
            if (hp > 0)
            {
                hp -= amt;
                gameObject.sendMessage("tookdamage", amt);

                if (hp < 0)
                    gameObject.sendMessage("killed");
            }
        }

        private void applyheal(int amt)
        {
            if (hp != maxhp)
            {
                hp += amt;
                if (hp > maxhp)
                    hp = maxhp;

                gameObject.sendMessage("healed", amt);
            }
        }
    }
}

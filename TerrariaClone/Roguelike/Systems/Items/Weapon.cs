using GameName1.Roguelike.Scripts;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using zSprite;

namespace GameName1.Roguelike.Systems.Items
{
    public abstract class Weapon
    {
        public string texture;

        public float attackDelay = 0.25f;
        public float duration = 0.1f;
        public float weaponDamage = 10f;
        public int flags = 0;
        
        public double nextAttack { get { return lastAttack + attackDelay; } }
        private double lastAttack = 0;

        public void attack(GameObject parent, Vector2 target)
        {
            var currentTime = Root.instance.time.fixedTimeD;
            if (currentTime >= nextAttack)
            {
                doattack(parent, target);
                lastAttack = currentTime;
            }
        }


        protected abstract void doattack(GameObject parent, Vector2 target);        
    }
}

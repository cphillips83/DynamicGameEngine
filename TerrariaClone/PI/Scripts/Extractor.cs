using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using zSprite;

namespace GameName1.PI.Scripts
{
    public class Extractor : Building
    {
        private Player _player;
        private float extractTimer = 2;
        private float extractTimeRemaining = 2;        

        private void init()
        {
            _player = rootObject.getScript<Player>();
        }

        protected override void updateBuilt()
        {
            if (extractTimeRemaining > 0f)
            {
                extractTimeRemaining -= time.deltaTime;
            }
            else
            {

            }
        }

        protected override void renderBuilt()
        {
            if (extractTimeRemaining > 0f)
            {
                var g = Root.instance.graphics;
                var p = transform.DerivedPosition;
                var s = new Vector2(p.X - 50, p.Y);
                var e = new Vector2(p.X + 50, p.Y);
                var l = 1f - extractTimeRemaining / extractTimer;
                g.Draw(null, AxisAlignedBox.FromRect(s.X, s.Y - 3, 100, 6), Color.Red);
                g.Draw(null, AxisAlignedBox.FromRect(s.X, s.Y - 3, 100 * l, 6), Color.Blue);
            }
        }

        private void spriteclicked()
        {
            if (extractTimeRemaining <= 0f)
            {
                //show menu


                //collect
                extractTimeRemaining = extractTimer;
                
                _player.fuel += 25;
                if (_player.fuel >= _player.maxFuel)
                    _player.fuel = _player.maxFuel;
            }
        }


    }
}

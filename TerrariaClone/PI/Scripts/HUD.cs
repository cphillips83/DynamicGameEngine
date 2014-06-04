using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using zSprite;
using zSprite.Resources;

namespace GameName1.PI.Scripts
{
    public class HUD : Script
    {
        private Player _player = null;
        private BmFont _font = null;

        private void init()
        {
            _player = rootObject.getScript<Player>();
            _font = Root.instance.resources.findFont("content/fonts/arial.fnt");
        }


        private void render()
        {
            var screenSize = screen.size /2f;


            var g = Root.instance.graphics;
            g.DrawText(_font, 1, new Vector2(-screenSize.X, -screenSize.Y + 200), string.Format("Fuel: {0}/{1}", _player.fuel, _player.maxFuel), Color.Red);
            g.DrawText(_font, 1, new Vector2(-screenSize.X, -screenSize.Y + 220), string.Format("Organisms: {0}/{1}", _player.organisms, _player.maxOrganisms), Color.Green);
            g.DrawText(_font, 1, new Vector2(-screenSize.X, -screenSize.Y + 240), string.Format("Electronics: {0}/{1}", _player.electronics, _player.maxElectronics), Color.Blue);
            g.DrawText(_font, 1, new Vector2(-screenSize.X, -screenSize.Y + 260), string.Format("Credits: {0}", _player.credits), Color.Yellow);

        }

    }
}

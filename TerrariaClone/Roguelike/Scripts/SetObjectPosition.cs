using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using zSprite;

namespace GameName1.Roguelike.Scripts
{
    public class SetObjectPosition : Script
    {
        private void update()
        {
            if (Root.instance.input.WasKeyPressed(Keys.F))
            {
                var camera = Camera.current;
                var mp = Root.instance.input.MousePosition;
                gameObject.transform.Position = camera.screenToWorld(mp);
            }
        }
    }
}

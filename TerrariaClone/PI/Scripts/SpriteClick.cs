using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using zSprite;

namespace GameName1.PI.Scripts
{
    public class SpriteClick : Script
    {
        private Sprite _sprite = null;
        //private AxisAlignedBox bounds
        private void init()
        {
            _sprite = gameObject.getScript<Sprite>();
        }

        private void update()
        {

            if (input.WasLeftMousePressed)
            {
                var aabb = AxisAlignedBox.FromDimensions(transform.DerivedPosition, _sprite.material.textureSize);
                var p = mainCamera.screenToWorld(input.MousePosition);
                if (aabb.Intersects(p))
                {
                    gameObject.sendMessage("spriteclicked");
                }
            }
        }

        //private void render()
        //{
        //    var aabb = AxisAlignedBox.FromDimensions(transform.DerivedPosition, _sprite.material.textureSize);
        //    graphics.Draw(aabb, Color.Red);

        //}
    }
}

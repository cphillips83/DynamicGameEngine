using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameName1.TopDown.Components;
using Microsoft.Xna.Framework;
using zSprite;

namespace GameName1.TopDown.Controllers
{
    public class PlayerController : Script
    {
        private float speed = 250f;
        private VelocityComponent _velocity;
        private void init()
        {
            _velocity = this.gameObject.getScript<VelocityComponent>();
        }

        private void update()
        {
            _velocity.value = Vector2.Zero;

            if (Root.instance.input.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.D))
                _velocity.value.X += speed;

            if (Root.instance.input.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.A))
                _velocity.value.X -= speed;

            if (Root.instance.input.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.W))
                _velocity.value.Y -= speed;

            if (Root.instance.input.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.S))
                _velocity.value.Y += speed;
        }
    }
}

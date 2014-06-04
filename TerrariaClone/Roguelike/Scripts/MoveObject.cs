using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using zSprite;

namespace GameName1.Roguelike.Scripts
{
    public class MoveObject : Script
    {
        public float speed = 150f;

        private void update()
        {
            var root = Root.instance;

            var d = new Vector2(0, 0);
            if (root.input.IsKeyDown(Keys.A)) d.X -= 1;
            if (root.input.IsKeyDown(Keys.D)) d.X += 1;
            if (root.input.IsKeyDown(Keys.W)) d.Y -= 1;
            if (root.input.IsKeyDown(Keys.S)) d.Y += 1;

            if (d != Vector2.Zero)
                gameObject.transform.Position += d * root.time.deltaTime * speed;
        }
    }
}

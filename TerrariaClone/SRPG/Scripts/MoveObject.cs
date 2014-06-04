using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using zSprite;

namespace GameName1.SRPG.Scripts
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
            if (root.input.IsKeyDown(Keys.W)) d.Y += 1;
            if (root.input.IsKeyDown(Keys.S)) d.Y -= 1;

            if (d != Vector2.Zero)
                gameObject.transform.Position += d * root.time.deltaTime * speed;
        }

        private float x = 0;
        private float dx = 1;
        private void fixedupdate()
        {
            x += dx;
            if (x > 99 || x < 1)
                dx = -dx;                
        }

        private void render()
        {
            //Root.instance.graphics.DrawLine(0, null, null, Vector2.Zero, new Vector2(0, 100), Color.Red);
            //Root.instance.graphics.DrawLine(0, null, null, Vector2.Zero, new Vector2(100, 0), Color.Red);
            //Root.instance.graphics.DrawLine(0, null, null, new Vector2(0, x), new Vector2(100, 100), Color.Red);
            //Root.instance.graphics.DrawLine(0, null, null, new Vector2(100, 0), new Vector2(100, 100), Color.Red);
            //Root.instance.graphics.DrawRect(0, null, null, AxisAlignedBox.FromRect(Vector2.Zero, Vector2.One * 100), Color.Green);
        }
    }
}

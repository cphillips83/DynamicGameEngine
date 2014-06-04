using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using zSprite;

namespace GameName1.Roguelike.Scripts
{
    public class Player : Script
    {
        private float speed = 150;

        private Systems.Items.Weapon left = null;
        private Systems.Items.Weapon right = null;

        private void init()
        {
            left = new Systems.Items.Melee();
            left.texture = "content/textures/sword2.png";
            left.flags = Physics.QUERY_ENEMIES;
        }

        private void fixedupdate()
        {
            var root = Root.instance;
            var d = new Vector2(0, 0);

            if (root.input.IsKeyDown(Keys.A)) d.X -= 1;
            if (root.input.IsKeyDown(Keys.D)) d.X += 1;
            if (root.input.IsKeyDown(Keys.W)) d.Y -= 1;
            if (root.input.IsKeyDown(Keys.S)) d.Y += 1;

            if (root.input.IsLeftMouseDown && left != null)
                left.attack(this.gameObject, Camera.mainCamera.screenToWorld(Root.instance.input.MousePosition));

            //if (d != Vector2.Zero)
            gameObject.sendMessage("velocity", d * speed);
        }

        //private void render()
        //{
        //    var aabb2 = AxisAlignedBox.FromDimensions(this.gameObject.transform.DerivedPosition, Vector2.One * 25);
        //    Root.instance.graphics.DrawRect(1, null, null, aabb2, Color.Green);
        //}
        private double lastImmunity = 0;
        private float immunityWindow = 0.25f;
        private void applydamage(GameObject attacker, float amt)
        {
            if (Root.instance.time.fixedTimeD > lastImmunity + immunityWindow)
            {
                lastImmunity = Root.instance.time.fixedTimeD;

                var go = Root.instance.RootObject.createChild(gameObject.name + " - scrollDamage");
                go.transform.DerivedPosition = gameObject.transform.DerivedPosition;

                var text = go.createScript<ScrollText>();
                text.color = Color.Red;
                text.fadeColor = new Color(Color.Red, 0f);
                text.text = amt.ToString();
                text.renderQueue = 10;
            }
        }

    }
}

using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using zSprite;
using zSprite.Resources;

namespace GameName1.SRPG.Scripts
{
    //    public class CommandMenuItem : Script
    //    {
    //        private CommandMenu menu = null;

    //        public bool selected = false;
    //        public string text = null;
    //        public Color color = Color.White;

    //        private void render()
    //        {
    ////            if(selected)
    ////                Root.instance.graphics.Draw(menu.renderQueue, null, null, AxisAlignedBox.
    //            Root.instance.graphics.DrawText(menu.renderQueue, menu.font,
    //                1f, gameObject.transform.DerivedPosition,
    //                text, color);
    //        }
    //    }

    public class CommandMenu : Script
    {
        public BmFont font;
        public string fontName;
        public int renderQueue = 0;
        private List<string> options = new List<string>();
        private int selectedIndex = 0;
        private bool showMenu = false;
        private GameObject target;

        private float x = 0;

        public void hide()
        {
            showMenu = false;
        }

        public void setTarget(GameObject go)
        {
            target = go;
            selectedIndex = 0;
            options.Clear();
        }

        public void show()
        {
            showMenu = true;
        }

        private void ongui()
        {
            if (Root.instance.gui.buttonold(new Vector2(x, 100), "abcdefghiABCDEFGHIJKLMNOPQRSTUVWXYZ!@#$%^&*()"))
            {
                //do something
            }

            if (Root.instance.input.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.Left))
                x -= Root.instance.time.deltaTime;
            if (Root.instance.input.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.Right))
                x += Root.instance.time.deltaTime;

            Root.instance.gui.label(new Vector2(x, 200), "absasdasd");
            Root.instance.gui.label(0, 1f, AxisAlignedBox.FromRect(new Vector2(x, 400), new Vector2(10, 100)), null, 0f, Color.Pink, "abcdefghijklmnopqrstuvwxyz");
        }

        private void init()
        {
            font = Root.instance.resources.findFont(fontName);
            showMenu = true;
            options.Add("test 2");
            options.Add("test 1");
            options.Add("test 1");
            options.Add("test 1");
        }

        //private void render()
        //{
        //    if (showMenu && options.Count > 0)
        //    {
        //        var sp = this.gameObject.transform.DerivedPosition;
        //        var p = sp;

        //        var maxWidth = 0f;

        //        for (var i = 0; i < options.Count; i++)
        //        {
        //            var o = options[i];

        //            var size = font.MeasureString(o);
        //            if (size.X > maxWidth)
        //                maxWidth = size.X;

        //            p.Y += 2;
        //            Root.instance.graphics.DrawText(Game.QUEUE_COMMAND_MENU, font, 1, p, o, Color.White, 1f);
        //            p.Y += font.MaxLineHeight + 2;
        //        }

        //        var ep = new Vector2(maxWidth, p.Y - sp.Y);
        //        var padding = new Vector2(10, 10);

        //        var sip = selectedIndex * new Vector2(0, font.MaxLineHeight + 4) + new Vector2(-2, -8 - padding.Y);
        //        var sipSize = new Vector2(maxWidth, font.MaxLineHeight) + new Vector2(4, 4);

        //        Root.instance.graphics.DrawRect(Game.QUEUE_COMMAND_MENU, null, null, AxisAlignedBox.FromRect(sp - padding, ep + padding * 2), new Color(Color.White, 0.25f), 0.1f);
        //        Root.instance.graphics.Draw(Game.QUEUE_COMMAND_MENU, null, null, AxisAlignedBox.FromRect(sip + p, sipSize), new Color(Color.Blue, 0.25f), 0f);
        //        Root.instance.graphics.Draw(Game.QUEUE_COMMAND_MENU, null, null, AxisAlignedBox.FromRect(sp - padding, ep + padding * 2), new Color(Color.Blue, 0.25f), 0f);
        //    }
        //}
    }
}
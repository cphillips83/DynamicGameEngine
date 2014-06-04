using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using zSprite;

namespace GameName1.SRPG.Systems
{
    public class GUI : Script
    {
        //public int BUTTON_TOP_LEFT { get; private set; }
        //public int BUTTON_TOP_MIDDLE { get; private set; }
        //public int BUTTON_TOP_RIGHT { get; private set; }
        //public int BUTTON_MIDDLE_LEFT { get; private set; }
        //public int BUTTON_MIDDLE_MIDDLE { get; private set; }
        //public int BUTTON_MIDDLE_RIGHT { get; private set; }
        //public int BUTTON_BOTTOM_LEFT { get; private set; }
        //public int BUTTON_BOTTOM_MIDDLE { get; private set; }
        //public int BUTTON_BOTTOM_RIGHT { get; private set; }

        //private SpriteSheet sheet;

        private interface IGUIRenderable
        {
            void render();
        }

        public void init()
        {
            //sheet = new SpriteSheet();
            //sheet.texture = Root.instance.resources.findTexture("content/textures/gui.png");

            //BUTTON_TOP_LEFT = sheet.add(AxisAlignedBox.FromRect(new Vector2(0, 0), new Vector2(9, 9)));
            //BUTTON_TOP_MIDDLE = sheet.add(AxisAlignedBox.FromRect(new Vector2(9, 0), new Vector2(102, 9)));
            //BUTTON_TOP_RIGHT = sheet.add(AxisAlignedBox.FromRect(new Vector2(111, 0), new Vector2(9, 9)));

            //BUTTON_MIDDLE_LEFT = sheet.add(AxisAlignedBox.FromRect(new Vector2(0, 9), new Vector2(9, 9)));
            //BUTTON_MIDDLE_MIDDLE = sheet.add(AxisAlignedBox.FromRect(new Vector2(9, 9), new Vector2(9, 12)));
            //BUTTON_MIDDLE_RIGHT = sheet.add(AxisAlignedBox.FromRect(new Vector2(21, 9), new Vector2(9, 9)));

            //BUTTON_BOTTOM_LEFT = sheet.add(AxisAlignedBox.FromRect(new Vector2(0, 18), new Vector2(9, 9)));
            //BUTTON_BOTTOM_MIDDLE = sheet.add(AxisAlignedBox.FromRect(new Vector2(9, 18), new Vector2(9, 12)));
            //BUTTON_BOTTOM_RIGHT = sheet.add(AxisAlignedBox.FromRect(new Vector2(21, 18), new Vector2(9, 9)));
        }

        private void render()
        {
            //var scale = new Vector2(2, 2);
            //sheet.draw(0, BUTTON_TOP_LEFT, AxisAlignedBox.FromRect(0, 0, 9, 9), Color.White);
            //sheet.draw(0, BUTTON_TOP_MIDDLE, AxisAlignedBox.FromRect(9, 0, 102, 9), Color.White);
            //sheet.draw(0, BUTTON_TOP_RIGHT, AxisAlignedBox.FromRect(111, 0, 9, 9), Color.White);
        }

        private struct GUIButton : IGUIRenderable
        {
            public AxisAlignedBox rect;
            public string text;

            public void render()
            {
            }
        }
    }
}
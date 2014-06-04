using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using zSprite;
using zSprite.Managers;

namespace GameName1
{
    public class DebugLabel : Script
    {

        private Vector2 offset = new Vector2(100, 100);
        private Vector2 size = new Vector2(200, 30);
        private Vector2 margin = new Vector2(10, 10);

        private GUIAnchor[] anchors = new GUIAnchor[] {
            GUIAnchor.UpperLeft, GUIAnchor.UpperCenter, GUIAnchor.UpperRight,
            GUIAnchor.MiddleLeft, GUIAnchor.MiddleCenter, GUIAnchor.MiddleRight,
            GUIAnchor.LowerLeft, GUIAnchor.LowerCenter, GUIAnchor.LowerRight
        };

        private void ongui()
        {
            var index = 0;
            for (var y = 0; y < 3; y++)
                for (var x = 0; x < 3; x++)
                {
                    gui.skin.label.alignment = anchors[index++];

                    var p = new Vector2(x, y) * (size + margin) + offset;
                    var aabb = AxisAlignedBox.FromRect(p, size);
                    //var aabb = AxisAlignedBox.FromRect(
                    gui.button(aabb, new GUIContent("1234567890 0909 ",resources.createMaterialFromTexture("content/textures/ship.png") ));
                    //gui.label2(AxisAlignedBox.FromRect(100, 500, 300, 300), new GUIContent("1234", new GUITexture() { material = resources.createMaterialFromTexture("content/textures/ship.png") }));
                    gui.layout.DoLabel(new GUIContent("test"), gui.skin.label, null);
                }
        }
    }
}

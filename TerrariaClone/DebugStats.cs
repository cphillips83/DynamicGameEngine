using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using zSprite;

namespace GameName1
{
    public class DebugStats : Script
    {
        private void ongui()
        {
            var gui = Root.instance.gui;
            gui.label(new Vector2(0, 0), string.Format("fps: {0}", (int)(1f / Root.instance.time.smoothedTimeDeltaD)));
            gui.label(new Vector2(0, 20), string.Format("drawCalls: {0}", Root.instance.graphics.drawCalls));
            gui.label(new Vector2(0, 40), string.Format("spritesSubmitted: {0}", Root.instance.graphics.spritesSubmitted));
            gui.label(new Vector2(0, 60), string.Format("activeGOs: {0}", Root.instance.totalGOs));
            gui.label(new Vector2(0, 80), string.Format("update: {0}/{1}", Root.instance.lastUpdate, Root.instance.totalUpdate / 1000));
            gui.label(new Vector2(0, 100), string.Format("fixedupdate: {0}/{1}", Root.instance.lastFixedUpdate, Root.instance.totalFixedUpdate / 1000));
            gui.label(new Vector2(0, 120), string.Format("render: {0}/{1}", graphics.lastFrameRender, graphics.totalFrameRender / 1000));

        }
    }
}

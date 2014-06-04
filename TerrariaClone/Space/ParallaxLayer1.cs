using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using zSprite;

namespace GameName1.Space
{
    public class ParallaxLayer1 : Script
    {
        private Settings _settings;

        public GameObject layer;

        private void init()
        {
            _settings = rootObject.getScript<Settings>();
        }

        private void loadsector(int sectorid)
        {
            createLayer();
            createSun();
        }

        private void createLayer()
        {
            layer = rootObject.createChild("parallaxLayer1");

            var parallax = layer.createScript<ParallaxScroller>();
            parallax.speed *= 0.95f;
        }

        private void createSun()
        {
            var sun = layer.createChild("sun");
            sun.transform.Position = new Vector2(-500, -500);

            var sunSprite = sun.createScript<Sprite>();
            sunSprite.material = Root.instance.resources.createMaterialFromTexture("content/textures/stars/Star K eg9.bmp");
            sunSprite.material.SetSamplerState(SamplerState.LinearClamp);
            sunSprite.material.SetBlendState(BlendState.Additive);
            //sprite.material.textureName = "content/textures/sun.png";
            sunSprite.material.transparency = new Color(1, 1, 1);
            sunSprite.size = new Vector2(500, 500);
            sunSprite.renderQueue = Game.LAYER_BACKGROUND;

            var flareSprite = sun.createScript<Sprite>();
            flareSprite.material = Root.instance.resources.createMaterialFromTexture("content/textures/flares2/sun.png");
            flareSprite.material.SetBlendState(BlendState.Additive);
            flareSprite.material.SetSamplerState(SamplerState.LinearClamp);
            flareSprite.color = new Color(0.1f, 0.1f, 0.1f);
            //sprite.color = new Color(
            flareSprite.size = new Vector2(2000, 2000);
            flareSprite.renderQueue = Game.LAYER_BACKGROUND;

            var coronaSprite = sun.createScript<Sprite>();
            coronaSprite.material = Root.instance.resources.createMaterialFromTexture("content/textures/flares2/corona.png");
            coronaSprite.material.SetBlendState(BlendState.Additive);
            coronaSprite.material.SetSamplerState(SamplerState.LinearClamp);
            coronaSprite.color = new Color(0.3f, 0.5f, 0.5f);
            //sprite.color = new Color(
            coronaSprite.size = new Vector2(2000, 2000);
            flareSprite.renderQueue = Game.LAYER_BACKGROUND;
        }

        private void unloadsector(int sectorid)
        {
            if (layer != null)
                layer.destroy();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using zSprite;

namespace GameName1.Space
{
    public class ParallaxLayer2 : SpriteQuadTree
    {
        private Settings _settings;
        private float _speed = 0.9f;

        public GameObject layer;

        private void init()
        {
            _settings = rootObject.getScript<Settings>();
        }

        private void loadsector(int sectorid)
        {
            createLayer();
            spawnPlanet(123, Vector2.Zero, 1f, 3);
        }

        private void createLayer()
        {
            layer = rootObject.createChild("parallaxLayer2");
        }

        protected void spawnPlanet(int seed, Vector2 location, float tightness, int moons)
        {
            var random = new Random(seed);

            var root = Root.instance;
            var parallaxGO = layer;
            var planetSpace = parallaxGO.createChild("planetspace");
            planetSpace.transform.Position = location;

            var parallaxScript2 = layer.createScript<ParallaxScroller>();
            parallaxScript2.speed = 0.85f;

            var planetGO = planetSpace.createChild("planet");
            {
                var id = random.Next(0, 82) + 1;
                var sprite = planetGO.createScript<Sprite>();
                sprite.material = root.resources.createMaterialFromTexture("content/textures/planets/planet (" + id + ").png");
                sprite.material.SetSamplerState(SamplerState.LinearClamp);
                sprite.renderQueue = Game.LAYER_PARALLAX2;
                //sprite.material.transparency = new Color(1, 1, 1);
                sprite.size = new Vector2(400, 400);
            }

            {
                var sprite = planetGO.createScript<Sprite>();
                sprite.material = Root.instance.resources.createMaterialFromTexture("content/textures/flare/tunel.png");
                sprite.material.SetBlendState(BlendState.Additive);
                sprite.material.SetSamplerState(SamplerState.LinearClamp);
                sprite.color = new Color(0.4f, 0.2f, 0.1f);
                sprite.renderQueue = Game.LAYER_PARALLAX2;
                //sprite.color = new Color(
                sprite.size = new Vector2(400, 400);
            }

            for (var i = 0; i < moons; i++)
            {
                var rotation = random.Next(-MathHelper.Pi, MathHelper.Pi);
                var dir = new Vector2(Utility.Cos(rotation), Utility.Sin(rotation));
                var distance = random.Next(400, 750);
                var moonGO = planetSpace.createChild("moon" + i);

                var sprite = moonGO.createScript<Sprite>();
                sprite.material = root.resources.createMaterialFromTexture("content/textures/moon chunks/moon (" + random.Next(1, 13) + ").png");
                sprite.material.SetSamplerState(SamplerState.LinearClamp);
                sprite.renderQueue = Game.LAYER_PARALLAX2;
                //sprite.material.transparency = new Color(1, 1, 1);
                sprite.size = new Vector2(100, 100);
                moonGO.transform.Scale = Vector2.One * random.Next(0.75f, 1.25f);
                moonGO.transform.Orientation = random.Next(-MathHelper.Pi, MathHelper.Pi);
                moonGO.transform.Position = dir * distance * tightness;
            }
        }


        private void genMoon(float x, float y)
        {

        }

        private void unloadsector(int sectorid)
        {
            if (layer != null)
                layer.destroy();
        }
    }
}

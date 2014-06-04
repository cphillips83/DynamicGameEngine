using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using zSprite;
using zSprite.Resources;

namespace GameName1.Space
{
    public class ParallaxLayer3 : SpriteQuadTree
    {
        private Settings _settings;
        private float _speed = 0.6f;

        private GameObject _layer;

        private void init()
        {
            _settings = rootObject.getScript<Settings>();
        }

        private void loadsector(int sectorid)
        {
            createLayer();
            createDebris();
        }

        private void createLayer()
        {
            _layer = gameObject.createChild("parallaxLayer3");
        }

        private void createDebris()
        {
            var len = random.Next(30, 60);
            for (var i = 0; i < len; i++)
            {
                var x = random.Next(-_settings.SectorHalfSize, _settings.SectorHalfSize);
                var y = random.Next(-_settings.SectorHalfSize, _settings.SectorHalfSize);
                //var x = random.Next(-1500, 1500);
                //var y = random.Next(-1500, 1500);
                createDebris(x, y);
            }
        }

        private void createDebris(float x, float y)
        {

            var sprite = new SpriteQuadTreeItem();
            //if (random.NextFloat() < 0.65f)
            //{
            //    var id = random.Next(0, 40) + 1;
            //    sprite.material = resources.createMaterialFromTexture("content/textures/satellite/sat (" + id + ").png");
            //}
            //else
            {
                var id = random.Next(0, 4) + 1;
                sprite.material = resources.createMaterialFromTexture("content/textures/moon chunks/meteordebris (" + id + ").png");
            }//sprite.material.SetBlendState(BlendState.Additive);
            //var sprite = _layer.createScript<ParallaxBackground>();
            sprite.material.SetSamplerState(SamplerState.LinearWrap);
            //sprite.speed = _speed;
            //sprite.transform.Scale = new Vector2(1.25f, 1.25f);
            //sprite.scale = new Vector2(0.5f, 0.5f);
            sprite.position = new Vector2(x, y);
            sprite.renderQueue = Game.LAYER_PARALLAX3;
            sprite.size = sprite.material.textureSize;
            //sprite.color = new Color(0.3f, 0.3f, 0.3f);
            addSprite(sprite);
        }

        protected override AxisAlignedBox getRenderBounds()
        {
            var bounds = mainCamera.worldBounds;
            bounds.Center *= (1f - _speed);
            return bounds;
        }

        public override void renderItem(Material material, int renderQueue, Vector2 position, AxisAlignedBox src, Color color, float rotation, Vector2 origin, Vector2 size, Vector2 scale, SpriteEffects spriteEffect)
        {
            position += mainCamera.transform.DerivedPosition * _speed;
            base.renderItem(material, renderQueue, position, src, color, rotation, origin, size, scale, spriteEffect);
        }

        private void unloadsector(int sectorid)
        {
            _layer.destroy();
            clear();
        }
    }
}

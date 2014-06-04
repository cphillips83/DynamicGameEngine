using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using zSprite;
using zSprite.Resources;

namespace GameName1.Digger.World
{
    public class TerrainRenderer : Script
    {
        public int x;
        public int y;
        public int width;
        public int height;

        private static Material _material = null;
        //private static TextureRef _texture = null;

        private Transform _transform;
        private Terrain _terrain;

        private void init()
        {
            //if (_material == null)
            //{
            //    _material = Root.instance.resources.createMaterial();
            //    //_material.textureName = "textures/terrain.png";
            //    _material.SetSamplerState(SamplerState.PointClamp); ;
            //}

            //if (_texture == null)
            //    _texture = Root.instance.resources.findTexture("content/textures/terrain.png");

            _material = Root.instance.resources.createMaterialFromTexture("content/textures/terrain.png"); 

            _transform = this.gameObject.transform2();
            _terrain = Root.instance.RootObject.getScript<Terrain>();
        }

        private void render()
        {
            var ap = _transform.DerivedPosition;
            var areaRect = AxisAlignedBox.FromRect(ap, Settings.TileSizeV * 8);
            var cameraBounds = Camera.mainCamera.worldBounds;
            cameraBounds.Inflate(Settings.TileSizeV * 32);
            if (cameraBounds.Intersects(areaRect))
            {
                for (var xx = x; xx < x + width; xx++)
                {
                    for (var yy = y; yy < y + height; yy++)
                    {
                        var b = _terrain.foreground[xx + (yy * _terrain.width)];
                        if (b > 0)
                        {
                            var p = new Vector2(xx - x, yy - y) * Settings.TileSizeV + _transform.DerivedPosition;
                            var destRect = AxisAlignedBox.FromRect(p, Settings.TileSizeV);

                            var sp = new Vector2(b & 0xf, b >> 4) * Settings.TileSizeV;
                            var srcRect = AxisAlignedBox.FromRect(sp, Settings.TileSizeV);

                            Root.instance.graphics.Draw(0, _material, destRect, srcRect, Color.White);
                        }
                    }
                }
            }
            //Root.instance.graphics.Draw(0, null, 
        }

    }
}

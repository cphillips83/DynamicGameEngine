using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using zSprite;
using zSprite.Resources;

namespace GameName1.Space
{
    public class TargetRecticle : Script
    {
        private Transform _transform;
        private Material _material;
        private TextureRef _texture;

        private void init()
        {
            _transform = this.gameObject.transform2();
            _material = Root.instance.resources.createMaterialFromTexture("content/textures/selection.png");
            _material.SetBlendState(BlendState.NonPremultiplied);

        }

        private void update()
        {
            if (_transform != null)
            {

            }
        }

        private void render()
        {
            if (_transform != null)
            {

            }
        }
    }
}

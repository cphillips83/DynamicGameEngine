using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using zSprite;

namespace GameName1.BulletHell.Scripts
{
    public class Player : Entity
    {

        private void init()
        {
            //_material = resources.createMaterialFromTexture("content/textures/bullethell/player.png");
            //_material.SetBlendState(BlendState.Additive);
            radius = 10;

            gameObject.createScript<PlayerMovement>();

            var cursorGO = rootObject.createChild("cursor");
            var trackMouse = cursorGO.createScript<TrackMouse>();
            var cursorSprite = cursorGO.createScript<Sprite>();
            cursorSprite.material = resources.createMaterialFromTexture("content/textures/bullethell/cursor.png");
            cursorSprite.rotation = 0f;
            cursorSprite.origin = Vector2.Zero;

            gameObject.createScript<PlayerWeapon>();
        }

        private void update()
        {
            var wp = mainCamera.screenToWorld(input.MousePosition);
            transform.LookAt(wp);
        }
    }
}

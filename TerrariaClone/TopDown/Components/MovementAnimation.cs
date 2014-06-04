using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using zSprite;
using zSprite.Scripts;

namespace GameName1.TopDown.Components
{
    public class MovementAnimation : SpriteAnimation
    {
        private VelocityComponent _velocity;
        private string currentAnimation = "face-east";
        private void init()
        {
            _velocity = this.gameObject.getScript<VelocityComponent>();
            addGrid(6, 4);
            fps = 0.10f;
            size = new Vector2(32, 64);
            material = Root.instance.resources.createMaterialFromTexture("content/textures/sprites_map_claudius.png");
            addAnimation("face-south", 0);
            addAnimation("face-west", 6);
            addAnimation("face-north", 12);
            addAnimation("face-east", 18);
            addAnimation("walk-south", 1, 2, 3, 4, 5);
            addAnimation("walk-west", 7, 8, 9, 10, 11);
            addAnimation("walk-north", 13, 14, 15, 16, 17);
            addAnimation("walk-east", 19, 20, 21, 22, 23);
            origin = new Vector2(0.5f, 1);
        }

        private void update()
        {
            var animation = currentAnimation;
            if (_velocity.value.Y < 0) animation = "walk-north";
            else if (_velocity.value.Y > 0) animation = "walk-south";
            else if (_velocity.value.X < 0) animation = "walk-west";
            else if (_velocity.value.X > 0) animation = "walk-east";
            else if (_velocity.value.X == 0 && _velocity.value.Y == 0)
                animation = currentAnimation.Replace("walk", "face");

            if (animation != currentAnimation)
            {
                setAnimation(animation);
                currentAnimation = animation;
            }
        }
    }
}

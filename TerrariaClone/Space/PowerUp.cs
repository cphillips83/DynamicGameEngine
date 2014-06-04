using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using zSprite;
using zSprite.Resources;

namespace GameName1.Space
{

    public enum PowerUpType : int
    {
        Damage,
        Speed,
        Defense
    }

    public class PowerUp : Script
    {
        public PowerUpType type;

        private void init()
        {
            var sprite = this.gameObject.createScript<Sprite>();
            if (type == PowerUpType.Damage)
                sprite.material = resources.createMaterialFromTexture("content/textures/cockpit2green.png");
            else if (type == PowerUpType.Defense)
                sprite.material = resources.createMaterialFromTexture("content/textures/cockpit1green.png");
            else if (type == PowerUpType.Speed)
                sprite.material = resources.createMaterialFromTexture("content/textures/cockpit4green.png");

        }

        private void fixedupdate()
        {
            var thisSprite = this.gameObject.getScript<Sprite>();
            var thisaabb = AxisAlignedBox.FromDimensions(transform.DerivedPosition, thisSprite.size);
            var player = rootObject.find("player");
            if (player != null)
            {
                var playerSprite = player.getScriptWithChildren<Sprite>();
                var playeraabb = AxisAlignedBox.FromDimensions(player.transform.DerivedPosition, playerSprite.size);

                if (playeraabb.Intersects(thisaabb))
                {
                    var playerController = player.getScriptWithChildren<PlayerController>();
                    this.gameObject.destroy();
                    if (type == PowerUpType.Damage)
                        playerController.damageModifier += 0.1f;
                    else if (type == PowerUpType.Defense)
                        playerController.defenseModifier += 0.1f;
                    //else if (type == PowerUpType.Speed)
                    //    playerController.MaxAcceleration += 0.5f;                        
                }
            }
        }
    }
}

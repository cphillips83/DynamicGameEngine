using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using zSprite;

namespace GameName1.BulletHell.Scripts
{
    public class PlayerManager : Script
    {
        private GameObject _playerGO;

        private void reset()
        {
        }

        private void respawn()
        {
            _playerGO = rootObject.createChild("player");

            var player = _playerGO.createScript<Player>();
            var playerSprite = _playerGO.createScript<Sprite>();
            playerSprite.material = resources.createMaterialFromTexture("content/textures/bullethell/player.png");
        }

        private void playerdead()
        {
            _playerGO.destroy();
        }
    }
}

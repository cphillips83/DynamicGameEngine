
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using zSprite;

namespace GameName1.BulletHell.Scripts
{
    public class Init : Script
    {
        private void init()
        {
            Root.instance.resources.addSearchPath("..\\..\\..\\");
            rootObject.createScript<DebugStats>();
            gameObject.destroy();

            rootObject.createScript<EnemyManager>();
            rootObject.createScript<WaveManager>();
            rootObject.createScript<PlayerStatus>();
            rootObject.createScript<PlayerManager>();

            var cameraGO = Root.instance.RootObject.createChild("camera");
            var camera = cameraGO.createScript<BloomCamera>();
            camera.clear = Color.Black;

            var cameraSprite = cameraGO.createScript<Sprite>();
            cameraSprite.material = resources.createMaterialFromTexture("content/textures/cockpit1green.png");
            cameraSprite.offset = new Vector2(-1000, -1000);
        }
    }
}

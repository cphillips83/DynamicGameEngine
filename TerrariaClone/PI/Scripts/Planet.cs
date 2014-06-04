using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using zSprite;
using zSprite.Resources;

namespace GameName1.PI.Scripts
{

    public class Planet : Script
    {
        private List<Building> _buildings = new List<Building>();

        private CommandCenter _commandCenter;
        private bool _hasCommandCenter = false;
        private bool _commandCenterReady = false;

        //private CommandCenter _commandCenter;

        private Sprite _sprite;

        private void init()
        {
            var _material = Root.instance.resources.createMaterialFromTexture("content/textures/planetred.png");
            _sprite = gameObject.createScript<Sprite>();
            _sprite.material = _material;
            _sprite.size = new Vector2(1024, 1024);
        }

        private void update()
        {
            //            foreach (var b in _buildings)
            //                b.update();
        }

        private void render()
        {
            var g = Root.instance.graphics;
            g.DrawCircle(null, Vector2.Zero, 460, 100, Color.Pink);

            //            foreach (var b in _buildings)
            //                b.render();
        }

        public bool canBuild(string building)
        {
            switch (building)
            {
                case "COMMAND": return !_hasCommandCenter;
                case "EXTRACT": return _hasCommandCenter && !_commandCenter.isBuilding;
                case "RESEARCH": return _hasCommandCenter && !_commandCenter.isBuilding;
                case "BARRACKS": return _hasCommandCenter && !_commandCenter.isBuilding;
                case "CANNON": return _hasCommandCenter && !_commandCenter.isBuilding;
                case "STORAGE": return _hasCommandCenter && !_commandCenter.isBuilding;
            }

            return false;
        }

        public bool placeBuilding(string building, Material m)
        {
            switch (building)
            {
                case "COMMAND":
                    if (canBuild(building))
                    {
                        gameObject.prefabCommandCenter(m, mainCamera.screenToWorld(input.MousePosition), out _commandCenter);
                        _hasCommandCenter = true;
                        //_commandCenter = new CommandCenter();
                        //_commandCenter.go = gameObject.createChild("commandcenter");
                        //_commandCenter.sprite = _commandCenter.go.createScript<Sprite>();
                        //_commandCenter.sprite.material = m;
                        //_commandCenter.sprite.size = _commandCenter.sprite.material.textureSize;
                        //_commandCenter.go.transform.Position = mainCamera.screenToWorld(input.MousePosition);
                        //_buildings.Add(_commandCenter);
                        return true;
                    }
                    break;
                case "EXTRACT":
                    if (canBuild(building))
                    {
                        Extractor extractor;
                        gameObject.prefabExtractor(m, mainCamera.screenToWorld(input.MousePosition), out extractor);
                        return true;
                    }
                    break;
                case "RESEARCH":
                case "BARRACKS":
                case "CANNON":
                case "STORAGE":
                    if (canBuild(building))
                    {
                        gameObject.prefabGenericBuilding(building, m, mainCamera.screenToWorld(input.MousePosition));
                        //return;
                        //var _buiding = new GenericBuilding();
                        //_buiding.go = gameObject.createChild(building);
                        //_buiding.sprite = _buiding.go.createScript<Sprite>();
                        //_buiding.sprite.material = m;
                        //_buiding.sprite.size = _buiding.sprite.material.textureSize;
                        //_buiding.go.transform.Position = mainCamera.screenToWorld(input.MousePosition);
                        //_buildings.Add(_buiding);
                        return true;
                    }
                    break;
            }

            return false;
        }



    }
}

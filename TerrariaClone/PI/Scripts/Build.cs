using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using zSprite;
using zSprite.Resources;

namespace GameName1.PI.Scripts
{
    public class Build : Script
    {
        //private GameObject _command, _extractor, _research, _barracks, _cannon, _storage;
        private Material _command, _extractor, _research, _barracks, _cannon, _storage;
        private string[] _buildActions = new string[] { "COMMAND", "EXTRACT", "RESEARCH", "BARRACKS", "CANNON", "STORAGE" };
        private string _building = null;
        private Planet _planet = null;

        private void init()
        {
            //var size = screen.size / 2;
            //_command = gameObject.prefabBuild("content/textures/cockpit1Green.png", size.X - 50, size.Y);
            //_extractor = gameObject.prefabBuild("content/textures/cockpit2Green.png", size.X - 50, size.Y - 75);
            //_research = gameObject.prefabBuild("content/textures/cockpit3Green.png", size.X - 50, size.Y - 150);
            //_barracks = gameObject.prefabBuild("content/textures/cockpit4Green.png", size.X - 50, size.Y - 225);
            //_cannon = gameObject.prefabBuild("content/textures/cockpit5Green.png", size.X - 50, size.Y - 300);
            //_storage = gameObject.prefabBuild("content/textures/wing1Green.png", size.X - 50, size.Y - 375);

            _command = resources.createMaterialFromTexture("content/textures/cockpit1Green.png");
            _extractor = resources.createMaterialFromTexture("content/textures/cockpit2Green.png");
            _research = resources.createMaterialFromTexture("content/textures/cockpit3Green.png");
            _barracks = resources.createMaterialFromTexture("content/textures/cockpit4Green.png");
            _cannon = resources.createMaterialFromTexture("content/textures/cockpit5Green.png");
            _storage = resources.createMaterialFromTexture("content/textures/wing1Green.png");

            //_command = gameObject.createScript<Sprite>();

            _planet = rootObject.find("planet").getScript<Planet>();

        }

        private void render()
        {

        }

        private void ongui()
        {
            var g = Root.instance.graphics;
            var firstSet = false;

            for (var i = 0; i < _buildActions.Length; i++)
            {
                var shouldBuild = input.WasKeyPressed(input.GetKeyByIndex(i));
                var canBuild = _planet.canBuild(_buildActions[i]);
                if (canBuild)
                    shouldBuild |= gui.button(AxisAlignedBox.FromRect(new Vector2(20 + i * 120, screen.height - 60), new Vector2(100, 40)), _buildActions[i]);

                if (shouldBuild && canBuild)
                {
                    _building = _buildActions[i];
                    firstSet = true;
                }
            }

            if (!firstSet)
            {
                if (input.WasKeyPressed(Keys.D1))
                    _building = null;

                switch (_building)
                {
                    case "COMMAND": drawMouseBuild(_command); break;
                    case "EXTRACT": drawMouseBuild(_extractor); break;
                    case "RESEARCH": drawMouseBuild(_research); break;
                    case "BARRACKS": drawMouseBuild(_barracks); break;
                    case "CANNON": drawMouseBuild(_cannon); break;
                    case "STORAGE": drawMouseBuild(_storage); break;
                }
            }
        }

        private void drawMouseBuild(Material m)
        {
            var size = m.textureSize;
            //var wp
            graphics.Draw(m, AxisAlignedBox.FromRect(gui.screenToGUI(input.MousePosition) - size / 2, size), Color.White);

            if (input.WasLeftMousePressed)
            {
                if (_planet.placeBuilding(_building, m))
                    _building = null;
            }
        }

        //private void build(string command)
        //{
        //    switch (command)
        //    {
        //        case "COMMAND": _building = command;
        //        case "EXTRACT":
        //        case "RESEARCH":
        //        case "BARRACKS":
        //        case "CANNON":
        //        case "STORAGE":
        //            break;
        //    }
        //}
    }
}

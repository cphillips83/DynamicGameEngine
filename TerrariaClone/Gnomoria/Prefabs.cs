﻿using GameName1.Gnomoria.Scripts;
using GameName1.Gnomoria.Scripts.AI;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using zSprite;
using GameName1.Gnomoria.Scripts.Navigation;
using GameName1.Gnomoria.Scripts.World;
using GameName1.Gnomoria.Scripts.Tools;

namespace GameName1.Gnomoria
{
    public static class Prefabs
    {
        public static GameObject prefabPlayer(this GameObject parent)
        {
            var go = parent.createChild("player");
            go.transform.Position = new Vector2(200, 200);
          
            return go;
        }

        public static GameObject prefabWorld(this GameObject parent)
        {
            //var wc = parent.createChild("worldContainer");
            //
            //var go = parent.createChild("world");
            parent.createScript<WorldManager>();
            parent.createScript<GameInput>();
            parent.createScript<WorldRenderer>();
            parent.createScript<AIManager>();
            parent.createScript<NavManager>();
            return parent;
        }

        public static GameObject prefabTools(this GameObject parent)
        {
            var go = parent.createChild("tools");
            go.createScript<ToolManager>();
            go.createScript<Inspector>();
            go.createScript<DigWall>();
            return go;
        }

        //public static GameObject prefabEnemy(this GameObject parent)
        //{
        //    var go = parent.createChild();
        //    go.transform.Depth = 1f;
        //    go.transform.Position = new Vector2(12.5f * Map.tileSize, 12.5f * Map.tileSize);

        //    var ai = go.createScript<SimpleAI>();
        //    var sprite = go.createScript<JsonAnimation>();
        //    sprite.file = "content/sprites/skeleton.json"; //textureName = "content/textures/stickfigure.png";
        //    //sprite.color = Color.Red;
        //    //sprite.size = new Vector2(16, 32) * Game.WORLD_SCALE;
        //    //sprite.origin = new Vector2(0.5f, 0.5f);

        //    var physics = go.createScript<Physics>();
        //    physics.shape = AxisAlignedBox.FromRect(Vector2.Zero, new Vector2(16f, 32f) * Game.WORLD_SCALE);

        //    var stats = go.createScript<Stats>();
        //    stats.maxhp = 10;
        //    stats.hp = 10;
        //    stats.xp = 10;

        //    var collider = go.createScript<Collider>();
        //    collider.flags = Physics.QUERY_ENEMIES;
        //    collider.collidesWith = Physics.QUERY_PLAYER;
        //    //collider.notifyObject = go;
        //    collider.size = new Vector2(16, 32) * Game.WORLD_SCALE;
        //    //collider.origin = new Vector2(0.5f, 0.5f);

        //    var knockback = go.createScript<Knockback>();
        //    var flash = go.createScript<FlashSpriteOnDamage>();

        //    return go;
        //}

        public static GameObject prefabCamera(this GameObject parent)
        {
            var go = parent.createChild("camera");
            go.createScript<Camera>();
            go.createScript<MoveObject>();

            //go.transform.Position = new Vector2(1000, 1000);

            //var trackObject = go.createScript<TrackObject>();
            //trackObject.objectName = "player";
            //trackObject.keepInBounds = AxisAlignedBox.FromDimensions(Vector2.Zero, new Vector2(300, 150));
            //go.createScript<MoveObject>();

            return go;
        }

    }
}

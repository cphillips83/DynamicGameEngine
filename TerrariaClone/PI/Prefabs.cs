using GameName1.PI.Scripts;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using zSprite;
using zSprite.Resources;

namespace GameName1.PI
{
    public static class Prefabs
    {
        //public static GameObject prefabPlayer(this GameObject parent)
        //{
        //    var go = parent.createChild("player");
        //    //go.createScript<VelocityComponent>();
        //    //go.createScript<Camera>();
        //    //go.createScript<PlayerController>();
        //    go.createScript<Player>();
        //    go.transform.Depth = 1f;
        //    //go.createScript<MoveObject>();
        //    var sprite = go.createScript<Sprite>();
        //    sprite.material = Root.instance.resources.createMaterialFromTexture("content/textures/stickfigure.png");
        //   // sprite.textureName = "content/textures/stickfigure.png";
        //    sprite.size = new Vector2(16, 32) * Game.WORLD_SCALE;
        //    sprite.origin = new Vector2(0.5f, 0.5f);
            
        //    //go.createScript<MovementAnimation>();
        //    go.createScript<SetObjectPosition>();
        //    var physics = go.createScript<Physics>();
        //    physics.shape = AxisAlignedBox.FromRect(Vector2.Zero, new Vector2(16f, 32f) * Game.WORLD_SCALE);

        //    var flashOnDamage = go.createScript<FlashSpriteOnDamage>();
        //    //var weapon = go.createScript<MeleeWeapon>();

        //    go.transform.Position = new Vector2(12.5f * Map.tileSize, 12.5f * Map.tileSize);

        //    var stats = go.createScript<Stats>();
        //    stats.maxhp = 100;
        //    stats.hp = 100;
        //    stats.xp = 0;

        //    var collider = go.createScript<Collider>();
        //    collider.flags = Physics.QUERY_PLAYER;
        //    collider.collidesWith = Physics.QUERY_ENEMIES;
        //    //collider.notifyObject = go;
        //    collider.size = new Vector2(16, 32) * Game.WORLD_SCALE;
        //    collider.origin = new Vector2(0.5f, 0.5f);  

        //    return go;
        //}

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

        public static GameObject prefabPlanet(this GameObject parent)
        {
            var go = parent.createChild("planet");
            go.createScript<Planet>();

            return go;
        }

        public static GameObject prefabBuild(this GameObject parent, string texture, float x, float y)
        {
            var go = parent.createChild();
            var sprite = go.createScript<Sprite>();
            sprite.material = Root.instance.resources.createMaterialFromTexture(texture);
            sprite.renderQueue = 15;
            sprite.size = new Vector2(50, 50);

            go.transform.Position = new Vector2(x, y - 50);

            return go;
        }

        public static GameObject prefabCamera(this GameObject parent)
        {
            var go = parent.createChild("camera");
            go.createScript<Camera>();

            //var trackObject = go.createScript<TrackObject>();
            //trackObject.objectName = "player";
            //trackObject.keepInBounds = AxisAlignedBox.FromDimensions(Vector2.Zero, new Vector2(300, 150));

            return go;
        }

        public static GameObject prefabCommandCenter(this GameObject parent, Material m, Vector2 p, out CommandCenter building)
        {
            var go = parent.createChild("commandcenter");
            //_commandCenter = new CommandCenter();
            //go = gameObject.createChild("commandcenter");
            building = go.createScript<CommandCenter>();
            building.setBuildTime(2);
            
            var sprite = go.createScript<Sprite>();
            sprite.material = m;
            sprite.origin = new Vector2(0.5f, 0.5f);
            sprite.size =sprite.material.textureSize;
            //go.transform.Position = mainCamera.screenToWorld(input.MousePosition);
            go.transform.Position = p;
            //_buildings.Add(_commandCenter);
            return go;
        }

        public static GameObject prefabExtractor(this GameObject parent, Material m, Vector2 p, out Extractor building)
        {
            var go = parent.createChild("extractor");
            //_commandCenter = new CommandCenter();
            //go = gameObject.createChild("commandcenter");
            building = go.createScript<Extractor>();
            building.setBuildTime(2);

            go.createScript<SpriteClick>();

            var sprite = go.createScript<Sprite>();            
            sprite.material = m;
            sprite.origin = new Vector2(0.5f, 0.5f);
            sprite.size = sprite.material.textureSize;
            //go.transform.Position = mainCamera.screenToWorld(input.MousePosition);
            go.transform.Position = p;
            //_buildings.Add(_commandCenter);
            return go;
        }

        public static GameObject prefabGenericBuilding(this GameObject parent, string buildingName, Material m, Vector2 p)
        {
            var go = parent.createChild(buildingName);
            //_commandCenter = new CommandCenter();
            //go = gameObject.createChild("commandcenter");
            var building = go.createScript<Building>();
            building.setBuildTime(25);
            var sprite = go.createScript<Sprite>();
            sprite.material = m;
            sprite.origin = new Vector2(0.5f, 0.5f);
            sprite.size = sprite.material.textureSize;
            //go.transform.Position = mainCamera.screenToWorld(input.MousePosition);
            go.transform.Position = p;
            //_buildings.Add(_commandCenter);
            return go;
        }
    }
}

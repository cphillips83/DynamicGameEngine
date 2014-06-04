using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using zSprite;
using zSprite.Json;
using zSprite.Resources;

namespace GameName1.SRPG.Scripts
{
    public class Map : Script
    {
        public enum MapTileType
        {
            Invalid,
            Blocked,
            Wall,
            Room,
            Open
        }

        public struct MapTile
        {
            public int x, y;
            public MapTileType tileType;

            public static MapTile Invalid = new MapTile() { tileType = MapTileType.Invalid };
            public static MapTile Blocked = new MapTile() { tileType = MapTileType.Blocked };

            public bool Movable { get { return !(tileType == MapTileType.Invalid || tileType == MapTileType.Blocked || tileType == MapTileType.Wall); } }
        }

        public const int tileSize = (int)(32 * Game.WORLD_SCALE);
        public const int tileSizeHs = (int)(32 / 2f * Game.WORLD_SCALE);
        private JsonNumber _width = new JsonNumber(25);
        private JsonNumber _height = new JsonNumber(25);

        public int width { get { return (int)_width.Number; } }
        public int height { get { return (int)_height.Number; } }

        private MapTile[,] tiles;

        //private Material _material;
        private Material _room, _wall, _blocked, _open;

        private void init()
        {
            _width = this.gameObject.add("mapwidth", _width);
            _height = this.gameObject.add("mapheight", _height);

            tiles = new MapTile[(int)_width.Number, (int)_height.Number];

            //_material = Root.instance.resources.createMaterial();
            //_material.SetSamplerState(SamplerState.PointClamp);

            _room = Root.instance.resources.createMaterialFromTexture("content/textures/tiles/plain2.png");
            _room.SetSamplerState(SamplerState.PointClamp);
            
            _wall = Root.instance.resources.createMaterialFromTexture("content/textures/tiles/plain2.png");
            _wall.SetSamplerState(SamplerState.PointClamp);
            
            _blocked = Root.instance.resources.createMaterialFromTexture("content/textures/tiles/plain2.png");
            _blocked.SetSamplerState(SamplerState.PointClamp);
            
            _open = Root.instance.resources.createMaterialFromTexture("content/textures/tiles/plain2.png");
            _open.SetSamplerState(SamplerState.PointClamp);

            var r = new Random(1234567);

            var rooms = 10;
            while (rooms > 0)
            {
                var x = r.Next(0, (int)_width.Number - 10);
                var y = r.Next(0, (int)_height.Number - 10);
                var w = r.Next(4, 8);
                var h = r.Next(4, 8);

                if (addRoom(x, y, w, h))
                    rooms--;
            }

            //addRoom(5, 5, 15, 15);


            //_plain = Root.instance.resources.findTexture("content/textures/tiles/plain.png");
            //_shadow_hort = Root.instance.resources.findTexture("content/textures/tiles/shadow-hort.png");
            //_shadow_vert = Root.instance.resources.findTexture("content/textures/tiles/shadow-vert.png");
        }

        private bool addRoom(int x, int y, int w, int h)
        {
            var x2 = Math.Min(x + w, (int)_width.Number);
            var y2 = Math.Min(y + h, (int)_height.Number);

            var x1 = x;
            var y1 = y;

            if (x2 - x1 < 2 || y2 - y1 < 2)
                return false;

            for (var xr = x1; xr < x2; xr++)
            {
                for (var yr = y1; yr < y2; yr++)
                {
                    if (xr == x1 || xr == x2 - 1 || yr == y1 || yr == y2 - 1)
                    {
                        if (tiles[xr, yr].tileType != MapTileType.Room)
                            tiles[xr, yr].tileType = MapTileType.Wall;
                    }
                    else
                    {
                        tiles[xr, yr].tileType = MapTileType.Room;
                    }
                }
            }

            return true;
        }

        private void update()
        {
            //var aabb = AxisAlignedBox.FromDimensions(Vector2.One * 250, Vector2.One * 25);
            //var player = Root.instance.RootObject.find("player");
            //var p = player.transform.DerivedPosition;

            //var aabb2 = AxisAlignedBox.FromDimensions(p, Vector2.One * 25);

            //var mtv = aabb2.collide(aabb);
            //if (mtv.intersects)
            //    player.transform.DerivedPosition += (float)mtv.overlap * mtv.smallest.normal;
            //System.Diagnostics.Debug.WriteLine();
        }

        private void render()
        {
            {
                var color1 = new Vector3(0.8f, 0.8f, 0.8f);
                var color2 = new Vector3(0.7f, 0.7f, 0.7f);


                var screenAABB = Camera.mainCamera.worldBounds;
                var wx0 = Math.Floor(screenAABB.X0 / tileSize);
                var wx1 = Math.Ceiling(screenAABB.X1 / tileSize);
                var wy0 = Math.Floor(screenAABB.Y0 / tileSize);
                var wy1 = Math.Ceiling(screenAABB.Y1 / tileSize);

                var minX = (int)Math.Max(0, wx0);
                var maxX = (int)Math.Min(_width.Number, wx1);
                var minY = (int)Math.Max(0, wy0);
                var maxY = (int)Math.Min(_height.Number, wy1);

                for (var y = minY; y < maxY; y++)
                {
                    for (var x = minX; x < maxX; x++)
                    {
                        //var map = getTile(x, y);

                        var r = getTileRect(x, y);
                        //var p = r.Minimum;

                        //var c = color1.Lerp(color2, z / 10f);
                        //if (shadowmap[x, y])
                        //    c *= 0.9f;
                        var texture = _open;
                        var color = Color.Black;
                        switch (tiles[x, y].tileType)
                        {
                            case MapTileType.Blocked:
                                texture = _blocked;
                                break;
                            case MapTileType.Room:
                                texture = _room;
                                color = new Color(color1.Lerp(color2, (1 + PerlinSimplexNoise.noise(x * 0.5f, y * 0.5f)) / 2));
                                break;
                            case MapTileType.Wall:
                                texture = _wall;
                                color = new Color(0.2f, 0.2f, 0.2f);
                                break;
                        }

                        Root.instance.graphics.Draw(0, texture, r, color, 0);

                        //if (z == heights[x, y] - 1)
                        //{
                        //    if (((x + 1 < size && heights[x + 1, y] > heights[x, y])))
                        //        Root.instance.graphics.Draw(0, _shadow_hort, _material, AxisAlignedBox.FromRect(p, tileSize, tileSize), Color.White, z + 0.1f);
                        //    if (((y - 1 >= 0 && heights[x, y - 1] > heights[x, y])))
                        //        Root.instance.graphics.Draw(0, _shadow_vert, _material, AxisAlignedBox.FromRect(p, tileSize, tileSize), Color.White, z + 0.1f);

                        //    if (((x - 1 >= 0 && heights[x - 1, y] > heights[x, y])))
                        //        Root.instance.graphics.Draw(0, _shadow_hort, _material, AxisAlignedBox.FromRect(p, tileSize, tileSize), Color.White, SpriteEffects.FlipHorizontally, z + 0.1f);
                        //    if (((y + 1 < size && heights[x, y + 1] > heights[x, y])))
                        //        Root.instance.graphics.Draw(0, _shadow_vert, _material, AxisAlignedBox.FromRect(p, tileSize, tileSize), Color.White, SpriteEffects.FlipVertically, z + 0.1f);
                        //}

                        //if ((x - 1 < 0 || heights[x - 1, y] < heights[x, y]) && z == heights[x, y] - 1)
                        //{
                        //    Root.instance.graphics.DrawLine(0, null, null, new Vector2(x * tileSize, y * tileSize - z * tileHeight), new Vector2(x * tileSize, (y + 1) * tileSize - z * tileHeight), Color.Black, z + 0.1f);
                        //}
                        //if ((x + 1 >= size || heights[x + 1, y] < heights[x, y]) && z == heights[x, y] - 1)
                        //{
                        //    Root.instance.graphics.DrawLine(0, null, null, new Vector2((x + 1) * tileSize, y * tileSize - z * tileHeight), new Vector2((x + 1) * tileSize, (y + 1) * tileSize - z * tileHeight), Color.Black, z + 0.1f);
                        //}
                        //if ((y - 1 < 0 || heights[x, y - 1] < heights[x, y]) && z == heights[x, y] - 1)
                        //{
                        //    Root.instance.graphics.DrawLine(0, null, null, new Vector2(x * tileSize, y * tileSize - z * tileHeight), new Vector2((x + 1) * tileSize, y * tileSize - z * tileHeight), Color.Black, z + 0.1f);
                        //}
                        //if ((y + 1 >= size || heights[x, y + 1] < heights[x, y]) && z == heights[x, y] - 1)
                        //{
                        //    Root.instance.graphics.DrawLine(0, null, null, new Vector2(x * tileSize, (y + 1) * tileSize - z * tileHeight), new Vector2((x + 1) * tileSize, (y + 1) * tileSize - z * tileHeight), Color.Black, z + 0.1f);
                        //}
                        //if ((x - 1 < 0 || heights[x - 1, y] < z + 1) && (y + 1 >= size || heights[x, y + 1] < z + 1))
                        //    Root.instance.graphics.DrawLine(0, null, null, new Vector2(x * tileSize, (y + 1) * tileSize - z * tileHeight), new Vector2(x * tileSize, (y + 1) * tileSize - z * tileHeight + tileHeight), Color.Black, z + 0.1f);
                        //if ((x + 1 >= size || heights[x + 1, y] < z + 1) && (y + 1 >= size || heights[x, y + 1] < z + 1))
                        //    Root.instance.graphics.DrawLine(0, null, null, new Vector2((x + 1) * tileSize, (y + 1) * tileSize - z * tileHeight), new Vector2((x + 1) * tileSize, (y + 1) * tileSize - z * tileHeight + tileHeight), Color.Black, z + 0.1f);

                    }
                }

                //Root.instance.graphics.DrawRect(1, null, null, AxisAlignedBox.FromDimensions(Vector2.One * 250, Vector2.One * 25), Color.Pink);
            }
            //{
            //    var go = Root.instance.RootObject.find("player");
            //    var physics = go.getScript<Physics>();

            //    var screenAABB = physics.shape;
            //    var wx0 = Math.Floor(screenAABB.X0 / tileSize);
            //    var wx1 = Math.Ceiling(screenAABB.X1 / tileSize);
            //    var wy0 = Math.Floor(screenAABB.Y0 / tileSize);
            //    var wy1 = Math.Ceiling(screenAABB.Y1 / tileSize);

            //    var minX = (int)Math.Max(0, wx0);
            //    var maxX = (int)Math.Min(_width.Number, wx1);
            //    var minY = (int)Math.Max(0, wy0);
            //    var maxY = (int)Math.Min(_height.Number, wy1 );

            //    var hs = tileSize * Game.WORLD_SCALE * Vector2.One;
            //    var aabb = AxisAlignedBox.FromRect(Vector2.Zero, hs);
            //    var mtv = MinimumTranslationVector.Zero;
            //    for (var y = minY; y < maxY; y++)
            //    {
            //        for (var x = minX; x < maxX; x++)
            //        {
            //            if (tiles[x, y].tileType != MapTileType.Room)
            //            {
            //                aabb.Center = new Vector2(x, y) * tileSize * Game.WORLD_SCALE + hs / 2;

            //                Root.instance.graphics.DrawRect(1, null, null, aabb, Color.Green);
            //            }
            //        }
            //    }

            //}
        }

        //private void queryCollision(AxisAlignedBox shape, List<MinimumTranslationVector> collisions)
        //{
        //    var screenAABB = shape;
        //    var wx0 = Math.Floor(screenAABB.X0 / tileSize);
        //    var wx1 = Math.Ceiling(screenAABB.X1 / tileSize);
        //    var wy0 = Math.Floor(screenAABB.Y0 / tileSize);
        //    var wy1 = Math.Ceiling(screenAABB.Y1 / tileSize);

        //    var minX = (int)Math.Max(0, wx0);
        //    var maxX = (int)Math.Min(_width.Number, wx1);
        //    var minY = (int)Math.Max(0, wy0);
        //    var maxY = (int)Math.Min(_height.Number, wy1);

        //    var hs = tileSize * Game.WORLD_SCALE * Vector2.One;
        //    var aabb = AxisAlignedBox.FromRect(Vector2.Zero, hs);
        //    var mtv = MinimumTranslationVector.Zero;
        //    for (var y = minY; y < maxY; y++)
        //    {
        //        for (var x = minX; x < maxX; x++)
        //        {
        //            if (tiles[x, y].tileType != MapTileType.Room)
        //            {
        //                aabb.Center = new Vector2(x, y) * tileSize * Game.WORLD_SCALE + hs / 2;

        //                var testmtv = shape.collide(aabb);
        //                if (testmtv.intersects)
        //                {
        //                    if (!mtv.intersects || testmtv.overlap < mtv.overlap)
        //                        mtv = testmtv;
        //                }
        //            }
        //        }
        //    }

        //    if (mtv.intersects)
        //        collisions.Add(mtv);
        //}

        //private void queryBroadphaseAABB(CollisionQuery query, List<Collision> collisions)
        //{
        //    if (query.flag.CheckFlags(Physics.QUERY_WORLD))
        //    {
        //        var screenAABB = query.aabb;
        //        var wx0 = Math.Floor(screenAABB.X0 / tileSize);
        //        var wx1 = Math.Ceiling(screenAABB.X1 / tileSize);
        //        var wy0 = Math.Floor(screenAABB.Y0 / tileSize);
        //        var wy1 = Math.Ceiling(screenAABB.Y1 / tileSize);

        //        var minX = (int)Math.Max(0, wx0);
        //        var maxX = (int)Math.Min(_width.Number, wx1);
        //        var minY = (int)Math.Max(0, wy0);
        //        var maxY = (int)Math.Min(_height.Number, wy1);

        //        var hs = tileSize * Game.WORLD_SCALE * Vector2.One;
        //        //var aabb = AxisAlignedBox.FromRect(Vector2.Zero, hs);
        //        var mtv = MinimumTranslationVector.Zero;
        //        for (var y = minY; y < maxY; y++)
        //        {
        //            for (var x = minX; x < maxX; x++)
        //            {
        //                if (tiles[x, y].tileType != MapTileType.Room)
        //                {
        //                    var aabb = AxisAlignedBox.FromDimensions(new Vector2(x, y) * tileSize * Game.WORLD_SCALE + hs / 2, hs);

        //                    if (query.aabb.Intersects(aabb))
        //                        collisions.Add(new Collision(Physics.QUERY_WORLD, gameObject, aabb));
        //                }
        //            }
        //        }
        //    }
        //}

        public MapTile getTile(int x, int y)
        {
            if (x < 0 || y < 0 || x >= _width.Number || y >= _height.Number)
                return MapTile.Invalid;

            return tiles[x, y];
        }

        public Point worldToMap(Vector2 wp)
        {
            return new Point((int)Math.Floor(wp.X / tileSize / Game.WORLD_SCALE), (int)Math.Floor(wp.Y / tileSize / Game.WORLD_SCALE));
        }

        public Vector2 mapToWorld(Point mp)
        {
            return new Vector2(mp.X, mp.Y) * tileSize * Game.WORLD_SCALE + (new Vector2(tileSize, tileSize) / 2f);
        }

        public Point getTilePoint(int id)
        {
            var x1 = id >> 16;
            var y1 = id & 0xffff;
            return new Point(x1, y1);
        }

        public AxisAlignedBox getTileRect(int x, int y)
        {
            return AxisAlignedBox.FromRect(new Vector2(x, y) * tileSize - new Vector2(tileSizeHs, tileSizeHs),
                new Vector2(tileSize, tileSize));
        }

        public int getTileId(Point p)
        {
            return (p.X << 16) + p.Y;
        }

        public Point[] getNeighbors(Point p)
        {
            var points = new Point[4];
            points[0] = new Point(p.X - 1, p.Y);
            points[1] = new Point(p.X + 1, p.Y);
            points[2] = new Point(p.X, p.Y - 1);
            points[3] = new Point(p.X, p.Y + 1);
            return points;
        }

        public int[] getNeighborIds(Point p)
        {
            var ids = new int[4];
            ids[0] = getTileId(new Point(p.X - 1, p.Y));
            ids[1] = getTileId(new Point(p.X + 1, p.Y));
            ids[2] = getTileId(new Point(p.X, p.Y - 1));
            ids[3] = getTileId(new Point(p.X, p.Y + 1));
            return ids;
        }

        //public int[] getNeighborIds(int id)
        //{
        //    return getNeighborIds(getTilePoint(id));
        //}

    }
}

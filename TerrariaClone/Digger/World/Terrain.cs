using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using zSprite;
using zSprite.Scripts;

namespace GameName1.Digger.World
{
    public struct BlockData
    {
        private byte light;
        
    }

    public class Terrain : Script
    {
        public int GridWidth { get; private set; }
        public int GridHeight { get; private set; }

        public int width = 1024;
        public int height = 1024;

        public byte[] background = null;
        public byte[] foreground = null;
        public BlockData[] blockData = null;
        public byte[] sundepth = null;

        private Camera _camera;

        private void init()
        {
            _camera = Camera.mainCamera;
            //_camera.gameObject.transform().Scale = new Vector2(0.75f, 0.75f);
            //_camera.gameObject.transform().Position = _camera.gameObject.transform().Position - Vector2.UnitY * 10;

            background = new byte[width * height];
            foreground = new byte[width * height];
            blockData = new BlockData[width * height];
            sundepth = new byte[width];

            for (var x = 0; x < width; x++)
            {
                if (x == 500 || x == 524)
                    for (var y = 0; y < height; y++)
                        SetForeground(x, y, 2);

                for (var y = height / 2; y < height; y++)
                {
                    SetBackground(x, y, 1);
                    SetForeground(x, y, 2);
                }
            }

            updateGameObjects();

        }

        private void updateGameObjects()
        {
            var cameraBounds = _camera.worldBounds;
            cameraBounds.Inflate(Settings.TileSizeV * 16);

            var cameraPos = _camera.gameObject.transform2().DerivedPosition;

            for (var x = 0; x < width / 8; x++)
            {
                for (var y = 0; y < height / 8; y++)
                {
                    var p = new Vector2(x, y);
                    var wp = new Vector2(x, y) * 8 * Settings.TileSizeV;
                    var dist = (int)((cameraPos / 8 / Settings.TileSizeV) - p).LengthSquared();

                    var seenByCamera = cameraBounds.Contains(wp);
                    if (!seenByCamera && gameObjects.ContainsKey(p))
                    {
                        gameObjects[p].destroy();
                        gameObjects.Remove(p);
                        return;
                    }
                    else if (seenByCamera && !gameObjects.ContainsKey(p))
                    {
                        var child = this.gameObject.createChild();
                        var tr = child.createScript<TerrainRenderer>();
                        var transform = child.transform2();
                        transform.Position = wp;
                        tr.x = x * 8;
                        tr.y = y * 8;
                        tr.width = 8;
                        tr.height = 8;

                        //var tc = child.createComponent<TerrainCollider>();
                        //tc.x = x * 8;
                        //tc.y = y * 8;
                        //tc.width = 8;
                        //tc.height = 8;

                        gameObjects.Add(p, child);
                    }
                }
            }
        }

        //private void render()
        //{
        //    var material = Root.instance.materials.find("basewhite");
        //    var maxdist = 4 * 4;

        //    var cameraBounds = _camera.WorldBounds;
        //    cameraBounds.Inflate(Settings.TileSizeV * 8);

        //    var cameraPos = _camera.gameObject.transform().DerivedPosition;
        //    Root.instance.graphics.DrawRect(15, material, cameraPos - Settings.TileSizeV / 2, cameraPos + Settings.TileSizeV / 2, Color.Red);

        //    var scale = 8;
        //    var sVector = new Vector2(scale, scale);

        //    for (var x = 0; x < width / 8; x++)
        //    {
        //        for (var y = 0; y < height / 8; y++)
        //        {
        //            var p = new Vector2(x, y);
        //            var wp = new Vector2(x, y) * 8 * Settings.TileSizeV;
        //            var dist = (int)((cameraPos / 8 / Settings.TileSizeV) - p).LengthSquared();

        //            if (dist < maxdist)
        //            {
        //                Root.instance.graphics.DrawLine(15, material, cameraPos, wp, Color.Green);
        //                //Root.instance.graphics.DrawRect(15, material, p, p + Settings.TileSizeV * 32, Color.Red);
        //            }

        //            if (cameraBounds.Contains(wp))
        //                Root.instance.graphics.DrawRect(16, material, wp, wp + new Vector2(8, 8) * 16, Color.Red);
        //        }
        //    }
        //}

        private Dictionary<Vector2, GameObject> gameObjects = new Dictionary<Vector2, GameObject>();
        private void fixedupdate()
        {
            updateGameObjects();

        }

        public byte GetBackground(int x, int y)
        {
            return background[x + (y * width)];
        }

        public byte GetForeground(int x, int y)
        {
            return foreground[x + (y * width)];
        }

        private void SetBackground(int x, int y, byte b)
        {
            background[x + (y * width)] = b;
        }

        private void SetForeground(int x, int y, byte b)
        {
            foreground[x + (y * width)] = b;
        }

        public Vector2 move(AxisAlignedBox box, Vector2 velocity, out bool isOnGround, out bool hitSide, out bool hitHead)
        {
            isOnGround = false;
            hitSide = false;
            hitHead = false;

            var sx = (int)Math.Floor(velocity.X);
            var sy = (int)Math.Floor(velocity.Y);
            var tx = Math.Abs((int)sx);
            var ty = Math.Abs((int)sy);
            var dx = Math.Sign(velocity.X);
            var dy = Math.Sign(velocity.Y);
            var px0 = box.X0;
            var px1 = box.X1;
            var py0 = box.Y0;
            var py1 = box.Y1;

            while (tx >= 1 || ty >= 1)
            {
                if (tx >= 1)
                {
                    tx -= 1;
                    px0 += dx;
                    px1 += dx;

                    var bx0 = Math.Max((int)(Math.Floor(px0 / Settings.TileSize)), 0);
                    var by0 = Math.Max((int)(Math.Floor(py0 / Settings.TileSize)), 0);
                    var bx1 = Math.Min((int)(Math.Floor(px1 / Settings.TileSize)), width);
                    var by1 = Math.Min((int)(Math.Floor(py1 / Settings.TileSize)), height);

                    //for (var x = bx0; x < bx1; x++)
                    {
                        for (var y = by0; y < by1; y++)
                        {
                            var x = dx < 0 ? bx0 : bx1;
                            var id = foreground[x + (y * width)];
                            var block = ForegroundBlockDescriptor.blocks[id];
                            if (block.isSolid)
                            {
                                hitSide = true;
                                px0 -= dx;
                                px1 -= dx;
                                break;
                            }
                        }
                    }
                }

                if (ty >= 1)
                {
                    ty -= 1;
                    py0 += dy;
                    py1 += dy;

                    var bx0 = Math.Max((int)(Math.Floor(px0 / Settings.TileSize)), 0);
                    var by0 = Math.Max((int)(Math.Floor(py0 / Settings.TileSize)), 0);
                    var bx1 = Math.Min((int)(Math.Floor(px1 / Settings.TileSize)), width);
                    var by1 = Math.Min((int)(Math.Floor(py1 / Settings.TileSize)), height);

                    for (var x = bx0; x < bx1; x++)
                    {
                        var y = dy < 0 ? by0 : by1;
                        var id = foreground[x + (y * width)];
                        var block = ForegroundBlockDescriptor.blocks[id];
                        if (block.isSolid)
                        {
                            if (dy > 0)
                                isOnGround = true;
                            else
                                isOnGround = false;

                            py0 -= dy;
                            py1 -= dy;
                            break;
                        }
                    }
                }
            }

            var p0 = new Vector2(px0, py0);
            var p1 = new Vector2(px1, py1);
            var s = new Vector2(sx, sy);
            return ((p0 + p1) / 2f);
        }

        
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using zSprite;
using zSprite.Resources;

namespace GameName1.TopDown.Components.World
{
    public class MapRenderer : Script
    {
        private const int size = 256;
        //private const int tileSize = 72;
        private const int tileHeight = 24;
        private const int tileSize = 48;
        private byte[, ,] map = new byte[size, size, 10];
        private bool[,] shadowmap = new bool[size, size];
        private byte[,] heights = new byte[size, size];
        private Vector3[] tileColors = new Vector3[10];

        //private Material _material = null;
        private Material _plain = null;
        private Material _shadow_hort = null;
        private Material _shadow_vert = null;

        private void init()
        {
            //_material = Root.instance.resources.createMaterial();
            //_material.SetSamplerState(SamplerState.PointClamp);

            _plain = Root.instance.resources.createMaterialFromTexture("content/textures/tiles/plain.png");
            _plain.SetSamplerState(SamplerState.PointClamp);
            
            _shadow_hort = Root.instance.resources.createMaterialFromTexture("content/textures/tiles/shadow-hort.png");
            _shadow_hort.SetSamplerState(SamplerState.PointClamp);
            
            _shadow_vert = Root.instance.resources.createMaterialFromTexture("content/textures/tiles/shadow-vert.png");
            _shadow_vert.SetSamplerState(SamplerState.PointClamp);

            tileColors[0] = new Vector3(1f, 0.5f, 0.1f);
            tileColors[1] = new Vector3(0f, 0.8f, 0f);

            //var r = new Random();
            for (var x = 0; x < size; x++)
            {
                for (var y = 0; y < size; y++)
                {

                    //var maxHeight = r.Next(1, 10);
                    //var dst = new Vector2(x, y) - new Vector2(size, size) / 2;
                    //var maxHeight = Math.Max(0, dst.Length());
                    //heights[x, y] = (byte)Math.Min(10, Math.Ceiling(maxHeight));
                    //heights[x, y] = 1;
                    //map[x, y, 0] = 1;
                    //var maxHeight = Math.Min(10, Math.Sqrt((x - size / 2) * (y - size / 2)));
                    //for (var z = 0; z < heights[x, y]; z++)
                    {
                        heights[x, y] = 1;
                        //map[x, y, z] = 1;
                    }
                }
            }
            //heights[0, 0] = 2;
            //map[0, 0, 1] = 1;

            updateshadows();

        }

        private void update()
        {
            if (Root.instance.input.WasLeftMousePressed || Root.instance.input.WasRightMousePressed)
            {
                var mp = Root.instance.input.MousePosition;
                var wp = Camera.mainCamera.screenToWorld(mp);

                var x = (int)Math.Floor(wp.X / tileSize);
                var y = (int)Math.Floor(wp.Y / tileSize);


                if (x >= 0 && x < size && y >= 0 && y < size)
                {
                    if (Root.instance.input.WasLeftMousePressed && heights[x, y] < 10)
                    {
                        map[x, y, heights[x, y]] = 1;
                        heights[x, y]++;
                    }
                    else if (Root.instance.input.WasRightMousePressed && heights[x, y] > 1)
                    {
                        heights[x, y]--;
                        map[x, y, heights[x, y]] = 0;
                    }
                }
            }
        }

        private void render()
        {

            var color1 = new Vector3(1f, 0.5f, 0.1f);
            var color2 = new Vector3(0f, 0.8f, 0f);
            var screenAABB = Camera.mainCamera.worldBounds;
            var wx0 = Math.Floor(screenAABB.X0 / tileSize);
            var wx1 = Math.Ceiling(screenAABB.X1 / tileSize);
            var wy0 = Math.Floor(screenAABB.Y0 / tileSize);
            var wy1 = Math.Ceiling(screenAABB.Y1 / tileSize);

            var minX = (int)Math.Max(0, wx0);
            var maxX = (int)Math.Min(size, wx1);
            var minY = (int)Math.Max(0, wy0);
            var maxY = (int)Math.Min(size, wy1 + 10);


            for (var y = minY; y < maxY; y++)
            {
                for (var x = minX; x < maxX; x++)
                {
                    for (var z = 0; z < heights[x, y]; z++)
                    {
                        var p = new Vector2(x * tileSize, y * tileSize - z * tileHeight);

                        //var c = color1.Lerp(color2, z / 10f);
                        //if (shadowmap[x, y])
                        //    c *= 0.9f;
                        Root.instance.graphics.Draw(0, _plain, AxisAlignedBox.FromRect(p, tileSize, 72), new Color(tileColors[map[x, y, z]]), z);

                        if (z == heights[x, y] - 1)
                        {
                            if (((x + 1 < size && heights[x + 1, y] > heights[x, y])))
                                Root.instance.graphics.Draw(0, _shadow_hort, AxisAlignedBox.FromRect(p, tileSize, tileSize), Color.White, z + 0.1f);
                            if (((y - 1 >= 0 && heights[x, y - 1] > heights[x, y])))
                                Root.instance.graphics.Draw(0, _shadow_vert, AxisAlignedBox.FromRect(p, tileSize, tileSize), Color.White, z + 0.1f);

                            if (((x - 1 >= 0 && heights[x - 1, y] > heights[x, y])))
                                Root.instance.graphics.Draw(0, _shadow_hort, AxisAlignedBox.FromRect(p, tileSize, tileSize), Color.White, SpriteEffects.FlipHorizontally, z + 0.1f);
                            if (((y + 1 < size && heights[x, y + 1] > heights[x, y])))
                                Root.instance.graphics.Draw(0, _shadow_vert, AxisAlignedBox.FromRect(p, tileSize, tileSize), Color.White, SpriteEffects.FlipVertically, z + 0.1f);
                        }

                        if ((x - 1 < 0 || heights[x - 1, y] < heights[x, y]) && z == heights[x, y] - 1)
                        {
                            Root.instance.graphics.DrawLine(new Vector2(x * tileSize, y * tileSize - z * tileHeight), new Vector2(x * tileSize, (y + 1) * tileSize - z * tileHeight), Color.Black, z + 0.1f);
                        }
                        if ((x + 1 >= size || heights[x + 1, y] < heights[x, y]) && z == heights[x, y] - 1)
                        {
                            Root.instance.graphics.DrawLine(new Vector2((x + 1) * tileSize, y * tileSize - z * tileHeight), new Vector2((x + 1) * tileSize, (y + 1) * tileSize - z * tileHeight), Color.Black, z + 0.1f);
                        }
                        if ((y - 1 < 0 || heights[x, y - 1] < heights[x, y]) && z == heights[x, y] - 1)
                        {
                            Root.instance.graphics.DrawLine(new Vector2(x * tileSize, y * tileSize - z * tileHeight), new Vector2((x + 1) * tileSize, y * tileSize - z * tileHeight), Color.Black, z + 0.1f);
                        }
                        if ((y + 1 >= size || heights[x, y + 1] < heights[x, y]) && z == heights[x, y] - 1)
                        {
                            Root.instance.graphics.DrawLine(new Vector2(x * tileSize, (y + 1) * tileSize - z * tileHeight), new Vector2((x + 1) * tileSize, (y + 1) * tileSize - z * tileHeight), Color.Black, z + 0.1f);
                        }
                        if ((x - 1 < 0 || heights[x - 1, y] < z + 1) && (y + 1 >= size || heights[x, y + 1] < z + 1))
                            Root.instance.graphics.DrawLine(new Vector2(x * tileSize, (y + 1) * tileSize - z * tileHeight), new Vector2(x * tileSize, (y + 1) * tileSize - z * tileHeight + tileHeight), Color.Black, z + 0.1f);
                        if ((x + 1 >= size || heights[x + 1, y] < z + 1) && (y + 1 >= size || heights[x, y + 1] < z + 1))
                            Root.instance.graphics.DrawLine(new Vector2((x + 1) * tileSize, (y + 1) * tileSize - z * tileHeight), new Vector2((x + 1) * tileSize, (y + 1) * tileSize - z * tileHeight + tileHeight), Color.Black, z + 0.1f);

                    }
                }
            }
        }

        private void updateshadows()
        {
            for (var x = 0; x < size; x++)
            {
                for (var y = 0; y < size; y++)
                {
                    if (!shadowmap[x, y])
                        projectshadow(x, y, heights[x, y]);
                }
            }
        }

        private void projectshadow(int x, int y, int z)
        {
            for (var i = 1; i < z; i++)
            {
                var zz = z - i;
                var xx = x + i;
                var yy = y + i;

                if (zz < 0 || xx >= size || yy >= size)
                    break;

                if (heights[xx, yy] > zz)
                    break;

                shadowmap[xx, yy] = true;
            }

        }

        public bool checkCollisionByWorldPos(Vector2 p, int height)
        {
            var fx = (int)Math.Floor(p.X / tileSize);
            var fy = (int)Math.Floor((p.Y + (height - 2) * tileHeight) / tileSize);

            if (fx < 0 || fx >= size || fy < 0 || fy >= size || height < 0)
                return true;

            return heights[fx, fy] >= height;
        }
    }
}

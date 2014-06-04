using GameName1.Gnomoria.Scripts.Tiles;
using GameName1.Gnomoria.Scripts.World.GameObjects.Placeable;
using GameName1.Gnomoria.Scripts.GameObjects;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using zSprite;
using zSprite.Collections;
using zSprite.Managers;

namespace GameName1.Gnomoria.Scripts.World
{
    public enum ToolType2
    {
        DigWall,
        DigRamp,
        PlaceDirtWall,

    }

    public class WorldManager : Script
    {
        public MapTile[] blocks;
        public PlaceableObject[] placableObjects = new PlaceableObject[256];
        //public WorldObjectDescriptors descriptors = new WorldObjectDescriptors();

        public bool isLoaded = false;

        public int mapWidth = 64;
        public int mapHeight = 64;
        public int mapDepth = 32;

        public int blockSize = 32;
        public int blockSizeOver2 = 16;
        public int blockSizeOver4 = 8;

        public int wallSize = 32;
        public int wallSzieOver2 = 16;
        public int wallSizeOver4 = 8;

        public int floorSize = 8;
        public int floorSizeOver2 = 4;
        public float floorSizeOver4 = 2;

        public Vector2 offset;
        public float currentDepthOffset;
        public int currentMapDepth = 0;
        public ToolType selectedTool = ToolType.None;

        public Point startIsoSelection;
        public bool isSelecting;
        public AxisAlignedBox drawRect;

        public ObjectPool<List<WorldObject>> worldObjectListPool = new ObjectPool<List<WorldObject>>();

        public List<Vector3i> dig = new List<Vector3i>();

        public MapTile getTile(int x, int y, int z)
        {
            //if (x < 0 || y < 0)
            //    return MapTile.DirtFloor;
            if (!Utility.BetweenOrEq(x, 0, mapWidth))
                return MapTile.Empty;
            else if (!Utility.BetweenOrEq(z, 0, mapDepth))
                return MapTile.Empty;
            else if (!Utility.BetweenOrEq(y, 0, mapHeight))
                return MapTile.Empty;

            var index = Arrays.index3(mapWidth, mapHeight, x, y, z);
            return blocks[index];
        }

        public void setTile(int x, int y, int z, MapTile tile)
        {
            var index = Arrays.index3(mapWidth, mapHeight, x, y, z);
            blocks[index] = tile;
        }

        public void placeObject(int x, int y, int z, WorldObject o)
        {
            var block = getTile(x, y, z);

            if (block.objects == 0)
            {
                block.objects = (ushort)(worldObjectListPool.GetFreeItem() + 1);
                worldObjectListPool[block.objects - 1] = new List<WorldObject>();
                setTile(x, y, z, block);
            }

            var objects = worldObjectListPool[block.objects - 1];
            objects.Add(o);
        }

        public void removeObject(int x, int y, int z, WorldObject o)
        {
            var block = getTile(x, y, z);

            if (block.objects != 0)
            {
                var objects = worldObjectListPool[block.objects - 1];
                if (objects.Remove(o))
                {

                    if (objects.Count == 0)
                    {
                        worldObjectListPool.FreeItem(block.objects - 1);
                        block.objects = 0;
                        setTile(x, y, z, block);
                    }

                    return;
                }
            }

            throw new Exception("could not find object");
        }

        //private void addDescriptor(WorldObjectDescriptor desc)
        //{
        //    descriptors[desc.type] = desc;
        //}

        private void init()
        {
            //addDescriptor(new WorldObjectDescriptor() { isPlacable = true, 
            //descriptors[WorldObjectType.DirtFloor] = new WorldObjectDescriptor() { isPlacable = true, isSolid = true, color = 

            Camera.mainCamera.transform.Scale = new Vector2(3f, 3f);


            addPlaceable(new DirtWall(this));
            addPlaceable(new DirtRamp(this));
        }

        private void loadmap()
        {

            blocks = Arrays.create3<MapTile>(mapWidth, mapHeight, mapDepth);
            for (var x = 0; x < mapWidth; x++)
            {
                for (var y = 0; y < mapHeight; y++)
                {
                    var zh = (int)(Math.Abs(SimplexNoise.noise(x * noiseAmount, y * noiseAmount, 0) * 50)) + 1;
                    //zh = 10;
                    for (var z = 0; z < zh; z++)
                    {
                        blocks[Arrays.index3(mapWidth, mapHeight, x, y, z)] = MapTile.DirtWall;

                        //if (y == 0 && x == 0 && z == 0)
                        //    blocks[Arrays.index3(mapWidth, mapHeight, x, y, z)] = MapTile.DirtWall;
                        //if (y == 2 && x == 0 && z == 0)
                        //    blocks[Arrays.index3(mapWidth, mapHeight, x, y, z)] = MapTile.DirtWall;
                        //if (y == 10 && x == 0 && z == 4)
                        //    blocks[Arrays.index3(mapWidth, mapHeight, x, y, z)] = MapTile.DirtWall;
                        //if (y == 2 && x == 0 && z == 1)
                        //    blocks[Arrays.index3(mapWidth, mapHeight, x, y, z)] = MapTile.DirtWall;
                        //if (y == 4 && x == 0 && z == 2)
                        //    blocks[Arrays.index3(mapWidth, mapHeight, x, y, z)] = MapTile.DirtWall;
                        //if (y == 6 && x == 0 && z == 3)
                        //    blocks[Arrays.index3(mapWidth, mapHeight, x, y, z)] = MapTile.DirtWall;
                        //if (y == 8 && x == 0 && z == 4)
                        //    blocks[Arrays.index3(mapWidth, mapHeight, x, y, z)] = MapTile.DirtWall;
                        //if (y == 10 && x == 0 && z == 4)
                        //    //if (y == 6 && x == 0 && z == 3)
                        //        blocks[Arrays.index3(mapWidth, mapHeight, x, y, z)] = MapTile.DirtFloor;
                        //if (y == 20 && x == 0 && z == 8)
                        //    blocks[Arrays.index3(mapWidth, mapHeight, x, y, z)] = MapTile.DirtFloor;




                        //if (y == 1 && x == 0 && z == 1)
                        //    blocks[Arrays.index3(mapWidth, mapHeight, x, y, z)] = MapTile.DirtFloor;
                        //if (y == 1 && x == 1 && z == 1)
                        //    blocks[Arrays.index3(mapWidth, mapHeight, x, y, z)] = MapTile.DirtFloor;
                        //if (y == 0 && x == 1 && z == 1)
                        //    blocks[Arrays.index3(mapWidth, mapHeight, x, y, z)] = MapTile.DirtWall;
                        ////if (y == 2 && x == 0 && z == 1)
                        ////    blocks[Arrays.index3(mapWidth, mapHeight, x, y, z)] = MapTile.DirtWall;
                        //if (y == 1 && x == 0 && z == 0)
                        //    blocks[Arrays.index3(mapWidth, mapHeight, x, y, z)] = MapTile.DirtWall;
                        //if (y == 1 && x == 1 && z == 0)
                        //    blocks[Arrays.index3(mapWidth, mapHeight, x, y, z)] = MapTile.DirtWall;
                        //if (y == 0 && x == 1 && z == 0)
                        //    blocks[Arrays.index3(mapWidth, mapHeight, x, y, z)] = MapTile.DirtWall;
                        //var zz = z + 1;
                        //if ((y < 6 / zz && x < 6 / zz))
                        //if (y == 0 && x == 1)
                        //    blocks[Arrays.index3(mapWidth, mapHeight, x, y, z)] = MapTile.DirtFloor;
                        //else if (y == 2 && x == 1)
                        //    blocks[Arrays.index3(mapWidth, mapHeight, x, y, z)] = MapTile.DirtFloor;
                        //else if (y == 1 && x == 0)
                        //    blocks[Arrays.index3(mapWidth, mapHeight, x, y, z)] = MapTile.DirtFloor;
                        //else if (y == 1 && x == 1)
                        //    blocks[Arrays.index3(mapWidth, mapHeight, x, y, z)] = MapTile.DirtFloor;
                    }
                }
            }

            isLoaded = true;
            rootObject.broadcast("maploaded");
        }

        public void addPlaceable(PlaceableObject po)
        {
            placableObjects[po.id] = po;
        }

        public void setDepth(int newDepth)
        {
            currentMapDepth = Utility.Clamp(newDepth, 0, mapDepth);
            currentDepthOffset = -currentMapDepth * (blockSize / 2);

            //var cp = Camera.mainCamera.gameObject.transform.Position;
            //cp.Y = -mapDepth * (blockSize / 2);
            //Camera.mainCamera.gameObject.transform.Position = cp;
        }

        private float noiseAmount = 0.010f;
        public List<Vector3i> selectedTiles = new List<Vector3i>();

        private void update()
        {
            if (isLoaded)
            {
                offset = new Vector2(Camera.mainCamera.viewport.Width, Camera.mainCamera.viewport.Height) / -2f;
                var aabb = mainCamera.worldBounds;
                aabb.Inflate(-330, -190);
                drawRect = aabb;

                if (input.WasKeyPressed(Microsoft.Xna.Framework.Input.Keys.OemPlus))
                    currentMapDepth = Utility.Min(currentMapDepth + 1, mapDepth - 1);
                else if (input.WasKeyPressed(Microsoft.Xna.Framework.Input.Keys.OemMinus))
                    currentMapDepth = Utility.Max(currentMapDepth - 1, 0);

                if (input.WasLeftMousePressed)
                {
                    var im = mouseWallIso;
                    if (im.X >= 0 && im.X < mapWidth && im.Y >= 0 && im.Y < mapHeight)
                    {
                        for (int z = currentMapDepth; z >= 0; z--)
                        {
                            var block = getTile(im.X, im.Y, z);
                            var topBlock = getTile(im.X, im.Y, z + 1);

                            if (block.isWalkable)
                            {
                                //placeObject(im.X, im.Y, z, new ZombieWorker(im.X, im.Y, z));
                                break;
                            }
                        }
                    }
                }

                //var min = minDrawableIso;
                //var max = maxDrawableIso;
                //var miso = mouseIso;
                //var worldStart = isoToWorld(startIsoSelection.X, startIsoSelection.Y) / new Vector2(blockSize, blockSizeOver2);
                //var worldEnd = isoToWorld(mouseIso.X, mouseIso.Y) / new Vector2(blockSize, blockSizeOver2);
                //var diamondStart = IsoHelper.ToDiamondIso(worldStart);
                //var diamondEnd = IsoHelper.ToDiamondIso(worldEnd);

                //for (var y = max.Y; y >= min.Y; y--)
                //{
                //    for (var x = min.X; x <= max.X; x++)
                //    {
                //        if (x >= 0 && x <= world.mapWidth - 1)
                //        {
                //            for (int z = world.currentMapDepth; z >= 0; z--)
                //            {
                //                var depthCorrectedY = y - (world.currentMapDepth - z) * 2;
                //                if (depthCorrectedY < 0)
                //                    break;

                //                if (depthCorrectedY >= 0 && depthCorrectedY <= world.mapHeight - 1)
                //                {
                //                    var index = Arrays.index3(world.mapWidth, world.mapHeight, x, depthCorrectedY, z);
                //                    var block = world.blocks[index];
                //                    if (block.type != TileType.Empty)
                //                    {
                //                        blocksToRender[z].Add(new Point(x, depthCorrectedY));
                //                        if (!block.descriptor.IsTransparent)
                //                            break;
                //                    }
                //                }
                //            }
                //        }
                //    }
                //}

                if (!isSelecting && input.IsLeftMouseDown)
                {
                    isSelecting = true;
                    startIsoSelection = mouseWallIso;
                }
                else if (isSelecting && !input.IsLeftMouseDown)
                {
                    isSelecting = false;
                    foreach (var tile in selectedTiles)
                    {
                        var block = getTile(tile.x, tile.y, tile.z);
                        if (block.hasPlaceable)
                            block.placedType = WorldObjectType.Missing;

                        setTile(tile.x, tile.y, tile.z, block);
                    }
                }

                //if (input.WasKeyPressed(Microsoft.Xna.Framework.Input.Keys.OemOpenBrackets))
                //    noiseAmount -= 0.005f;
                //if (input.WasKeyPressed(Microsoft.Xna.Framework.Input.Keys.OemCloseBrackets))
                //    noiseAmount += 0.005f;

                //var min = minDrawableIso;
                //var max = maxDrawableIso;
                //min.Y -= 20;

                //for (var y = max.Y; y >= min.Y; y--)
                //{
                //    if (y >= 0 && y <= mapHeight - 1)
                //    {
                //        for (var x = min.X; x <= max.X; x++)
                //        {
                //            if (x >= 0 && x <= mapWidth - 1)
                //            {
                //                for (var z = 0; z < mapDepth; z++)
                //                    blocks[Arrays.index3(mapWidth, mapHeight, x, y, z)] = MapBlock.empty;

                //                var zh = (int)(Math.Abs(SimplexNoise.noise(x * noiseAmount, y * noiseAmount, 0) * 50)) + 10;
                //                for (var z = 0; z < zh; z++)
                //                    blocks[Arrays.index3(mapWidth, mapHeight, x, y, z)] = (z == zh - 1) ? MapBlock.grass : MapBlock.dirt;
                //            }
                //        }
                //    }
                //}
            }
        }

        private void ongui()
        {
            if (isLoaded)
            {
                gui.label(new Vector2(0, 100), string.Format("depth: {0}", currentMapDepth));
                gui.label(new Vector2(0, 120), string.Format("noise: {0}", noiseAmount));

                //if (input.WasKeyPressed(Keys.))
                //    selectedTool = ToolType.None;
                if (input.WasKeyPressed(Keys.D1))
                    selectedTool = ToolType.DigWall;

                //graphics.Draw(spriteSheetMaterial, AxisAlignedBox.FromRect(0, 400, 32, 32), AxisAlignedBox.FromRect(0, 96, 32, 32), selectedTool == ToolType.Dig ? Color.White : Color.Gray);

                if (isSelecting)
                {
                    //var miso = mouseIso;
                    gui.label(new Vector2(0, 140), string.Format(""));
                }

                gui.label(new Vector2(0, 160), "Tool: ");

                //gui.label2(AxisAlignedBox.FromRect(100, 200, 150, 150), new GUIContent("1234 1234 1234 1234", new GUITexture() { material = resources.createMaterialFromTexture("content/textures/ship.png") }));
                //gui.label2(AxisAlignedBox.FromRect(100, 500, 300, 300), new GUIContent("1234", new GUITexture() { material = resources.createMaterialFromTexture("content/textures/ship.png") }));
            }
        }

        public ScreenToTileRay findMouseFloorTile()
        {
            var mp = input.MousePosition;
            var wp = mainCamera.screenToWorld(mp);
            var z = currentMapDepth;
            var iso = worldToIso(wp);

            var p = new Vector3i(iso.X, iso.Y, z);
            var tile = getTile(iso.X, iso.Y, z);
            while (iso.Y >= 0 && p.z >= 0)
            {
                if (tile.hasFloor)
                    return new ScreenToTileRay() { block = tile, hit = true, iso = p, objectType = ScreenToTileRayObject.Floor };

                wp -= new Vector2(0, blockSize);
                iso = worldToIso(wp);
                p = new Vector3i(iso.X, iso.Y, p.z - 1);
            }

            return ScreenToTileRay.none;
        }


        public Point mouseWallIso
        {
            get
            {
                var mp = input.MousePosition;
                var wp = mainCamera.screenToWorld(mp + new Vector2(0, blockSize));

                return worldToIso(wp);
            }
        }

        public Point mouseFloorIso
        {
            get
            {
                var mp = input.MousePosition;
                var wp = mainCamera.screenToWorld(mp);

                return worldToIso(wp);
            }
        }

        public Point minDrawableIso
        {
            get
            {
                var min = worldToIso(drawRect.Minimum);

                var minrx = min.X % 2;
                var minry = min.Y % 2;

                var minQuad = getQuadrant(min, drawRect.Minimum);

                if (minry == 0 && (minQuad & TileQuadrant.left) == TileQuadrant.left)
                    min.X--;

                if ((minQuad & TileQuadrant.top) == TileQuadrant.top)
                    min.Y--;

                min.Y -= 2;

                return min;
            }
        }

        public Point maxDrawableIso
        {
            get
            {
                var max = worldToIso(drawRect.Maximum);

                var maxrx = max.X % 2;
                var maxry = max.Y % 2;

                var maxQuad = getQuadrant(max, drawRect.Maximum);

                if (maxry == 1 && (maxQuad & TileQuadrant.right) == TileQuadrant.right)
                    max.X++;

                if ((maxQuad & TileQuadrant.bottom) == TileQuadrant.bottom)
                    max.Y++;

                //max.Y++;

                return max;
            }
        }

        public IEnumerable<Point> drawableIso
        {
            get
            {
                var min = minDrawableIso;
                var max = maxDrawableIso;

                for (var y = min.Y; y <= max.Y; y++)
                {
                    for (var x = min.X; x <= max.X; x++)
                    {
                        if (x == min.X || x == max.X)
                        {
                            var wp = isoToWorld(x, y);
                            if (wp.X + blockSize < drawRect.Minimum.X)
                                continue;
                            else if (x == max.X && wp.X > drawRect.Maximum.X)
                                continue;
                        }

                        yield return new Point(x, y);
                    }
                }
            }
        }

        public Point adjustIsoForDepth(Point p, int depth)
        {
            p.X += depth;
            p.Y += depth;
            return p;
        }

        public Vector2 isoToWorld(int x, int y)
        {
            if ((y & 1) == 1)
                return new Vector2(x * blockSize + (blockSize / 2), y * blockSizeOver4);

            return new Vector2(x * blockSize, y * blockSizeOver4);
        }

        public Vector2 isoToWorld(int x, int y, int depth)
        {
            var p = isoToWorld(x, y);
            return p - new Vector2(0, depth * blockSizeOver2 + floorSizeOver2 * depth);
        }

        public Point screenToIso(Vector2 p)
        {
            var cp = mainCamera.transform.DerivedPosition;
            var adjusted = cp + p;
            var worldIso = IsoHelper.FromDiamondIso(adjusted);
            var iso = worldIso / blockSizeOver2;

            return iso.ToPoint();
        }

        public Point worldToIso(Vector2 p)
        {
            var np = p;
            if (p.Y < 0)
                np.Y = -np.Y;
            if (p.X < 0)
                np.X = -np.X;

            var result = _worldToIso(np);

            if (p.Y < 0)
                result.Y = -result.Y - 2;
            if (p.X < 0)
            {
                if ((result.Y % 2) == 0)
                    result.X = -result.X - 1;
                else
                    result.X = -result.X - 2;
            }

            return result;
        }

        private Point _worldToIso(Vector2 p)
        {
            int ax, ay, bx, by;

            var _xp = (int)p.X;
            var _yp = (int)p.Y;
            var cx = _xp;
            var cy = _yp;
            var posX = (int)Math.Floor(_xp / (float)blockSizeOver2);
            var posY = (int)Math.Floor(_yp / (float)blockSizeOver4);
            if ((posX % 2) == (posY % 2))
            {
                ax = (posX) * blockSizeOver2;
                ay = (posY + 1) * blockSizeOver4;
                bx = (posX + 1) * blockSizeOver2;
                by = (posY) * blockSizeOver4;
                //var offset = getIsoOffset(ax, ay, bx, by, cx, cy);                
                if (getIsoOffset(ax, ay, bx, by, cx, cy) < 0)
                {
                    var r = ((((posY - 1) % 2) == 0) ? 0 : -1);
                    //if (posY < 1)
                    //    r = -r;
                    return new Point((int)Math.Floor(posX / 2f) + r, posY - 1);
                }
                else
                {
                    return new Point((int)Math.Floor(posX / 2f), posY);
                }
            }
            else
            {
                ax = (posX) * blockSizeOver2;
                ay = (posY) * blockSizeOver4;
                bx = (posX + 1) * blockSizeOver2;
                by = (posY + 1) * blockSizeOver4;
                if (getIsoOffset(ax, ay, bx, by, cx, cy) < 0)
                {
                    return new Point((int)Math.Floor(posX / 2f), posY - 1);
                }
                else
                {
                    //if (posY < 0)
                    //    return new Point(((int)Math.Ceiling(posX / 2f) + (((posY - 1) % 2 == 0) ? -1 : 0)), posY);
                    return new Point(((int)Math.Floor(posX / 2f) + (((posY - 1) % 2 == 0) ? -1 : 0)), posY);
                }
            }
        }

        public int getIsoOffset(float ax, float ay, float bx, float by, float cx, float cy)
        {
            // below = 1, above = -1, on = 0;
            var slope = (by - ay) / (bx - ax);
            var yIntercept = ay - ax * slope;
            var cSolution = (slope * cx) + yIntercept;

            if (slope != 0)
            {
                if (cy > cSolution)
                {
                    return bx > ax ? 1 : -1;
                }
                if (cy < cSolution)
                {
                    return bx > ax ? -1 : 1;
                }
                return 0;
            }
            return 0;
        }

        public TileQuadrant getQuadrant(Point iso, Vector2 p)
        {
            var wp = isoToWorld(iso.X, iso.Y) + new Vector2(blockSize / 2, blockSize / 4);
            if (p.X < wp.X)
            {
                if (p.Y < wp.Y)
                    return TileQuadrant.topleft;
                else
                    return TileQuadrant.bottomleft;
            }
            else
            {
                if (p.Y < wp.Y)
                    return TileQuadrant.topright;
                else
                    return TileQuadrant.bottomright;
            }

        }

        //public MapChunk getMapChunkFromIso(Vector3i iso)
        //{
        //    var mcp = iso / mapChunkSize;
        //    var mcw = mapWidth / mapChunkSize;
        //    var mch = mapHeight / mapChunkSize;
        //    var mcd = mapDepth / mapChunkSize;
        //}

    }

}

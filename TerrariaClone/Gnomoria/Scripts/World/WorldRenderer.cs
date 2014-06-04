using GameName1.Gnomoria.Scripts.Tiles;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using zSprite;
using zSprite.Resources;
using Microsoft.Xna.Framework.Input;
using GameName1.Gnomoria.Scripts.Tools;
using GameName1.Gnomoria.Scripts.WorldRendering;

namespace GameName1.Gnomoria.Scripts.World
{
    public struct WorldRenderItem
    {
        public Material material;
        public Color color;
        public AxisAlignedBox srcRect;
        public AxisAlignedBox dstRect;
    }

    public enum ScreenToTileRayObject
    {
        Floor,
        Placable
    }

    public struct ScreenToTileRay
    {
        public bool hit;
        public Vector3i iso;
        public ScreenToTileRayObject objectType;
        public MapTile block;

        public static ScreenToTileRay none { get { return new ScreenToTileRay() { hit = false }; } }
    }

    public class WorldRenderer : Script
    {
        public const int WORLD_RENDER_QUEUE = 8;
        private WorldManager world;
        private ToolManager tm;

        private List<Point>[] blocksToRender;

        public WorldSpriteSheet spriteSheet = new WorldSpriteSheet();
        public Material spriteSheetMaterial;
        private WorldObjectRenderer[] worldObjectRenderers;
        private WorldRenderLevel[] renderGraph;
        private int loopcount = 0;

        public List<WorldRenderItem>[] renderItems;

        private void buildGraph()
        {
            for (var x = 0; x < world.mapWidth; x++)
                for (var c = 0; c < world.mapHeight + world.mapDepth * 2; c++)
                    buildGraph(x, c);
        }

        public int ConvertWorldYToScreenY(int y, int z)
        {
            return y + (world.mapDepth - z) * 2;
        }

        public int ConvertScreenYToWorldY(int c, int z)
        {
            return 2 * z - 2 * world.mapDepth + c;
        }

        public int GetZFromWorldAndScreenY(int y, int c)
        {
            return (y + 2 * world.mapDepth - c) / 2;
        }

        private void buildGraph(int x, int c)
        {
            /* c = y + (d - z) * 2 
             * z = (y+2*d-c)/2
             * y = 2*z-2*d+c
             * (y=0, z=1) = 0 + (1 - 1) * 2 = 0
             * (y=0, z=0) = 0 + (1 - 0) * 2 = 2
             * (y=1, z=1) = 1 + (1 - 1) * 2 = 1
             * (y=1, z=0) = 1 + (1 - 0) * 2 = 3
             * 
             * (y=2, z=1) = 2 + (1 - 1) * 2 = 2
             * 
             * y = 2*z-2*d+c
             * y = 2*1-2*1+2
             * 
             */

            var d = world.mapDepth;
            //var maxZ = Utility.Min(world.mapDepth, (y + 2 * d - (world.mapHeight - 1)) / 2);


            var graph = renderGraph[x];
            graph.Clear(c);

            var current = -1;
            //for (var z = 0; z < world.mapDepth; z++)
            for (var z = 0; z < d; z++)
            {
                // c = y + (d - z) * 2 
                //var c = cc + (d - z) * 2;
                //if (c < 0)
                //    break;

                //   y = 2*z-2*d+c
                var y = 2 * z - 2 * d + c;
                if (y >= 0 && y < world.mapHeight)// y < world.mapHeight + world.mapDepth * 2)
                {

                    var block = world.getTile(x, y, z);
                    if (block.hasFloor)
                    {
                        if (current == -1)
                            current = z;
                    }
                    else if (current != -1)
                    {
                        graph.AddSegment(c, new WorldRenderSegment(current, z - 1));
                        current = -1;
                    }
                }
                else if (current != -1)
                {
                    graph.AddSegment(c, new WorldRenderSegment(current, z - 1));
                    current = -1;
                }

            }

            if (current != -1)
                graph.AddSegment(c, new WorldRenderSegment(current, d - 1));

        }

        private void init()
        {
            graphics.setSortOrder(WORLD_RENDER_QUEUE, RenderQueueSortMode.PreserverOrder);
            world = Root.instance.RootObject.getScript<WorldManager>();
            tm = rootObject.find("tools").getScript<ToolManager>();

            worldObjectRenderers = new WorldObjectRenderer[256];

            spriteSheetMaterial = resources.createMaterialFromTexture("content/textures/gnomoria.png");
            spriteSheetMaterial.SetSamplerState(SamplerState.PointClamp);

            #region Load Sprite Sheet
            spriteSheet.addSprite("dirtfloor", new WorldSprite(AxisAlignedBox.FromRect(0, 52, 32, 20), 32, 20, 0, 0));
            spriteSheet.addSprite("dirtwall", new WorldSprite(AxisAlignedBox.FromRect(0, 72, 32, 32), 32, 32, 0, -16));
            spriteSheet.addSprite("floorselection", new WorldSprite(AxisAlignedBox.FromRect(0, 0, 32, 20), 32, 20, 0, 0));
            spriteSheet.addSprite("wallselection", new WorldSprite(AxisAlignedBox.FromRect(0, 20, 32, 32), 32, 32, 0, -16));

            worldObjectRenderers[(int)WorldObjectType.DirtFloor] = new SpriteRenderer(this, "dirtfloor", new Color(0x96, 0x63, 0x30));
            worldObjectRenderers[(int)WorldObjectType.LightDirtFloor] = new SpriteRenderer(this, "dirtfloor", new Color(0xB4, 0x81, 0x4E));
            worldObjectRenderers[(int)WorldObjectType.DarkDirtFloor] = new SpriteRenderer(this, "dirtfloor", new Color(0x78, 0x45, 0x30));

            worldObjectRenderers[(int)WorldObjectType.ClayFloor] = new SpriteRenderer(this, "dirtfloor", new Color(0x66, 0x33, 0x00));
            worldObjectRenderers[(int)WorldObjectType.LightClayFloor] = new SpriteRenderer(this, "dirtfloor", new Color(0x84, 0x1e, 0x51));
            worldObjectRenderers[(int)WorldObjectType.DarkClayFloor] = new SpriteRenderer(this, "dirtfloor", new Color(0x48, 0x00, 0x15));

            worldObjectRenderers[(int)WorldObjectType.DirtWall] = new SpriteRenderer(this, "dirtwall", new Color(0x96, 0x63, 0x30));
            //worldObjectRenderers[(int)WorldObjectType.LightDirtWall] = new SpriteRenderer(this, "dirtwall", new Color(0xB4, 0x81, 0x4E));
            //worldObjectRenderers[(int)WorldObjectType.DarkDirtWall] = new SpriteRenderer(this, "dirtwall", new Color(0x78, 0x45, 0x30));

            //worldObjectRenderers[(int)WorldObjectType.ClayWall] = new SpriteRenderer(this, "dirtwall", new Color(0x66, 0x33, 0x00));
            //worldObjectRenderers[(int)WorldObjectType.LightClayWall] = new SpriteRenderer(this, "dirtwall", new Color(0x84, 0x1e, 0x51));
            //worldObjectRenderers[(int)WorldObjectType.DarkClayWall] = new SpriteRenderer(this, "dirtwall", new Color(0x48, 0x00, 0x15));

            worldObjectRenderers[(int)WorldObjectType.FloorSelection] = new SpriteRenderer(this, "floorselection", new Color(0x0, 0x88, 0x88));
            worldObjectRenderers[(int)WorldObjectType.WallSelection] = new SpriteRenderer(this, "wallselection", new Color(0x0, 0x88, 0x88));
            #endregion
        }

        private void maploaded()
        {
            renderGraph = new WorldRenderLevel[world.mapWidth];
            for (var x = 0; x < world.mapWidth; x++)
                renderGraph[x] = new WorldRenderLevel(x, world.mapHeight, world.mapDepth);

            buildGraph();
            //renderGraphLookup = new Point[world.mapWidth, world.mapHeight];

            //for (var y = 0; y < world.mapHeight; y++)
            //{
            //    for (var z = world.mapDepth - 1; z >= 0; z--)
            //    {

            //    }
            //}

            renderItems = new List<WorldRenderItem>[world.mapDepth];
            blocksToRender = new List<Point>[world.mapDepth];
            for (var i = 0; i < world.mapDepth; i++)
            {
                blocksToRender[i] = new List<Point>();
                renderItems[i] = new List<WorldRenderItem>(256);
            }
        }

        private void update()
        {
            if (world.isLoaded)
            {
                var min = world.minDrawableIso;
                var max = world.maxDrawableIso;

                updateRenderableList(min, max);
            }
        }

        private IEnumerable<Point> renderableGrid
        {
            get
            {
                var min = world.minDrawableIso;
                var max = world.maxDrawableIso;

                min.X = Utility.Max(min.X, 0);
                max.X = Utility.Min(max.X, world.mapWidth - 1);
                min.Y = Utility.Max(min.Y, 0);
                max.Y = Utility.Min(max.Y, (world.mapHeight + world.mapDepth * 2) - 1);

                //for (var y = max.Y; y >= min.Y; y--)
                for (var y = min.Y; y <= max.Y; y++)
                {
                    for (var x = max.X; x >= min.X; x--)
                        yield return new Point(x, y);

                    //for (var x = max.X; x >= min.X; x -= 2)
                    //    yield return new Point(x, y);

                    //for (var x = max.X - 1; x >= min.X; x -= 2)
                    //    yield return new Point(x, y);

                }
            }
        }

        public Vector3i fadetarget;
        public bool fade = false;

        private void updateRenderableList(Point min, Point max)
        {
            fade = false;
            var tool = tm.getCurrentTool();
            tool.startRendering();
            loopcount = 0;

            //min.X = Utility.Max(min.X, 0);
            //max.X = Utility.Min(max.X, world.mapWidth - 1);
            //min.Y = Utility.Max(min.Y, 0);
            //max.Y = Utility.Min(max.Y, (world.mapHeight + world.mapDepth * 2) - 1);

            for (var z = 0; z < world.mapDepth; z++)
            {
                blocksToRender[z].Clear();
                renderItems[z].Clear();
            }

            foreach (var xy in renderableGrid)
            //            for (var y = max.Y; y >= min.Y; y--)
            {
                var xx = xy.X;
                var yy = xy.Y;
                var zz = world.currentMapDepth;

                //var x = xx;
                //var y = yy;
                for (var i = 0; i < 4; i++)
                {
                    var y = yy - i * 2;
                    var x = xx;
                    var z = zz - i;

                    while (y >= 0 && z >= 0)
                    {
                        //var yy = Utility.Min(world.mapHeight - 1, max.Y - (world.currentMapDepth - z) * 2);
                        //for (var x = min.X; x <= max.X; x++)
                        //                for (var x = max.X; x >= min.X; x--)
                        //                var graph = renderGraph[x];

                        //foreach (var cp in screenIso(x, y, world.currentMapDepth))
                        ////for (var z = world.currentMapDepth; z >= 0; z--)
                        //{
                        loopcount++;

                        //    x = cp.x;
                        //    y = cp.y;
                        //    var z = cp.z;

                        //var c = y + (world.mapDepth - z) * 2;
                        var block = world.getTile(x, y, z);

                        if (block.hasFloor || block.hasPlaceable || block.objects != 0)
                        {
                            var p = new Point(x, y);
                            //if (block.hasFloor)
                            //    RenderObject(block.floorType, p.X, p.Y, z, 1f);

                            tool.renderFloor(block, p.X, p.Y, z);

                            //if (block.hasPlaceable)
                            //{
                            //    var po = world.placableObjects[block.placedId];
                            //    if (po != null)
                            //        po.render(this, block, p.X, p.Y, z, 1f);
                            //}

                            tool.renderPlaceable(block, p.X, p.Y, z);

                            if (block.objects != 0)
                            {
                                var objects = world.worldObjectListPool[block.objects - 1];
                                foreach (var o in objects)
                                    o.Render(this, p.X, p.Y, z, 1f);
                            }


                        }

                        if (block.hasPlaceable)
                            break;

                        y -= 10;
                        z -= 4;
                    }

                    //continue;
                    //WorldRenderSegment seg;
                    //if (graph.findClosest(y, world.currentMapDepth, out seg))
                    //{


                    //    //for (var z = Utility.Min(seg.max, world.currentMapDepth); z <= world.currentMapDepth; z++)
                    //    {
                    //        loopcount++;
                    //        blocksToRender[z].Add(new Point(x, graph.getY(y, z)));
                    //    }

                    //    //for (int z = world.currentMapDepth; z >= 0; z--)
                    //    //{
                    //    //    loopcount++;

                    //    //    var depthCorrectedY = y - (world.currentMapDepth - z) * 2;
                    //    //    if (depthCorrectedY < 0)
                    //    //        break;

                    //    //    if (depthCorrectedY >= 0 && depthCorrectedY <= world.mapHeight - 1 /*&& !blocked[x - min.X, depthCorrectedY - min.Y]*/)
                    //    //    {
                    //    //        {
                    //    //            var index = Arrays.index3(world.mapWidth, world.mapHeight, x, depthCorrectedY, z);
                    //    //            var block = world.blocks[index];
                    //    //            //if (block.hasFloor)
                    //    //            //{
                    //    //            //    blocksToRender[z].Add(new Point(x, depthCorrectedY));
                    //    //            //}

                    //    //            //tool.renderFloor(block, x, depthCorrectedY, z);



                    //    //            if (block.objects != 0)
                    //    //            {
                    //    //                var objects = world.worldObjectListPool[block.objects - 1];
                    //    //                foreach (var o in objects)
                    //    //                    o.Render(this, x, depthCorrectedY, z, 1f);
                    //    //            }

                    //    //            tool.renderPlaceable(block, x, depthCorrectedY, z);

                    //    //            if (block.hasPlaceable)
                    //    //            {
                    //    //                var po = world.placableObjects[block.placedId];
                    //    //                if (po != null)
                    //    //                    po.render(th    is, block, x, depthCorrectedY, z, 1f);
                    //    //            }


                    //    //            tool.renderFloor(block, x, depthCorrectedY, z);

                    //    //            if (block.hasFloor)
                    //    //            {
                    //    //                RenderObject(block.floorType, x, depthCorrectedY, z, 1f);
                    //    //                //graph.getOverlaps


                    //    //            }
                    //    //            //if (block.hasPlaceable)
                    //    //            //    blocked[x - min.X, depthCorrectedY - min.Y] = true;

                    //    //        }
                    //    //    }
                    //    //}
                    //}
                }
            }
        }

        private void ongui()
        {
            var tool = tm.getCurrentTool();
            if (tool != null)
                tool.doGui();

            gui.label(new Vector2(300, 300), loopcount.ToString());
            gui.label(new Vector2(300, 320), renderCount.ToString());
        }

        private int renderCount = 0;
        private void renderRenderableList(/*Point min, Point max*/)
        {
            //var min = world.minDrawableIso;
            //var max = world.maxDrawableIso;

            renderCount = 0;
            //updateRenderableList(min, max);
            //return;
            //var tool = tm.getCurrentTool();
            //tool.startRendering();

            //world.selectedTiles.Clear();

            //var misoWall = world.mouseWallIso;
            //var misoFloor = world.mouseFloorIso;
            //var worldStart = world.isoToWorld(world.startIsoSelection.X, world.startIsoSelection.Y) / new Vector2(world.blockSize, world.blockSizeOver2);
            //var worldEnd = world.isoToWorld(world.mouseWallIso.X, world.mouseWallIso.Y) / new Vector2(world.blockSize, world.blockSizeOver2);
            //var diamondStart = IsoHelper.ToDiamondIso(worldStart);
            //var diamondEnd = IsoHelper.ToDiamondIso(worldEnd);

            //var fade = false;
            for (var z = 0; z <= world.currentMapDepth; z++)
            {
                var items = renderItems[z];
                //for (int i = items.Count - 1; i >= 0; i--)
                for (var i = 0; i < items.Count; i++)
                {
                    var c = items[i].color;
                    if (fade)
                    {
                        c.A = Utility.Max((byte)(c.A - 128), (byte)0);
                    }

                    graphics.Draw(items[i].material, items[i].dstRect, items[i].srcRect, c.Subtract((world.currentMapDepth - z) * 4));
                }
                //continue;
                //var blocks = blocksToRender[z];
                //for (int i = blocks.Count - 1; i >= 0; i--)
                //{

                //    renderCount++;
                //    var p = blocks[i];
                //    var index = Arrays.index3(world.mapWidth, world.mapHeight, p.X, p.Y, z);
                //    var block = world.blocks[index];

                //    if (block.hasFloor)
                //        RenderObject(block.floorType, p.X, p.Y, z, 1f);

                //    tool.renderFloor(block, p.X, p.Y, z);

                //    if (block.hasPlaceable)
                //    {
                //        var po = world.placableObjects[block.placedId];
                //        if (po != null)
                //            po.render(this, block, p.X, p.Y, z, fade ? 0.5f : 1f);
                //    }

                //    tool.renderPlaceable(block, p.X, p.Y, z);

                //    if (block.objects != 0)
                //    {
                //        var objects = world.worldObjectListPool[block.objects - 1];
                //        foreach (var o in objects)
                //            o.Render(this, p.X, p.Y, z, 1f);
                //    }


                //    //if (!world.isSelecting && world.currentMapDepth == z)
                //    //{
                //    //    if (input.IsKeyDown(Keys.LeftShift) && misoFloor.X == p.X && misoFloor.Y == p.Y && block.hasFloor)
                //    //    {
                //    //        RenderObject(WorldObjectType.FloorSelection, p.X, p.Y, z, 0.5f);

                //    //        fade = true;
                //    //    }
                //    //}

                //    //if (block.hasPlaceable)
                //    //{
                //    //    var po = world.placableObjects[block.placedId];
                //    //    if(po != null)
                //    //        po.render(this, block, p.X, p.Y, z, fade ? 0.5f : 1f);
                //    //    //RenderObject(block.placedType, p.X, p.Y, z, fade ? 0.5f : 1f);
                //    //}
                //    //}

                //    //for (int i = blocksToRender[z].Count - 1; i >= 0; i--)
                //    //{
                //    //    var p = blocksToRender[z][i];
                //    //    var index = Arrays.index3(world.mapWidth, world.mapHeight, p.X, p.Y, z);
                //    //    var block = world.blocks[index];

                //    ////if (world.isSelecting && world.currentMapDepth == z)
                //    ////{
                //    ////    var worldPoint = world.isoToWorld(p.X, p.Y) / new Vector2(world.blockSize, world.blockSizeOver2);
                //    ////    var diamondPoint = IsoHelper.ToDiamondIso(worldPoint);

                //    ////    if (Utility.BetweenOrEq(diamondPoint.X, diamondStart.X, diamondEnd.X) &&
                //    ////        Utility.BetweenOrEq(diamondPoint.Y, diamondStart.Y, diamondEnd.Y))
                //    ////    {
                //    ////        RenderObject(WorldObjectType.WallSelection, p.X, p.Y, z, 1f);
                //    ////        world.selectedTiles.Add(new Vector3i(p.X, p.Y, z));
                //    ////    }
                //    ////}
                //    ////else if (!world.isSelecting && world.currentMapDepth == z)
                //    ////{
                //    ////    if (!input.IsKeyDown(Keys.LeftShift) && misoWall.X == p.X && misoWall.Y == p.Y && block.hasPlaceable)
                //    ////        RenderObject(WorldObjectType.WallSelection, p.X, p.Y, z, 0.5f);
                //    ////    if (!world.isSelecting && world.currentMapDepth == z)
                //    ////    {
                //    ////        if (input.IsKeyDown(Keys.LeftShift) && misoFloor.X == p.X && misoFloor.Y == p.Y && block.hasFloor)
                //    ////        {
                //    ////            RenderObject(WorldObjectType.FloorSelection, p.X, p.Y, z, 0.5f);

                //    ////        }
                //    ////    }
                //    ////    //if (block.hasFloor)
                //    ////    //    RenderObject(WorldObjectType.FloorSelection, p.X, p.Y, z);
                //    ////}
                //    //}

                //    //for (int i = blocksToRender[z].Count - 1; i >= 0; i--)
                //    //{
                //    //    var p = blocksToRender[z][i];
                //    //    var index = Arrays.index3(world.mapWidth, world.mapHeight, p.X, p.Y, z);
                //    //    var block = world.blocks[index];

                //}

                blocksToRender[z].Clear();
            }

            //tool.endRendering();

            //var min = world.minDrawableIso;
            //var max = world.maxDrawableIso;


            //gui.skin.label.fontSize = 0.40f;
            //gui.skin.label.alignment = GUIAnchor.MiddleCenter;
            //for(var x =min.X; x <= max.X;x++)
            //    for (var y = min.Y; y <= max.Y; y++)
            //    {
            //        var rp = world.isoToWorld(x, y, world.currentMapDepth) + new Vector2(0, world.currentMapDepth * (world.blockSizeOver2 + world.floorSizeOver2));
            //        var c = y + (world.mapDepth - world.currentMapDepth) * 2;
            //        graphics.DrawRect(AxisAlignedBox.FromRect(rp, world.blockSize, world.blockSizeOver2), Color.Red);
            //        gui.label2(AxisAlignedBox.FromRect(rp, world.blockSize, world.blockSizeOver2), c);
            //    }
            //gui.skin.label.fontSize = 1f;

        }

        private void render()
        {
            if (world.isLoaded)
            {
                graphics.pushRenderQueue(WORLD_RENDER_QUEUE);

                renderRenderableList();

                drawOcclussion();
                graphics.popRenderQueue();
            }
        }

        private void drawOcclussion()
        {
            var worldBounds = mainCamera.worldBounds;

            var left = AxisAlignedBox.FromRect(
              worldBounds.Minimum,
              new Vector2(world.drawRect.Minimum.X - worldBounds.Minimum.X, worldBounds.Height));

            graphics.Draw(left, new Color(0f, 0f, 0f, 0.25f));

            var right = AxisAlignedBox.FromRect(
                new Vector2(world.drawRect.Maximum.X, worldBounds.Minimum.Y),
                new Vector2(worldBounds.Maximum.X - world.drawRect.Maximum.X, worldBounds.Height));

            graphics.Draw(right, new Color(0f, 0f, 0f, 0.25f));

            var top = AxisAlignedBox.FromRect(
                new Vector2(world.drawRect.Minimum.X, worldBounds.Minimum.Y),
                new Vector2(world.drawRect.Width, world.drawRect.Minimum.Y - worldBounds.Minimum.Y));

            graphics.Draw(top, new Color(0f, 0f, 0f, 0.25f));

            var bottom = AxisAlignedBox.FromRect(
                new Vector2(world.drawRect.Minimum.X, world.drawRect.Maximum.Y),
                new Vector2(world.drawRect.Width, worldBounds.Maximum.Y - world.drawRect.Maximum.Y));

            graphics.Draw(bottom, new Color(0f, 0f, 0f, 0.25f));
            graphics.DrawRect(world.drawRect, Color.Wheat);
        }

        public void RenderFloor()
        {

        }

        public void RenderObject(WorldObjectType objectType, int x, int y, int z, float alpha)
        {
            var p = world.isoToWorld(x, y, z) + new Vector2(0, world.currentMapDepth * (world.blockSizeOver2 + world.floorSizeOver2));

            var renderer = (worldObjectRenderers[(int)objectType] ?? worldObjectRenderers[(int)WorldObjectType.Missing]);
            renderer.Render(this, p, z, alpha);
        }

        public void queueRender(WorldSprite sprite, int x, int y, int z, Color color)
        {
            var p = world.isoToWorld(x, y, z) + new Vector2(0, world.currentMapDepth * (world.blockSizeOver2 + world.floorSizeOver2));

            var item = new WorldRenderItem();
            item.color = color;
            item.dstRect = sprite.getDrawRect(p);
            item.srcRect = sprite.textureSource;
            item.material = spriteSheetMaterial;

            var items = renderItems[z];
            items.Add(item);
        }

        public IEnumerable<Vector3i> screenIso2(int x, int y, int z)
        {
            while (y >= 0 && z >= 0)
            {
                yield return new Vector3i(x, y, z);

                y -= 10;
                z -= 4;
            }
        }

        public IEnumerable<Vector3i> screenIso(int x, int y, int z)
        {
            var d = world.mapDepth;
            for (var zz = z; zz >= 0; zz--)
            {
                var c = y - (z - zz) * 2;
                yield return new Vector3i(x, c, zz);
            }
        }

        public ScreenToTileRay screenToTile(Vector2 wp)
        {
            var p = world.worldToIso(wp);
            var currentDepth = world.currentMapDepth;
            var d = world.mapDepth;

            foreach (var iso in screenIso(p.X, p.Y, world.currentMapDepth))
            {
                //var p = world.isoToWorld(x, y, z) + new Vector2(0, world.currentMapDepth * (world.blockSizeOver2 + world.floorSizeOver2));

                {
                    var brp = new Vector3i(iso.x, iso.y + 2, iso.z);
                    var br = world.getTile(brp.x, brp.y, brp.z);
                    if (br.hasPlaceable)
                        return new ScreenToTileRay() { hit = true, objectType = ScreenToTileRayObject.Placable, iso = brp, block = br };
                }

                var quadrant = world.getQuadrant(new Point(iso.x, iso.y), wp);
                if ((quadrant & TileQuadrant.right) == TileQuadrant.right)
                {
                    var brp = new Vector3i(iso.x, iso.y + 1, iso.z);
                    var br = world.getTile(brp.x, brp.y, brp.z);
                    if (br.hasPlaceable)
                        return new ScreenToTileRay() { hit = true, objectType = ScreenToTileRayObject.Placable, iso = brp, block = br };
                }

                if ((quadrant & TileQuadrant.left) == TileQuadrant.left)
                {
                    var brp = new Vector3i(iso.x - 1, iso.y + 1, iso.z);
                    var br = world.getTile(brp.x, brp.y, brp.z);
                    if (br.hasPlaceable)
                        return new ScreenToTileRay() { hit = true, objectType = ScreenToTileRayObject.Placable, iso = brp, block = br };
                }
                {
                    var block = world.getTile(iso.x, iso.y, iso.z);
                    if (block.hasPlaceable)
                        return new ScreenToTileRay() { hit = true, objectType = ScreenToTileRayObject.Placable, iso = iso, block = block };
                    else if (block.hasFloor)
                        return new ScreenToTileRay() { hit = true, objectType = ScreenToTileRayObject.Floor, iso = iso, block = block };
                }
            }
            return ScreenToTileRay.none;
        }
    }
}

using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using zSprite;
using zSprite.AI.Pathfinding;
using GameName1.Gnomoria.Scripts.World;
using zSprite.Collections;

namespace GameName1.Gnomoria.Scripts.Navigation
{

    public class NavManager : Script
    {
        private WorldManager world;
        public NavLevel[] navGraph;

        //private int navStepsPerFrame = 200;

        //private ObjectPool<NavGraphAstar> astarPool = new ObjectPool<NavGraphAstar>();

        private void init()
        {
            world = Root.instance.RootObject.getScript<WorldManager>();
        }

        private void maploaded()
        {
            navGraph = new NavLevel[world.mapDepth];
            for (int i = 0; i < navGraph.Length; i++)
            {
                navGraph[i] = new NavLevel(world.mapWidth, world.mapHeight, i);
                if (i > 0)
                {
                    navGraph[i - 1].up = navGraph[i];
                    navGraph[i].down = navGraph[i - 1];
                }
            }

            buildGraph();
        }

        private void update()
        {
            if (world.isLoaded)
            {

            }
        }

        private void ongui()
        {

            if (world.isLoaded)
            {
                //var size = new Vector2(48, 48);
                //var offset = new Vector2(200, 200);

                //for (var y = 0; y < 8; y++)
                //    for (var x = 0; x < 8; x++)
                //    {
                //        var color = Color.Gray;
                //        if (x == start.x && y == start.y)
                //            color = Color.Green;
                //        else if (x == end.x && y == end.y)
                //            color = Color.Red;
                //        else if (data[x, y])
                //            color = Color.Black;
                //        else
                //        {
                //            var cell = astar.getCell(new Vector3i(x, y, 0));
                //            if (cell.HasValue)
                //            {
                //                if (cell.Value.closed)
                //                    color = Color.Teal;
                //                else if (cell.Value.visited)
                //                    color = Color.Blue;
                //                else
                //                    color = Color.Gray;
                //            }
                //            //var partition = getPartition(x, y);
                //            //color = partition.color;
                //        }

                //        var aabb = AxisAlignedBox.FromRect(new Vector2(x, y) * size + offset, size);
                //        graphics.Draw(aabb, color);

                //        graphics.DrawRect(aabb, Color.Purple);
                //    }
            }
        }

        private void buildGraph()
        {
            for (int d = 0; d < world.mapDepth; d++)
                buildGraph(d);
        }

        private void buildGraph(int depth)
        {
            buildGraph(depth, GraphDirection.X);
            buildGraph(depth, GraphDirection.Y);
        }

        private void buildGraph(int depth, GraphDirection dir)
        {
            buildGraph(depth, dir, -1);
        }

        private void buildGraph(int depth, GraphDirection dir, int index)
        {
            var iSize = world.mapWidth;
            var jSize = world.mapHeight;

            if (dir == GraphDirection.Y)
                Utility.Swap(ref iSize, ref jSize);

            var iStart = 0;
            if (index != -1)
            {
                iStart = index;
                iSize = index + 1;
            }

            for (int i = iStart; i < iSize; i++)
            {
                if (dir == GraphDirection.X)
                    navGraph[depth].ClearX(i);
                else
                    navGraph[depth].ClearY(i);

                var hasUp = false;
                var hasDown = false;
                var current = -1;
                for (int j = 0; j < jSize; j++)
                {
                    var block = dir == GraphDirection.X ? world.getTile(i, j, depth) : world.getTile(j, i, depth);
                    //var desc = block.wallDescriptor;
                    if (block.isWalkable)
                    {
                        //if(desc.AllowsMovementUp)
                        if (current == -1)
                            current = j;
                    }
                    else if (current != -1)
                    {
                        if (dir == GraphDirection.X)
                            navGraph[depth].AddSegmentX(current, new NavSegment(current, j - 1));
                        else
                            navGraph[depth].AddSegmentY(current, new NavSegment(current, j - 1));

                        current = -1;
                        hasUp = false;
                        hasDown = false;
                    }
                }

                if (current != -1)
                {
                    if (dir == GraphDirection.X)
                        navGraph[depth].AddSegmentX(current, new NavSegment(current, jSize - 1));
                    else
                        navGraph[depth].AddSegmentY(current, new NavSegment(current, jSize - 1));
                }
            }
        }

        private Color[] colorSampler = new Color[]
        {
            Color.LightBlue,
            Color.LightGreen,
            Color.LightYellow,
            Color.DarkBlue,
            //Color.DarkGreen,
            Color.Yellow,
            Color.YellowGreen,
            Color.MistyRose,
            Color.Orange,
            Color.Pink,
            Color.PeachPuff,
            Color.Turquoise,
            Color.Gold,
            Color.Brown
        };

        private void render()
        {

        }
    }
}

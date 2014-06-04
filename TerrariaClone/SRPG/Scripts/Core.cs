using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using zSprite;
using zSprite.Collections;

namespace GameName1.SRPG.Scripts
{
    public class Core : Script
    {
        public int x, y;

        private Map map;
        private AStar pathing;

        public int speed = 3;
        private int moveDistance = 3;
        private bool showMovement = true;


        private void init()
        {
            map = Root.instance.RootObject.getScript<Map>();
            pathing = new AStar(map);
        }

        private void moveTo(int mx, int my)
        {
            //var p0 = new Point(x, y);
            //var p1 = new Point(mx, my);
            //pathing.init((p0.X << 16) + p0.Y, (p1.X << 16) + p1.Y);
            //pathing.step(100);
            //pathing.init(x, y);
            //            pathing.step
        }

        private void update()
        {
            var mp = map.worldToMap(gameObject.transform.DerivedPosition);
            x = mp.X;
            y = mp.Y;
        }

        private void render()
        {
            if (showMovement)
            {
                var visited = new HashSet<int>();
                var openlist = new PriorityQueue<int, int>();
                var moves = new List<KeyValuePair<int, int>>();
                var start = map.getTileId(new Point(x, y));

                visited.Add(start);
                openlist.Enqueue(0, start);

                while (openlist.Count > 0)
                {
                    var next = openlist.Dequeue();
                    var p = map.getTilePoint(next.Value);

                    var neighbors = map.getNeighborIds(p);
                    foreach (var n in neighbors)
                    {
                        var np = map.getTilePoint(n);
                        if (!visited.Contains(n) && map.getTile(np.X, np.Y).Movable)
                        {
                            var move = next.Key + 1;
                            if (move < moveDistance)
                                openlist.Enqueue(move, n);

                            moves.Add(new KeyValuePair<int, int>(n, move));
                            visited.Add(n);
                        }
                    }
                }

                foreach (var m in moves)
                {
                    var p = map.getTilePoint(m.Key);
                    var aabb = map.getTileRect(p.X, p.Y);
                    //var mp = map.mapToWorld(p) - new Vector2(Map.tileSize, Map.tileSize) / 2;
                    //var aabb = AxisAlignedBox.FromRect(mp, Map.tileSize, Map.tileSize);

                    Root.instance.graphics.Draw( aabb, new Color(Color.Blue, 0.25f), 1);
                }
            }
        }
    }
}

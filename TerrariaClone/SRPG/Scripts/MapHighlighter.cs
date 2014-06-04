using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using zSprite;
using zSprite.Collections;

namespace GameName1.SRPG.Scripts
{
    public class MapHighlighter : Script
    {
        public int mx, my;

        private Map map;

        public Color color = Color.White;
        private int moveDistance = 3;
        private bool show = true;

        private void showMapHighlight(int x, int y, int l, Color c)
        {
            show = true;
            mx = x;
            my = y;
            moveDistance = l;
            color = c;
        }

        private void clearMapHighlight()
        {
            show = false;
        }

        private void init()
        {
            map = Root.instance.RootObject.getScript<Map>();
        }

        private void render()
        {
            if (show)
            {
                var visited = new HashSet<int>();
                var openlist = new PriorityQueue<int, int>();
                var moves = new List<KeyValuePair<int, int>>();
                var start = map.getTileId(new Point(mx, my));

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
                    var mp = map.mapToWorld(p) - new Vector2(Map.tileSize, Map.tileSize) / 2;
                    var aabb = AxisAlignedBox.FromRect(mp, Map.tileSize, Map.tileSize);

                    Root.instance.graphics.Draw(0, aabb, new Color(Color.Blue, 0.25f), 1);
                }
            }
        }
    }
}

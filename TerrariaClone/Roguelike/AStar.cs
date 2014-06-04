using GameName1.Roguelike.Scripts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using zSprite.AI.Pathfinding;

namespace GameName1.Roguelike
{
    public class AStar : AStar2
    {
        private Map map = null;
        public AStar(Map map)
        {
            this.map = map;
        }

        protected override KeyValuePair<int, int> getScore(int src, int dst)
        {
            var x1 = src >> 16;
            var y1 = src & 0xffff;
            var x2 = dst >> 16;
            var y2 = dst & 0xffff;

            return new KeyValuePair<int, int>((Math.Abs(x1 - x2) + Math.Abs(y1 - y2)) * 10, 10);
        }

        protected override void getNeighbors(int src, List<int> moves)
        {
            var x = src >> 16;
            var y = src & 0xffff;

            if (x > 0)
                processDirection(x - 1, y, moves);
            if (y > 0)
                processDirection(x, y - 1, moves);
            if (x < 0xf)
                processDirection(x + 1, y, moves);
            if (y < 0xf)
                processDirection(x, y + 1, moves);

            //var x = src >> 16;
            //var y = src & 0xffff;

            //if (x > 0)
            //    moves.Add(((x - 1) << 16) + y);
            //if (y > 0)
            //    moves.Add((x << 16) + y - 1);
            //if (x < 0xf)
            //    moves.Add(((x + 1) << 16) + y);
            //if (y < 0xf)
            //    moves.Add((x << 16) + y + 1);
        }

        private void processDirection(int x, int y, List<int> moves)
        {
            var tile = map.getTile(x, y);
            if (tile.Movable)
                moves.Add((x << 16) + y);
        }

    }
}

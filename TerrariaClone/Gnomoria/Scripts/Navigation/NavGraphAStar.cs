using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using zSprite.AI.Pathfinding;

namespace GameName1.Gnomoria.Scripts.Navigation
{
    public class NavGraphAstar : GenericAStar<Vector3i>
    {
        public NavManager nav;
        protected override GenericAStarScore getScore(Vector3i src, Vector3i dst)
        {
            return new GenericAStarScore() { g = 10, h = (Math.Abs(dst.z - src.z) + Math.Abs(dst.x - src.x) + Math.Abs(dst.y - src.y)) * 10 };
        }

        protected override void getNeighbors(Vector3i src, List<Vector3i> moves)
        {
            var level = nav.navGraph[src.z];
            foreach (var segmentOverlap in level.getAdjacentSegments(src.x, src.y))
            {
                if (segmentOverlap.direction == GraphDirection.X)
                {
                    var y = Utility.Clamp(this.end.y, segmentOverlap.segment.max, segmentOverlap.segment.min);
                    moves.Add(new Vector3i(segmentOverlap.plane, y, segmentOverlap.depth));
                }
                else
                {
                    var x = Utility.Clamp(this.end.x, segmentOverlap.segment.max, segmentOverlap.segment.min);
                    moves.Add(new Vector3i(x, segmentOverlap.plane, segmentOverlap.depth));
                }
            }

            //foreach (var p in nav.getNeighborsLeft(src.X, src.Y))
            //{
            //    var y = Utility.Clamp(this.end.Y, p.max, p.min);
            //    moves.Add(new Point(src.X - 1, y));
            //}

            //foreach (var p in nav.getNeighborsRight(src.X, src.Y))
            //{
            //    var y = Utility.Clamp(this.end.Y, p.max, p.min);
            //    moves.Add(new Point(src.X + 1, y));
            //}

            //foreach (var p in nav.getNeighborsNorth(src.X, src.Y))
            //{
            //    var x = Utility.Clamp(this.end.X, p.max, p.min);
            //    moves.Add(new Point(x, src.Y - 1));
            //}

            //foreach (var p in nav.getNeighborsSouth(src.X, src.Y))
            //{
            //    var x = Utility.Clamp(this.end.X, p.max, p.min);
            //    moves.Add(new Point(x, src.Y + 1));
            //}
        }
    }

}

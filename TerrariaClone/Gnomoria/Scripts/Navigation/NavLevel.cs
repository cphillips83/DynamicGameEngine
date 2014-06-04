using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameName1.Gnomoria.Scripts.Navigation
{
    public class NavLevel
    {
        public NavLevel down, up;
        public int depth = 0;
        private List<NavSegment>[] xSegments;
        private List<NavSegment>[] ySegments;


        public NavLevel(int width, int height, int depth)
        {
            xSegments = new List<NavSegment>[width];
            ySegments = new List<NavSegment>[height];
        }

        public void AddSegmentX(int x, NavSegment seg)
        {
            if (xSegments[x] == null)
                xSegments[x] = new List<NavSegment>();

            xSegments[x].Add(seg);
        }

        public void AddSegmentY(int y, NavSegment seg)
        {
            if (ySegments[y] == null)
                ySegments[y] = new List<NavSegment>();
            ySegments[y].Add(seg);
        }

        public void ClearX(int x)
        {
            if (xSegments[x] != null)
                xSegments[x].Clear();
        }

        public void ClearY(int y)
        {
            if (ySegments[y] != null)
                ySegments[y].Clear();
        }

        public void ClearXY(int x, int y)
        {
            ClearX(x);
            ClearY(y);
        }

        public IEnumerable<NavSegment> getOverlapsX(int x, NavSegment seg)
        {
            if (x >= 0 && x < xSegments.Length - 1)
            {
                var segments = xSegments[x];
                foreach (var xSeg in segments)
                    if (xSeg.isOverlapping(seg))
                        yield return xSeg;
            }
        }

        public IEnumerable<NavSegment> getOverlapsY(int y, NavSegment seg)
        {
            if (y >= 0 && y < ySegments.Length - 1)
            {
                var segments = ySegments[y];
                foreach (var ySeg in segments)
                    if (ySeg.isOverlapping(seg))
                        yield return ySeg;
            }
        }

        public bool getPartitionX(int x, int y, out NavSegment seg)
        {
            if (x >= 0 && x < xSegments.Length && y >= 0 && y < ySegments.Length)
            {
                var segments = xSegments[x];
                var test = new NavSegment(y, y);
                for (var i = 0; i < segments.Count; i++)
                {
                    if (segments[i].isOverlapping(test))
                    {
                        seg = segments[i];
                        return true;
                    }
                }
            }

            seg = new NavSegment();
            return false;
        }

        public bool getPartitionY(int x, int y, out NavSegment seg)
        {
            if (x >= 0 && x < xSegments.Length && y >= 0 && y < ySegments.Length)
            {
                var segments = ySegments[y];
                var test = new NavSegment(x, x);
                for (var i = 0; i < segments.Count; i++)
                {
                    if (segments[i].isOverlapping(test))
                    {
                        seg = segments[i];
                        return true;
                    }
                }
            }

            seg = new NavSegment();
            return false;
        }

        public IEnumerable<NavSegmentOverlap> getAdjacentSegments(int x, int y)
        {
            NavSegment xSeg, ySeg;

            if (getPartitionX(x, y, out xSeg))
            {
                foreach (var seg in getOverlapsX(x - 1, xSeg))
                    yield return new NavSegmentOverlap(x - 1, depth, GraphDirection.X, seg);

                foreach(var seg in getOverlapsX(x + 1, xSeg))
                    yield return new NavSegmentOverlap(x + 1, depth, GraphDirection.X, seg);
            }

            if (getPartitionY(x, y, out ySeg))
            {
                foreach (var seg in getOverlapsY(y - 1, ySeg))
                    yield return new NavSegmentOverlap(y - 1, depth, GraphDirection.Y, seg);
               
                foreach (var seg in getOverlapsY(y + 1, ySeg))
                    yield return new NavSegmentOverlap(y + 1, depth, GraphDirection.Y, seg);
            }
        }
    }

}

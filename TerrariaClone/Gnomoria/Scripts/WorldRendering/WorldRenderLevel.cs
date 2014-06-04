using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameName1.Gnomoria.Scripts.WorldRendering
{
    //y

    public class WorldRenderLevel
    {
        public int x = 0;
        public int height, depth;
        private List<WorldRenderSegment>[] cSegments;

        public WorldRenderLevel(int x, int height, int depth)
        {
            this.x = x;
            this.height = height;
            this.depth = depth;
            cSegments = new List<WorldRenderSegment>[height + depth * 2];
        }

        public void AddSegment(int c, WorldRenderSegment seg)
        {
            if (cSegments[c] == null)
                cSegments[c] = new List<WorldRenderSegment>();

            cSegments[c].Add(seg);
        }

        public void Clear(int c)
        {
            if (cSegments[c] != null)
                cSegments[c].Clear();
        }

        public IEnumerable<WorldRenderSegment> getOverlaps(int c, WorldRenderSegment seg)
        {
            if (c >= 0 && x < cSegments.Length)
            {
                var segments = cSegments[c];
                foreach (var cSeg in segments)
                    if (cSeg.isOverlapping(seg))
                        yield return cSeg;
            }
        }

        public bool findClosest(int c, int z, out WorldRenderSegment cSeg)
        {
            if (c >= 0 && c < cSegments.Length)
            {
                var segments = cSegments[c];
                if (segments != null)
                {
                    var cTest = new WorldRenderSegment(z, z);
                    for (int i = segments.Count - 1; i >= 0; i--)
                    {
                        cSeg = segments[i];
                        if (cSeg.isOverlapping(cTest))
                            return true;

                        if (cSeg.max < z)
                            return true;
                    }
                }
            }

            cSeg = new WorldRenderSegment();
            return false;
        }

        public bool getPartition(int c, int z, out WorldRenderSegment seg)
        {
            if (c >= 0 && c < height)
            {
                var segments = cSegments[c];
                var test = new WorldRenderSegment(z, z);
                for (var i = 0; i < segments.Count; i++)
                {
                    if (segments[i].isOverlapping(test))
                    {
                        seg = segments[i];
                        return true;
                    }
                }
            }

            seg = new WorldRenderSegment();
            return false;
        }

        public int getY(int c, int z)
        {
            //y = 2*z-2*d+c
            var d = depth;
            return 2 * z - 2 * d + c;
        }

    }

}

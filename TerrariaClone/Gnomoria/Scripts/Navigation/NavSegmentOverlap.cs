using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameName1.Gnomoria.Scripts.Navigation
{
    public struct NavSegmentOverlap
    {
        public int plane;
        public int depth;
        public GraphDirection direction;
        public NavSegment segment;

        public NavSegmentOverlap(int plane, int depth, GraphDirection dir, NavSegment seg)
        {
            this.plane = plane;
            this.depth = depth;
            this.direction = dir;
            this.segment = seg;
        }
    }
}

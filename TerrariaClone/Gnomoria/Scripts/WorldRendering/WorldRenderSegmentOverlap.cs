using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameName1.Gnomoria.Scripts.WorldRendering
{
    public struct WorldRenderSegmentOverlap
    {
        public int x, y;
        public int depth;
        public WorldRenderSegment segment;

        public WorldRenderSegmentOverlap(int x, int y, int depth, WorldRenderSegment seg)
        {
            this.x = x;
            this.y = y;
            this.depth = depth;
            this.segment = seg;
        }
        
    }
}

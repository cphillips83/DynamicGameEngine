using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace GameName1.Gnomoria.Scripts.WorldRendering
{
    [StructLayout(LayoutKind.Explicit, Size = 2)]
    public struct WorldRenderSegment
    {
        [FieldOffset(0)]
        public byte min;
        [FieldOffset(1)]
        public byte max;

        public WorldRenderSegment(int min, int max)
        {
            this.min = (byte)min;
            this.max = (byte)max;
        }

        public bool isOverlapping(WorldRenderSegment other)
        {
            return (other.min >= min && other.min <= max) || (other.max >= min && other.max <= max) ||
                (min >= other.min && min <= other.max) || (max >= other.min && max <= other.max);
        }

        public WorldRenderSegment overlap(WorldRenderSegment other)
        {
            return new WorldRenderSegment() { min = Utility.Max(min, other.min), max = Utility.Min(max, other.max) };
        }

    }
}


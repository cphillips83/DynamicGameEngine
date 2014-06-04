using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace GameName1.Gnomoria.Scripts.Navigation
{
    [StructLayout(LayoutKind.Explicit, Size = 4)]
    public struct NavSegment
    {
        [FieldOffset(0)]
        public byte flags;
        [FieldOffset(1)]
        public byte min;
        [FieldOffset(2)]
        public byte max;
        [FieldOffset(3)]
        public byte misc;

        public NavSegment(int min, int max)
        {
            this.min = (byte)min;
            this.max = (byte)max;
            this.flags = 0;
            this.misc = 0;
        }

        private void setFlag(int flag, bool val)
        {
            if (val)
                flags |= (byte)(1 << flag);
            else
                flags &= (byte)(byte.MaxValue ^ (byte)(1 << flag));
        }

        private bool getFlag(int flag)
        {
            return (flags & (1 << flag)) == (1 << flag);
        }

        public bool isOverlapping(NavSegment other)
        {
            return (other.min >= min && other.min <= max) || (other.max >= min && other.max <= max) ||
                (min >= other.min && min <= other.max) || (max >= other.min && max <= other.max);
        }

        public NavSegment overlap(NavSegment other)
        {
            return new NavSegment() { min = Utility.Max(min, other.min), max = Utility.Min(max, other.max) };
        }


    }
}


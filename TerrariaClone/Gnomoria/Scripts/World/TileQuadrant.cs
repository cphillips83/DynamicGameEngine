using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameName1.Gnomoria.Scripts.World
{
    [Flags]
    public enum TileQuadrant
    {
        top = 1,
        bottom = 2,
        left = 4,
        right = 8,
        topleft = top | left,
        topright = top | right,
        bottomleft = bottom | left,
        bottomright = bottom | right,
    }
}

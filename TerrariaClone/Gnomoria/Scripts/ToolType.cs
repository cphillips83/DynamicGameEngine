﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameName1.Gnomoria.Scripts
{
    [Flags]
    public enum ToolType : int
    {
        None,
        DigWall,
        RemoveFloor,
    }
}

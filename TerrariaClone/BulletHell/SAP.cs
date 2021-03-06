﻿using GameName1.BulletHell.Scripts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using zSprite;

namespace GameName1.BulletHell
{
    public interface ISAP
    {
        int min { get; }
        int max { get; }
        bool testCollision(ISAP other);
    }

    public class SAP
    {
        public struct SAPItem : IRadixKey
        {
            public ISAP item;
            public int Key { get { return item.min; } }
        }

        // array of integers to hold values
        private ISAP[] a = new ISAP[1024];

        
        
    }
}

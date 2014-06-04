using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using zSprite.Collections;

namespace GameName1.Digger.World
{
    public static class ForegroundBlockDescriptor
    {
        public static readonly ForegroundBlock air;
        public static readonly ForegroundBlock lightdirt;
        public static readonly ForegroundBlock dirt;

        public readonly static ForegroundBlock[] blocks = new ForegroundBlock[256];

        static ForegroundBlockDescriptor()
        {
            #region air
            var air = new ForegroundBlock();
            air.id = 0;
            air.drag = 0.95875f;
            air.isEmpty = true;
            air.isSolid = false;

            for (var i = 0; i < 256; i++)
                blocks[i] = air;
            #endregion

            #region lightdirt
            lightdirt = new ForegroundBlock();
            lightdirt.id = 1;
            lightdirt.friction = 0.646875f;
            lightdirt.isEmpty = false;
            lightdirt.isSolid = true;
            blocks[lightdirt.id] = dirt;
            //dirt.collisionMap.SetAll(ushort.MaxValue);
            //dirt.collisionX = ushort.MaxValue;
            //dirt.collisionY = ushort.MaxValue;
            #endregion

            #region dirt
            dirt = new ForegroundBlock();
            dirt.id = 2;
            dirt.friction = 0.646875f;
            dirt.isEmpty = false;
            dirt.isSolid = true;
            blocks[dirt.id] = dirt;
            //dirt.collisionMap.SetAll(ushort.MaxValue);
            //dirt.collisionX = ushort.MaxValue;
            //dirt.collisionY = ushort.MaxValue;
            #endregion
        }

    }

    [StructLayout(LayoutKind.Sequential)]
    public struct ForegroundBlock
    {
        public byte id;
        public float friction;
        public float drag;

        public bool isEmpty;
        public bool isSolid;
        public bool isTransparent { get { return !isEmpty && !isSolid; } }

        //public ushort collisionX;
        //public ushort collisionY;
        //public FixedArray16<ushort> collisionMap;

        //public bool isEmpty { get { return collisionX == 0 && collisionY == 0; } }
        //public bool isSolid { get { return collisionX == ushort.MaxValue && collisionY == ushort.MaxValue; } }

    }
}

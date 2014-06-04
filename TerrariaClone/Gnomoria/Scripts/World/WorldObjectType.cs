using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameName1.Gnomoria.Scripts.World
{
    public enum WorldObjectType
    {
        Missing = 0,

        DirtFloor,
        DarkDirtFloor,
        LightDirtFloor,
        ClayFloor,
        LightClayFloor,
        DarkClayFloor,

        StoneFloor,
        DarkStoneFloor,
        LightStoneFloor,
        WoodFloor,

        DirtWall,
        DarkDirtWall,
        LightDirtWall,
        ClayWall,
        LightClayWall,
        DarkClayWall,

        Grass,
        FloorSelection,
        WallSelection,
        ZombieWorker
        //ToolbarPickaxe,
    }

    public struct WorldObjectDescriptor
    {
        public bool emitsLight;
        public Color color;
        public Color emittedColor;
        public bool allowsMovementUp;
        public bool allowsMovementDown;

        public bool isSolid;
        public bool isFloor;
        public bool isPlacable;
        public bool isLiquid;
        public bool isTransparent;

        public WorldObjectType type;
       
    }

    public class WorldObjectDescriptors
    {
        private WorldObjectDescriptor[] descriptors = new WorldObjectDescriptor[256];

        public WorldObjectDescriptor this[WorldObjectType index]
        {
            get { return descriptors[(byte)index]; }
            set { descriptors[(byte)index] = value; }
        }

        public WorldObjectDescriptor this[int index]
        {
            get { return descriptors[index]; }
            set { descriptors[index] = value; }
        }
    }
}

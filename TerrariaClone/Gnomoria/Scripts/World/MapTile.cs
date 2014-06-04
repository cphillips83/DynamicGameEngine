using GameName1.Gnomoria.Scripts.World.GameObjects.Placeable;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace GameName1.Gnomoria.Scripts.World
{

    [StructLayout(LayoutKind.Sequential)]
    public struct MapTile
    {
        public byte floorId;
        public byte placedId;
        public byte light;
        public byte placeableFlags;

        public ushort objects;
        public ushort flags;

        public bool isQueuedForWork
        {
            get { return getFlag(0); }
            set { setFlag(0, value); }
        }

        private void setFlag(int flag, bool val)
        {
            if (val)
                flags |= (ushort)(1 << flag);
            else
                flags &= (ushort)(ushort.MaxValue ^ (ushort)(1 << flag));
        }

        private bool getFlag(int flag)
        {
            return (flags & (1 << flag)) == (1 << flag);
        }

        public void setPlaceableFlag(int flag, bool val)
        {
            if (val)
                placeableFlags |= (byte)(1 << placeableFlags);
            else
                placeableFlags &= (byte)(byte.MaxValue ^ (byte)(1 << placeableFlags));
        }

        public bool getPlaceableFlag(int flag)
        {
            return (placeableFlags & (1 << placeableFlags)) == (1 << placeableFlags);
        }

        public WorldObjectType placedType { get { return (WorldObjectType)placedId; } set { placedId = (byte)value; } }
        public WorldObjectType floorType { get { return (WorldObjectType)floorId; } set { floorId = (byte)value; } }

        public bool hasFloor { get { return floorType != WorldObjectType.Missing; } }
        public bool hasPlaceable { get { return placedId > 0; } }
        public bool isWalkable { get { return hasFloor && !hasPlaceable; } }

        

        //public TileDescriptor wallDescriptor { get { return TileDescriptors.instance[wallType]; } }

        public static MapTile Empty { get { return new MapTile() { floorType = WorldObjectType.Missing, placedType = WorldObjectType.Missing }; } }
        public static MapTile GrassFloor { get { return new MapTile() { floorType = WorldObjectType.Grass, placedType = WorldObjectType.Missing }; } }

        public static MapTile DirtFloor { get { return new MapTile() { floorType = WorldObjectType.DirtFloor, placedType = WorldObjectType.Missing }; } }
        public static MapTile LightDirtFloor { get { return new MapTile() { floorType = WorldObjectType.LightDirtFloor, placedType = WorldObjectType.Missing }; } }
        public static MapTile DarkDirtFloor { get { return new MapTile() { floorType = WorldObjectType.DarkDirtFloor, placedType = WorldObjectType.Missing }; } }

        public static MapTile DirtWall { get { return new MapTile() { floorType = WorldObjectType.DirtFloor, placedId = PlaceableObject.DIRT_WALL }; } }
        public static MapTile DirtRamp { get { return new MapTile() { floorType = WorldObjectType.DirtFloor, placedId = PlaceableObject.DIRT_RAMP }; } }
        //public static MapTile LightDirtWall { get { return new MapTile() { floorType = WorldObjectType.LightDirtFloor, placedType = WorldObjectType.LightDirtWall }; } }
        //public static MapTile DarkDirtWall { get { return new MapTile() { floorType = WorldObjectType.DarkDirtFloor, placedType = WorldObjectType.DarkDirtWall }; } }

        //public static MapTile ClayFloor { get { return new MapTile() { floorType = WorldObjectType.ClayFloor, placedType = WorldObjectType.Missing }; } }
        //public static MapTile LightClayFloor { get { return new MapTile() { floorType = WorldObjectType.LightClayFloor, placedType = WorldObjectType.Missing }; } }
        //public static MapTile DarkClayFloor { get { return new MapTile() { floorType = WorldObjectType.DarkClayFloor, placedType = WorldObjectType.Missing }; } }

        //public static MapTile ClayWall { get { return new MapTile() { floorType = WorldObjectType.ClayFloor, placedType = WorldObjectType.ClayWall }; } }
        //public static MapTile LightClayWall { get { return new MapTile() { floorType = WorldObjectType.LightClayFloor, placedType = WorldObjectType.LightClayWall }; } }
        //public static MapTile DarkClayWall { get { return new MapTile() { floorType = WorldObjectType.DarkClayFloor, placedType = WorldObjectType.DarkClayWall }; } }

    }
}

using GameName1.Gnomoria.Scripts.World;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameName1.Gnomoria.Scripts.World
{
    //public class TileDescriptor
    //{
    //    public int BlockID;
    //    public WorldObjectType WorldObjectType;
    //    public bool EmitsLight;
    //    public Color Color;
    //    public Color EmittedColor;
    //    public bool IsLiquid;
    //    public bool IsTransparent;
    //    public bool IsPhysicsBlock;
    //    public bool AllowsMovementUp;
    //    public bool AllowsMovementDown;
    //    //public WorldObjectRenderer Renderer;

    //    public TileDescriptor(WorldObjectType objectType)
    //    {
    //        this.BlockID = (int)objectType;
    //        this.WorldObjectType = objectType;
    //        this.EmitsLight = false;
    //        this.EmittedColor = Color.Black;
    //        this.Color = Color.White;
    //        this.IsTransparent = false;
    //        this.IsLiquid = false;
    //        this.IsPhysicsBlock = true;
    //        //this.Renderer = null;
    //    }
    //}

    //public class TileDescriptors
    //{
    //    public static readonly TileDescriptors instance = new TileDescriptors();
    //    private TileDescriptor[] blockDescriptors = new TileDescriptor[256];

    //    private TileDescriptors()
    //    {
    //        TileDescriptor block;

    //        //air
    //        block = new TileDescriptor(WorldObjectType.Missing);
    //        block.IsTransparent = true;
    //        block.IsPhysicsBlock = false;

    //        AddBlock(block);

    //        //grass
    //        block = new TileDescriptor(WorldObjectType.Grass);
    //        AddBlock(block);

    //        //dirt
    //        block = new TileDescriptor(WorldObjectType.DirtWall);
    //        AddBlock(block);

    //        ////water
    //        //block = new BlockDescriptor(BlockType.Water);
    //        //block.IsTransparent = true;
    //        //block.IsPhysicsBlock = false;
    //        //block.IsLiquid = true;
    //        //block.SetAllTextureSides(3, 4);
    //        //AddBlock(block);

    //        ////sand
    //        //block = new BlockDescriptor(BlockType.Sand);
    //        //block.SetAllTextureSides(2, 1);
    //        //AddBlock(block);

    //        ////tree trunk
    //        //block = new BlockDescriptor(BlockType.TreeTrunk);
    //        //block.SetTextureSideY(5, 1);
    //        //block.SetTextureSideXZ(4, 1);
    //        //AddBlock(block);

    //        ////leaves
    //        //block = new BlockDescriptor(BlockType.Leaves);
    //        //block.SetAllTextureSides(4, 3);
    //        //block.IsTransparent = true;
    //        //block.Color = Color.Green;
    //        //AddBlock(block);

    //        ////stone
    //        //block = new BlockDescriptor(BlockType.Stone);
    //        //block.SetAllTextureSides(1, 0);
    //        //AddBlock(block);

    //        ////gold
    //        //block = new BlockDescriptor(BlockType.Gold);
    //        //block.SetAllTextureSides(0, 2);
    //        //AddBlock(block);

    //        ////iron
    //        //block = new BlockDescriptor(BlockType.Iron);
    //        //block.SetAllTextureSides(1, 2);
    //        //AddBlock(block);

    //        ////coal
    //        //block = new BlockDescriptor(BlockType.Coal);
    //        //block.SetAllTextureSides(2, 2);
    //        //AddBlock(block);

    //        ////lava
    //        //block = new BlockDescriptor(BlockType.Lava);
    //        //block.SetAllTextureSides(15, 15);
    //        //block.EmitsLight = true;
    //        //block.IsLiquid = true;
    //        //block.EmittedColor = Color.Red;
    //        //AddBlock(block);

    //        ////bedrock
    //        //block = new BlockDescriptor(BlockType.BedRock);
    //        //block.SetAllTextureSides(0, 0);
    //        //AddBlock(block);
    //    }

    //    public static MapTile Empty { get { return new MapTile() { wallType = WorldObjectType.Missing }; } }
    //    public static MapTile Grass { get { return new MapTile() { wallType = WorldObjectType.Grass }; } }

    //    public static MapTile DirtFloor { get { return new MapTile() { wallType = WorldObjectType.DirtFloor }; } }
    //    public static MapTile LightDirtFloor { get { return new MapTile() { wallType = WorldObjectType.LightDirtFloor }; } }
    //    public static MapTile DarkDirtFloor { get { return new MapTile() { wallType = WorldObjectType.DarkDirtFloor }; } }

    //    public static MapTile DirtWall { get { return new MapTile() { wallType = WorldObjectType.DirtWall }; } }
    //    public static MapTile LightDirtWall { get { return new MapTile() { wallType = WorldObjectType.LightDirtWall }; } }
    //    public static MapTile DarkDirtWall { get { return new MapTile() { wallType = WorldObjectType.DarkDirtWall }; } }

    //    public static MapTile ClayFloor { get { return new MapTile() { wallType = WorldObjectType.ClayFloor }; } }
    //    public static MapTile LightClayFloor { get { return new MapTile() { wallType = WorldObjectType.LightClayFloor }; } }
    //    public static MapTile DarkClayFloor { get { return new MapTile() { wallType = WorldObjectType.DarkClayFloor }; } }

    //    public static MapTile ClayWall { get { return new MapTile() { wallType = WorldObjectType.ClayWall }; } }
    //    public static MapTile LightClayWall { get { return new MapTile() { wallType = WorldObjectType.LightClayWall }; } }
    //    public static MapTile DarkClayWall { get { return new MapTile() { wallType = WorldObjectType.DarkClayWall }; } }


    //    public TileDescriptor this[int index]
    //    {
    //        get
    //        {
    //            if (index < 0 || index > 255)
    //                throw new ArgumentOutOfRangeException("index");

    //            return blockDescriptors[index];
    //        }
    //        private set
    //        {
    //            if (index < 0 || index > 255)
    //                throw new ArgumentOutOfRangeException("index");

    //            blockDescriptors[index] = value;
    //        }
    //    }

    //    public TileDescriptor this[WorldObjectType type]
    //    {
    //        get
    //        {
    //            return this[(int)type];
    //        }
    //        private set
    //        {
    //            this[(int)type] = value;
    //        }
    //    }

    //    private void AddBlock(TileDescriptor block)
    //    {
    //        this[block.BlockID] = block;
    //    }
    //}
}

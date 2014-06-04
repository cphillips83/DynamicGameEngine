using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace GameName1.Digger
{
    public static class Settings
    {
        public const int TileSize = 16;
        public static readonly Vector2 TileSizeV = new Vector2(TileSize, TileSize);
        
        public const int TileHalfSize = TileSize / 2;
        public const int ChunkSize = 32;
        public const int ChunkSize2 = ChunkSize * ChunkSize;
        public const int ChunkHalfSize = ChunkSize / 2;
        

    }
}

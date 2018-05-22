using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace MazeGame
{
    enum TileType
    {
        Floor, Wall, Door, AdjacentDoorTiles, ChanceTile
    }
    class Tile
    {
        public Rectangle TileRect;
        public Texture2D TileTexture;
        public Color TileColor;
        public TileType Tiletype;
        public int TileDrawLayer;
        public int TileTimer;

        public Tile(Rectangle TileR, Texture2D TileT, Color TileC, TileType type, int TileDrawL)
        {
            TileRect = TileR;
            TileTexture = TileT;
            TileColor = TileC;
            Tiletype = type;
            TileDrawLayer = TileDrawL;
            TileTimer = 0;
            CreateDoorTimer(type);

        }

        private void CreateDoorTimer(TileType type)
        {
            if (type == TileType.Door)
            {
                TileTimer = 240; // 4 secs, 2 for open 2 for closed.
            }
        }
    }
}

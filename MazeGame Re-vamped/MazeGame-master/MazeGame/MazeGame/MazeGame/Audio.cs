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
using System.IO;

namespace MazeGame
{
    class Audio
    {
        List<Song> songs = new List<Song>();
        GameState gameState;

        public Audio(List<Song> songs, GameState gameState)
        {
            this.songs = songs;
            this.gameState = gameState;
        }
    }
}

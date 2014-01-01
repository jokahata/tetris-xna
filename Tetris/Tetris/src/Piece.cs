using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

namespace Tetris.src
{
    
    /// <summary>
    /// Represents a tetris piece 
    /// </summary>
    class Piece
    {

        Texture2D sprite;
        int[] pieceConfig;
        int rotation;
        int pieceNumber;

        public Piece(Texture2D sprite, int[] pieceConfig, int pieceNumber)
        {
            this.sprite = sprite;
            this.pieceConfig = pieceConfig;
            this.pieceNumber = pieceNumber;
            rotation = 0;

        }

    }
}

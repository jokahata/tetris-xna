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

        private Texture2D sprite;

        public Texture2D Sprite
        {
            set { this.sprite = value; }
            get { return this.sprite; }
        }

        private int[,] pieceConfig;

        public int[,] PieceConfig
        {
            get { return this.pieceConfig; }
        }

        private int rotation;

        public int Rotation
        {
            get { return this.rotation; }
        }

        private int pieceNumber;
        public int PieceNumber
        {
            get { return this.pieceNumber; }
        }

        public Piece(Texture2D sprite, int[,] pieceConfig, int pieceNumber)
        {
            this.sprite = sprite;
            this.pieceConfig = pieceConfig;
            this.pieceNumber = pieceNumber;
            rotation = 0;

        }

        public void rotateClockwise()
        {
            rotation = (rotation + 1) % 4;
        }

        public void rotateCounterClockwise()
        {
            if (rotation == 0) { rotation = 0; }
            else rotation -= 1;
        }
    }
}

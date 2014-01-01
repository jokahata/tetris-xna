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
        int[][] plan;
        int rotation;
        int selection;

        public Piece(Texture2D sprite, int[][] plan, int selection)
        {
            this.sprite = sprite;
            this.plan = plan;
            this.selection = selection;
            rotation = 0;

        }

    }
}

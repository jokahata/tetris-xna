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

        private int rotation;
        public int Rotation
        {
            get { return this.rotation; }
        }

        private int pieceID;
        public int PieceID
        {
            get { return this.pieceID; }
        }

        private int rowSize;
        public int RowSize
        {
            get { return this.rowSize; }
        }

        //Use a 2 dimensional array that holds a jagged array to represent each piece
        static int[][,] pieces = 
        {
                /*0: Square Block*/
                          new int[,]{ {0, 1, 1, 0,
                                       0, 1, 1, 0,
                                       0, 0, 0, 0},

                                      {0, 1, 1, 0,
                                       0, 1, 1, 0,
                                       0, 0, 0, 0},

                                      {0, 1, 1, 0,
                                       0, 1, 1, 0,
                                       0, 0, 0, 0},

                                      {0, 1, 1, 0,
                                       0, 1, 1, 0,
                                       0, 0, 0, 0}},

                /*1: Straight block*/
                          new int[,]{ {0, 0, 0, 0,
                                       1, 1, 1, 1,
                                       0, 0, 0, 0,
                                       0, 0, 0, 0},

                                      {0, 0, 1, 0,
                                       0, 0, 1, 0,
                                       0, 0, 1, 0,
                                       0, 0, 1, 0},

                                      {0, 0, 0, 0,
                                       0, 0, 0, 0,
                                       1, 1, 1, 1,
                                       0, 0, 0, 0,},

                                      {0, 1, 0, 0,
                                       0, 1, 0, 0,
                                       0, 1, 0, 0,
                                       0, 1, 0, 0}},

                /*2: Right L Block*/
                          new int[,]{ {1, 0, 0,
                                       1, 1, 1,
                                       0, 0, 0},

                                      {0, 1, 1,
                                       0, 1, 0,
                                       0, 1, 0},

                                      {0, 0, 0,
                                       1, 1, 1,
                                       0, 0, 1},

                                      {0, 1, 0,
                                       0, 1, 0,
                                       1, 1, 0}},

            /*3: Left L Block*/
                          new int[,]{ {0, 0, 1,
                                       1, 1, 1,
                                       0, 0, 0},

                                      {0, 1, 0,
                                       0, 1, 0,
                                       0, 1, 1},

                                      {0, 0, 0,
                                       1, 1, 1,
                                       1, 0, 0},

                                      {1, 1, 0,
                                       0, 1, 0,
                                       0, 1, 0}},

                /*4: Right Z Block*/
                        new int[,]{   {0, 1, 1,
                                       1, 1, 0,
                                       0, 0, 0},

                                      {0, 1, 0,
                                       0, 1, 1,
                                       0, 0, 1},

                                      {0, 0, 0,
                                       0, 1, 1,
                                       1, 1, 0},

                                      {1, 0, 0,
                                       1, 1, 0,
                                       0, 1, 0}},

                /*5: T Block*/
                        new int[,]{   {0, 1, 0,
                                       1, 1, 1,
                                       0, 0, 0},

                                      {0, 1, 0,
                                       0, 1, 1,
                                       0, 1, 0},

                                      {0, 0, 0,
                                       1, 1, 1,
                                       0, 1, 0},

                                      {0, 1, 0,
                                       1, 1, 0,
                                       0, 1, 0}},

                /*6: Left Z Block*/
                        new int[,]{   {1, 1, 0,
                                       0, 1, 1,
                                       0, 0, 0},

                                      {0, 0, 1, 
                                       0, 1, 1,
                                       0, 1, 0},

                                      {0, 0, 0, 
                                       1, 1, 0,
                                       0, 1, 1},

                                      {0, 1, 0,
                                       1, 1, 0,
                                       1, 0, 0}}

        };

        static private int[,] leftEdge = {/*Square*/{1, 1, 1, 1},
                                   /*Strai*/ {0, 2, 0, 1},
                                   /*Ri L*/  {0, 1, 0, 1},
                                   /*Le L*/  {0, 1, 0, 0},
                                   /*Ri Z*/  {0, 1, 0, 0},
                                   /*T*/     {0, 1, 0, 0},
                                   /*Le Z*/  {0, 1, 0, 0}};

        static private int[,] rightEdge = {/*Square*/{2, 2, 2, 2},
                                    /*Strai*/ {3, 2, 3, 1},
                                    /*Ri L*/  {2, 2, 2, 1},
                                    /*Le L*/  {2, 2, 2, 1},
                                    /*Ri Z*/  {2, 2, 2, 1},
                                    /*T*/     {2, 2, 2, 1},
                                    /*Le Z*/  {2, 2, 2, 1}};

        /// <summary>
        /// Take in the correct sprite and number for configuration
        /// </summary>
        /// <param name="sprite">The sprite for this piece</param>
        /// <param name="pieceID">The number of this piece in the array</param>
        public Piece(Texture2D sprite, int pieceID)
        {
            this.sprite = sprite;
            //TEMP
            this.pieceConfig = pieces[pieceID];
            this.pieceID = pieceID;
            rotation = 0;
            // RowSize is 4 if a square or straight line, else 3
            rowSize = pieceID == 0 || pieceID == 1 ? 4 : 3;

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

        public int getLeftEdge()
        {
            return leftEdge[pieceID, rotation];
        }

        public int getRightEdge()
        {
            return rightEdge[pieceID, rotation];
        }

        public int getValueAtPoint(int x)
        {
            return pieceConfig[rotation, x];
        }

        public int getLength()
        {
            return pieceConfig.GetLength(1);
        }
    }
}

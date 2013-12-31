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
        //Use a 2 dimensional array that holds a jagged array to represent each piece
        int[,][] pieces = 
        {
                /*0: Square Block*/
                          { new int[] {0, 1, 1, 0,
                                       0, 1, 1, 0,
                                       0, 0, 0, 0},

                            new int[] {0, 1, 1, 0,
                                       0, 1, 1, 0,
                                       0, 0, 0, 0},

                            new int[] {0, 1, 1, 0,
                                       0, 1, 1, 0,
                                       0, 0, 0, 0},

                            new int[] {0, 1, 1, 0,
                                       0, 1, 1, 0,
                                       0, 0, 0, 0}},

                /*1: Straight block*/
                           {new int[] {0, 0, 0, 0,
                                       1, 1, 1, 1,
                                       0, 0, 0, 0,
                                       0, 0, 0, 0},

                            new int[] {0, 0, 1, 0,
                                       0, 0, 1, 0,
                                       0, 0, 1, 0,
                                       0, 0, 1, 0},

                            new int[] {0, 0, 0, 0,
                                       0, 0, 0, 0,
                                       1, 1, 1, 1,
                                       0, 0, 0, 0,},

                            new int[] {0, 1, 0, 0,
                                       0, 1, 0, 0,
                                       0, 1, 0, 0,
                                       0, 1, 0, 0}},

                /*2: Right L Block*/
                           {new int[] {1, 0, 0,
                                       1, 1, 1,
                                       0, 0, 0},

                            new int[] {0, 1, 1,
                                       0, 1, 0,
                                       0, 1, 0},

                            new int[] {0, 0, 0,
                                       1, 1, 1,
                                       0, 0, 1},

                            new int[] {0, 1, 0,
                                       0, 1, 0,
                                       1, 1, 0}},

            /*3: Left L Block*/
                           {new int[] {0, 0, 1,
                                       1, 1, 1,
                                       0, 0, 0},

                            new int[] {0, 1, 0,
                                       0, 1, 0,
                                       0, 1, 1},

                            new int[] {0, 0, 0,
                                       1, 1, 1,
                                       1, 0, 0},

                            new int[] {1, 1, 0,
                                       0, 1, 0,
                                       0, 1, 0}},

                /*4: Right Z Block*/
                          { new int[] {0, 1, 1,
                                       1, 1, 0,
                                       0, 0, 0},

                            new int[] {0, 1, 0,
                                       0, 1, 1,
                                       0, 0, 1},

                            new int[] {0, 0, 0,
                                       0, 1, 1,
                                       1, 1, 0},

                            new int[] {1, 0, 0,
                                       1, 1, 0,
                                       0, 1, 0}},

                /*5: T Block*/
                          { new int[] {0, 1, 0,
                                       1, 1, 1,
                                       0, 0, 0},

                            new int[] {0, 1, 0,
                                       0, 1, 1,
                                       0, 1, 0},

                            new int[] {0, 0, 0,
                                       1, 1, 1,
                                       0, 1, 0},

                            new int[] {0, 1, 0,
                                       1, 1, 0,
                                       0, 1, 0}},

                /*6: Left Z Block*/
                          { new int[] {1, 1, 0,
                                       0, 1, 1,
                                       0, 0, 0},

                            new int[] {0, 0, 1, 
                                       0, 1, 1,
                                       0, 1, 0},

                            new int[] {0, 0, 0, 
                                       1, 1, 0,
                                       0, 1, 1},

                            new int[] {0, 1, 0,
                                       1, 1, 0,
                                       1, 0, 0}}

        };
        Texture2D sprite;

    }
}

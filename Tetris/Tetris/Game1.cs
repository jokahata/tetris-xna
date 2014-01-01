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
using Tetris.src;

namespace Tetris
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        //Use a 2 dimensional array that holds a jagged array to represent each piece
        int[][,] pieces = 
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

        static int blockSize;

        //locArray will hold what blocks are on the board
        int[] locArray;
        //gameArray will hold the current board in motion
        int[] gameArray;
        static int xTiles = 10;
        static int yTiles = 22;

        // Play area variables
        Texture2D playArea;
        static int marginX = 25;
        static int marginY = 25;
        static Vector2 playAreaLocation = new Vector2(marginX, marginY);
        static int playAreaWidth;
        static int playAreaHeight;

        //Sprites
        Texture2D sprite0, sprite1, sprite2, sprite3, sprite4, sprite5, sprite6;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";


            // Change the resolution
            graphics.PreferredBackBufferWidth = 800;
            graphics.PreferredBackBufferHeight = 650;
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {

            base.Initialize();

            // Make the window pop up at the top left corner
            var form = (System.Windows.Forms.Form)System.Windows.Forms.Control.FromHandle(this.Window.Handle);
            form.Location = new System.Drawing.Point(0, 0);
            

            locArray= new int[xTiles * yTiles];
            // Represent empty location as -1
            for (int i = 0; i < locArray.Length; i++)
            {
                locArray[i] = -1;
            }

            //REMOVE
            locArray[5] = 0;
            locArray[0] = 0;

            // Tetris area variables
            xTiles = 10;
            yTiles = 22;

            
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);


            playArea = Content.Load<Texture2D>("playArea");
            sprite0 = Content.Load<Texture2D>("block0");
            getBounds();
        }

        /// <summary>
        /// Get the correct heights from the textures
        /// </summary>
        protected void getBounds()
        {
            blockSize = sprite0.Height;

            playAreaWidth = playArea.Width;
            playAreaHeight = playArea.Height;


            // Checks that the dimensions are correct
            if (playAreaWidth != xTiles * blockSize)
            {
                Console.WriteLine("PlayAreaWidth is not as expected");
            }
            if (playAreaHeight != yTiles * blockSize)
            {
                Console.WriteLine("PlayAreaHeight is not as expected");
            }

        }
        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            //Don't need to use since this is a  small game
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            //TODO: Add random bag
            // The player controlled piece
            Piece curPiece = new Piece(sprite0, pieces[0], 0);
            //TODO: Change source index to spawner
            int sourceIndex = 0;

            // GetLength(1) gets the length of the second array
            for (int i = 0; i < curPiece.PieceConfig.GetLength(1); i++)
            {

            }

            processKeyboard();

            base.Update(gameTime);
        }

        private void processKeyboard()
        {
            KeyboardState k = Keyboard.GetState();

        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // Drawing
            spriteBatch.Begin();
            spriteBatch.Draw(playArea, playAreaLocation, Color.White);

            // Draw out pieces on board
            for (int i = 0; i < xTiles * yTiles; i++)
            {
                if (locArray[i] != -1)
                {
                    //TODO: Add piece drawing
                    //TODO: Offset the drawing because of the invisible tiles above
                    Texture2D correctTexture = null;
                    Vector2 loc = new Vector2(marginX + (i % yTiles) * blockSize , marginY + (i / yTiles) * blockSize );
                    switch(locArray[i])
                    {
                        case 0:
                            correctTexture = sprite0;
                            break;
                        default:
                            Console.WriteLine("An incorrect integer was put into the locArray");
                            break;
                    }
                    spriteBatch.Draw(correctTexture, loc, Color.White);
                }
            }
            //Go through a separate array for blocks. When done moving, take in the pieces and add them to the array, keeping track of the color
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}

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

        int blockSize;

        // Tetris area variables
        int[] locArray;
        int xTiles = 10;
        int yTiles = 22;

        // Play area variables
        Texture2D playArea;
        Vector2 playAreaLocation = new Vector2(25, 25);
        int playAreaWidth;
        int playAreaHeight;

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
            // TODO: Add your initialization logic here

            base.Initialize();

            var form = (System.Windows.Forms.Form)System.Windows.Forms.Control.FromHandle(this.Window.Handle);
            form.Location = new System.Drawing.Point(0, 0);
            

            locArray= new int[xTiles * yTiles];
            blockSize = 20;

            // Tetris area variables
            xTiles = 10;
            yTiles = 22;

            // Play area variables
            playAreaWidth = xTiles * blockSize;
            playAreaHeight = xTiles * blockSize;
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
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
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

            // TODO: Add your update logic here

            base.Update(gameTime);
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
            for (int i = 0; i < xTiles * yTiles; i++)
            {
                if (locArray[i] == 1)
                {
                    //TODO: Add piece drawing
                }
            }
            //Go through a separate array for blocks. When done moving, take in the pieces and add them to the array, keeping track of the color
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}

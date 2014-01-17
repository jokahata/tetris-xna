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


        static int blockSize;

        //statArray (stationary array) will hold what blocks are on the board
        int[] statArray;

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

        float currentTime = 0f;
        int sourceIndex = 0;

        private Piece curPiece;
        //Sprites
        Texture2D block0, block1, block2, block3, block4, block5, block6, nullBlock, background;

        //Booleans for keypresses
        Boolean keyPressedRight = false;
        Boolean keyPressedLeft = false;
        Boolean keyPressedUp = false;
        Boolean keyPressedDown = false;
        Boolean keyPressedSpace = false;

        Boolean run = true;
        Boolean needNewPiece = true;
        Boolean[] grabBag = new Boolean[7];
        //TODO:

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


            statArray = new int[xTiles * yTiles];
            gameArray = new int[xTiles * yTiles];
            // Represent empty location as -1
            for (int i = 0; i < statArray.Length; i++)
            {
                statArray[i] = -1;

            }

            //Copy statArray to gameArray
            Array.Copy(statArray, gameArray, xTiles * yTiles);


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
            block0 = Content.Load<Texture2D>("block0");
            //TODO: Make prettier blocks
            block1 = Content.Load<Texture2D>("block1");
            block2 = Content.Load<Texture2D>("block2");
            block3 = Content.Load<Texture2D>("block3");
            block4 = Content.Load<Texture2D>("block4");
            block5 = Content.Load<Texture2D>("block5");
            block6 = Content.Load<Texture2D>("block6");
            nullBlock = Content.Load<Texture2D>("nullBlock");
            background = Content.Load<Texture2D>("bg");
            getBounds();
        }

        /// <summary>
        /// Get the correct heights from the textures
        /// </summary>
        protected void getBounds()
        {
            blockSize = block0.Height;

            playAreaWidth = playArea.Width;
            playAreaHeight = playArea.Height;


            // Checks that the dimensions are correct
            if (playAreaWidth != xTiles * blockSize)
            {
                Console.WriteLine("PlayAreaWidth is not as expected");
            }
            if (playAreaHeight != (yTiles - 2) * blockSize)
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

            if (run)
            {
                currentTime += (float)gameTime.ElapsedGameTime.TotalSeconds; //Time passed since last Update() 
                //DropDown
                if (currentTime >= 1)
                {
                    //printOutArray();
                    if (checkCollisionDown())
                    {
                        needNewPiece = true;
                        checkLineCompletion();
                        //TODO: Call line completion checker
                    }
                    else
                    {
                        sourceIndex += xTiles;
                    }
                    currentTime--;
                }


                // The player controlled piece
                if (needNewPiece)
                {
                    copyGameArrayToStatArray();
                    getNewPiece();
                    needNewPiece = false;
                }
                //TODO: Change source index to spawner

                // Reset gameArray
                copyStatArrayToGameArray();

                /* LOGIC: COPY PIECE ARRAY ONTO GAME BOARD */
                copyPieceOntoGameBoard();
            }
            processKeyboard();
            base.Update(gameTime);
        }

        /// <summary>
        /// Checks if lines are completed, deletes them and shifts blocks down
        /// </summary>
        private void checkLineCompletion()
        {
            //TODO: Optimize by running on multiple lines
            int linesDeleted = 0;
            //Index of first deleted line
            int startIndex = 0;
            for (int y = 0; y < yTiles; y++)
            {
                Boolean isLineComplete = true;
                for (int x = 0; x < xTiles; x++)
                {
                    int index = x + (y * xTiles);
                    if (statArray[index] == -1)
                    {
                        isLineComplete = false;
                    }
                }
                if (isLineComplete)
                {
                    if (linesDeleted == 0)
                    {
                        startIndex = y * xTiles;
                    }
                    linesDeleted++;
                }
                if (linesDeleted > 0 && (!isLineComplete || y == yTiles - 1))
                {
                    deleteLinesAndShift(linesDeleted, startIndex);
                    copyStatArrayToGameArray();
                    linesDeleted = 0;
                    startIndex = 0;
                }
            }
        }

        private void deleteLinesAndShift(int lines, int startIndex)
        {
            int[] tempArray = new int[xTiles * yTiles];
            Array.Copy(statArray, tempArray, xTiles * yTiles);
            Array.Copy(tempArray, 0, statArray, lines * xTiles, startIndex);
        }

        private void copyPieceOntoGameBoard()
        {
            int arrLength = curPiece.getLength();
            for (int i = 0; i < arrLength; i++)
            {
                if (curPiece.getValueAtPoint(i) == 1)
                {
                    //Offsets it 
                    gameArray[sourceIndex + (i % curPiece.RowSize) + ((i / curPiece.RowSize) * xTiles)] = curPiece.PieceID;
                }
            }
        }

        private void getNewPiece()
        {
            Random random = new Random();
            int randomNumber = random.Next(0, 6);
            int attempts = 0;

            while (grabBag[randomNumber])
            {
                randomNumber = random.Next(0, 7);
                attempts++;
                if (attempts >= 6)
                {
                    for (int i = 0; i < grabBag.Length; i++)
                    {
                        grabBag[i] = false;
                    }
                }
            }
            grabBag[randomNumber] = true;

            switch (randomNumber)
            {
                case 0:
                    curPiece = new Piece(block0, randomNumber);
                    sourceIndex = 3;
                    break;
                case 1:
                    curPiece = new Piece(block1, randomNumber);
                    sourceIndex = 3;    
                    break;
                case 2:
                    curPiece = new Piece(block2, randomNumber);
                    sourceIndex = 3;
                    break;
                case 3:
                    curPiece = new Piece(block3, randomNumber);
                    sourceIndex = 3;
                    break;
                case 4:
                    curPiece = new Piece(block4, randomNumber);
                    sourceIndex = 3;
                    break;
                case 5:
                    curPiece = new Piece(block5, randomNumber);
                    sourceIndex = 3;
                    break;
                case 6:
                    curPiece = new Piece(block6, randomNumber);
                    sourceIndex = 3;
                    break;
                default:
                    Console.WriteLine("An incorrect integer was taken from the grab bag");
                    break;
            }


        }

        private void copyGameArrayToStatArray()
        {
            Array.Copy(gameArray, statArray, xTiles * yTiles);
        }

        private void copyStatArrayToGameArray()
        {
            Array.Copy(statArray, gameArray, xTiles * yTiles);
        }

        private void processKeyboard()
        {
            KeyboardState k = Keyboard.GetState();
            //Move right
            if (k.IsKeyDown(Keys.Right))
            {
                //TODO: Add timer for when key is pressed so that scrolling happens
                if (!keyPressedRight && !checkCollisionRight())
                {
                    keyPressedRight = true;
                    sourceIndex++;
                }
            }

            //Release right
            if (keyPressedRight && k.IsKeyUp(Keys.Right))
            {
                keyPressedRight = false;
            }

            //Move left
            if (k.IsKeyDown(Keys.Left))
            {
                if (!keyPressedLeft && !checkCollisionLeft())
                {
                    keyPressedLeft = true;
                    sourceIndex--;
                }
            }

            //Release left
            if (keyPressedLeft && k.IsKeyUp(Keys.Left))
            {
                keyPressedLeft = false;
            }

            //Rotate
            if (k.IsKeyDown(Keys.Up))
            {

                if (!keyPressedUp)
                {
                    tryRotating();
                    keyPressedUp = true;
                }
            }

            //Release left
            if (keyPressedUp && k.IsKeyUp(Keys.Up))
            {
                keyPressedUp = false;
            }

            //Soft Drop
            if (k.IsKeyDown(Keys.Down))
            {
                if (!keyPressedDown)
                {
                    keyPressedDown = true;
                    if (!checkCollisionDown())
                    {
                        sourceIndex += xTiles;
                    }
                }
            }

            if (keyPressedDown && k.IsKeyUp(Keys.Down))
            {
                keyPressedDown = false;
            }


            //Hard Drop
            if (k.IsKeyDown(Keys.Space))
            {
                if (!keyPressedSpace)
                {
                    while (!checkCollisionDown())
                    {
                        sourceIndex += xTiles;
                    }
                    copyStatArrayToGameArray();
                    currentTime = 0;
                    copyPieceOntoGameBoard();
                    copyGameArrayToStatArray();
                    checkLineCompletion();
                    needNewPiece = true;
                    keyPressedSpace = true;
                }
            }

            //Release space
            if (keyPressedSpace && k.IsKeyUp(Keys.Space))
            {
                keyPressedSpace = false;
            }

            //Pause
            if (k.IsKeyDown(Keys.P))
            {
                run = !run;
            }
        }

        private Boolean checkCollisionRight()
        {
            //Check if on right edge of playArea
            if ((sourceIndex % xTiles) + curPiece.getRightEdge() == xTiles - 1) { return true; }
            //Check if an individual block is next to a block on the right
            for (int y = 0; y < curPiece.getLength() / curPiece.RowSize; y++)
            {
                for (int x = 0; x < curPiece.RowSize; x++)
                {
                    // Finds right edge of that row
                    Boolean isRightEdge = curPiece.getValueAtPoint(x + y * curPiece.RowSize) == 1 && (x == curPiece.RowSize - 1 || curPiece.getValueAtPoint(x + 1 + y * curPiece.RowSize) == 0);
                    if (isRightEdge && gameArray[sourceIndex + x + 1 + y * xTiles] != -1)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        private Boolean checkCollisionLeft()
        {
            //Check if an individual block is next to a block on the left
            for (int y = 0; y < curPiece.getLength() / curPiece.RowSize; y++)
            {
                for (int x = 0; x < curPiece.RowSize; x++)
                {
                    // Finds left edge of that row
                    Boolean isLeftEdge = curPiece.getValueAtPoint(x + y * curPiece.RowSize) == 1 && (x == 0 || curPiece.getValueAtPoint(x - 1 + y * curPiece.RowSize) == 0);
                    if (isLeftEdge && (gameArray[sourceIndex + x - 1 + y * xTiles] != -1 || (sourceIndex + x) % xTiles == 0))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        private Boolean checkCollisionDown()
        {
            //TODO: Possibly generalize this because similar to hasCollision
            int tempIndex = sourceIndex + xTiles;
            int arrLength = curPiece.getLength();
            for (int i = 0; i < arrLength; i++)
            {
                int curIndex = tempIndex + (i % curPiece.RowSize) + ((i / curPiece.RowSize) * xTiles);
                if (curPiece.getValueAtPoint(i) == 1)
                {
                    //Check outside bounds of array
                    if (curIndex >= xTiles * yTiles) { return true; }
                    //Check if piece is already there
                    if (statArray[curIndex] != -1) { return true; }
                }
            }
            return false;
        }

        /// <summary>
        /// Check rotation and use the simple implementation of wallkicks
        /// </summary>
        /// <returns></returns>
        private void tryRotating()
        {
            //Straight block has different test cases
            if (!hasCollision(0, 0)) {
                curPiece.rotateClockwise();

            }
            else if (!hasCollision(-1, 0) )
            {
                curPiece.rotateClockwise();
                sourceIndex--;
            }
            else if (!hasCollision(1, 0))
            {
                curPiece.rotateClockwise();
                sourceIndex++;
            }
            else
            {
                //Fail to rotate
            }
        }

        /// <summary>
        /// Helper method for checkRotationCollision
        /// </summary>
        /// <param name="x">The offset horizontally</param>
        /// <param name="y">The offset vertically</param>
        /// <returns>True if there is a collision, false otherwise</returns>
        private Boolean hasCollision(int x, int y)
        {
            
            int arrLength = curPiece.getLength();
            int tempIndex = sourceIndex + x + y * xTiles;
            
            //Check if rotation throws the piece outside the game area
            int prevLeftEdge = curPiece.getLeftEdge();
            int prevRightEdge = curPiece.getRightEdge();
            curPiece.rotateClockwise();

            //Check if the offset throws the piece off the board
            if (x < 0 && (tempIndex + prevLeftEdge) % xTiles > (sourceIndex + prevLeftEdge) % xTiles || x > 0 && tempIndex % xTiles < sourceIndex % xTiles)
            {
                curPiece.rotateCounterClockwise();
                return true;
            }

            int half = xTiles / 2;
            
            
            if ((tempIndex + prevLeftEdge) % xTiles < half && ((tempIndex + curPiece.getLeftEdge()) % xTiles) > half)
            {
                curPiece.rotateCounterClockwise();
                return true;
            }
            
            if ((tempIndex + prevRightEdge) % xTiles > half && ((tempIndex + curPiece.getRightEdge()) % xTiles) < half)
            {
                curPiece.rotateCounterClockwise();
                return true;
            }


            for (int i = 0; i < arrLength; i++)
            {
                if (curPiece.getValueAtPoint(i) == 1)
                {
                    //Offsets it
                    //Check outside bounds of array
                    int curIndex = tempIndex + (i % curPiece.RowSize) + ((i / curPiece.RowSize) * xTiles);
                    if (curIndex >= xTiles * yTiles) 
                    {
                        curPiece.rotateCounterClockwise();
                        return true;
                    }
                    //Check if there is already a piece there
                    if (statArray[curIndex] != -1)
                    {
                        curPiece.rotateCounterClockwise();
                        return true;
                    } 
                }
            }
            curPiece.rotateCounterClockwise();
            return false;
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
            spriteBatch.Draw(background, new Vector2(0, 0), Color.White);
            spriteBatch.Draw(playArea, playAreaLocation, Color.White);

            // Draw out pieces on board
            for (int i = 0; i < xTiles * yTiles; i++)
            {

                Texture2D correctTexture = nullBlock;
                Vector2 loc = new Vector2(marginX + (i % xTiles) * blockSize, marginY + ((i - 2 * xTiles) / xTiles) * blockSize);
                switch (gameArray[i])
                {
                    case -1:
                        break;
                    case 0:
                        correctTexture = block0;
                        break;
                    case 1:
                        correctTexture = block1;
                        break;
                    case 2:
                        correctTexture = block2;
                        break;
                    case 3:
                        correctTexture = block3;
                        break;
                    case 4:
                        correctTexture = block4;
                        break;
                    case 5:
                        correctTexture = block5;
                        break;
                    case 6:
                        correctTexture = block6;
                        break;
                    default:
                        Console.WriteLine("An incorrect integer was put into the gameArray: " + gameArray[i]);
                        break;
                }
                // Draw only the tiles within the area
                if (i > 2 * xTiles - 1)
                {
                    spriteBatch.Draw(correctTexture, loc, Color.White);
                }
            }
            spriteBatch.End();

            base.Draw(gameTime);
        }

        private void printOutArray(int[] arr, String arrayName)
        {
            Console.WriteLine("Printing out: " + arrayName);
            for (int y = 0; y < yTiles; y++)
            {
                
                for (int x = 0; x < xTiles; x++)
                {
                    if (arr[y * xTiles + x] == -1)
                    {
                        Console.Write("7");
                    }
                    else
                    {
                        Console.Write(arr[y * xTiles + x]);
                    }   
                }
                Console.WriteLine();
            }
            Console.WriteLine();
        }
    }
}

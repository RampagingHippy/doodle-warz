#region Using Statements
using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.GamerServices;
#endregion

namespace DoodleWarz
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private SpriteFont _spriteFont;

        public static int screenWidth { get; set; }
        public static int screenHeight { get; set; }

        private GameTime _curTime = new GameTime();
        private GameTime _prevTime = new GameTime();

        private Player _playerOne;

        private static GameState _gameState;

        private KeyboardState _currentState;
        private KeyboardState _oldState;

        private static Keys _pauseKey; //Key that was used to pause game (prevents other players from unpausing)

        public Game1()
            : base()
        {
            _graphics = new GraphicsDeviceManager(this);
            _graphics.PreferredBackBufferWidth = 1280;
            _graphics.PreferredBackBufferHeight = 720;
            this.IsFixedTimeStep = false;
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            screenHeight = GraphicsDevice.Viewport.Height;
            screenWidth = GraphicsDevice.Viewport.Width;
            
            _playerOne = new Player(PlayerIndex.One);
            _playerOne.InitializeControls();

            _gameState = GameState.Playing;

            ItemManager.Initialize(2);

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            _spriteFont = Content.Load<SpriteFont>("Fonts/InGame");

            _playerOne.LoadContent(Content, "Textures/Doodle_Dragon2");
            ItemManager.LoadContent(Content, "Textures/Item");
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            _currentState = Keyboard.GetState();

            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed ||
                _currentState.IsKeyDown(Keys.Escape))
                Exit();

            switch (_gameState)
            {
                case GameState.Playing:
                    #region Main Game Loop
                {
                    //update players,items, etc.
                    _currentState = Keyboard.GetState();   
                    _playerOne.Update(_oldState, _currentState, gameTime);
                    ItemManager.Update(gameTime);

                    //check player + item collisions
                    ItemManager.CheckPlayerItemCollisions(_playerOne);
                }
                break;
                    #endregion
                case GameState.Paused:
                    #region Game Paused
                {
                    if (_oldState.IsKeyUp(_pauseKey) && _currentState.IsKeyDown(_pauseKey))
                    {
                        _gameState = GameState.Playing;
                    }
                }
                break;
                    #endregion
            }
            _oldState = _currentState;
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            _spriteBatch.Begin();
            _playerOne.Draw(_spriteBatch);

            if (_gameState == GameState.Paused)
                _spriteBatch.DrawString(_spriteFont, "PAUSED", new Vector2(screenWidth / 2, 0), Color.White);

            ItemManager.Draw(_spriteBatch);
            _spriteBatch.End();

            base.Draw(gameTime);
        }

        public static void ChangeGameState(GameState newState){
            if (newState == GameState.Paused)
            {
                if (_gameState == GameState.Playing)
                    _gameState = GameState.Paused;
                else if (_gameState == GameState.Paused)
                    _gameState = GameState.Playing;       
            }
        }

        //toggles pause state
        //saves pause key to local var
        //to prevent other players from unpausing
        public static void PauseGame(Keys playerPauseKey)
        {
            _pauseKey = playerPauseKey;

            if (_gameState == GameState.Playing)
                _gameState = GameState.Paused;
            else if (_gameState == GameState.Paused)
                _gameState = GameState.Playing;  
        }
    }
}

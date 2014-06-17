using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;

namespace DoodleWarz
{
    public class Player : GameObject
    {
        private Vector2 _baseVelocity = new Vector2(200, 200);

        private GameTime _prevTime = new GameTime();
        private PlayerIndex _playerIndex;
        private PlayerActionStruct _controls;

        private int _health = 10;
        private int _power = 0;
        private float _speed = 0;

        Point[] _frameOrder = new Point[] { new Point(1, 1), new Point(2, 1), new Point(2, 2), new Point(0, 2) };
        private int _frameIndex = 0;
        private float millisecondsPerFrame = 250;
        private float timeSinceLastFrame = 0;
        private const int SPRITE_WIDTH = 100;
        private const int SPRITE_HEIGHT = 100;

        public Player(PlayerIndex playerIndex)
        {
            this._playerIndex = playerIndex;
        }

        public override void LoadContent(ContentManager content, string asset)
        {
            _position = new Vector2(10f, 10f);
            base.LoadContent(content, asset);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            //spriteBatch.Draw(this._texture, this._position, Color.Yellow);
            spriteBatch.Draw(_texture,
                new Rectangle((int)_position.X,(int)_position.Y, SPRITE_WIDTH, SPRITE_HEIGHT),
                new Rectangle(_frameOrder[_frameIndex].X * 400, _frameOrder[_frameIndex].Y * 400, 400, 400),
                Color.Orange);
        }

        protected override Rectangle _bounds
        {
            get
            {
              return new Rectangle((int)_position.X, (int)_position.Y, SPRITE_WIDTH, SPRITE_HEIGHT);
            }
        }

        public void Update(KeyboardState oldState, KeyboardState newState, GameTime gameTime)
        {
            #region Check Player Movement
            float delta = (float)gameTime.ElapsedGameTime.TotalSeconds;
            Vector2 movement = delta * _baseVelocity * (_speed/20 + 1);

            Vector2 moveDirection = Vector2.Zero;

            if (oldState.IsKeyUp(_controls.pause) && newState.IsKeyDown(_controls.pause))
            {
                Game1.PauseGame(_controls.pause);
            }
            if (newState.IsKeyDown(_controls.moveUp))
            {
                moveDirection += new Vector2(0f, -1f);
            }
            if (newState.IsKeyDown(_controls.moveDown))
            {
                moveDirection += new Vector2(0f, 1f);
            }

            if (newState.IsKeyDown(_controls.moveLeft))
            {
                moveDirection += new Vector2(-1f, 0f);
            }
            if (newState.IsKeyDown(_controls.moveRight))
            {
                moveDirection += new Vector2(1f, 0f);
            }

            //Normalize to prevent faster diagonal movement
            //Do not normalize if still zero, or will result
            //in NaN values
            if (!moveDirection.Equals(Vector2.Zero)) 
                moveDirection.Normalize();

            Vector2 FinalV = movement * moveDirection;
            Move(movement * moveDirection);
            #endregion

            //update frame animation
            timeSinceLastFrame += (float)gameTime.ElapsedGameTime.Milliseconds;
            if (timeSinceLastFrame >= millisecondsPerFrame)
            {
                _frameIndex++;
                if (_frameIndex > _frameOrder.Length - 1)
                {
                    _frameIndex = 0;
                }
                timeSinceLastFrame = 0f;
            }
        }

        public void InitializeControls()
        {
            switch (_playerIndex)
            {
                case PlayerIndex.One:
                {
                        _controls.moveUp = Keys.W;
                        _controls.moveDown = Keys.S;
                        _controls.moveLeft = Keys.A;
                        _controls.moveRight = Keys.D;
                        _controls.pause = Keys.Back;
                        break;
                }
                case PlayerIndex.Two:
                {
                        _controls.moveUp = Keys.Up;
                        _controls.moveDown = Keys.Down;
                        _controls.moveLeft = Keys.Left;
                        _controls.moveRight = Keys.Right;
                        _controls.pause = Keys.RightShift;
                        break;
                }
            }
        }

        public void ItemCollision(Item item)
        {
            ItemType updateType = item.playerCollision();
            updateStats(updateType);
        }

        private void updateStats(ItemType stat)
        {
            switch (stat)
            {
                case ItemType.Health:
                    _health++;
                    break;

                case ItemType.Power:
                    _power++;
                    break;

                case ItemType.Speed:
                    _speed++;
                    break;
            }
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;


namespace DoodleWarz
{
    public class Item : GameObject
    {
        private bool _isActive = false;
        private ItemType _type;
        private float _timer = 10;
        private const float RESPAWN_TIME = 1;
        private const int TEXTURE_WIDTH = 20;
        private const int TEXTURE_HEIGHT = 20;
        private static Random _randPosition = new Random();
        private Color _drawColor;

        protected override Rectangle _bounds
        {
            get { return new Rectangle((int)_position.X, (int)_position.Y, TEXTURE_WIDTH, TEXTURE_HEIGHT); }
        }

        public Item(ItemType itemType)
        {
            _type = itemType;
            _drawColor = Color.White;
        }

        public Item(ItemType itemType, Color drawColor)
        {
            _type = itemType;
            _drawColor = drawColor;
        }

        public override void LoadContent(ContentManager content, string asset)
        {
            base.LoadContent(content, asset);
            this._position = new Vector2((float)_randPosition.Next(Game1.screenWidth),
                (float)_randPosition.Next(Game1.screenHeight));
        }

        public override void Update(GameTime gameTime)
        {
            if (!_isActive) //countdown until active again
            {
                _timer -= (float)gameTime.ElapsedGameTime.TotalSeconds;
                if (_timer <= 0)
                {
                    _isActive = true;
                    _timer = RESPAWN_TIME;
                }
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (_isActive)
                spriteBatch.Draw(this._texture,
                    new Rectangle((int)_position.X, (int)_position.Y, TEXTURE_WIDTH, TEXTURE_HEIGHT),
                    _drawColor);
        }

        public override bool Collides(GameObject gameObject)
        {
            if (_isActive == true)
                return base.Collides(gameObject);
            else
                return false;
        }

        //event where player collects item
        //return item type for player to update
        public ItemType playerCollision()
        {
            _isActive = false;
            _timer = RESPAWN_TIME;
            _position = new Vector2((float)_randPosition.Next(Game1.screenWidth - TEXTURE_WIDTH),
                (float)_randPosition.Next(Game1.screenHeight - TEXTURE_HEIGHT));
            return _type;
        }
    }
}

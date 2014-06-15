using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace DoodleWarz
{
    public class GameObject
    {
        protected Vector2 _position;
        protected Texture2D _texture;

        public virtual void LoadContent(ContentManager content, String texture)
        {
            _texture = content.Load<Texture2D>(texture);
        }

        public virtual void Draw(SpriteBatch Spritebatch)
        {
            Spritebatch.Draw(_texture, _position, Color.White);
        }

        public virtual void Move(Vector2 amount)
        {
            _position += amount;
            if (_position.X < 0)
                _position.X = Game1.screenWidth;
            else if (_position.X > Game1.screenWidth)
                _position.X = 0;

            if (_position.Y < 0)
                _position.Y = Game1.screenHeight;
            else if (_position.Y > Game1.screenHeight)
                _position.Y = 0;
        }

        public virtual void Update(GameTime gameTime) { }

        protected virtual Rectangle _bounds
        {
            get { return new Rectangle((int)_position.X, (int)_position.Y, _texture.Width, _texture.Height); }
        }

        public virtual bool Collides(GameObject gameObject)
        {
            return this._bounds.Intersects(gameObject._bounds);
        }

    }

}

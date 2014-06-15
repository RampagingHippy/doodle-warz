using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace DoodleWarz
{
    public static class ItemManager
    {
        private static List<Item> _items = new List<Item>();
        private static int _itemAmount;
        private static ItemFactory _factory;

        public static void Initialize(int amount)
        {
            _itemAmount = amount;
            _factory = new ItemFactory();
            PopulateList();
        }

        public static void LoadContent(ContentManager content, string asset)
        {
            foreach (Item i in _items)
                i.LoadContent(content, asset);
        }

        private static void PopulateList()
        {
            foreach (ItemType t in Enum.GetValues(typeof(ItemType)))
            {
                for (int i = 0; i < _itemAmount; i++)
                    _items.Add(_factory.CreateNewItem(t));
            }
        }

        public static void Update(GameTime gameTime)
        {
            foreach (Item i in _items)
            {
                i.Update(gameTime);
            }
        }

        public static void Draw(SpriteBatch spriteBatch)
        {
            foreach (Item i in _items)
            {
                i.Draw(spriteBatch);
            }
        }

        public static void CheckPlayerItemCollisions(Player player)
        {
            foreach (Item i in _items)
            {
                if (i.Collides(player))
                {
                    player.ItemCollision(i);
                }
            }
        }
    }
}

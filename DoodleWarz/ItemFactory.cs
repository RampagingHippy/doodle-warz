using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DoodleWarz
{
    public class ItemFactory
    {
        private static Color[] _colorList = new Color[3]
        {
            Color.White, //Health
            Color.Red, //Speed
            Color.Orange //Power
        };

        public ItemFactory() { }

        public Item CreateNewItem(ItemType type)
        {
            Color drawColor = _colorList[(int)type];
            return new Item(type, drawColor);
        }

    }
}

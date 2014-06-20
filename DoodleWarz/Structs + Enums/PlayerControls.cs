using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Input;

namespace DoodleWarz
{
    //All the actions a player may take
    //each tied to a key or gamepad button
    //for use by players for reading inputs
    public struct PlayerControls
    {
        public Keys moveUp;
        public Keys moveDown;
        public Keys moveLeft;
        public Keys moveRight;
        public Keys pause;
        public Keys attack;
    }
}

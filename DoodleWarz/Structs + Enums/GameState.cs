using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DoodleWarz
{
    //The various states the game may take
    //Used for controlling flow and updates
    //in main game loop
    public enum GameState
    {
        Playing,
        Paused
    }
}

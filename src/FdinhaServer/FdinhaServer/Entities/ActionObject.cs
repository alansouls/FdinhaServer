using System;

namespace FdinhaServer.Entities
{
    [Serializable]
    public class ActionObject
    {
        public Player Player;
        public Action Action;
        public Card Card;
        public int Guess;
        public ServerRoom Room;
    }
}

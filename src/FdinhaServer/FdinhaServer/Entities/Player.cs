using System;
using System.Collections.Generic;

namespace FdinhaServer.Entities
{
    [Serializable]
    public struct Player
    {
        public bool Valid;
        public int Lives;
        public string Name;
        public List<Card> Cards;
        public string Id;

        public override bool Equals(object obj)
        {
            return obj is Player player &&
                   Id == player.Id;
        }

        public override int GetHashCode()
        {
            return 2108858624 + Id.GetHashCode();
        }

        public static bool operator ==(Player a, Player b)
        {
            return a.Id == b.Id;
        }

        public static bool operator !=(Player a, Player b)
        {
            return a.Id != b.Id;
        }
    }

}

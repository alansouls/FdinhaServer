using FdinhaServer.Entities;
using System;
using System.Collections.Generic;

namespace FdinhaServer.Messages
{
    [Serializable]
    public class ResponseMessage
    {
        public string Id;

        public Player Player;

        public bool AdjustPlayer;

        public bool CanPlay;

        public bool GuessingRound;

        public List<Card> Table;

        public List<GameState> GameStates;

        public List<Player> Players;
    }
}

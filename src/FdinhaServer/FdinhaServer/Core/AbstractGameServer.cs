using FdinhaServer.Entities;
using FdinhaServer.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

namespace FdinhaServer.Core
{
    public abstract class AbstractGameServer : IGameServer
    {
        protected Dictionary<Player, IPEndPoint> PlayersIps;
        protected Dictionary<ServerRoom, MatchController> Rooms;
        protected List<string> MessagesRead { get; set; }

        public virtual void HandleMessage(MessageModel message, IPEndPoint groupEP)
        {
            MessagesRead.Add(message.MessageId);
            var action = message.Action;
            var match = Rooms.Where(r => r.Key.Name == action.Room.Name && r.Key.Password == action.Room.Password).FirstOrDefault().Value;
            switch (message.Action.Action)
            {
                case Entities.Action.ADD_PLAYER:
                    Console.WriteLine("Message with action ADD_PLAYER");
                    AddPlayer(action, groupEP, match);
                    break;
                case Entities.Action.GUESS:
                    Console.WriteLine("Message with action GUESS");
                    Guess(action, groupEP, match);
                    break;
                case Entities.Action.PASS:
                    Console.WriteLine("Message with action PASS");
                    Pass(action, groupEP, match);
                    break;
                case Entities.Action.PLAY_CARD:
                    Console.WriteLine("Message with action PLAY_CARD");
                    PlayCard(action, groupEP, match);
                    break;
                case Entities.Action.START_GAME:
                    Console.WriteLine("Message with action START_GAME");
                    StartGame(action, groupEP, match);
                    break;
            }
        }

        public void StartGame(ActionObject action, IPEndPoint groupEP, MatchController match)
        {
            match.StartGame();
        }

        public void AddPlayer(ActionObject action, IPEndPoint groupEP, MatchController match)
        {
            match.AddPlayer(action.Player);
            PlayersIps.Add(action.Player, groupEP);
            UpdateGameState(match);
        }

        public void Guess(ActionObject action, IPEndPoint groupEP, MatchController match)
        {
            if (action.Player != match.CurrentPlayer)
                return;
            match.Guess(action.Player, action.Guess);
        }

        public void Pass(ActionObject action, IPEndPoint groupEP, MatchController match)
        {
            if (action.Player != match.CurrentPlayer)
                return;
            match.Pass(action.Player);
        }

        public void PlayCard(ActionObject action, IPEndPoint groupEP, MatchController match)
        {
            if (action.Player != match.CurrentPlayer)
                return;
            match.PlayCard(action.Player, action.Card);
        }

        public abstract void StartServer();

        public abstract void UpdateGameState(MatchController match);
    }
}

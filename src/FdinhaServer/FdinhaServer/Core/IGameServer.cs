using FdinhaServer.Entities;
using FdinhaServer.Messages;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace FdinhaServer.Core
{
    public interface IGameServer
    {
        void AddPlayer(ActionObject action, IPEndPoint groupEP, MatchController match);
        void Guess(ActionObject action, IPEndPoint groupEP, MatchController match);
        void HandleMessage(MessageModel message, IPEndPoint groupEP);
        void Pass(ActionObject action, IPEndPoint groupEP, MatchController match);
        void PlayCard(ActionObject action, IPEndPoint groupEP, MatchController match);
        void StartGame(ActionObject action, IPEndPoint groupEP, MatchController match);
        void StartServer();
        void UpdateGameState(MatchController match);
    }
}

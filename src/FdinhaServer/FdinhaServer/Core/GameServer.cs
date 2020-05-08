using FdinhaServer.Entities;
using FdinhaServer.Messages;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace FdinhaServer.Core
{
    public class GameServer
    {
        private readonly UdpClient _udpClient;
        private readonly int listenPort;

        public GameServer(int port)
        {
            listenPort = port;
            _udpClient = new UdpClient(port);
            PlayersIps = new Dictionary<Player, IPEndPoint>();
            MessagesRead = new List<string>();
        }

        public void StartServer()
        {
            try
            {
                _udpClient.BeginReceive(new AsyncCallback((a) => MessageReceived(a)), null);
            }
            catch (SocketException e)
            {
                Console.WriteLine(e.Message);
            }
        }

        private void MessageReceived(IAsyncResult a)
        {
            try
            {
                IPEndPoint groupEP = new IPEndPoint(IPAddress.Any, 0);
                var bytes = _udpClient.EndReceive(a, ref groupEP);
                var json = Encoding.UTF8.GetString(bytes, 0, bytes.Length);

                var message = JsonConvert.DeserializeObject<MessageModel>(json);
                if (!MessagesRead.Contains(message.MessageId))
                    HandleMessage(message, groupEP);
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error receiving message, exception message: ({e.Message})");
            }
            finally
            {
                StartServer();
            }
        }

        public void HandleMessage(MessageModel message, IPEndPoint groupEP)
        {
            MessagesRead.Add(message.MessageId);
            var action = message.Action;
            var match = Rooms.Where(r => r.Key.Name == action.Room.Name && r.Key.Password == action.Room.Password).FirstOrDefault().Value;
            switch (message.Action.Action)
            {
                case Entities.Action.ADD_PLAYER:
                    AddPlayer(action, groupEP, match);
                    break;
                case Entities.Action.GUESS:
                    Guess(action, groupEP, match);
                    break;
                case Entities.Action.PASS:
                    Pass(action, groupEP, match);
                    break;
                case Entities.Action.PLAY_CARD:
                    PlayCard(action, groupEP, match);
                    break;
                case Entities.Action.START_GAME:
                    StartGame(action, groupEP, match);
                    break;
                case Entities.Action.CREATE_ROOM:
                    CreateRoom(action, groupEP);
                    break;
            }
        }

        private void SendResponseCreateRoomMessage(CreateRoomResponseMessage message, IPEndPoint endPoint)
        {
            var dgram = GetMessageBytes(message);
            _udpClient.Send(dgram, dgram.Length, endPoint);
        }

        private void CreateRoom(ActionObject action, IPEndPoint groupEP)
        {
            try
            {
                var room = action.Room;
                if (Rooms.Where(r => r.Key.Name == room.Name).Any())
                {
                    SendResponseCreateRoomMessage(new CreateRoomResponseMessage
                    {
                        Id = Guid.NewGuid(),
                        Success = false,
                        Message = "Room already exists"
                    }, groupEP);
                }
                else
                {
                    Rooms.Add(room, new MatchController(this));
                    SendResponseCreateRoomMessage(new CreateRoomResponseMessage
                    {
                        Id = Guid.NewGuid(),
                        Success = true,
                        Message = "Room created succesfuly"
                    }, groupEP);
                }
            }
            catch (Exception e)
            {
                SendResponseCreateRoomMessage(new CreateRoomResponseMessage
                {
                    Id = Guid.NewGuid(),
                    Success = false,
                    Message = $"Exception thrown on server, send this message to support: ({e.Message})"
                }, groupEP);
                Console.WriteLine(e.Message);
            }
        }

        private void StartGame(ActionObject action, IPEndPoint groupEP, MatchController match)
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

        public void UpdateGameState(MatchController match)
        {
            foreach (var p in PlayersIps.Keys)
            {
                var message = new ResponseMessage
                {
                    Id = Guid.NewGuid().ToString(),
                    GameStates = GameState.MountGameStates(match.Guesses, match.Wins),
                    GuessingRound = match.IsGuessing,
                    Table = match.Table.ToList(),
                    CanPlay = p == match.CurrentPlayer,
                    AdjustPlayer = true,
                    Player = match.Players.Where(x => x.Id == p.Id).FirstOrDefault(),
                    Players = match.Players
                };
                var bytes = GetMessageBytes(message);
                _udpClient.Send(bytes, bytes.Length, PlayersIps[p]);
            }
        }

        public void SendPlayerUpdate(Player player)
        {
            var message = new ResponseMessage
            {
                Id = Guid.NewGuid().ToString(),
                AdjustPlayer = true,
                Player = player
            };
            var bytes = GetMessageBytes(message);
            _udpClient.Send(bytes, bytes.Length, PlayersIps[player]);
        }

        private static byte[] GetMessageBytes(ResponseMessage response)
        {
            var json = JsonConvert.SerializeObject(response);
            byte[] bytes = Encoding.UTF8.GetBytes(json);
            return bytes;
        }

        private static byte[] GetMessageBytes(CreateRoomResponseMessage response)
        {
            var json = JsonConvert.SerializeObject(response);
            byte[] bytes = Encoding.UTF8.GetBytes(json);
            return bytes;
        }

        public void Close()
        {
            _udpClient.Close();
        }

        public Dictionary<Player, IPEndPoint> PlayersIps;
        public Dictionary<ServerRoom, MatchController> Rooms;
        public List<string> MessagesRead { get; set; }
    }
}

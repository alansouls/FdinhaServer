using System;
using System.Collections.Generic;
using System.Linq;

namespace FdinhaServer.Entities
{
    [Serializable]
    public class GameState
    {
        public Player Player;
        public int Guesses;
        public int Wins;

        public static List<GameState> MountGameStates(IDictionary<Player, int> guesses, IDictionary<Player, int> wins)
        {
            var result = new List<GameState>();
            result.AddRange(guesses.Select(g => new GameState
            {
                Player = g.Key,
                Guesses = g.Value
            }));
            result.ForEach(r =>
            {
                r.Wins = wins[r.Player];
            });

            return result;
        }

        public static IDictionary<Player, int> GuessesDictionary(List<GameState> gameStates)
        {
            var result = new Dictionary<Player, int>();
            gameStates.ForEach(a => result.Add(a.Player, a.Guesses));
            return result;
        }

        public static IDictionary<Player, int> WinsDictionary(List<GameState> gameStates)
        {
            var result = new Dictionary<Player, int>();
            gameStates.ForEach(a => result.Add(a.Player, a.Wins));
            return result;
        }
    }
}

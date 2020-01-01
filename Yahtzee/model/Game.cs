using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.IO;
using Newtonsoft.Json;

namespace Yahtzee.model
{
    [JsonObject(MemberSerialization.OptIn)]
    class Game
    {
        [JsonProperty]
        private List<Player> m_players = new List<Player>();

        [JsonProperty]
        private int m_rollsPerRound = 3; ////// should this be here/hardcoded ??

        public Game (Dictionary<string, int> a_players)
        {
            a_players.ToList().ForEach(player => 
                m_players.Add(new Player(player.Key, player.Value, new strategy.StrategyFactory())));

            CreatedDate = DateTime.Now;
            Status = "InProgress";
            Round = 1;
            NextPlayer = 0;
        }

        [JsonProperty]
        public DateTime CreatedDate { get; }

        [JsonProperty]
        public string Status { get; private set; }

        [JsonProperty]
        public int Round { get; private set; }

        [JsonProperty]
        public int NextPlayer { get; private set; }
 
        public bool Play()
        {
            var currentPlayer = m_players[NextPlayer];

            if (currentPlayer.PlayerType == Player.Type.Gamer)
            {
                return true;
            }
            else if (currentPlayer.PlayerType == Player.Type.Computer)
            {
                currentPlayer.PlayRound(m_rollsPerRound);
            }
            else
            {
                throw new Exception("Player Type is not recognised.");
                // thread?
                // play() again?
            }
            
            return false;


             ///  TESTING
            // Console.WriteLine("BLAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA");
            // Console.WriteLine($"THIS IS THE CURRENT PLAYER: {currentPlayer.Name}");
            // Thread.Sleep(2000);

            /*
            foreach (Player p in m_players)
            {
                for (int i = 0; i < 13; i++)
                {
                    p.PlayRound(m_rollsPerRound);
                }

                var scores = p.ScoreCard.GetScores();
                Console.WriteLine($"Player name: {p.Name}, Final Score {scores[15]}");
            }
            */
        }

        public void SaveGame()
        {
            // if (game not finished)
            string json = JsonConvert.SerializeObject(this, Formatting.Indented);
            File.WriteAllText(@"c:\Users\amids\1dv607\yahtzee_1dv607\Yahtzee\data\gameInProgress.json", json);

            // else (ie game complete)
            // string appendText = "This is extra text" + Environment.NewLine;
            // File.AppendAllText(path, appendText);

        }
    }
}

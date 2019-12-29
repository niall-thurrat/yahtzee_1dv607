using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

        public Game ()
        {
            CreatedDate = DateTime.Now;
        }

        [JsonProperty]
        public DateTime CreatedDate { get; }
 
        public void NewGame(Dictionary<string, int> a_players)
        {
            // create players
            a_players.ToList().ForEach(player => 
                m_players.Add(new Player(player.Key, player.Value, new strategy.StrategyFactory())));

            // DEAL WITH GAMERS!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
            foreach (Player p in m_players)
            {
                for (int i = 0; i < 13; i++)
                {
                    p.PlayRound(m_rollsPerRound);

                    var scores = p.ScoreCard.GetScores();
                    Console.WriteLine($"Player name: {p.Name}, Final Score {scores[15]}");
                }
            }
        }

        public void SaveGame()
        {
            // if (game not finished)
            string json = JsonConvert.SerializeObject(this, Formatting.Indented);
            File.WriteAllText(@"c:\Users\amids\1dv607\yahtzee_1dv607\Yahtzee\data\currentGame.json", json);

            // else (ie game complete)
            // string appendText = "This is extra text" + Environment.NewLine;
            // File.AppendAllText(path, appendText);

        }
    }
}

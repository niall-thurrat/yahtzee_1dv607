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
            NextPlayerIndex = 0;
        }

        [JsonProperty]
        public DateTime CreatedDate { get; }

        [JsonProperty]
        public string Status { get; private set; }

        [JsonProperty]
        public int Round { get; private set; }

        [JsonProperty]
        public int NextPlayerIndex { get; private set; }
 
        public bool Play()
        {
            var nextPlayer = m_players[NextPlayerIndex];

            // computer players do their stuff until it's a gamer's turn or game ends
            while (nextPlayer.PlayerType == Player.Type.Computer
                && Status == "InProgress")
            {
                nextPlayer.PlayRound(m_rollsPerRound);
                UpdateGameProgress();
                nextPlayer = m_players[NextPlayerIndex];
            }
            
            if (nextPlayer.PlayerType == Player.Type.Gamer)
            {

                nextPlayer.RollDice();
                return true;
            }

            return false;
        }

        public void GamerRolls()
        {
            var nextPlayer = m_players[NextPlayerIndex]; // should this be a property on the game class?  in play above also

            nextPlayer.RollDice();
        }

        // TEST CODE
        // Console.WriteLine($"NEXT PLAYER INDEX: {NextPlayerIndex}");
        // Thread.Sleep(1500);

        public void UpdateGameProgress()
        {
            // If there's another player to play in this round
            if (NextPlayerIndex < m_players.Count - 1)
            {
                NextPlayerIndex++;
            }
            // else start a new round
            else
            {   
                Round++;
                NextPlayerIndex = 0;

                // game is finished after 13 rounds ///hard coded 13 round here !!!!!!!!!!!!!!!!
                if (Round == 14)
                {
                    Status = "Finished";
                }
            }
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

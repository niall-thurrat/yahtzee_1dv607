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
                m_players.Add(new Player(
                    player.Key,
                    player.Value,
                    m_rollsPerRound,
                    new strategy.StrategyFactory())));

            CreatedDate = DateTime.Now;
            Status = "InProgress";
            Round = 1;
            CurrentPlayerIndex = 0;
        }

        [JsonProperty]
        public DateTime CreatedDate { get; }

        [JsonProperty]
        public string Status { get; private set; }

        [JsonProperty]
        public int Round { get; private set; }

        [JsonProperty]
        public int CurrentPlayerIndex { get; private set; }
 
        public bool Play()
        {
            var player = m_players[CurrentPlayerIndex];

            // computer players do their stuff until gamer's turn or game ends
            while (player.PlayerType == Player.Type.Computer
                && Status == "InProgress")
            {
                player.PlayRound();
                UpdateGameProgress();
                player = m_players[CurrentPlayerIndex];
            }
            
            if (player.PlayerType == Player.Type.Gamer)
            {
                player.RollDice();
                return true;
            }

            return false;
        }

        public void UpdateGameProgress()
        {
            // If there's another player to play in this round
            if (CurrentPlayerIndex < m_players.Count - 1)
            {
                CurrentPlayerIndex++;
            }
            // else start a new round
            else
            {   
                Round++;
                CurrentPlayerIndex = 0;

                // game is finished after 13 rounds ///hard coded 13 round here !!!!!!!!!!!!!!!!
                if (Round == 14)
                {
                    Status = "Finished";
                }
            }
        }

        public void GamerRolls()
        {
            var player = m_players[CurrentPlayerIndex]; // should this be a property on the game class?  in play above also
            player.RollDice();
        }

        public void GamerHoldsDie(int dieIndex)
        {
            var player = m_players[CurrentPlayerIndex];
            player.HoldDie(dieIndex);
        }

        public void GamerSelectsCat(int categoryIndex)
        {
            var player = m_players[CurrentPlayerIndex];
            var dice = player.Dice;
            var cats = player.ScoreCard.GetCategories();

            foreach (Category c in cats)
            {
                if ((int)c.CatType == categoryIndex)
                {
                    player.ScoreCard.Update(dice, c.CatType);
                }
            }
            
            /// /////////////////////////////////////////////////////////////////////////// HANDLE USER INPUT ERROR
        }

        // TEST CODE
        // Console.WriteLine($"NEXT PLAYER INDEX: {NextPlayerIndex}");
        // Thread.Sleep(1500);

        public int GetRollsLeft()
        {
            var player = m_players[CurrentPlayerIndex];
            return player.RollsLeft;
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

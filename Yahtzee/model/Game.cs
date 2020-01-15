using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace Yahtzee.model
{
    [Serializable]
    [JsonObject(MemberSerialization.OptIn)]
    class Game
    {
        public enum GameStatus
        {
            InProgress = 0,
            Finished,
            Unfinished,
            Discarded
        }

        [JsonProperty]
        private List<Player> m_players = new List<Player>();

        public Game (Dictionary<string, int> a_players, int a_rollsPerRound)
        {
            a_players.ToList().ForEach(player => 
                m_players.Add(new Player(
                    player.Key,
                    player.Value,
                    a_rollsPerRound,
                    new strategy.StrategyFactory())));

            CreatedDate = DateTime.Now;
            RollsPerRound = a_rollsPerRound;
            Status = GameStatus.InProgress;
            CurrentRound = 1;
            CurrentPlayerIndex = 0;
        }

        [JsonProperty]
        public DateTime CreatedDate { get; }

        [JsonProperty]
        public int RollsPerRound { get; }

        [JsonProperty]
        public GameStatus Status { get; set; }

        [JsonProperty]
        public int CurrentRound { get; private set; }

        [JsonProperty]
        public int CurrentPlayerIndex { get; private set; }
 
        public bool ComputerPlays()
        {
            var player = m_players[CurrentPlayerIndex];

            if (player.PlayerType == Player.Type.Computer && Status == GameStatus.InProgress)
            {
                player.ComputerPlaysRound();
                UpdateGameProgress();
                return true;
            }

            return false;
        }

        private void UpdateGameProgress()
        {
            // If there's another player to play in this round
            if (CurrentPlayerIndex < m_players.Count - 1)
            {
                CurrentPlayerIndex++;
            }
            // else start a new round
            else
            {   
                CurrentRound++;
                CurrentPlayerIndex = 0;

                // game is finished after 13 rounds 
                /// HARDCODED 13 ROUNDS
                if (CurrentRound == 14)
                {
                    Status = GameStatus.Finished;
                }
            }
        }

        public bool IsGamerNext()
        {
            var player = m_players[CurrentPlayerIndex];
            return player.PlayerType == Player.Type.Gamer ? true : false;
        }

        public void GamerRolls()
        {
            var player = m_players[CurrentPlayerIndex];
            player.RollDice();
        }

        public void GamerHoldsDie(int dieIndex)
        {
            var player = m_players[CurrentPlayerIndex];
            player.HoldDie(dieIndex);
        }

        public bool GamerSelectsCat(int categoryIndex)
        {
            try
            {
                var player = m_players[CurrentPlayerIndex];
                var dice = player.Dice;
                var cats = player.ScoreCard.GetCategories();

                foreach (Category c in cats)
                {
                    if ((int)c.CatType == categoryIndex)
                    {
                        player.ScoreCard.Update(dice, c.CatType);
                        UpdateGameProgress();
                        player.ResetRollsLeft();
                        player.UnholdAllDice();

                        return true;
                    }
                }
                return false;                
            }
            catch (Exception)
            {
                return false;
            }
        }

        public int GetRollsLeft()
        {
            var player = m_players[CurrentPlayerIndex];
            return player.RollsLeft;
        }
    }
}

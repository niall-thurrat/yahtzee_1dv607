using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Yahtzee.model
{
    class Game
    {
        private List<Player> m_players = new List<Player>();
        private int m_rollsPerRound = 3; ////// should this be here/hardcoded ??

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
    }
}

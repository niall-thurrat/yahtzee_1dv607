using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Yahtzee.model
{
    class Game
    {
        private List<Player> m_players = new List<Player>();
        private int m_rollsPerRound = 3;

        public Game()
        {
            m_players.Add(new Player("NiallBot", Player.Type.Computer, new rules.RulesFactory()));
        }

        public void PlayGame()
        {
            foreach (Player p in m_players) ////////////// FOR TESTING - COULD BE REMOVED
            {
                for (int i = 0; i < 13; i++)
                {
                    p.PlayRound(m_rollsPerRound);
                }
                p.ScoreCard.PrintFinalScore();
            }
        }

        public void NewGame()
        {
            // return m_dealer.NewGame(m_player); ////////////////////was bool!
            Console.WriteLine("OK lets play yahtzee");
            // Thread.Sleep(1000);
            // Do something!!!
        }
    }
}

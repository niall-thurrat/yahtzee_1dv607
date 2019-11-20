using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Yahtzee.model
{
    class Game
    {
        private List<Player> m_players = new List<Player>();

        public Game()
        {
            m_players.Add(new Player("NiallBot", 0));
        }

        public void PlayGame()
        {
            foreach (Player p in m_players)
            {
                p.PlayRound();
            }
        }
    }
}

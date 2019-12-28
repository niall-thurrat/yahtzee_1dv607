using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using UserInput = Yahtzee.view.UI.MainOption;

namespace Yahtzee.controller
{
    class Application
    {
        private view.UI m_view;
        private model.Game m_game;

		public Application(view.UI a_view, model.Game a_game)
		{
			m_view = a_view;
            m_game = a_game;
		}

        public bool Play()
        {
            m_view.DisplayWelcomeMessage();

            // m_view.DisplayPlayerDice(m_game.GetPlayerDice(), m_game.GetPlayerScore());

            // if (m_game.IsGameOver())
            // {
            //     m_view.DisplayGameOver(m_game.IsDealerWinner());
            // }

            UserInput input = m_view.GetInput();

            if (input == UserInput.Play)
            {
                m_game.NewGame();
            }
            else if (input == UserInput.Continue)
            {
                // m_game.Hit();
            }
            else if (input == UserInput.ViewPrevious)
            {
                // m_game.Stand();
            }

            return input != UserInput.Quit;
        }
    }
}

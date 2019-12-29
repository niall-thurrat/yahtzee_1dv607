using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using MainMenuInput = Yahtzee.view.UI.MainMenuInput;

namespace Yahtzee.controller
{
    class Application
    {
        private view.UI m_view;
        private model.Game m_game;

		public Application(view.UI a_view)
		{
			m_view = a_view;
		}

        public void Run()
        {
            m_view.DisplayMainMenu();

            // m_view.DisplayPlayerDice(m_game.GetPlayerDice(), m_game.GetPlayerScore());

            // if (m_game.IsGameOver())
            // {
            //     m_view.DisplayGameOver(m_game.IsDealerWinner());
            // }

            MainMenuInput input = m_view.GetMainInput();

            if (input == MainMenuInput.Play)
            {
                m_game = new model.Game();
                var players = m_view.GetPlayers();

                m_game.NewGame(players);
            }
            else if (input == MainMenuInput.Continue)
            {
                // m_game.Hit();
            }
            else if (input == MainMenuInput.ViewPrevious)
            {
                // m_game.Stand();
            }
            else
            {
                // m_game.QuitApp();;
            }
        }
    }
}

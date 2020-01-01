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

            var input = m_view.GetMainInput();

            /// ///////////////////////////////////////// CHANGE TO SWITCH
            if (input == MainMenuInput.Play)
            {
                var players = m_view.GetPlayers();
                m_game = new model.Game(players);
                
                bool IsGamersTurn = m_game.Play();

                if (IsGamersTurn)
                {
                    m_view.DisplayGameMenu();
                }

                // m_game.SaveGame();

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

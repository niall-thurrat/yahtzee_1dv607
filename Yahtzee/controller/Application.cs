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

        public bool Run()
        {
            m_view.DisplayMainMenu();

            var input = m_view.GetMainInput();

            /// ///////////////////////////////////////// CHANGE TO SWITCH
            if (input == MainMenuInput.Play)
            {
                var players = m_view.GetPlayers();
                m_game = new model.Game(players);
                
                while (m_game.Status == "InProgress")
                {
                    // computer players continue to play until it's a gamer's turn (true) or game ends (false)
                    bool GamerToPlay = m_game.Play();

                    // GAMER'S TURN
                    if (GamerToPlay)
                    {
                        m_view.DisplayGameMenu();
                    }
                    // GAME OVER
                    else
                    {
                        m_game.SaveGame();
                    }
                }
            
                return true;
            }
            else if (input == MainMenuInput.Continue)
            {
                // get/deserialize saved json object
                // parse necesary info
                // initialize and instanciate game object with info

                return true;
            }
            else if (input == MainMenuInput.ViewPrevious)
            {
                // menu to choose full details or short list
                // access data to display

                return true;
            }
            else if (input == MainMenuInput.Quit)
            {
                // display some sort of thanks for playing message
                // thread.sleep
                // clear
                return false;
            }

            return true;
        }
    }
}

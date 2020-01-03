using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using MainMenuInput = Yahtzee.view.UI.MainMenuInput;
using GameMenuInput = Yahtzee.view.UI.GameMenuInput;

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

            switch (input)
            {
                case MainMenuInput.Play:
                    var players = m_view.GetPlayers();
                    m_game = new model.Game(players);
                    
                    while (m_game.Status == "InProgress")
                    {
                        // computer players continue to play until it's a gamer's turn (true) or game ends (false)
                        bool GamerToPlay = m_game.Play();

                        // GAMER'S TURN
                        if (GamerToPlay)
                        {
                            while (GamerPlays());
                        }
                        // GAME OVER
                        else
                        {
                            m_game.SaveGame();
                        }
                    }
                
                    return true;

                case MainMenuInput.Continue:
                    // get/deserialize saved json object
                    // parse necesary info
                    // initialize and instanciate game object with info

                    return true;

                case MainMenuInput.ViewPrevious:
                    // menu to choose full details or short list
                    // access data to display

                    return true;

                case MainMenuInput.Quit:
                    // display some sort of thanks for playing message
                    // thread.sleep
                    // clear
                    return false;

                default:
                    // throw new Exception("menu input not recognised");
                    // thread.sleep 2 sec
                    return true;
            }
        }

        public bool GamerPlays()
        {
            m_view.DisplayGameMenu();

            var input = m_view.GetGameInput();

            switch (input)
            {
                case GameMenuInput.Roll:
                    m_game.Play("yippy");
                    /*
                    while (m_game.Status == "InProgress")
                    {
                        // computer players continue to play until it's a gamer's turn (true) or game ends (false)
                        bool GamerToPlay = m_game.Play();

                        // GAMER'S TURN
                        if (GamerToPlay)
                        {
                            GamerPlays();
                        }
                        // GAME OVER
                        else
                        {
                            m_game.SaveGame();
                        }
                    } */
                
                    return true;

                case GameMenuInput.HoldDie1:
                    // get/deserialize saved json object
                    // parse necesary info
                    // initialize and instanciate game object with info

                    return true;

                case GameMenuInput.ChooseCat:
                    // menu to choose full details or short list
                    // access data to display

                    return true;

                case GameMenuInput.Quit:
                    // player asked if would like to save
                    // handle if wants to save game
                    return false;

                default:
                    // throw new Exception("menu input not recognised");
                    // thread.sleep 2 sec
                    return true;
            }
        }
    }
}

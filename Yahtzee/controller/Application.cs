using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.IO;
using MainMenuInput = Yahtzee.view.UI.MainMenuInput;
using GameMenuInput = Yahtzee.view.UI.GameMenuInput;
using CatMenuInput = Yahtzee.view.UI.CatMenuInput;

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

            var input = m_view.GetMainInput(); /////////////////////////////////// make a game status enum

            switch (input)
            {
                case MainMenuInput.Play:
                    var players = m_view.GetPlayers();
                    m_game = new model.Game(players, 3);
                    
                    while (m_game.Status == "InProgress")
                    {
                        // COMPUTER PLAYERS PLAY (until it's a gamer's turn or game ends)
                        while(m_game.ComputerPlays());

                        // GAMER PLAYER PLAYS ONE ROUND
                        while(GamerPlaysRound());
                    }

                    if (m_game.Status == "Finished")
                    {
                        // FOR TESTING
                        Console.WriteLine("\nFINISHED");
                        Thread.Sleep(3000);
                        string gameJson = JsonConvert.SerializeObject(m_game, Formatting.Indented);
                        File.WriteAllText(@"c:\Users\amids\1dv607\yahtzee_1dv607\Yahtzee\data\gameInProgress.json", gameJson);
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
                // should this (and other switch error handling) be passsed to the UI to be thrown????????????????????????????
                    Console.WriteLine("\nERROR: menu input not recognised");
                    Thread.Sleep(3000);
                    return true;
            }
        }

        // RETURNS FALSE WHEN GAMER ROUND FINISHED
        public bool GamerPlaysRound()
        {
            int rollsLeft = m_game.GetRollsLeft();

            // Gamer gets initial dice rolled
            if (rollsLeft == m_game.RollsPerRound)
            {
                m_game.GamerRolls(); // is this game logic????????????????????????????????????????????????????????????????????
            }

            string gameJson = JsonConvert.SerializeObject(m_game, Formatting.Indented);
            
            // FOR TESTING
            File.WriteAllText(@"c:\Users\amids\1dv607\yahtzee_1dv607\Yahtzee\data\gameInProgress.json", gameJson);

            m_view.DisplayGameDetails(gameJson, m_game.CurrentPlayerIndex);
            m_view.DisplayGameMenu(rollsLeft);

            var input = m_view.GetGameInput();

            switch (input)
            {
                case GameMenuInput.Roll:
                    m_game.GamerRolls();                 

                    // FOR TESTING
                    File.WriteAllText(@"c:\Users\amids\1dv607\yahtzee_1dv607\Yahtzee\data\gameInProgress.json", gameJson);
                
                    return true;

                case GameMenuInput.HoldDie1:
                    m_game.GamerHoldsDie(0);
                    return true;
                
                case GameMenuInput.HoldDie2:
                    m_game.GamerHoldsDie(1);
                    return true;
                
                case GameMenuInput.HoldDie3:
                    m_game.GamerHoldsDie(2);
                    return true;
                
                case GameMenuInput.HoldDie4:
                    m_game.GamerHoldsDie(3);
                    return true;
                
                case GameMenuInput.HoldDie5:
                    m_game.GamerHoldsDie(4);
                    return true;

                case GameMenuInput.ChooseCat:
                    GamerChoosesCategory(gameJson);
                    m_game.UpdateGameProgress();

                    // FOR TESTING
                    File.WriteAllText(@"c:\Users\amids\1dv607\yahtzee_1dv607\Yahtzee\data\gameInProgress.json", gameJson);

                    return false;

                case GameMenuInput.Quit:
                    // m_view.DisplaySaveOption()
                    // var input = m_view.GetSaveDecision();

                    // QUIT GAME - WITH SAVE
                    // if (input == SaveMenuInput.Save)
                    // {
                    //     m_game.Status == "Saved"
                    //     m_game.SaveGame();
                    // }
                    // QUIT GAME / WITHOUT SAVE
                    // else (input == SaveMenuInput.NoSave)
                    // {
                    //     m_game.Status == "Delete"
                    // }

                    m_game.Status = "TEST CHANGE";
                    return false;

                default:
                    Console.WriteLine("\nERROR: menu input not recognised");
                    Thread.Sleep(3000);
                    return true;
            }
        }

        public void GamerChoosesCategory(string gameJson)
        {
            m_view.DisplayGameDetails(gameJson, m_game.CurrentPlayerIndex);
            m_view.DisplayCategoryMenu();

            var input = m_view.GetCatInput();

            switch (input)
            {
                case CatMenuInput.Ones:
                    m_game.GamerSelectsCat((int)input);
                    break;
/*
                case GameMenuInput.HoldDie1:
                    m_game.GamerHoldsDie(0);
                    return true;
                
                case GameMenuInput.HoldDie2:
                    m_game.GamerHoldsDie(1);
                    return true;
                
                case GameMenuInput.HoldDie3:
                    m_game.GamerHoldsDie(2);
                    return true;
                
                case GameMenuInput.HoldDie4:
                    m_game.GamerHoldsDie(3);
                    return true;
                
                case GameMenuInput.HoldDie5:
                    m_game.GamerHoldsDie(4);
                    return true;

                case GameMenuInput.ChooseCat:
                    GamerChoosesCategory(gameJson);
                    return false;

                case GameMenuInput.Quit:
                    // player asked if would like to save
                    // handle if wants to save game
                    return false;
*/
                default:
                    Console.WriteLine("\nERROR: menu input not recognised");
                    Thread.Sleep(3000);
                    break;
            }
        }
    }
}

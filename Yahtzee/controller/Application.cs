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

            var input = m_view.GetMainInput();

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
                        SaveFinishedGame();

                        string gameJson = JsonConvert.SerializeObject(m_game, Formatting.Indented);
                        m_view.DisplayGameDetails(gameJson, m_game.CurrentPlayerIndex, m_game.CurrentRound, 0);
                        m_view.DisplayGameOver();                   
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
                    m_view.TextToConsole("Thanks for playing Yahtzee. Goodbye!");
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
            if (m_game.IsGamerNext() && m_game.Status == "InProgress")
            {
                int rollsLeft = m_game.GetRollsLeft();

                // Gamer gets initial dice rolled
                if (rollsLeft == m_game.RollsPerRound)
                {
                    m_game.GamerRolls();
                }

                string gameJson = JsonConvert.SerializeObject(m_game, Formatting.Indented);

                m_view.DisplayGameDetails(gameJson, m_game.CurrentPlayerIndex, m_game.CurrentRound, m_game.GetRollsLeft());
                m_view.DisplayGameMenu(rollsLeft);

                var input = m_view.GetGameInput();

                switch (input)
                {
                    case GameMenuInput.Roll:
                        m_game.GamerRolls();                    
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
                        return GamerSelectsCat(gameJson) ? false : true;

                    case GameMenuInput.Quit:
                        // m_view.DisplaySaveOption()
                        // var input = m_view.GetSaveDecision();

                        // QUIT GAME - WITH SAVE
                        // if (input == SaveMenuInput.Save)
                        // {
                            m_game.Status = "Unfinished";
                            SaveUnfinishedGame(gameJson);
                        // }
                        // QUIT GAME / WITHOUT SAVE
                        // else (input == SaveMenuInput.NoSave)
                        // {
                        //     m_game.Status == "Delete"
                        // }

                        return false;

                    default:
                        Console.WriteLine("\nERROR: menu input not recognised"); // move to view
                        Thread.Sleep(3000);
                        return true;
                }
            }

            return false;
        }

        public bool GamerSelectsCat(string gameJson)
        {
            int rollsLeft = m_game.GetRollsLeft();
            m_view.DisplayGameDetails(gameJson, m_game.CurrentPlayerIndex, m_game.CurrentRound, rollsLeft);
            m_view.DisplayCategoryMenu();

            var catMenuinput = m_view.GetCatInput();
            return m_game.GamerSelectsCat((int)catMenuinput);
        }

        public void SaveUnfinishedGame(string gameJson)
        {
            File.WriteAllText(@"c:\Users\amids\1dv607\yahtzee_1dv607\Yahtzee\data\gameInProgress.json", gameJson);
        }

        public void SaveFinishedGame()
        {
            // get/parse object from file
            // check if there are 10 entries

            // IF NOT 10 add
            // IF 10, remove oldest then add

            // serialise/send to file


            // string json = JsonConvert.SerializeObject(this, Formatting.Indented);
            // File.WriteAllText(@"c:\Users\amids\1dv607\yahtzee_1dv607\Yahtzee\data\gameInProgress.json", json);

            // string appendText = "This is extra text" + Environment.NewLine;
            // File.AppendAllText(path, appendText);

        }
    }
}

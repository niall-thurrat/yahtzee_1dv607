using Newtonsoft.Json;
using System.IO;
using MainMenuInput = Yahtzee.view.UI.MainMenuInput;
using GameMenuInput = Yahtzee.view.UI.GameMenuInput;
using ListInput = Yahtzee.view.UI.ListInput;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

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
                    // SKIPS PLAYER/GAME CREATION IF CONTINUING SAVED GAME // for this to work the object will need deleted when saved! otherwise only 1 game can be played because of this if statement
                    if (m_game == null)
                    {
                        var players = m_view.GetPlayers();
                        m_game = new model.Game(players, 3);
                    }
                    
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
                        m_view.DisplayGameDetails(
                            gameJson, m_game.CurrentPlayerIndex, m_game.CurrentRound, 0);
                        m_view.DisplayGameOver();                   
                    }

                    return true;

                case MainMenuInput.Continue:
                    // DESERIALIZE
                    string workingDirectory = Directory.GetCurrentDirectory();
                    string path = Path.Combine(workingDirectory, @"data\gameInProgress.txt");

                    IFormatter formatter = new BinaryFormatter();
                    Stream stream = new FileStream(path, FileMode.Open, FileAccess.Read);

                    // NEED SOME SORT OF  if (stream != nuthin)....
                    
                    m_game = (model.Game)formatter.Deserialize(stream);
                    stream.Close();

                    m_game.Status = "InProgress";
                    goto case MainMenuInput.Play;

                    // else
                    // {
                    //     m_view.TextToConsole("Sorry, there is no saved game");
                    // }
                    // return true;
                        
                case MainMenuInput.ViewPrevious:
                    var listType = m_view.GetListType();
                    
                    if (listType == ListInput.ShortList)
                    {
                        m_view.TextToConsole("THIS AIN'T BEEN DEVELOPED YET BOSS");
                    }
                    else
                    {
                        m_view.TextToConsole("THIS AIN'T BEEN DEVELOPED EITHER BOSS");
                    }

                    return true;

                case MainMenuInput.Quit:
                    m_view.TextToConsole("Thanks for playing Yahtzee. Goodbye!");
                    return false;

                default:
                    m_view.TextToConsole("\nERROR: menu input not recognised");
                    return true;
            }
        }

        // RETURNS FALSE WHEN GAMER ROUND IS FINISHED
        public bool GamerPlaysRound()
        {
            if (m_game.IsGamerNext() && m_game.Status == "InProgress")
            {
                int rollsLeft = m_game.GetRollsLeft();

                // First roll completes automatically for gamer
                if (rollsLeft == m_game.RollsPerRound)
                {
                    m_game.GamerRolls();
                }

                string gameJson = JsonConvert.SerializeObject(m_game, Formatting.Indented);

                m_view.DisplayGameDetails(
                    gameJson, m_game.CurrentPlayerIndex, m_game.CurrentRound, m_game.GetRollsLeft());
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
                                SaveUnfinishedGame();
                        // }
                        // QUIT GAME / WITHOUT SAVE
                        // else (input == SaveMenuInput.NoSave)
                        // {
                        //     m_game.Status == "Delete"
                        // }

                        return false;

                    default:
                        m_view.TextToConsole("\nERROR: menu input not recognised");
                        return true;
                }
            }

            return false;
        }

        public bool GamerSelectsCat(string gameJson)
        {
            int rollsLeft = m_game.GetRollsLeft();

            m_view.DisplayGameDetails(
                gameJson, m_game.CurrentPlayerIndex, m_game.CurrentRound, rollsLeft);
            m_view.DisplayCategoryMenu();

            var CatMenuInput = m_view.GetCatInput();

            return m_game.GamerSelectsCat((int)CatMenuInput);
        }

        public void SaveUnfinishedGame()
        {
            string workingDirectory = Directory.GetCurrentDirectory();
            string path = Path.Combine(workingDirectory, @"data\gameInProgress.txt");

            if (!File.Exists(path))
            {
                string newDir = Path.Combine(workingDirectory, "data");
                Directory.CreateDirectory(newDir);
            }

            IFormatter formatter = new BinaryFormatter();
            Stream stream = new FileStream(path, FileMode.Create, FileAccess.Write);

            formatter.Serialize(stream, m_game);
            stream.Close();
        }

        public void SaveFinishedGame()
        {
            // NEED TO WORK OUT A WAY TO SERIALIZE MULTIPLE GAMES
        }
    }
}

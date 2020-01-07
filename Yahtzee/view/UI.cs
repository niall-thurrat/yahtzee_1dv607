using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Yahtzee.view
{
    class UI
    {
        // //////////////////////////////////// can this be done with menu items???
        public enum MainMenuInput
        {
            Play = 0,
            Continue,
            ViewPrevious,
            Quit,
            InvalidEntry
        }

        public enum GameMenuInput
        {
            Roll = 0,
            HoldDie1,
            HoldDie2,
            HoldDie3,
            HoldDie4,
            HoldDie5,
            ChooseCat,
            Quit,
            InvalidEntry
        }

        public enum CatMenuInput
        {
            Ones = 0,
            Twos,
            Threes,
            Fours,
            Fives,
            Sixes,
            x3,
            x4,
            FullHouse,
            Small,
            Large,
            Yahtzee,
            Chance,
            InvalidEntry
        }

        public void DisplayMainMenu()
        {
            Console.Clear();
            Console.WriteLine(
                "Welcome to Yahtzee, the world's most awesome game!" +
                "\n\nMAIN MENU" +
                "\n  1 Play New Game" +
                "\n  2 Continue Saved Game" +
                "\n  3 View Previous Games" +
                "\n  4 Quit App");
                Console.Write("\nSELECTION: ");
        }

        public void DisplayGameMenu(int rollsLeft)
        {
            Console.WriteLine("\nGAME MENU");

            if (rollsLeft > 0)
            {
                Console.WriteLine(
                "  1 Hold/unhold Dice 1" +
                "\n  2 Hold/unhold Dice 2" +
                "\n  3 Hold/unhold Dice 3" +
                "\n  4 Hold/unhold Dice 4" +
                "\n  5 Hold/unhold Dice 5" +
                "\n  6 Roll The Dice");
            }

            Console.WriteLine(
                "  7 Select a category" +
                "\n  8 Quit Game");
                Console.Write("\nSELECTION: ");
        }

        public void DisplayCategoryMenu()
        {
            Console.WriteLine(
                "\nCATEGORY MENU" +
                "\n  1 Ones" +
                "\n  2 Twos" +
                "\n  3 Threes" +
                "\n  4 Fours" +
                "\n  5 Fives" +
                "\n  6 Sixes" +
                "\n  7 Three of a kind" +
                "\n  8 Four of a kind" +
                "\n  9 Full House" +
                "\n  10 Small" +
                "\n  11 Large" +
                "\n  12 Yahtzee" +
                "\n  13 Chance" +
                "\n  14 Return to Game Menu");
                Console.Write("\nSELECTION: ");
        }

        public MainMenuInput GetMainInput()
        {
            try
            {
                int input = int.Parse(Console.ReadLine());

                switch (input)
                {
                    case 1:
                        return MainMenuInput.Play;

                    case 2:
                        return MainMenuInput.Continue;
                    
                    case 3:
                        return MainMenuInput.ViewPrevious;
                    
                    case 4:
                        return MainMenuInput.Quit;

                    default:
                        return MainMenuInput.InvalidEntry;
                }
            }
            catch (Exception)
            {
                return MainMenuInput.InvalidEntry;
            }
        }

        // handle roll and hold being selected when they shouldn't - if statements required in case 1-6 !!!!!!!!!!!!!!!!!!!

        public GameMenuInput GetGameInput()
        {
            int input = int.Parse(Console.ReadLine());

            switch (input)
            {
                case 1:
                    return GameMenuInput.HoldDie1;

                case 2:
                    return GameMenuInput.HoldDie2;
                
                case 3:
                    return GameMenuInput.HoldDie3;
                
                case 4:
                    return GameMenuInput.HoldDie4;
                
                case 5:
                    return GameMenuInput.HoldDie5;
                
                case 6:
                    return GameMenuInput.Roll;
                
                case 7:
                    return GameMenuInput.ChooseCat;
                
                case 8:
                    return GameMenuInput.Quit;

                default:
                    return GameMenuInput.InvalidEntry;
            }
        }

        public CatMenuInput GetCatInput()
        {
            int input = int.Parse(Console.ReadLine());

            switch (input)
            {
                case 1:
                    return CatMenuInput.Ones;

                case 2:
                    return CatMenuInput.Twos;
                
                case 3:
                    return CatMenuInput.Threes;
                
                case 4:
                    return CatMenuInput.Fours;
                
                case 5:
                    return CatMenuInput.Fives;
                
                case 6:
                    return CatMenuInput.Sixes;
                
                case 7:
                    return CatMenuInput.x3;
                
                case 8:
                    return CatMenuInput.x4;

                case 9:
                    return CatMenuInput.FullHouse;
                
                case 10:
                    return CatMenuInput.Small;
                
                case 11:
                    return CatMenuInput.Large;
                
                case 12:
                    return CatMenuInput.Yahtzee;
                
                case 13:
                    return CatMenuInput.Chance;

                default:
                    return CatMenuInput.InvalidEntry;
            }
        }

        public Dictionary<string, int> GetPlayers()
        {
            var players = new Dictionary<string, int>();
            int playerAmount;
            int comNumber = 1;

            try
            {
                Console.Clear();
                Console.Write("\nSelect how many players (1-5) you would like to play: ");
                playerAmount = int.Parse(Console.ReadLine());

                if (playerAmount < 1 || playerAmount > 5)
                {
                    TextToConsole("\nYou didn't enter a number between 1-5!");//  refactor this into handelError() or throw custom error - prob better as func so other errors in catch blocks can call it
                    this.GetPlayers();
                }
                else
                {
                    for (int i = 0; i < playerAmount; i++)
                    {
                        string playerName = "";
                        int playerType = 0;

                        Console.Clear();
                        Console.Write($"\nSelect whether you want Player #{i + 1} to be:" +
                        "\n  1 Computer" +
                        "\n  2 Gamer" +
                        "\n\nSELECTION: ");
                        playerType = int.Parse(Console.ReadLine());

                        if (playerType < 1 || playerType > 2)
                        {
                            TextToConsole("\nYou didn't enter either number 1 or 2.... START AGAIN!");
                            this.GetPlayers();
                        }
                        else
                        {
                            if (playerType == 1)
                            {
                                playerName = $"Computer #{comNumber}";
                                playerType = 0; /////////////////////////////// feels like this should be an enum / feels very weird changing playerType from 1 to 0 also, but 0 in menu would be weird and enum should start with 0 i guess

                                players.Add(playerName, playerType);

                                comNumber++;
                            }
                            else if (playerType == 2)
                            {
                                Console.Clear();
                                Console.Write($"\nEnter the name (1-11 letters) for Player #{i + 1}: ");
                                playerName = Console.ReadLine();

                                if (playerName.Length < 1 || playerName.Length > 11)
                                {
                                    TextToConsole("\nYour name is not the right length! START AGAIN.");
                                    players.Clear();
                                    this.GetPlayers();
                                }
                                else if (players.ContainsKey(playerName))
                                {
                                    TextToConsole("\nThis name has been used! START AGAIN.");
                                    players.Clear();
                                    this.GetPlayers();
                                }
                                else
                                {
                                    playerType = 1; /////////////////////////////// as above
                                    players.Add(playerName, playerType);
                                }
                            }
                        }
                    }

                    TextToConsole("\nOK!! Now lets play yahtzee :)");
                }
            }
            catch (FormatException)
            {
                TextToConsole("\nYou didn't give a valid entry! START AGAIN");
                this.GetPlayers();
            }
            catch (Exception ex)
            {
                TextToConsole(ex.Message);
                this.GetPlayers();
            }

            return players;
        }

        public void DisplayGameOver()
        {
            Console.Write("\n\n  GAME OVER" +
                "\n\nYour game has been saved to the 10 game log book." +
                "\nPress any key to continue... ");
            Console.ReadKey();
        }
        
        public void DisplayGameDetails(string gameJson, int playerIndex, int round, int rollsLeft)
        {
            JObject o = JObject.Parse(gameJson);
            JArray players = (JArray)o.SelectToken("m_players"); // use this array to iterate through????????????
            int playersCount = players.Count;

            Console.Clear();
            Console.WriteLine("GAME CARD");

            Console.Write("--------------------");
            for (int i = 0; i < playersCount; i++)
            {
                Console.Write("|-------------");
            }

            Console.Write("\nCATEGORY            ");
            foreach (var player in players)
            {
                string playerName = (string)player.SelectToken("Name");
                Console.Write($"|{playerName, -13}");
            }

            Console.Write("\n--------------------");
            for (int i = 0; i < playersCount; i++)
            {
                Console.Write("|-------------");
            }

            Console.Write("\nOnes                "); ////// FIX THIS COPY AND PASTE MADNESS
            WriteCatScores("cat_Ones", players);

            Console.Write("\nTwos                ");
            WriteCatScores("cat_Twos", players);

            Console.Write("\nThrees              ");
            WriteCatScores("cat_Threes", players);

            Console.Write("\nFours               ");
            WriteCatScores("cat_Fours", players);

            Console.Write("\nFives               ");
            WriteCatScores("cat_Fives", players);

            Console.Write("\nSixes               ");
            WriteCatScores("cat_Sixes", players);

            Console.Write("\nUPPER SECTION BONUS ");
            WriteCatScores("cat_UpperBonus", players);

            Console.Write("\nThree of a kind     ");
            WriteCatScores("cat_x3", players);

            Console.Write("\nFour of a kind      ");
            WriteCatScores("cat_x4", players);

            Console.Write("\nFull House          ");
            WriteCatScores("cat_FullHouse", players);

            Console.Write("\nSmall               ");
            WriteCatScores("cat_Small", players);

            Console.Write("\nLarge               ");
            WriteCatScores("cat_Large", players);

            Console.Write("\nYahtzee             ");
            WriteCatScores("cat_Yahtzee", players);

            Console.Write("\nChance              ");
            WriteCatScores("cat_Chance", players);

            Console.Write("\nEXTRA YAHTZEE BONUS ");
            WriteCatScores("cat_YahtzeeBonus", players);

            Console.Write("\n------ TOTAL ------ ");
            WriteTotalScore(players);

            if (round < 14)
            {
                string name = (string)o.SelectToken($"m_players[{playerIndex}].Name");

                string d1Value = (string)o.SelectToken($"m_players[{playerIndex}].Dice[0].m_value");
                bool d1Hold = (bool)o.SelectToken($"m_players[{playerIndex}].Dice[0].IsHeld");

                string d2Value = (string)o.SelectToken($"m_players[{playerIndex}].Dice[1].m_value");
                bool d2Hold = (bool)o.SelectToken($"m_players[{playerIndex}].Dice[1].IsHeld");

                string d3Value = (string)o.SelectToken($"m_players[{playerIndex}].Dice[2].m_value");
                bool d3Hold = (bool)o.SelectToken($"m_players[{playerIndex}].Dice[2].IsHeld");

                string d4Value = (string)o.SelectToken($"m_players[{playerIndex}].Dice[3].m_value");
                bool d4Hold = (bool)o.SelectToken($"m_players[{playerIndex}].Dice[3].IsHeld");

                string d5Value = (string)o.SelectToken($"m_players[{playerIndex}].Dice[4].m_value");
                bool d5Hold = (bool)o.SelectToken($"m_players[{playerIndex}].Dice[4].IsHeld");

                Console.WriteLine($"\n\nPLAYER: {name}   ROUND: {round}   ROLLS LEFT: {rollsLeft}");
                Console.WriteLine($"CURRENT DICE: {d1Value}{IsHeld(d1Hold)}, {d2Value}{IsHeld(d2Hold)}, " +
                $"{d3Value}{IsHeld(d3Hold)}, {d4Value}{IsHeld(d4Hold)}, {d5Value}{IsHeld(d5Hold)}");
            }
        }

        private void WriteCatScores(string category, JArray players)
        {
            foreach (var player in players)
            {
                JObject scoreCard = (JObject)player.SelectToken("ScoreCard");
                JObject cat = (JObject)scoreCard.SelectToken(category);
                Nullable<int> score = ( Nullable<int>)cat.SelectToken("Score");

                if (score.HasValue)
                {
                    Console.Write($"|{score, -13}");
                }
                else
                {
                    Console.Write($"|             ");
                }
            }
        }

        private void WriteTotalScore(JArray players)
        {
            foreach (var player in players)
            {
                JObject scoreCard = (JObject)player.SelectToken("ScoreCard");
                int score = (int)scoreCard.SelectToken("TotalScore");
                Console.Write($"|{score, -13}");
            }
        }

        private void TextToConsole(string text)
        {
            Console.Clear();
            Console.WriteLine(text);
            Thread.Sleep(2000);
        }

        private string IsHeld(bool isHeld) => (isHeld) ? " (Held)" : "";
     }
}

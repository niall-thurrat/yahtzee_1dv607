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
        }

        public void DisplayGameMenu(int rollsLeft)
        {
            Console.Clear();
            Console.WriteLine("\nGAME MENU");

            if (rollsLeft > 0)
            {
                Console.WriteLine(
                "  1 Roll The Dice" +
                "\n  2 Hold Dice 1" +
                "\n  3 Hold Dice 2" +
                "\n  4 Hold Dice 3" +
                "\n  5 Hold Dice 4" +
                "\n  6 Hold Dice 5");
            }

            Console.WriteLine(
                "  7 Select a category" +
                "\n  8 Quit Game");
        }

        // ////////////////////// CHANGE TO SWITCH

        public MainMenuInput GetMainInput()
        {
            int input = int.Parse(Console.ReadLine());

            if (input == 1)
            {
                return MainMenuInput.Play;
            }
            else if (input == 2)
            {
                return MainMenuInput.Continue;
            }
            else if (input == 3)
            {
                return MainMenuInput.ViewPrevious;
            }
            else if (input == 4)
            {
                return MainMenuInput.Quit;
            }
            return MainMenuInput.InvalidEntry;
        }

        public GameMenuInput GetGameInput()
        {
            int input = int.Parse(Console.ReadLine());

            if (input == 1)
            {
                return GameMenuInput.Roll;
            }
            else if (input == 2)
            {
                return GameMenuInput.HoldDie1;
            }
            else if (input == 3)
            {
                return GameMenuInput.HoldDie2;
            }
            else if (input == 4)
            {
                return GameMenuInput.HoldDie3;
            }
            else if (input == 5)
            {
                return GameMenuInput.HoldDie4;
            }
            else if (input == 6)
            {
                return GameMenuInput.HoldDie5;
            }
            else if (input == 7)
            {
                return GameMenuInput.ChooseCat;
            }
            else if (input == 8)
            {
                return GameMenuInput.Quit;
            }
            return GameMenuInput.InvalidEntry;
        }

        public Dictionary<string, int> GetPlayers()
        {
            var players = new Dictionary<string, int>();
            int playerAmount;
            int comNumber = 1;

            try
            {
                Console.Clear();
                Console.WriteLine("\nSelect how many players (1-5) you would like to play: ");
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
                        Console.WriteLine($"\nSelect whether you want Player #{i + 1} to be:" +
                        "\n1 Computer" +
                        "\n2 Gamer");
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
                                Console.WriteLine($"\nEnter the name (1-11 letters) for Player #{i + 1}:");
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

        /// DISPLAY CURRENT PLAYER DETIALS + GAME SCORECARD (use serialized game object to do this - pass it through control)
        /// dont forget to make rendering of game details adaptable to Triple strategy and scorecard
        
        public void DisplayGameDetails(string gameJson)
        {
            JObject o = JObject.Parse(gameJson);
            int player1RollsLeft = (int)o.SelectToken("m_players[0].RollsLeft");

            Console.WriteLine("\ntheres gonna be some fun right here");
            Console.WriteLine($"{player1RollsLeft}");
        }

        private void TextToConsole(string text)
        {
            Console.Clear();
            Console.WriteLine(text);
            Thread.Sleep(2000);
        }
     }
}

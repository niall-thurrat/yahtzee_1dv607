using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

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

        public void DisplayMainMenu()
        {
            Console.Clear();
            Console.WriteLine(
                "Welcome to Yahtzee, the world's most awesome game!" +
                "\n\nMAIN MENU" +
                "\n\n1 Play New Game" +
                "\n2 Continue Saved Game" +
                "\n3 View Previous Games" +
                "\n4 Quit");
        }

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
                    Console.Clear(); //  refactor this into handelError() or throw custom error - prob better as func so other errors in catch blocks can call it
                    Console.WriteLine("\nYou didn't enter a number between 1-5!");
                    Thread.Sleep(2000);
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
                            Console.Clear();
                            Console.WriteLine("\nYou didn't enter either number 1 or 2.... START AGAIN!");
                            Thread.Sleep(2000);
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
                                    Console.Clear();
                                    Console.WriteLine("\nYour name is not the right length! START AGAIN.");
                                    Thread.Sleep(2000);
                                    players.Clear();

                                    this.GetPlayers();
                                }
                                else if (players.ContainsKey(playerName))
                                {
                                    Console.Clear();
                                    Console.WriteLine("\nThis name has been used! START AGAIN.");
                                    Thread.Sleep(2000);
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

                    Console.Clear();
                    Console.WriteLine("\nOK!! Now lets play yahtzee :)");
                    Thread.Sleep(2000);
                }
            }
            catch (FormatException)
            {
                Console.Clear();
                Console.WriteLine("\nYou didn't give a valid entry! START AGAIN");
                Thread.Sleep(2000);
                this.GetPlayers();
            }
            catch (Exception ex)
            {
                Console.Clear();
                Console.WriteLine(ex.Message);
                Thread.Sleep(2000);
                this.GetPlayers();
            }

            return players;
        }

    //     public void DisplayPlayerScoreCard(IEnumerable<model.Card> a_hand, int a_score)
    //     {
    //         DisplayHand("PLAYER", a_hand, a_score);
    //     }

    //     public void DisplayDealerHand(IEnumerable<model.Card> a_hand, int a_score)
    //     {
    //         DisplayHand("DEALER", a_hand, a_score);
    //     }

    //     private void DisplayHand(String a_name, IEnumerable<model.Card> a_hand, int a_score)
    //     {
    //         System.Console.Write("{0} HAS", a_name);

    //         if (a_hand.Count() == 0)
    //         {
    //             Console.WriteLine("  --  no cards");
    //             System.Console.WriteLine("Score: {0}", a_score);
    //             System.Console.WriteLine("");
    //         }
    //         else
    //         {
    //             foreach (model.Card c in a_hand)
    //             {
    //                 DisplayCard(c);
    //             }
    //             System.Console.WriteLine();
    //             System.Console.WriteLine("Score: {0}", a_score);
    //             System.Console.WriteLine("");
    //         }
    //     }

    //     public void DisplayCard(model.Card a_card)
    //     {
    //         System.Console.Write("  --  {0} of {1}", a_card.GetValue(), a_card.GetColor());
    //     }

    //     public void DisplayGameOver(bool a_dealerIsWinner)
    //     {
    //         System.Console.Write("GameOver: ");
    //         if (a_dealerIsWinner)
    //         {
    //             System.Console.WriteLine("Dealer Won!");
    //         }
    //         else
    //         {
    //             System.Console.WriteLine("You Won!");
    //         }
            
    //     }
     }
}

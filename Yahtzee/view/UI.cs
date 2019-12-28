using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Yahtzee.view
{
    class UI
    {
        public enum MainOption
        {
            Play = 0,
            Continue,
            ViewPrevious,
            Quit,
            InvalidEntry
        }

        public void DisplayWelcomeMessage()
        {
            System.Console.Clear();
            System.Console.WriteLine("Welcome to Yahtzee, the world's most awesome game!");
            System.Console.WriteLine(
                "\n\nPlay New Game - type 'p'" +
                "\nContinue Saved Game - type 'c'" +
                "\nView Previous Games - type 'v'" +
                "\nQuit - type 'q'"
                );
        }

        public MainOption GetInput()
        {
            int input = System.Console.In.Read();

            if (input == 'p')
            {
                return MainOption.Play;
            }
            else if (input == 'c')
            {
                return MainOption.Continue;
            }
            else if (input == 'v')
            {
                return MainOption.ViewPrevious;
            }
            else if (input == 'q')
            {
                return MainOption.Quit;
            }
            return MainOption.InvalidEntry;
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

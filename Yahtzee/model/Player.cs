using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Yahtzee.model
{
    class Player
    {
        public enum Type
        {
            Computer = 0,
            Gamer
        }

        private String m_name;
        private Type m_playerType;
        private ScoreCard m_scoreCard = new ScoreCard();
        private List<Die> m_dice = new List<Die>();
        private int rollsLeft = 3; //////////////////////////////////////// should this be here?

        public Player(String a_name, Type a_playerType)
        {
            m_name = a_name;
            m_playerType = a_playerType;
        }

        public void PlayRound()
        {
            RollDice();
            GetValues();
            Strategy();
        }

        private void RollDice()
        {
            ClearDice(); // will need changed as dice are held /////////////////////////////

            for (int i = 0; i < 5; i++)
            {
                m_dice.Add(new Die());
                m_dice[i].Roll();
            }
        }

        private void FreezeDie(int dieNumber)
        {
            for (int i = 0; i < 5; i++)
            {
                if (dieNumber == i)
                {
                    m_dice[i].ChangeStatus(0); ////////////////////// adapt this method to freeze AND roll??
                }
            }
        }

        private void GetValues()
        {
            foreach (Die die in m_dice)
            {
                Console.Write($"-  {die.GetValue()}  ");
            }
            Console.WriteLine();
        }

        public void ClearDice()
        {
            m_dice.Clear();
        }

       public void Strategy() // WILL BE REQUIRED: ScoreCard m_scoreCard, List<Die> m_dice, int rollsLeft
        {
            // m_scoreCard.GetScores(m_dice); returns a dictionary?? Ones: 2, Twos: 0  ...

          //  if (rollsLeft == 0)
         //   {
                m_scoreCard.UpdateScoreCard("Ones", m_dice); // which! m_scoreCard.m_categories[0].GetName() , m_dice
                m_scoreCard.PrintScoreCard();
         //   }
            /* foreach (Die d in m_dice)
            {
                if (d.GetValue() == 5)
                {
                    d.ChangeStatus(Die.Status.Freeze);
                }
            } 
        }        */
        } 

        public int CalcScore()
        {
            int score = 0; /// total of dice value - move into scorecard or strategy??

            foreach (Die die in m_dice)
            {
                score += die.GetValue();
            }

            return score;
       }
    }
}

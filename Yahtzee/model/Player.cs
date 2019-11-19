using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Yahtzee.model
{
    class Player
    {
        private ScoreCard m_scoreCard = new ScoreCard();
        private List<Die> m_dice = new List<Die>();
        private int rollsLeft = 3; //////////////////////////////////////// should this be here?

        public void Strategy(ScoreCard m_scoreCard, List<Die> m_dice, int rollsLeft)
        {
            // will become separate class which plugs into  interface
            // decision making
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

        private void updateScoreCard()
        {
            // do something
        }

        public void GetValues()
        {
            RollDice();

            // /*
            foreach (Die die in m_dice)
            {
                Console.Write($"-  {die.GetValue()}  ");
            }
                Console.WriteLine();
                Console.WriteLine($"TOTAL ROLL VALUE: {this.CalcScore()}  "); //*/

                m_scoreCard.CheckDice(m_dice);
        }

        public void ClearDice()
        {
            m_dice.Clear();
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

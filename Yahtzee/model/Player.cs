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

        public void RollDice()
        {
            ClearDice(); // will need changed as dice are held /////////////////////////////

            for (int i = 0; i < 5; i++)
            {
                m_dice.Add(new Die());
                m_dice[i].Roll();
            }
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
            int score = 0;

            foreach (Die die in m_dice)
            {
                score += die.GetValue();
            }

            return score;
       }
    }
}

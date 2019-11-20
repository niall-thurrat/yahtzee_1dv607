using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Yahtzee.model.rules
{
    class OriginalPlayStrategy : IPlayStrategy
    {
        public void Use(ScoreCard scoreCard, List<Die> dice, int rollsLeft) 
        {
            // maybe model. needed
            // m_scoreCard.GetScores(m_dice); returns a dictionary?? Ones: 2, Twos: 0  ...

            //  if (rollsLeft == 0)
            //   {
                scoreCard.UpdateScoreCard("Ones", dice); // which! m_scoreCard.m_categories[0].GetName() , m_dice
                scoreCard.PrintScoreCard();
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

        public int CalcScore(List<Die> m_dice)
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
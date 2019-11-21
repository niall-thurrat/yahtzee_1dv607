using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Yahtzee.model.rules
{
    class OriginalPlayStrategy : IPlayStrategy
    {
        /// returns a Category.Type which inform Player which category to score with or else NoCategory
        public Category.Type Use(Player player)
        {
            //var scoreCard = player.GetScoreCard();
            // var dice = player.GetDice();

            // CHECK YAHTZEE
            if (player.GetScoreCard().IsYahtzee(player.GetDice()))
            {
                return Category.Type.Yahtzee;
            }

            //  if (rollsLeft == 0)
            //   {
             //  TEST
             //   scoreCard.UpdateScoreCard("Ones", dice); // which! m_scoreCard.m_categories[0].GetName() , m_dice
            //    scoreCard.PrintScoreCard();
            //   }
        /* foreach (Die d in m_dice)
            {
                if (d.GetValue() == 5)
                {
                    d.ChangeStatus(Die.Status.Freeze);
                }
            } 
        }        */
            return Category.Type.NoCategory;
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
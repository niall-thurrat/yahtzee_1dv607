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
            var scoreCard = player.ScoreCard;
            var dice = player.GetDice();
          //  var rollsLeft = player.

            // CHECK BONUS YAHTZEE
            if (scoreCard.IsBonusYahtzee(dice))
            {
                return Category.Type.YahtzeeBonus;
            }

            // CHECK YAHTZEE
            if (scoreCard.IsYahtzee(dice))
            {
                return Category.Type.Yahtzee;
            }

            // CHECK FOR 3 OR 4 OF A KIND AN EMPTY UPPER CATEGORY
            if (scoreCard.IsThreeOfAKind(dice) || scoreCard.IsFourOfAKind(dice))
            {
                //
            }

            // CHECK FOUR OF A KIND
            // if (scoreCard.IsYahtzee(dice))
            // {
            //     return Category.Type.Yahtzee;
            // }

        /* foreach (Die d in m_dice)
            {
                if (d.GetValue() == 5)
                {
                    d.ChangeStatus(Die.Status.Freeze);
                }
            } 
        }        */

            return Category.Type.FullHouse;// Category.Type.NoCategory;
        }

        public Category.Type UseBonusYahtzee(Player player)
        {
            // enter decision logic here!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!1
            return Category.Type.FullHouse;
        }

        private int CalcScore(List<Die> m_dice)
        {
            int score = 0; // delete this??

            foreach (Die die in m_dice)
            {
                score += die.GetValue();
            }

            return score;
       }
    }
}
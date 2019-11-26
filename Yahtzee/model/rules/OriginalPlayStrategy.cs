using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cat = Yahtzee.model.Category.Type;

namespace Yahtzee.model.rules
{
    class OriginalPlayStrategy : IPlayStrategy
    {
        /// returns a Category.Type which inform Player which category to score with or else NoCategory
        public Cat Use(Player player, int rollsLeft)
        {
            var scoreCard = player.ScoreCard;
            var dice = player.GetDice();

            // CHECK FOR BONUS YAHTZEE
            if (scoreCard.IsBonusYahtzee(dice))
            {
                return Cat.YahtzeeBonus;
            }

            // CHECK FOR YAHTZEE
            if (scoreCard.IsYahtzee(dice))
            {
                return Cat.Yahtzee;
            }

            // CHECK FOR LARGE SEQUENCE
            if (scoreCard.IsSequence(dice, 5) && !scoreCard.IsUsed(Cat.Large))
            {
                return Cat.Large;
            }

            // CHECK FOR 3 OR 4 OF A KIND AN EMPTY UPPER CATEGORY
            if (scoreCard.IsThreeOfAKind(dice) || scoreCard.IsFourOfAKind(dice))
            {
                //
            }

            // CHECK FOUR OF A KIND
            // if (scoreCard.IsYahtzee(dice))
            // {
            //     return Cat.Yahtzee;
            // }

        /* foreach (Die d in m_dice)
            {
                if (d.GetValue() == 5)
                {
                    d.ChangeStatus(Die.Status.Freeze);
                }
            } 
        }        */

            return Cat.FullHouse;// Cat.NoCategory;
        }

        public Cat UseBonusYahtzee(Player player)
        {
            // enter decision logic here!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!1
            return Cat.FullHouse;
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
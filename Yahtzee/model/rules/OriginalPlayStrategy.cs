using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cat = Yahtzee.model.Category.Type;

namespace Yahtzee.model.rules
{
    class OriginalPlayStrategy : IPlayStrategy
    {
        /// returns a Category.Type which informs Player which category to use on
        /// their score card. NoCategory is returned when Player should roll again
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

            if (rollsLeft == 0)
            {
                // CHECK FOR SMALL SEQUENCE
                if (scoreCard.IsSequence(dice, 4) && !scoreCard.IsUsed(Cat.Small))
                {
                    return Cat.Small;
                }

                // CHECK FOR 3 OR 4 OF A KIND IN EMPTY UPPER CATEGORY 
                if (scoreCard.IsThreeOfAKind(dice) || scoreCard.IsFourOfAKind(dice))
                {
                    int commonValue;
                    int[] values = new int [5];

                    for (int i = 0; i < values.Count(); i++)
                    {
                        values[i] = dice[i].GetValue();
                    }

                    commonValue = values.Max();

                    switch (commonValue)
                    {
                        case 1:
                            if (!scoreCard.IsUsed(Cat.Ones)) /// CREATE SOME SORT OF ScoreCareLoopThroughUpperCats() - this switch stuff looks fucking terrible // WAIT UNTIL CREATING TRIPLE STRATEGY - this might have a big influence
                            {
                                return Cat.Ones; // would really like to find shorthand for these simple if statements
                            };
                            break;
                        case 2:
                            if (!scoreCard.IsUsed(Cat.Twos))
                            {
                                return Cat.Twos;
                            };
                            break;
                        case 3:
                            if (!scoreCard.IsUsed(Cat.Threes))
                            {
                                return Cat.Threes;
                            };
                            break;
                        case 4:
                            if (!scoreCard.IsUsed(Cat.Fours))
                            {
                                return Cat.Fours;
                            };
                            break;
                        case 5:
                            if (!scoreCard.IsUsed(Cat.Fives))
                            {
                                return Cat.Fives;
                            };
                            break;
                        case 6:
                            if (!scoreCard.IsUsed(Cat.Sixes))
                            {
                                return Cat.Sixes;
                            };
                            break;  
                        default:
                            throw new Exception("Dice value not recognised");
                    }

                    // CHECK FOR EMPTY 4 OF A KIND CATEGORY 
                    if (scoreCard.IsFourOfAKind(dice) && !scoreCard.IsUsed(Cat.x4))
                    {
                        return Cat.x4;
                    }

                    // CHECK FOR EMPTY FULL HOUSE CATEGORY 
                    if (scoreCard.IsFullHouse(dice) && !scoreCard.IsUsed(Cat.FullHouse))
                    {
                        return Cat.FullHouse;
                    }

                    // CHECK FOR EMPTY 3 OF A KIND CATEGORY 
                    if (scoreCard.IsThreeOfAKind(dice) && !scoreCard.IsUsed(Cat.x3))
                    {
                        /// //////////////////////////////////////////////////////////// NEED TO CHECK FOR FULL HOUSE HERE
                        return Cat.x3;
                    }

                    // COULD IMPLEMENT IF 2 LEFT AND NOT MANY ROUNDS LEFT OR 'WITH 2 GETS OVER THE 63' LOGIC - IF YOU HAVE TIME

                    if (!scoreCard.IsUsed(Cat.Chance))
                    {
                        return Cat.Chance;
                    }

                    // COULD IMPLEMENT WASTE LARGE/YATZEE IF ONLY 2 OR 3 ROUNDS LEFT

                    // else - make ifs above else if
                    // {
                    //     Cat firstUnusedCat = scoreCard.firstUnusedCategory()
                    //     return firstUnusedCat;
                    // }

                }
            }
            // ELSE ROLLS REMAINING
            else
            {

            }

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
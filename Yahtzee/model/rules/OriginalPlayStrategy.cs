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
        /// their scorecard. NoCategory is returned when Player should roll again
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

                // CHECK FOR 3 OR 4 OF A KIND
                if (scoreCard.IsThreeOfAKind(dice) || scoreCard.IsFourOfAKind(dice))
                {
                    // USE IN UPPER SECTION IF UNUSED
                    Cat upperCat = UpdateUpperSection(scoreCard, dice);
                    if (upperCat != Cat.NoCategory)
                    {
                        return upperCat;
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
                        return Cat.x3;
                    }
                }

                // CHECK FOR EMPTY CHANCE CATEGORY
                if (!scoreCard.IsUsed(Cat.Chance))
                {
                    return Cat.Chance;
                }

                // AS A LAST RESORT - USE THE FIRST EMPTY CATEGORY FOUND ON SCORECARD
                Cat firstUnusedCat = this.GetFirstUnusedCat(player); //////////////////////////////////////////////// is this necessary here?
                return firstUnusedCat;
            }
            // ELSE IF ROLLSLEFT > 0
            else
            {
                // IF 3 OR 4 OF A KIND - HOLD THEM AND ROLL AGAIN
                if (scoreCard.IsThreeOfAKind(dice) || scoreCard.IsFourOfAKind(dice))
                {
                    int commonValue; // some copy and paste here with no rolls left scenario above - refactor? 
                    int[] values = new int [5];

                    for (int i = 0; i < values.Count(); i++)
                    {
                        values[i] = dice[i].GetValue();
                    }

                    commonValue = values.GroupBy(v => v)
                            .OrderByDescending(g => g.Count())
                            .First()
                            .Key;

                    foreach (Die d in dice)
                    {
                        if (d.GetValue() == commonValue)
                        {
                            d.IsHeld = true;
                        }
                    }
                }

                // IF SMALL SEQUENCE EXISTS...
                if (scoreCard.IsSequence(dice, 4))
                {
                    // ...AND LARGE CAT NOT USED - ROLL FOR IT
                    if (!scoreCard.IsUsed(Cat.Large))
                    {
                        HoldSequenceValues(dice, 4);
                        return Cat.NoCategory;
                    }
                    // ...AND IS NOT USED BUT LARGE IS USED - USE SMALL
                    else if (!scoreCard.IsUsed(Cat.Small))
                    {
                        return Cat.Small;
                    }
                }

                // IF 3 IN A ROW ON FIRST GO AND SMALL NOT USED
                if (scoreCard.IsSequence(dice, 3)
                    && rollsLeft == 2 
                    && !scoreCard.IsUsed(Cat.Small))
                {
                    HoldSequenceValues(dice, 3);
                        return Cat.NoCategory;
                }
                
                // IF 2 IN ROW EXISTS
                

            }

            return Cat.NoCategory;
        }

        public Cat UseBonusYahtzee(Player player)
        {
            // enter decision logic here!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
            return Cat.FullHouse;
        }

        private Cat UpdateUpperSection(IScoreCard scoreCard, List<Die> dice)
        {
            int commonValue;
            int[] values = new int [5];

            for (int i = 0; i < values.Count(); i++)
            {
                values[i] = dice[i].GetValue();
            }

            commonValue = values.GroupBy(v => v)
                            .OrderByDescending(g => g.Count())
                            .First()
                            .Key;

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

            return Cat.NoCategory;
        }
                    

        private Cat GetFirstUnusedCat(Player player)
        {
            var iterableCats = player.ScoreCard.GetCategories();
            var cat = Cat.Ones;

            foreach (Category c in iterableCats)
            {
                if (!c.IsUsed())
                {
                    cat = c.CatType;
                    break;
                }
            }
            return cat;
        }

        private void HoldSequenceValues(List<Die> dice, int sequenceAmount)
        {
            int[] values = new int [5];
 
            for (int i = 0; i < values.Count(); i++)
            {
                values[i] = dice[i].GetValue();
            }

            Array.Sort(values);

                foreach (Die d in dice)
                {
                    for (int i = 0; i < sequenceAmount; i++)
                    {
                        if (d.GetValue() == values[i])
                        {
                            d.IsHeld = true;
                            // stops value holding multiple dice
                            values[i] = 0;
                        }
                    }
                }
        }
    }
}
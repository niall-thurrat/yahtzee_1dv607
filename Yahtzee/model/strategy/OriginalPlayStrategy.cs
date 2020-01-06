using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cat = Yahtzee.model.Category.Type;

namespace Yahtzee.model.strategy
{
    class OriginalPlayStrategy : IPlayStrategy
    {
        /// returns a Category.Type which informs Player which category to use on
        /// their scorecard. NoCategory is returned when Player should roll again
        public Cat Use(Player player, int rollsLeft)
        {
            var scoreCard = player.ScoreCard;
            var dice = player.Dice;
            
            UnholdAllDice(player);

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
                Cat firstUnusedCat = GetFirstUnusedCat(player); //////////////////////////////////////////////// is this necessary here?
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
                List<int> pairs = FindTwoOfAKind(dice); 
                foreach (int pairValue in pairs)
                {
                    var iterableCats = player.ScoreCard.GetCategories();

                    foreach (Category c in iterableCats)
                    {
                        if ((pairValue == c.UpperValue) && !c.IsUsed())
                        {
                            foreach (Die d in dice)
                            {
                                if (d.GetValue() == pairValue)
                                {
                                    d.IsHeld = true;
                                }
                            }
                            return Cat.NoCategory;
                        }
                    }
                }
            }

            return Cat.NoCategory;
        }

        private void UnholdAllDice(Player player)
        {
            foreach (Die d in player.Dice)
            {
                d.IsHeld = false;
            }
        }

        public Cat UseYahtzeeBonus(Player player)
        {
            var scoreCard = player.ScoreCard;

            if (!scoreCard.IsUsed(Cat.Large))
            {
                return Cat.Large;
            }
            else if (!scoreCard.IsUsed(Cat.Sixes))
            {
                return Cat.Sixes;
            }
            else if (!scoreCard.IsUsed(Cat.x4))
            {
                return Cat.x4;
            }
            else if (!scoreCard.IsUsed(Cat.Chance))
            {
                return Cat.Chance;
            }
            else if (!scoreCard.IsUsed(Cat.FullHouse))
            {
                return Cat.FullHouse;
            }
            else return GetFirstUnusedCat(player);
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

            var iterableCats = scoreCard.GetCategories();

            foreach (Category c in iterableCats)
            {
                if ((c.UpperValue == commonValue) && !scoreCard.IsUsed(c.CatType))
                {
                    return c.CatType;
                }
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

        private List<int> FindTwoOfAKind(List<Die> dice)
        {
            int[] values = new int [5];

            for (int i = 0; i < values.Count(); i++)
            {
                values[i] = dice[i].GetValue(); // getting array of values replicated in scorecard funcs - refactor??
            }

            int count = 1, tempCount;
            int tempNumber = 0;
            List<int> frequentNumber = new List<int>();
    
            for (int i = 0; i < (values.Length - 1); i++)
            {
                tempNumber = values[i];
                tempCount = 0;
                for (int j = 0; j < values.Length ; j++)
                {
                    if (tempNumber == values[j])
                    {
                        tempCount++;
                    }
                }
                // if a new highest number count is found
                if (tempCount > count)
                {
                    frequentNumber.Clear();
                    frequentNumber.Add(tempNumber);
                    count = tempCount;
                }
                // if an equally high number count is found
                if (tempCount == count && !frequentNumber.Contains(tempNumber))
                {
                    frequentNumber.Add(tempNumber);
                    frequentNumber.Sort();
                    frequentNumber.Reverse();
                }
            }

            return frequentNumber;
        }
    }
}
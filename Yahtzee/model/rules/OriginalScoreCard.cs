using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cat = Yahtzee.model.Category.Type;

namespace Yahtzee.model.rules
{
	class OriginalScoreCard : IScoreCard
	{   
        private Category cat_Ones = new Category(Cat.Ones, 0);
        private Category cat_Twos = new Category(Cat.Twos, 0);
        private Category cat_Threes = new Category(Cat.Threes, 0);
        private Category cat_Fours = new Category(Cat.Fours, 0);
        private Category cat_Fives = new Category(Cat.Fives, 0);
        private Category cat_Sixes = new Category(Cat.Sixes, 0);
        private Category cat_UpperBonus = new Category(Cat.UpperBonus, 0);
        private Category cat_x3 = new Category(Cat.x3, Category.Section.Lower); //// perhaps use alias for Category.sec - see if its going to be removed first though
        private Category cat_x4 = new Category(Cat.x4, Category.Section.Lower);
        private Category cat_FullHouse = new Category(Cat.FullHouse, Category.Section.Lower);
        private Category cat_Small = new Category(Cat.Small, Category.Section.Lower);
        private Category cat_Large = new Category(Cat.Large, Category.Section.Lower);
        private Category cat_Yahtzee = new Category(Cat.Yahtzee, Category.Section.Lower);
        private Category cat_Chance = new Category(Cat.Chance, Category.Section.Lower);
        private Category cat_YahtzeeBonus = new Category(Cat.YahtzeeBonus, Category.Section.Lower);

        public IEnumerable<Category> GetCategories()
        {
            List<Category> catList = new List<Category>
            {
                cat_Ones, cat_Twos, cat_Threes, cat_Fours, cat_Fives, cat_Sixes, 
                cat_x3, cat_x4, cat_FullHouse, cat_Small, cat_Large, cat_Yahtzee, cat_Chance,
            };
        
            return catList.Cast<Category>();
        }

        public void Update(Cat category, List<Die> dice)
        {
            switch (category)
            {
                case Cat.Ones:
                    UpdateUpperSection(dice, 1, Cat.Ones);
                    break;
                case Cat.Twos:
                    UpdateUpperSection(dice, 2, Cat.Twos);
                    break;
                case Cat.Threes:
                    UpdateUpperSection(dice, 3, Cat.Threes);
                    break;
                case Cat.Fours:
                    UpdateUpperSection(dice, 4, Cat.Fours);
                    break;
                case Cat.Fives:
                    UpdateUpperSection(dice, 5, Cat.Fives);
                    break;
                case Cat.Sixes:
                    UpdateUpperSection(dice, 6, Cat.Sixes);
                    break;
                case Cat.x3:
                    UpdateX3(dice);
                    break;
                case Cat.x4:
                    UpdateX4(dice);
                    break;
                case Cat.FullHouse:
                    UpdateFullHouse(dice);
                    break;
                case Cat.Small:
                    UpdateSmall(dice);
                    break;
                case Cat.Large:
                    UpdateLarge(dice);
                    break;
                case Cat.Yahtzee:
                    UpdateYahtzee(dice);
                    break;
                case Cat.Chance:
                    UpdateChance(dice);
                    break;
                default:
                    throw new Exception("Scorecard category not recognised");
            }
        }

        public void UpdateWithBonusYahtzee(Cat chosenCat, List<Die> dice)
        {
            UpdateYahtzeeBonus(dice, chosenCat);
        }

        // perhaps section field on categories not required???
        private void UpdateUpperSection(List<Die> dice, int targetValue, Cat catToUpdate)
        {
            int score = 0;     

            foreach (Die die in dice)
            {
                if (die.GetValue() == targetValue)
                {
                    score += targetValue;
                }
            }

            var iterableCats = GetCategories();
            
            foreach (Category cat in iterableCats)
            {
                if (cat.CatType == catToUpdate)
                {
                    cat.Score = score;
                }
            }
            // throw new Exception("Dice value not recognised"); if no cat matches this just dies. OK? Switch handled this                 
        
            if (CheckUpperBonus() && !cat_UpperBonus.IsUsed())
            {
                cat_UpperBonus.Score = 35;
            }
        }

        private bool CheckUpperBonus()
        {
            int upperSectionScore = 0; // test this func!!!!!!!

            List<Category> upperCategories = new List<Category>
                { cat_Ones, cat_Twos, cat_Threes, cat_Fours, cat_Fives, cat_Sixes }; // AGAIN - why do they have this SECTION field?

            foreach (Category cat in upperCategories)
            {
                upperSectionScore += cat.Score; /////// this Score isn't linked ot the field!!!
            }

            // ////////////////////////////////////////////////////// do we handle if a section is selected that has been used already in this func?

            return upperSectionScore >= 63 ? true : false;
        }

        private void UpdateX3(List<Die> dice)
        {
            if (!cat_x3.IsUsed() && IsThreeOfAKind(dice))
            {
                int score = AddDiceValues(dice); //////////////////////////////// somehow refactor for x3, x4 and chance (sum of all values) to merge
                cat_x3.Score = score;
            }

            if (!cat_x3.IsUsed() && !IsThreeOfAKind(dice))
            {
                cat_x3.Score = 0;
            }

            // ////////////////////////////////////////////////////// else - handle if x3 used?
        }

        private void UpdateX4(List<Die> dice)
        {
            if (!cat_x4.IsUsed() && IsFourOfAKind(dice))
            {
                int score = AddDiceValues(dice);
                cat_x4.Score = score;
            }

            if (!cat_x4.IsUsed() && !IsFourOfAKind(dice))
            {
                cat_x4.Score = 0;
            }

            // ////////////////////////////////////////////////////// else - handle if x4 used
        }

        private void UpdateFullHouse(List<Die> dice)
        {
            if (!cat_FullHouse.IsUsed() && IsFullHouse(dice))
            {
                cat_FullHouse.Score = 25;
            }

            if (!cat_FullHouse.IsUsed() && !IsFullHouse(dice))
            {
                cat_FullHouse.Score = 0; // can be shortened if no third scenario, ie tries to use a used cat. Maybe a better way to write these anyhow?
            }

            // ////////////////////////////////////////////////////// else - handle if fullHouse used
        }

        private void UpdateSmall(List<Die> dice)
        {
            if (!cat_Small.IsUsed() && IsSequence(dice, 4))
            {
                cat_Small.Score = 30;
            }

            if (!cat_Small.IsUsed() && !IsSequence(dice, 4))
            {
                cat_Small.Score = 0;
            }

            // ////////////////////////////////////////////////////// else - handle if small used
        }

        private void UpdateLarge(List<Die> dice)
        {
            if (!cat_Large.IsUsed() && IsSequence(dice, 5))
            {
                cat_Large.Score = 40;
            }

            if (!cat_Large.IsUsed() && !IsSequence(dice, 5))
            {
                cat_Large.Score = 0;
            }

            // ////////////////////////////////////////////////////// else - handle if large used
            /// // if (cat_Large.IsUsed()) { throw new Exception ("Tried to use a category which has alread been used"); } SOMETHING LIKE THIS - USE?
        }


        private void UpdateYahtzee(List<Die> dice)
        {
            if (!cat_Yahtzee.IsUsed() && IsYahtzee(dice))
            {
                cat_Yahtzee.Score = 50;
            }

            if (!cat_Yahtzee.IsUsed() && !IsYahtzee(dice))
            {
                cat_Yahtzee.Score = 0;
            }

            // ////////////////////////////////////////////////////// else - handle if yahtzee used
        }

        private void UpdateYahtzeeBonus(List<Die> dice, Cat chosenCat)
        {
            // is this func necessary just for sake of having a private func?
            if (IsBonusYahtzee(dice) && chosenCat == Cat.FullHouse
                || chosenCat == Cat.Small || chosenCat == Cat.Large)
            {
                switch (chosenCat)
                {
                    case Cat.FullHouse:
                        cat_FullHouse.Score = 25;
                        break;
                    case Cat.Small:
                        cat_Small.Score = 30;
                        break;
                    case Cat.Large:
                        cat_Large.Score = 40;
                        break;
                    default:
                        throw new Exception("Scorecard category error");
                }
            }
            else this.Update(chosenCat, dice); // does this handle if chosenCat already taken?
        }

        private void UpdateChance(List<Die> dice)
        {
            if (!cat_Chance.IsUsed())
            {
                int score = AddDiceValues(dice);
                cat_Chance.Score = score;
            }
            // ////////////////////////////////////////////////////// else - handle if chance used
        }

        public bool IsThreeOfAKind(List<Die> dice)
        {
            int[] values = new int [5];
            int count = 0;

            for (int i = 0; i < values.Count(); i++)
            {
                values[i] = dice[i].GetValue();
            }

            int mode = values.GroupBy(v => v)
                            .OrderByDescending(g => g.Count())
                            .First()
                            .Key;

            foreach (Die d in dice)
            {
                if (d.GetValue() == mode)
                {
                    count++; 
                }
            }

            return count >= 3;
        }

        public bool IsFourOfAKind(List<Die> dice)
        {
            int[] values = new int [5]; ///////////////////////////////// refactor with 3 of a kind if you have time (incorporate so 2 possible? only useful in strategy)
            int count = 0;

            for (int i = 0; i < values.Count(); i++)
            {
                values[i] = dice[i].GetValue(); // getting array of values replicated in x3, x4, small, large, - refactor?? // also replicated in PlayStrategy
            }

            int mode = values.GroupBy(v => v)
                            .OrderByDescending(g => g.Count())
                            .First()
                            .Key;

            foreach (Die d in dice)
            {
                if (d.GetValue() == mode)
                {
                    count++; 
                }
            }

            return count >= 4;
        }

        public bool IsFullHouse(List<Die> dice)
        {
            int[] values = new int [5];

            for (int i = 0; i < values.Count(); i++)
            {
                values[i] = dice[i].GetValue(); // getting array of values replicated in x3, x4, small, large, - refactor??
            }

            var query = from i in values
                group i by i into g
                select new {g.Key, Count = g.Count()};

            int maxFrequency = query.Max(g => g.Count);
            int minFrequency = query.Min(g => g.Count);
            
            if ((maxFrequency == 3 || minFrequency == 2) &&
                (minFrequency == 3 || minFrequency == 2))
            {
                return true;
            }
            else return false;
        }
        
        public bool IsSequence(List<Die> dice, int sequenceAmount)
        {
            int[] values = new int [5];
            int highestCount = 1;
            int count = 1;

            for (int i = 0; i < values.Count(); i++)
            {
                values[i] = dice[i].GetValue();
            }

            Array.Sort(values);

            for (int i = 0; i < values.Count() - 1; i++)
            {
                if (values[i] + 1 == values[i + 1])
                {
                    count++;
                    if (count > highestCount)
                    {
                        highestCount = count;
                    }
                }
                else if (values[i] != values[i + 1])
                {
                    count = 1;
                }
            }

            return highestCount >= sequenceAmount;
        }

        public bool IsYahtzee(List<Die> dice)
        {
            int value = dice[0].GetValue();
            int count = 0;

            foreach (Die die in dice)
            {
                if (die.GetValue() == value)
                {
                    count += 1;
                }
            }
            
            return count == 5 ? true : false;
        }

        public bool IsBonusYahtzee(List<Die> dice)
        {
            if (IsYahtzee(dice) && cat_Yahtzee.IsUsed())
            {
                return true;
            }
            else return false;
        }

        public bool IsUsed(Cat queriedCatType)
        {
            var cats = GetCategories();
            bool isUsed = false; // not sure here how to get away will having this false - should return error instead of false if no cat matches

            foreach (Category c in cats)
            {
                if (c.CatType == queriedCatType)
                {
                    isUsed = c.IsUsed();
                }
            }

            return isUsed;
            // throw Exception("Category parameter not recognised"); !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
        }

        public void Print()
        {
            var cats = GetCategories();

            foreach (Category c in cats)
            {
                Console.WriteLine(c.ToString());
            }
        }

        private int AddDiceValues(List<Die> dice)
        {
            int score = 0;

            foreach (Die die in dice)
            {
                score += die.GetValue();
            }

            return score;
        }
	}
}
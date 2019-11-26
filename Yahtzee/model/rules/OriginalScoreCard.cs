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

        public void Update(Cat category, List<Die> dice)
        {
            switch (category)
            {
                case Cat.Ones:
                    UpdateUpperSection(dice, 1);
                    break;
                case Cat.Twos:
                    UpdateUpperSection(dice, 2);
                    break;
                case Cat.Threes:
                    UpdateUpperSection(dice, 3);
                    break;
                case Cat.Fours:
                    UpdateUpperSection(dice, 4);
                    break;
                case Cat.Fives:
                    UpdateUpperSection(dice, 5);
                    break;
                case Cat.Sixes:
                    UpdateUpperSection(dice, 6);
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
        private void UpdateUpperSection(List<Die> dice, int value)
        {
            int score = 0;
            
            foreach (Die die in dice)
            {
                if (die.GetValue() == value)
                {
                    score += value;
                }
            }

            switch (value)
            {
                case 1:
                    cat_Ones.UpdateScore(score);
                    break;
                case 2:
                    cat_Twos.UpdateScore(score);
                    break;
                case 3:
                    cat_Threes.UpdateScore(score);
                    break;
                case 4:
                    cat_Fours.UpdateScore(score);
                    break;
                case 5:
                    cat_Fives.UpdateScore(score);
                    break;
                case 6:
                    cat_Sixes.UpdateScore(score);
                    break;
                default:
                    throw new Exception("Dice value not recognised");
            }

            if (CheckUpperBonus() && !cat_UpperBonus.IsUsed())
            {
                cat_UpperBonus.UpdateScore(35);
            }
        }

        private bool CheckUpperBonus()
        {
            int upperSectionScore = 0; // test this func!!!!!!!

            List<Category> upperCategories = new List<Category>
                { cat_Ones, cat_Twos, cat_Threes, cat_Fours, cat_Fives, cat_Sixes }; // AGAIN - why do they have this SECTION field?

            foreach (Category cat in upperCategories)
            {
                upperSectionScore += cat.Score;
            }

            // ////////////////////////////////////////////////////// do we handle if a section is selected that has been used already in this func?

            return upperSectionScore >= 63 ? true : false;
        }

        private void UpdateX3(List<Die> dice)
        {
            if (!cat_x3.IsUsed() && IsThreeOfAKind(dice))
            {
                int score = AddDiceValues(dice); //////////////////////////////// somehow refactor for x3, x4 and chance (sum of all values) to merge
                cat_x3.UpdateScore(score);
            }

            if (!cat_x3.IsUsed() && !IsThreeOfAKind(dice))
            {
                cat_x3.UpdateScore(0);
            }

            // ////////////////////////////////////////////////////// else - handle if x3 used?
        }

        private void UpdateX4(List<Die> dice)
        {
            if (!cat_x4.IsUsed() && IsFourOfAKind(dice))
            {
                int score = AddDiceValues(dice);
                cat_x4.UpdateScore(score);
            }

            if (!cat_x4.IsUsed() && !IsFourOfAKind(dice))
            {
                cat_x4.UpdateScore(0);
            }

            // ////////////////////////////////////////////////////// else - handle if x4 used
        }

        private void UpdateFullHouse(List<Die> dice)
        {
            if (!cat_FullHouse.IsUsed() && IsFullHouse(dice))
            {
                cat_FullHouse.UpdateScore(25);
            }

            if (!cat_FullHouse.IsUsed() && !IsFullHouse(dice))
            {
                cat_FullHouse.UpdateScore(0); // can be shortened if no third scenario, ie tries to use a used cat. Maybe a better way to write these anyhow?
            }

            // ////////////////////////////////////////////////////// else - handle if fullHouse used
        }

        private void UpdateSmall(List<Die> dice)
        {
            if (!cat_Small.IsUsed() && IsSequence(dice, 4))
            {
                cat_Small.UpdateScore(30);
            }

            if (!cat_Small.IsUsed() && !IsSequence(dice, 4))
            {
                cat_Small.UpdateScore(0);
            }

            // ////////////////////////////////////////////////////// else - handle if small used
        }

        private void UpdateLarge(List<Die> dice)
        {
            if (!cat_Large.IsUsed() && IsSequence(dice, 5))
            {
                cat_Large.UpdateScore(40);
            }

            if (!cat_Large.IsUsed() && !IsSequence(dice, 5))
            {
                cat_Large.UpdateScore(0);
            }

            // ////////////////////////////////////////////////////// else - handle if large used
        }


        private void UpdateYahtzee(List<Die> dice)
        {
            if (!cat_Yahtzee.IsUsed() && IsYahtzee(dice))
            {
                cat_Yahtzee.UpdateScore(50);
            }

            if (!cat_Yahtzee.IsUsed() && !IsYahtzee(dice))
            {
                cat_Yahtzee.UpdateScore(0);
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
                        cat_FullHouse.UpdateScore(25);
                        break;
                    case Cat.Small:
                        cat_Small.UpdateScore(30);
                        break;
                    case Cat.Large:
                        cat_Large.UpdateScore(40);
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
                cat_Chance.UpdateScore(score);
            }
            // ////////////////////////////////////////////////////// else - handle if chance used
        }

        public bool IsThreeOfAKind(List<Die> dice)
        {
            int[] values = new int [5];

            for (int i = 0; i < values.Count(); i++)
            {
                values[i] = dice[i].GetValue();
            }

            var query = from i in values
                group i by i into g
                select new {g.Key, Count = g.Count()};

            int frequency = query.Max(g => g.Count);

            return frequency >= 3 ? true : false;
        }

        public bool IsFourOfAKind(List<Die> dice)
        {
            int[] values = new int [5]; ///////////////////////////////// refactor with 3 of a kind?

            for (int i = 0; i < values.Count(); i++)
            {
                values[i] = dice[i].GetValue(); // getting array of values replicated in x3, x4, small, large, - refactor?? // also replicated in PlayStrategy
            }

            var query = from i in values
                group i by i into g
                select new {g.Key, Count = g.Count()};

            int frequency = query.Max(g => g.Count);

            return frequency >= 4 ? true : false;
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
            int count = 1;
            bool gotSequence = false;

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
                   if (count == sequenceAmount)
                   {
                       gotSequence = true;
                   }
               }
               else count = 0;
            }

            return gotSequence;
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

        public bool IsUsed(Cat cat)
        {
            switch (cat)
            {
                case Cat.Ones:
                    return cat_Ones.IsUsed() ? true : false; /// ones-sixes and perhaps others not required? Removed Yahtzee already
                case Cat.Twos:
                    return cat_Twos.IsUsed() ? true : false; /// if this method is here, sure it is better to use this in this class than reference Category?
                case Cat.Threes:
                    return cat_Threes.IsUsed() ? true : false;
                case Cat.Fours:
                    return cat_Fours.IsUsed() ? true : false;
                case Cat.Fives:
                    return cat_Fives.IsUsed() ? true : false;
                case Cat.Sixes:
                    return cat_Sixes.IsUsed() ? true : false;
                case Cat.x3:
                    return cat_x3.IsUsed() ? true : false;
                case Cat.x4:
                    return cat_x4.IsUsed() ? true : false;
                case Cat.FullHouse:
                    return cat_FullHouse.IsUsed() ? true : false;
                case Cat.Small:
                    return cat_Small.IsUsed() ? true : false;
                case Cat.Large:
                    return cat_Large.IsUsed() ? true : false;
                default:
                    throw new Exception("Category parameter not recognised");
            }
        }

        public void Print()
        {
            List<Category> m_categories = new List<Category>
            {
                cat_Ones, cat_Twos, cat_Threes, cat_Fours, cat_Fives, cat_Sixes, 
                cat_UpperBonus,
                cat_x3, cat_x4, cat_FullHouse, cat_Small, cat_Large, cat_Yahtzee, cat_Chance,
                cat_YahtzeeBonus
            };
            
            foreach (Category c in m_categories)
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
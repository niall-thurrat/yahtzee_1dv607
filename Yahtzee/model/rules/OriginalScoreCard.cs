using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Yahtzee.model.rules
{
	class OriginalScoreCard : IScoreCard
	{   
        private Category cat_Ones = new Category(Category.Type.Ones, 0);
        private Category cat_Twos = new Category(Category.Type.Twos, 0);
        private Category cat_Threes = new Category(Category.Type.Threes, 0);
        private Category cat_Fours = new Category(Category.Type.Fours, 0);
        private Category cat_Fives = new Category(Category.Type.Fives, 0);
        private Category cat_Sixes = new Category(Category.Type.Sixes, 0);
        private Category cat_UpperBonus = new Category(Category.Type.UpperBonus, 0);
        private Category cat_x3 = new Category(Category.Type.x3, Category.Section.Lower);
        private Category cat_x4 = new Category(Category.Type.x4, Category.Section.Lower);
        private Category cat_FullHouse = new Category(Category.Type.FullHouse, Category.Section.Lower);
        private Category cat_Small = new Category(Category.Type.Small, Category.Section.Lower);
        private Category cat_Large = new Category(Category.Type.Large, Category.Section.Lower);
        private Category cat_Yahtzee = new Category(Category.Type.Yahtzee, Category.Section.Lower);
        private Category cat_Chance = new Category(Category.Type.Chance, Category.Section.Lower);
        private Category cat_YahtzeeBonus = new Category(Category.Type.YahtzeeBonus, Category.Section.Lower);

        public void Update(Category.Type category, List<Die> dice)
        {
            switch (category)
            {
                case Category.Type.Ones:
                    UpdateUpperSection(dice, 1);
                    break;
                case Category.Type.Twos:
                    UpdateUpperSection(dice, 2);
                    break;
                case Category.Type.Threes:
                    UpdateUpperSection(dice, 3);
                    break;
                case Category.Type.Fours:
                    UpdateUpperSection(dice, 4);
                    break;
                case Category.Type.Fives:
                    UpdateUpperSection(dice, 5);
                    break;
                case Category.Type.Sixes:
                    UpdateUpperSection(dice, 6);
                    break;
                case Category.Type.x3:
                    UpdateX3(dice);
                    break;
                case Category.Type.x4:
                    UpdateX4(dice);
                    break;
                case Category.Type.FullHouse:
                    UpdateFullHouse(dice);
                    break;
                case Category.Type.Small:
                    UpdateSmall(dice);
                    break;
                case Category.Type.Large:
                    UpdateLarge(dice);
                    break;
                case Category.Type.Yahtzee:
                    UpdateYahtzee(dice);
                    break;
                case Category.Type.Chance:
                    UpdateChance(dice);
                    break;
                default:
                    throw new Exception("Scorecard category not recognised");
            }
        }

        public void UpdateWithBonusYahtzee(Category.Type scoreCat, List<Die> dice)
        {
            UpdateYahtzeeBonus(dice, scoreCat);
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

            return upperSectionScore >= 63 ? true : false;
        }

        private void UpdateX3(List<Die> dice)
        {
            if (!cat_x3.IsUsed() && IsThreeOfAKind(dice))
            {
                int score = AddDiceValues(dice); //////////////////////////////// somehow refactor for x3, x4 and chance (sum of all values) to merge
                cat_x3.UpdateScore(score);
            }
            else Console.WriteLine("something went wrong"); ////////////////////// remove
        }

        private void UpdateX4(List<Die> dice)
        {
            if (!cat_x4.IsUsed() && IsFourOfAKind(dice))
            {
                int score = AddDiceValues(dice);
                cat_x4.UpdateScore(score);
            }
            else Console.WriteLine("something went wrong"); ////////////////////// remove
        }

        private void UpdateFullHouse(List<Die> dice)
        {
            if (!cat_FullHouse.IsUsed() && IsFullHouse(dice))
            {
                cat_FullHouse.UpdateScore(25);
            }
            else Console.WriteLine("something went wrong"); ////////////////////// remove
        }

        private void UpdateSmall(List<Die> dice)
        {
            if (!cat_Small.IsUsed() && IsSequence(dice, 4))
            {
                cat_Small.UpdateScore(30);
            }
            else Console.WriteLine("something went wrong"); ////////////////////// remove
        }

        private void UpdateLarge(List<Die> dice)
        {
            if (!cat_Large.IsUsed() && IsSequence(dice, 5))
            {
                cat_Large.UpdateScore(40);
            }
            else Console.WriteLine("something went wrong"); ////////////////////// remove
        }


        private void UpdateYahtzee(List<Die> dice)
        {
            if (!cat_Yahtzee.IsUsed() && IsYahtzee(dice))
            {
                cat_Yahtzee.UpdateScore(50);
            }
            // ////////////////////////////////////////////////////// handle if multiple yahtzees
        }

        private void UpdateYahtzeeBonus(List<Die> dice, Category.Type scoreCat)
        {
            // develop this SWITCH func !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
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

            // compute the maximum frequency
            int frequency = query.Max(g => g.Count);

            return frequency >= 3 ? true : false;
        }

        public bool IsFourOfAKind(List<Die> dice)
        {
            int[] values = new int [5]; ///////////////////////////////// refactor with 3 of a kind?

            for (int i = 0; i < values.Count(); i++)
            {
                values[i] = dice[i].GetValue(); // getting array of values replicated in x3, x4, small, large, - refactor??
            }

            var query = from i in values
                group i by i into g
                select new {g.Key, Count = g.Count()};

            // compute the maximum frequency
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

            // compute the frequency of max and min frequency values
            int maxFrequency = query.Max(g => g.Count);
            int minFrequency = query.Min(g => g.Count);
            
            if (maxFrequency == 3 || minFrequency == 2 &&
                minFrequency == 3 || minFrequency == 2)
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
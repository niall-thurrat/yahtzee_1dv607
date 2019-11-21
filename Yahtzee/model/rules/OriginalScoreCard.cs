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
        private Category cat_x3 = new Category(Category.Type.x3, Category.Section.Lower);
        private Category cat_x4 = new Category(Category.Type.x4, Category.Section.Lower);
        private Category cat_FullHouse = new Category(Category.Type.FullHouse, Category.Section.Lower);
        private Category cat_Small = new Category(Category.Type.Small, Category.Section.Lower);
        private Category cat_Large = new Category(Category.Type.Large, Category.Section.Lower);
        private Category cat_Yahtzee = new Category(Category.Type.Yahtzee, Category.Section.Lower);
        private Category cat_Chance = new Category(Category.Type.Chance, Category.Section.Lower);

        public void Update(Category.Type c, List<Die> dice)
        {
            if (c == Category.Type.Ones)
            {
                UpdateOnes(dice);
            }
            else if (c == Category.Type.Yahtzee)
            {
                UpdateYahtzee(dice);
            }
        }

        public void UpdateOnes(List<Die> dice)
        {
            int score = 0; /////// changed from private for interface implementation, but could be taken out of interface. However, not sure how these update methods will all pan out
            foreach (Die die in dice)
            {
                if (die.GetValue() == 1)
                {
                    score += 1;
                }
            }
            cat_Ones.UpdateScore(score);
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

        public void UpdateYahtzee(List<Die> dice)
        {
            if (!cat_Yahtzee.IsUsed())
            {
                cat_Yahtzee.UpdateScore(50);
            }
            // ////////////////////////////////////////////////////// handle if multiple yahtzees
        }

        public void Print()
        {
            List<Category> m_categories = new List<Category>
            {
                cat_Ones,
                cat_Yahtzee
            };
            
            foreach (Category c in m_categories)
            {
                Console.WriteLine(c.ToString());
            }
        }
	}
}
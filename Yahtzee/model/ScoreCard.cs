using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Yahtzee.model
{
	class ScoreCard
	{   
        public List<Category> m_categories = new List<Category>();

        public ScoreCard()
        {
            m_categories.Add(new Category(Category.Type.Ones, 0));
            m_categories.Add(new Category(Category.Type.Twos, 0));
            m_categories.Add(new Category(Category.Type.Threes, 0));
            m_categories.Add(new Category(Category.Type.Fours, 0));
            m_categories.Add(new Category(Category.Type.Fives, 0));
            m_categories.Add(new Category(Category.Type.Sixes, 0));
            m_categories.Add(new Category(Category.Type.x4, Category.Section.Lower));
            m_categories.Add(new Category(Category.Type.x3, Category.Section.Lower));
            m_categories.Add(new Category(Category.Type.FullHouse, Category.Section.Lower));
            m_categories.Add(new Category(Category.Type.Small, Category.Section.Lower));
            m_categories.Add(new Category(Category.Type.Large, Category.Section.Lower));
            m_categories.Add(new Category(Category.Type.Yahtzee, Category.Section.Lower));
            m_categories.Add(new Category(Category.Type.Chance, Category.Section.Lower));
        }

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

        private void UpdateOnes(List<Die> dice)
        {
            int score = 0;
            foreach (Die die in dice)
            {
                if (die.GetValue() == 1)
                {
                    score += 1;
                }
            }
            m_categories[0].UpdateScore(score);
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
            if (!m_categories[11].IsUsed())
            {
                m_categories[11].UpdateScore(50);
            }
            // ////////////////////////////////////////////////////// handle if multiple yahtzees
        }

        public void Print()
        {
            foreach (Category c in m_categories)
            {
                Console.WriteLine(c.ToString());
            }
            
        }
	}
}
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
            m_categories.Add(new Category("Ones", 0));
            m_categories.Add(new Category("Twos", 0));
            m_categories.Add(new Category("Threes", 0));
        }

        public void UpdateScoreCard(string category, List<Die> dice)
        {
            if (category == "Ones")
            {
                UpdateOnes(dice);
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

        public void PrintScoreCard()
        {
            foreach (Category c in m_categories)
            {
                Console.WriteLine(c.ToString());
            }
            
        }
	}
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Yahtzee.model
{
	class ScoreCard
	{   
        private List<Category> m_categories = new List<Category>();

        public ScoreCard()
        {
            m_categories.Add(new Category("Ones", 0, 5));
            m_categories.Add(new Category("Twos", 0, 10));
            m_categories.Add(new Category("Threes", 0, 15));
        }

        public void CheckDice(List<Die> dice)
        {
            int total_1s = CheckOnes(dice);
            Console.WriteLine($"The total number of ONEs is: {total_1s}");

            int total_2s = CheckTwos(dice);
            Console.WriteLine($"The total number of TWOs is: {total_2s}");

            int total_3s = CheckThrees(dice);
            Console.WriteLine($"The total number of THREEs is: {total_3s}");
        }

        private int CheckOnes(List<Die> dice) ////////////////////////////////////// refactor these - CheckUpperScores???
        {
            int total_1s = 0;

            foreach (Die die in dice)
            {
                if (die.GetValue() == 1)
                {
                    total_1s += 1;
                }
            }

            return total_1s;
        }

        private int CheckTwos(List<Die> dice)
        {
            int total_2s = 0;

            foreach (Die die in dice)
            {
                if (die.GetValue() == 2)
                {
                    total_2s += 1;
                }
            }

            return total_2s;
        }

        private int CheckThrees(List<Die> dice)
        {
            int total_3s = 0;

            foreach (Die die in dice)
            {
                if (die.GetValue() == 3)
                {
                    total_3s += 1;
                }
            }

            return total_3s;
        }
	}
}
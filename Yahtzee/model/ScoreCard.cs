using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Yahtzee.model
{
	class ScoreCard
	{
	/*	private List<Category> m_categories = new List<Category>();

        public ScoreCard()
        {
            m_categories.doSomething;
        }
        */

        public void CheckDice(List<Die> dice)
        {
            int result = CheckOnes(dice);
            Console.WriteLine($"The total number of ONEs is: {result}");
        }

        private int CheckOnes(List<Die> dice)
        {
            int totalOnes = 0;

            foreach (Die die in dice)
            {
                if (die.GetValue() == 1)
                {
                    totalOnes += 1;
                }
            }

            return totalOnes;
        }
	}
}
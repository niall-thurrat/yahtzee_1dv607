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
            foreach (Die die in dice)
            {
                Console.Write($"-  {die.GetValue()}  ");
            }
                Console.WriteLine();
                Console.WriteLine($"TOTAL ROLL VALUE:   "); // {this.CalcScore()}
        }
	}
}
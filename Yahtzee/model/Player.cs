using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Yahtzee.model
{
    class Player
    {
        private List<Die> m_rolledDice = new List<Die>();

        public void RollDice()
        {
            ClearDice(); // will need changed as dice are held /////////////////////////////

            for (int i = 0; i < 5; i++)
            {
                m_rolledDice.Add(new Die());
                m_rolledDice[i].Roll();
            }
        }

        public void GetValues()
        {
            RollDice();
            foreach (Die die in m_rolledDice)
            {
                Console.Write($"-  {die.GetValue()}  ");
            }
                Console.WriteLine();
                Console.WriteLine($"TOTAL ROLL VALUE: {this.CalcScore()}  ");
        }

        public void ClearDice()
        {
            m_rolledDice.Clear();
        }

        public int CalcScore()
        {
            int score = 0;

            foreach (Die die in m_rolledDice)
            {
                score += die.GetValue();
            }

            return score;
       }
    }
}

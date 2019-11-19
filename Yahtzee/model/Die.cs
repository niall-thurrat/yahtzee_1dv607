using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Yahtzee
{
    /// <summary>
    /// A die class based on Dice.cs by Tobias Ohlson
    /// https://github.com/tobias-dv-lnu/1dv607/blob/master/Component_DiceGame/DiceGame_csharp/DiceGameModel/Dice.cs
    /// </summary>
	class Dice
	{
		private int m_value;

		private static System.Random g_random = new System.Random();

		public Dice()
		{
			m_value = 0;
		}

		public void Roll()
		{
			m_value = g_random.Next(1, 7);
		}

		public int GetValue() {
			return m_value;	
		}
	}
}
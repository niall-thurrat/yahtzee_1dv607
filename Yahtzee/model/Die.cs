using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Yahtzee.model
{
    /// <summary>
    /// A die class based on Dice.cs by Tobias Ohlson
    /// https://github.com/tobias-dv-lnu/1dv607/blob/master/Component_DiceGame/DiceGame_csharp/DiceGameModel/Dice.cs
    /// </summary>
	class Die
	{
        public enum Status
        {
            Roll = 0,
            Freeze
        }

		private int m_value;
        private Status m_status;

		private static System.Random g_random = new System.Random();

		public Die()
		{
			m_value = 0;
            m_status = Status.Roll;
		}

		public void Roll()
		{
			m_value = g_random.Next(1, 7);
		}

		public int GetValue() {
			return m_value;	
		}

        public void ChangeStatus(Status newStatus)
        {
            m_status = newStatus; /////////////////////////////////////// should this just be a setter?
        }
	}
}
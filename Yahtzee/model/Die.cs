using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace Yahtzee.model
{
    /// <summary>
    /// A die class based on Dice.cs by Tobias Ohlson
    /// https://github.com/tobias-dv-lnu/1dv607/blob/master/Component_DiceGame/DiceGame_csharp/DiceGameModel/Dice.cs
    /// </summary>
	[JsonObject(MemberSerialization.OptIn)]
	class Die
	{
        [JsonProperty]
		private int m_value;
		private static System.Random g_random = new System.Random();

		public Die()
		{
			m_value = 0;
            IsHeld = false;
		}

		[JsonProperty]
		public bool IsHeld { get; set; }

		public void Roll()
		{
				m_value = g_random.Next(1, 7); // TESTvalue; param: int TESTvalue
		}

		public int GetValue() {
			return m_value;	//////////////// just a getter
		}
	}
}
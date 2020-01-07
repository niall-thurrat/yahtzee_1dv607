using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace Yahtzee.model
{
	[JsonObject(MemberSerialization.OptIn)]
	class Category
	{
		public enum Type
        {
            Ones = 0,
            Twos,
			Threes,
			Fours,
			Fives,
			Sixes,
			x3,
			x4,
			FullHouse,
			Small,
			Large,
			Yahtzee,
			Chance,
			UpperBonus,
			YahtzeeBonus,
			NoCategory
        }

		private Nullable<int> m_score = null;
		private bool m_isUsed = false;
		
		public Category(Type a_type, Nullable<int> a_upperValue)
		{
			CatType = a_type;
            UpperValue = a_upperValue;
		}

		[JsonProperty]
		public Type CatType { get; }

		public Nullable<int> UpperValue { get; }

		[JsonProperty]
		public Nullable<int> Score
		{
			get =>  m_score; 
			set
			{
				if (!m_isUsed && CatType != Type.YahtzeeBonus)
				{
					m_score = value;
					m_isUsed = true;
				}
				else if (CatType == Type.YahtzeeBonus)
				{
					if (value == m_score + 100)
					{
						m_score = value;
					}
				}
			}
		}

		public bool IsUsed()
		{
			return m_isUsed ? true : false; /////////////////////// can Nullable<int>.HasValue replace this???
		}
	}
}
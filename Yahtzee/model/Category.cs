using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Yahtzee.model
{
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
			UpperBonus,
			x3,
			x4,
			FullHouse,
			Small,
			Large,
			Yahtzee,
			Chance,
			YahtzeeBonus,
			NoCategory
        }

        private Nullable<int> m_upperValue;
		private int m_score;
		private bool m_isUsed = false;

		public Category(Type a_type, Nullable<int> a_upperValue)
		{
			CatType = a_type;
            m_upperValue = a_upperValue;
		}

		public Type CatType { get; }

		public Nullable<int> UpperValue { get => m_upperValue; }

		public int Score
			{
				get =>  m_score; 
				set
				{
					if (!m_isUsed)
					{
						m_score = value;
						m_isUsed = true;
					}
				}
			}

		public bool IsUsed()
		{
			return m_isUsed ? true : false;
		}

		public override string ToString()
		{
			return IsUsed() ? $"CATEGORY: {CatType}, SCORE: {Score}"
				: $"CATEGORY: {CatType}, SCORE: empty"; // should a nullable int be used for score??
		}
	}
}
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

        public enum Section
        {
            Upper = 0,
            Lower,
			Bonus
        }

        private Section m_section;
		private int m_score;
		private bool m_isUsed = false;

		public Category(Type a_type, Section a_section)
		{
			CatType = a_type;
            m_section = a_section;
		}

		public Type CatType { get; }

		public int Score
			{
				get =>  m_score; 
				set
				{
					if (m_isUsed == false)
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
			return IsUsed() ? $"CATEGORY: {CatType}, SECTION: {m_section}, SCORE: {Score}"
				: $"CATEGORY: {CatType}, SECTION: {m_section}, SCORE: empty";
		}
	}
}
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
			type = a_type;
            m_section = a_section;
		}

		public Type type { get; }

		public int Score { get; }

		public void UpdateScore(int a_score)
        {
				m_score = a_score;
				m_isUsed = true;
		}

		public bool IsUsed()
		{
			return m_isUsed ? true : false;
		}

		public override string ToString()
		{
			return IsUsed() ? $"CATEGORY: {type}, SECTION: {m_section}, SCORE: {m_score}"
				: $"CATEGORY: {type}, SECTION: {m_section}, SCORE: empty";
		}
	}
}
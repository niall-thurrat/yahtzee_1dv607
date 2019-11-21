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
			x3,
			x4,
			FullHouse,
			Small,
			Large,
			Yahtzee,
			Chance,
			NoCategory
        }

        public enum Section
        {
            Upper = 0,
            Lower,
			Bonus
        }

        private Section m_section;
		private bool m_isUsed = false;
        private int m_score;

		public Category(Type a_type, Section a_section)
		{
			type = a_type;
            m_section = a_section;
		}

		public Type type { get; }

		public bool IsUsed()
		{
			return m_isUsed ? true : false;
		}

		public void UpdateScore(int a_score)
        {
            //if (m_score){} ////////////////////////// shouldn't this be a simple getter and setter? should a Status enum be used here to avoid score changing?
				m_score = a_score;
				m_isUsed = true;
		}

		public override string ToString()
		{
			return IsUsed() ? $"CATEGORY: {type}, SECTION: {m_section}, SCORE: {m_score}"
				: $"CATEGORY: {type}, SECTION: {m_section}, SCORE: empty";
		}
	}
}
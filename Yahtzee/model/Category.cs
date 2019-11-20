using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Yahtzee.model
{
	class Category
	{
        public enum Section
        {
            Upper = 0,
            Lower,
			Bonus
        }

        private Section m_section;
		private bool m_isUsed = false;
        private int m_score;

		public Category(string a_name, Section a_section)
		{
			m_name = a_name;
            m_section = a_section;
		}

		public string m_name { get; }

		public string GetName()
		{
			return m_name;
		}

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
			return IsUsed() ? $"Name: {m_name}, Section: {m_section}, Score: {m_score}"
				: $"Name: {m_name}, Section: {m_section}, Score: EMPTY";
		}
	}
}
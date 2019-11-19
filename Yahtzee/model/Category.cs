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
            Lower
        }

		private string m_name;
        private Section m_section;
        private int m_possibleScore;
        private int m_score;

		public Category(string a_name, Section a_section, int a_possibleScore)
		{
			m_name = a_name;
            m_section = a_section;
            m_possibleScore = a_possibleScore;
		}

		public void CheckDice()
		{
            // do something
		}

		public void UpdateScore(int a_score)
        {
            // if bla bla
			m_score = a_score;	////////////////////////// shouldn't this be a simple getter and setter?
		}
	}
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Yahtzee.model
{
    class Player
    {
        public enum Type
        {
            Computer = 0,
            Gamer
        }

        private String m_name;
        private Type m_playerType;
        private ScoreCard m_scoreCard = new ScoreCard();
        private List<Die> m_dice = new List<Die>();
        private int rollsLeft = 3; //////////////////////////////////////// should this be here?
        private rules.IPlayStrategy m_playStrategy;

        public Player(String a_name, Type a_playerType, rules.RulesFactory a_rulesFactory)
        {
            m_name = a_name;
            m_playerType = a_playerType;
            m_playStrategy = a_rulesFactory.GetPlayStrategy();
        }

        public void PlayRound()
        {
            RollDice();
            GetValues();
            m_playStrategy.Use(m_scoreCard, m_dice, rollsLeft);
        }

        private void RollDice()
        {
            ClearDice(); // will need changed as dice are held /////////////////////////////

            for (int i = 0; i < 5; i++)
            {
                m_dice.Add(new Die());
                m_dice[i].Roll();
            }
        }

        private void FreezeDie(int dieNumber)
        {
            for (int i = 0; i < 5; i++)
            {
                if (dieNumber == i)
                {
                    m_dice[i].ChangeStatus(0); ////////////////////// adapt this method to freeze AND roll??
                }
            }
        }

        private void GetValues()
        {
            foreach (Die die in m_dice)
            {
                Console.Write($"-  {die.GetValue()}  ");
            }
            Console.WriteLine();
        }

        public void ClearDice()
        {
            m_dice.Clear();
        }
    }
}

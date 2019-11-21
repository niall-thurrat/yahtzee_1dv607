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
            bool rollAgain = true;

            while (rollAgain)
            {
                RollDice();
                GetValues(); // Just for console print - remove

                var decision = m_playStrategy.Use(this);

                if (decision != Category.Type.NoCategory)
                {
                    m_scoreCard.Update(decision, m_dice);
                }
                else
                {
                    Console.WriteLine("No Yahtzee");
                }

                rollAgain = false; /////////////////////////////////////////////////// deal with rollAgain
                
                m_scoreCard.Print();
            }
        }

        private void RollDice()
        {
            if (m_dice.Count() != 5)
            {
                for (int i = 0; i < 5; i++)
                {
                    m_dice.Add(new Die());
                }
            }

            // ClearDice(); // will need changed as dice are held /////////////////////////////

            foreach (Die d in m_dice)
            {
                if (!d.IsHeld)
                {
                    d.Roll();
                }
            }

            rollsLeft--;
        }

        private void HoldDie(int dieIndex)
        {
            for (int i = 0; i < 5; i++)
            {
                if (dieIndex == i)
                {
                    m_dice[i].IsHeld = true; /////////// perhaps IsHeld should not be so public?
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

        public ScoreCard GetScoreCard()
        {
            return m_scoreCard;
        }

        public List<Die> GetDice()
        {
            return m_dice != null ? m_dice : throw new Exception();
        }
    }
}

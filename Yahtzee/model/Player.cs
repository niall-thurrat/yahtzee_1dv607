using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cat = Yahtzee.model.Category.Type;

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
        private List<Die> m_dice = new List<Die>();
        private int rollsLeft = 3; //////////////////////////////////////// should this be here?
        private rules.IPlayStrategy m_playStrategy;

        public Player(String a_name, Type a_playerType, rules.RulesFactory a_rulesFactory)
        {
            m_name = a_name;
            m_playerType = a_playerType;
            m_playStrategy = a_rulesFactory.GetPlayStrategy();
            ScoreCard = a_rulesFactory.GetScoreCard();
        }

        public rules.IScoreCard ScoreCard { get; } // TIDY UP PROPERTIES WITH GETTERS AND SETTERS

        public void PlayRound()
        {
            bool rollAgain = true;

            while (rollAgain)
            {
                RollDice();
                GetValues(); // Just for console print - remove

                var decision = m_playStrategy.Use(this);

                if (decision != Cat.NoCategory)
                {
                    if (decision == Cat.YahtzeeBonus)
                    {
                        var bonusCatagory = m_playStrategy.UseBonusYahtzee(this);
                        ScoreCard.UpdateWithBonusYahtzee(bonusCatagory, m_dice);
                    }
                    else ScoreCard.Update(decision, m_dice);
                }
                else
                {
                    Console.WriteLine("Somethings wrong");
                }

                rollAgain = false; /////////////////////////////////////////////////// deal with rollAgain
                
                ScoreCard.Print();
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

            int[] TESTvalues = { 5, 5, 3, 3, 3 }; ////////////////////// remove TESTvalue!!!
            int TESTcount = 0; /////////////////////// remove

            foreach (Die d in m_dice)
            {
                if (!d.IsHeld)
                {
                    d.Roll(TESTvalues[TESTcount]);
                    TESTcount++; ///////////////////////// remove
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

        public List<Die> GetDice()
        {
            return m_dice != null ? m_dice : throw new Exception();
        }
    }
}

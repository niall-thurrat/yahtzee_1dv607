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
        private rules.IPlayStrategy m_playStrategy;

        public Player(String a_name, Type a_playerType, rules.RulesFactory a_rulesFactory)
        {
            m_name = a_name;
            m_playerType = a_playerType;
            m_playStrategy = a_rulesFactory.GetPlayStrategy();
            ScoreCard = a_rulesFactory.GetScoreCard();
        }

        public rules.IScoreCard ScoreCard { get; } // TIDY UP PROPERTIES WITH GETTERS AND SETTERS

        public void PlayRound(int rollsPerRound)
        {
            int rollsLeft = rollsPerRound;

            while (rollsLeft > 0)
            {
                RollDice();
                GetValues(); // Just for console print - remove
                rollsLeft--;

                Cat chosenCat = m_playStrategy.Use(this, rollsLeft);

                if (chosenCat != Cat.NoCategory)
                {
                    if (chosenCat == Cat.YahtzeeBonus)
                    {
                        Cat chosenBonusCat = m_playStrategy.UseBonusYahtzee(this);
                        ScoreCard.UpdateWithBonusYahtzee(chosenBonusCat, m_dice);
                    }
                    else ScoreCard.Update(chosenCat, m_dice);

                    rollsLeft = 0;
                    // I don't think dice need unheld after each round but keep eye on this - maybe do here with a private void UnHoldAllDice()
                    // Could use ClearDice(); instead of an unholdAll func.
                }

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

            int[] TESTvalues = { 2, 1, 3, 5, 5 }; ////////////////////// remove TESTvalue!!!
            int TESTcount = 0; /////////////////////// remove

            foreach (Die d in m_dice)
            {
                if (!d.IsHeld)
                {
                    d.Roll(TESTvalues[TESTcount]);
                    TESTcount++; ///////////////////////// remove
                }
            }
        }

        private void GetValues()
        {
            foreach (Die die in m_dice) ////////////////////////////// for testing - remove
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
            return m_dice != null ? m_dice : throw new Exception("dice not initialized yet"); ///// ERROR - they will never equal null. Isn't this just a getter anyway?
        }
    }
}

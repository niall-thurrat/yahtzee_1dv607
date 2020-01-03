using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using Cat = Yahtzee.model.Category.Type;

namespace Yahtzee.model
{
    [JsonObject(MemberSerialization.OptIn)]
    class Player
    {
        public enum Type
        {
            Computer = 0,
            Gamer
        }

        [JsonProperty]
        private List<Die> m_dice = new List<Die>();

        private strategy.IPlayStrategy m_playStrategy;

        public Player(String a_name, int a_playerType, int a_rollsLeft, strategy.StrategyFactory a_strategyFactory)
        {
            Name = a_name;
            PlayerType = (Type)a_playerType;
            m_playStrategy = a_strategyFactory.GetPlayStrategy();
            ScoreCard = a_strategyFactory.GetScoreCard();
            RollsLeft = a_rollsLeft;
        }

        [JsonProperty]
        public String Name { get; }

        [JsonProperty]
        public Type PlayerType { get; }

        [JsonProperty]
        public strategy.IScoreCard ScoreCard { get; } // TIDY UP PROPERTIES WITH GETTERS AND SETTERS

        [JsonProperty]
        public int RollsLeft { get; private set; }

        public void PlayRound(int rollsPerRound)
        {
            while (RollsLeft > 0)
            {
                RollDice();

                Cat chosenCat = m_playStrategy.Use(this, RollsLeft);

                if (chosenCat != Cat.NoCategory)
                {
                    if (chosenCat == Cat.YahtzeeBonus)
                    {
                        Cat chosenBonusCat = m_playStrategy.UseYahtzeeBonus(this);
                        ScoreCard.UpdateYahtzeeBonus(m_dice, chosenBonusCat);
                    }
                    else ScoreCard.Update(m_dice, chosenCat);

                    RollsLeft = 0;
                    // I don't think dice need unheld after each round but keep eye on this - maybe do here with a private void UnHoldAllDice()
                    ClearDice();
                }

                // TEST LINE ScoreCard.Print();
            }
        }

        public void RollDice()
        {
            if (m_dice.Count() != 5)
            {
                for (int i = 0; i < 5; i++)
                {
                    m_dice.Add(new Die());
                }
            }

            //int[] TESTvalues = { 5, 1, 3, 1, 5 }; //////////////////// remove TESTvalue!!!
            //int TESTcount = 0; /////////////////////// remove

            foreach (Die d in m_dice)
            {
                if (!d.IsHeld)
                {
                    d.Roll(); // TESTvalues[TESTcount]
                    // TESTcount++; ///////////////////////// remove
                }
            }
            
            RollsLeft--;
        }

        private void GetValues()
        {
            foreach (Die die in m_dice) ////////////////////////////// for testing - remove - or useful for UI?
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

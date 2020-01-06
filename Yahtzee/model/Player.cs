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
            RollsLeft = a_rollsLeft + 1;
        }

        [JsonProperty]
        public String Name { get; }

        [JsonProperty]
        public Type PlayerType { get; }

        [JsonProperty]
        public List<Die> Dice { 
            get { return m_dice; }
            private set { m_dice = value; }
        }        

        [JsonProperty]
        public strategy.IScoreCard ScoreCard { get; } // TIDY UP PROPERTIES WITH GETTERS AND SETTERS

        [JsonProperty]
        public int RollsLeft { get; private set; }

        public void PlayRound()
        {
            // /////////////////////////////// dont think this while loop is needed
            while (RollsLeft > 0)
            {
                RollDice();

                Cat chosenCat = m_playStrategy.Use(this, RollsLeft);

                if (chosenCat != Cat.NoCategory)
                {
                    if (chosenCat == Cat.YahtzeeBonus)
                    {
                        Cat chosenBonusCat = m_playStrategy.UseYahtzeeBonus(this);
                        ScoreCard.UpdateYahtzeeBonus(Dice, chosenBonusCat);
                    }
                    else ScoreCard.Update(Dice, chosenCat);

                    RollsLeft = 0;
                    // I don't think dice need unheld after each round but keep eye on this - maybe do here with a private void UnHoldAllDice()
                    ClearDice();
                }
            }
        }

        public void RollDice()
        {
            if (Dice.Count() != 5)
            {
                for (int i = 0; i < 5; i++)
                {
                    Dice.Add(new Die());
                }
            }

            foreach (Die d in Dice)
            {
                if (!d.IsHeld)
                {
                    d.Roll();
                }
            }
            
            RollsLeft--;
        }

        public void HoldDie(int dieIndex)
        {
            var die = Dice[dieIndex]; // should comp strategy be using this?
            die.IsHeld = die.IsHeld ? false : true;
        }

        public void ClearDice()
        {
            Dice.Clear();
        }
    }
}

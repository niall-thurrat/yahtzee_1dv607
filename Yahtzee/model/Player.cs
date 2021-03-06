using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Cat = Yahtzee.model.Category.Type;

namespace Yahtzee.model
{
    [Serializable]
    [JsonObject(MemberSerialization.OptIn)]
    class Player
    {
        public enum Type
        {
            Computer = 0,
            Gamer
        }

        private List<Die> m_dice = new List<Die>();

        private strategy.IPlayStrategy m_playStrategy;

        public Player(String a_name, int a_playerType,
            int a_rollsPerRound, strategy.StrategyFactory a_strategyFactory)
        {
            Name = a_name;
            PlayerType = (Type)a_playerType;
            m_playStrategy = a_strategyFactory.GetPlayStrategy();
            ScoreCard = a_strategyFactory.GetScoreCard();
            RollsPerRound = a_rollsPerRound;
            RollsLeft = a_rollsPerRound;
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
        public strategy.IScoreCard ScoreCard { get; }

        [JsonProperty]
        public int RollsLeft { get; private set; }

        public int RollsPerRound { get; private set; }

        public void ComputerPlaysRound()
        {
            ResetRollsLeft();

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

                        RollsLeft = 0;
                        UnholdAllDice();
                    }
                    else if (ScoreCard.Update(Dice, chosenCat))
                    {
                        RollsLeft = 0;
                        UnholdAllDice();
                    }
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
            var die = Dice[dieIndex];
            die.IsHeld = die.IsHeld ? false : true;
        }

        public void UnholdAllDice()
        {
            foreach (Die d in Dice)
            {
                if (d.IsHeld)
                {
                    d.IsHeld = false;
                }
            }
        }

        public void ResetRollsLeft()
        {
            RollsLeft = RollsPerRound;
        }
    }
}

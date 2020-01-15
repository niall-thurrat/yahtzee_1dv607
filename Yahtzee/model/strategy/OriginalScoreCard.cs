using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Cat = Yahtzee.model.Category.Type;

namespace Yahtzee.model.strategy
{
    [Serializable]
    [JsonObject(MemberSerialization.OptIn)]
	class OriginalScoreCard : IScoreCard
	{   
        [JsonProperty]
        private Category cat_Ones = new Category(Cat.Ones, 1);

        [JsonProperty]
        private Category cat_Twos = new Category(Cat.Twos, 2);

        [JsonProperty]
        private Category cat_Threes = new Category(Cat.Threes, 3);

        [JsonProperty]
        private Category cat_Fours = new Category(Cat.Fours, 4);

        [JsonProperty]
        private Category cat_Fives = new Category(Cat.Fives, 5);

        [JsonProperty]
        private Category cat_Sixes = new Category(Cat.Sixes, 6);

        [JsonProperty]
        private Category cat_UpperBonus = new Category(Cat.UpperBonus, null);

        [JsonProperty]
        private Category cat_x3 = new Category(Cat.x3, null);

        [JsonProperty]
        private Category cat_x4 = new Category(Cat.x4, null);

        [JsonProperty]
        private Category cat_FullHouse = new Category(Cat.FullHouse, null);

        [JsonProperty]
        private Category cat_Small = new Category(Cat.Small, null);

        [JsonProperty]
        private Category cat_Large = new Category(Cat.Large, null);

        [JsonProperty]
        private Category cat_Yahtzee = new Category(Cat.Yahtzee, null);

        [JsonProperty]
        private Category cat_Chance = new Category(Cat.Chance, null);

        [JsonProperty]
        private Category cat_YahtzeeBonus = new Category(Cat.YahtzeeBonus, null);

        [JsonProperty]
        public int TotalScore
        {
            get
            {
                var cats = GetCategories();
                int totalScore = 0;

                foreach (Category c in cats)
                {
                    if (c.Score.HasValue)
                    {
                        totalScore += (int)c.Score;
                    }
                }
                if (cat_UpperBonus.Score.HasValue)
                {
                    totalScore += (int)cat_UpperBonus.Score;
                }

                if (cat_YahtzeeBonus.Score.HasValue)
                {
                    totalScore += (int)cat_UpperBonus.Score;
                }

                return totalScore;
            }
        }

        public IEnumerable<Category> GetCategories()
        {
            List<Category> catList = new List<Category>
            {
                cat_Ones, cat_Twos, cat_Threes, cat_Fours, cat_Fives, cat_Sixes, 
                cat_x3, cat_x4, cat_FullHouse, cat_Small, cat_Large, cat_Yahtzee, cat_Chance,
            };
        
            return catList.Cast<Category>();
        }

        public void Update(List<Die> dice, Cat category)
        {
            switch (category)
            {
                case Cat.Ones:
                    UpdateUpperSection(dice, 1, Cat.Ones);
                    break;
                case Cat.Twos:
                    UpdateUpperSection(dice, 2, Cat.Twos);
                    break;
                case Cat.Threes:
                    UpdateUpperSection(dice, 3, Cat.Threes);
                    break;
                case Cat.Fours:
                    UpdateUpperSection(dice, 4, Cat.Fours);
                    break;
                case Cat.Fives:
                    UpdateUpperSection(dice, 5, Cat.Fives);
                    break;
                case Cat.Sixes:
                    UpdateUpperSection(dice, 6, Cat.Sixes);
                    break;
                case Cat.x3:
                    UpdateX3(dice);
                    break;
                case Cat.x4:
                    UpdateX4(dice);
                    break;
                case Cat.FullHouse:
                    UpdateFullHouse(dice);
                    break;
                case Cat.Small:
                    UpdateSmall(dice);
                    break;
                case Cat.Large:
                    UpdateLarge(dice);
                    break;
                case Cat.Yahtzee:
                    UpdateYahtzee(dice);
                    break;
                case Cat.Chance:
                    UpdateChance(dice);
                    break;
                default:
                    throw new Exception("Scorecard category not recognised");
            }
        }

        public void UpdateYahtzeeBonus(List<Die> dice, Cat chosenCat)
        {
            if (IsBonusYahtzee(dice))
            {
                var iterableCats = GetCategories();
            
                foreach (Category cat in iterableCats)
                {
                    if ((cat.CatType == chosenCat) && !cat.IsUsed)
                    {
                        if ((chosenCat == Cat.FullHouse || chosenCat == Cat.Small
                            || chosenCat == Cat.Large) && !cat.IsUsed)
                        {
                            switch (chosenCat)
                            {
                                case Cat.FullHouse:
                                        cat_FullHouse.Score = 25;
                                        cat_YahtzeeBonus.Score += 100;
                                    break;
                                    
                                case Cat.Small:
                                        cat_Small.Score = 30;
                                        cat_YahtzeeBonus.Score += 100;
                                    break;

                                case Cat.Large:
                                        cat_Large.Score = 40;
                                        cat_YahtzeeBonus.Score += 100;
                                    break;

                                default:
                                    throw new Exception("Bonus Yahtzee category error");
                            }
                        }
                        else
                        {
                            cat_YahtzeeBonus.Score += 100;
                            this.Update(dice, chosenCat);
                        }
                    }
                }
            }
            else throw new Exception("Error: Dice do not give Yahtzee bonus");
        }

        private void UpdateUpperSection(List<Die> dice, int targetValue, Cat catToUpdate)
        {
            int score = 0;     

            foreach (Die die in dice)
            {
                if (die.Value == targetValue)
                {
                    score += targetValue;
                }
            }

            var iterableCats = GetCategories();
            
            foreach (Category cat in iterableCats)
            {
                if ((cat.CatType == catToUpdate) && !cat.IsUsed)
                {
                    cat.Score = score;
                }
            }                
        
            if (CheckUpperBonus() && !cat_UpperBonus.IsUsed)
            {
                cat_UpperBonus.Score = 35;
            }
        }

        private bool CheckUpperBonus()
        {
            int upperSectionScore = 0;

            List<Category> upperCategories = new List<Category>
                { cat_Ones, cat_Twos, cat_Threes, cat_Fours, cat_Fives, cat_Sixes };

            foreach (Category cat in upperCategories)
            {
                if (cat.Score.HasValue)
                {
                    upperSectionScore += (int)cat.Score;
                }
            }

            return upperSectionScore >= 63;
        }

        private void UpdateX3(List<Die> dice)
        {
            if (!cat_x3.IsUsed && IsThreeOfAKind(dice))
            {
                int score = AddDiceValues(dice);
                cat_x3.Score = score;
            }

            if (!cat_x3.IsUsed && !IsThreeOfAKind(dice))
            {
                cat_x3.Score = 0;
            }
        }

        private void UpdateX4(List<Die> dice)
        {
            if (!cat_x4.IsUsed && IsFourOfAKind(dice))
            {
                int score = AddDiceValues(dice);
                cat_x4.Score = score;
            }

            if (!cat_x4.IsUsed && !IsFourOfAKind(dice))
            {
                cat_x4.Score = 0;
            }
        }

        private void UpdateFullHouse(List<Die> dice)
        {
            if (!cat_FullHouse.IsUsed && IsFullHouse(dice))
            {
                cat_FullHouse.Score = 25;
            }

            if (!cat_FullHouse.IsUsed && !IsFullHouse(dice))
            {
                cat_FullHouse.Score = 0;
            }
        }

        private void UpdateSmall(List<Die> dice)
        {
            if (!cat_Small.IsUsed && IsSequence(dice, 4))
            {
                cat_Small.Score = 30;
            }

            if (!cat_Small.IsUsed && !IsSequence(dice, 4))
            {
                cat_Small.Score = 0;
            }
        }

        private void UpdateLarge(List<Die> dice)
        {
            if (!cat_Large.IsUsed && IsSequence(dice, 5))
            {
                cat_Large.Score = 40;
            }

            if (!cat_Large.IsUsed && !IsSequence(dice, 5))
            {
                cat_Large.Score = 0;
            }
        }


        private void UpdateYahtzee(List<Die> dice)
        {
            if (!cat_Yahtzee.IsUsed && IsYahtzee(dice))
            {
                cat_Yahtzee.Score = 50;
            }

            if (!cat_Yahtzee.IsUsed && !IsYahtzee(dice))
            {
                cat_Yahtzee.Score = 0;
            }
        }

        private void UpdateChance(List<Die> dice)
        {
            if (!cat_Chance.IsUsed)
            {
                int score = AddDiceValues(dice);
                cat_Chance.Score = score;
            }
        }

        public bool IsThreeOfAKind(List<Die> dice)
        {
            int[] values = new int [5];
            int count = 0;

            for (int i = 0; i < values.Count(); i++)
            {
                values[i] = dice[i].Value;
            }

            int mode = values.GroupBy(v => v)
                            .OrderByDescending(g => g.Count())
                            .First()
                            .Key;

            foreach (Die d in dice)
            {
                if (d.Value == mode)
                {
                    count++; 
                }
            }

            return count >= 3;
        }

        public bool IsFourOfAKind(List<Die> dice)
        {
            int[] values = new int [5];
            int count = 0;

            for (int i = 0; i < values.Count(); i++)
            {
                values[i] = dice[i].Value;
            }

            int mode = values.GroupBy(v => v)
                            .OrderByDescending(g => g.Count())
                            .First()
                            .Key;

            foreach (Die d in dice)
            {
                if (d.Value == mode)
                {
                    count++; 
                }
            }

            return count >= 4;
        }

        public bool IsFullHouse(List<Die> dice)
        {
            int[] values = new int [5];

            for (int i = 0; i < values.Count(); i++)
            {
                values[i] = dice[i].Value;
            }

            var query = from i in values
                group i by i into g
                select new {g.Key, Count = g.Count()};

            int maxFrequency = query.Max(g => g.Count);
            int minFrequency = query.Min(g => g.Count);
            
            if ((maxFrequency == 3 || minFrequency == 2) &&
                (minFrequency == 3 || minFrequency == 2))
            {
                return true;
            }
            else return false;
        }
        
        public bool IsSequence(List<Die> dice, int sequenceAmount)
        {
            int[] values = new int [5];
            int highestCount = 1;
            int count = 1;

            for (int i = 0; i < values.Count(); i++)
            {
                values[i] = dice[i].Value;
            }

            Array.Sort(values);

            for (int i = 0; i < values.Count() - 1; i++)
            {
                if (values[i] + 1 == values[i + 1])
                {
                    count++;
                    if (count > highestCount)
                    {
                        highestCount = count;
                    }
                }
                else if (values[i] != values[i + 1])
                {
                    count = 1;
                }
            }

            return highestCount >= sequenceAmount;
        }

        public bool IsYahtzee(List<Die> dice)
        {
            int value = dice[0].Value;
            int count = 0;

            foreach (Die die in dice)
            {
                if (die.Value == value)
                {
                    count += 1;
                }
            }
            
            return count == 5 ? true : false;
        }

        public bool IsBonusYahtzee(List<Die> dice)
        {
            if (IsYahtzee(dice) && cat_Yahtzee.IsUsed)
            {
                return true;
            }
            else return false;
        }

        public bool IsUsed(Cat queriedCatType)
        {
            var cats = GetCategories();
            bool isUsed = false;

            foreach (Category c in cats)
            {
                if (c.CatType == queriedCatType)
                {
                    isUsed = c.IsUsed;
                }
            }

            return isUsed;
        }

        private int AddDiceValues(List<Die> dice)
        {
            int score = 0;

            foreach (Die die in dice)
            {
                score += die.Value;
            }

            return score;
        }
	}
}
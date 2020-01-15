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

        public bool Update(List<Die> dice, Cat category)
        {
            switch (category)
            {
                case Cat.Ones:
                    return UpdateUpperSection(dice, 1, Cat.Ones);
                    
                case Cat.Twos:
                    return UpdateUpperSection(dice, 2, Cat.Twos);
                    
                case Cat.Threes:
                    return UpdateUpperSection(dice, 3, Cat.Threes);
                    
                case Cat.Fours:
                    return UpdateUpperSection(dice, 4, Cat.Fours);
                    
                case Cat.Fives:
                    return UpdateUpperSection(dice, 5, Cat.Fives);
                    
                case Cat.Sixes:
                    return UpdateUpperSection(dice, 6, Cat.Sixes);
                    
                case Cat.x3:
                    return UpdateX3(dice);
                    
                case Cat.x4:
                    return UpdateX4(dice);
                    
                case Cat.FullHouse:
                    return UpdateFullHouse(dice);
                    
                case Cat.Small:
                    return UpdateSmall(dice);
                    
                case Cat.Large:
                    return UpdateLarge(dice);
                    
                case Cat.Yahtzee:
                    return UpdateYahtzee(dice);
                    
                case Cat.Chance:
                    return UpdateChance(dice);
                    
                default:
                    // no exception or message here?
                    return false;
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
                            // /////////////////////////////////////////////////////////// xxx use Update bool to handle if cat has been chosen alread
                            Update(dice, chosenCat);
                        }
                    }
                }
            }
            else throw new Exception("Error: Dice do not give Yahtzee bonus");
        }

        private bool UpdateUpperSection(List<Die> dice, int targetValue, Cat catToUpdate)
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

                    if (CheckUpperBonus() && !cat_UpperBonus.IsUsed)
                    {
                        cat_UpperBonus.Score = 35;
                    }

                    return true;
                }
            }                
        
            return false;
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

        private bool UpdateX3(List<Die> dice)
        {
            if (!cat_x3.IsUsed)
            {
                if (IsThreeOfAKind(dice))
                {
                    int score = AddDiceValues(dice);
                    cat_x3.Score = score;
                }
                else
                {
                    cat_x3.Score = 0;
                }

                return true;
            }
            
            return false;
        }

        private bool UpdateX4(List<Die> dice)
        {
            if (!cat_x4.IsUsed)
            {
                if (IsFourOfAKind(dice))
                {
                    int score = AddDiceValues(dice);
                    cat_x4.Score = score;
                }
                else
                {
                    cat_x4.Score = 0;
                }

                return true;
            }

            return false;
        }

        private bool UpdateFullHouse(List<Die> dice)
        {
            if (!cat_FullHouse.IsUsed)
            {
                if (IsFullHouse(dice))
                {
                    cat_FullHouse.Score = 25;
                }
                else
                {
                    cat_FullHouse.Score = 0;
                }

                return true;
            }

            return false;
        }

        private bool UpdateSmall(List<Die> dice)
        {
            if (!cat_Small.IsUsed)
            {
                if (IsSequence(dice, 4))
                {
                    cat_Small.Score = 30;
                }
                else
                {
                    cat_Small.Score = 0;
                }

                return true;
            }

            return false;
        }

        private bool UpdateLarge(List<Die> dice)
        {
            if (!cat_Large.IsUsed)
            {
                if (IsSequence(dice, 5))
                {
                    cat_Large.Score = 40;
                }
                else
                {
                    cat_Large.Score = 0;
                }

                return true;
            }

            return false;
        }


        private bool UpdateYahtzee(List<Die> dice)
        {
            if (!cat_Yahtzee.IsUsed)
            {
                if (IsYahtzee(dice))
                {
                    cat_Yahtzee.Score = 50;
                }
                else
                {
                    cat_Yahtzee.Score = 0;
                }

                return true;
            }

            return false;
        }

        private bool UpdateChance(List<Die> dice)
        {
            if (!cat_Chance.IsUsed)
            {
                int score = AddDiceValues(dice);
                cat_Chance.Score = score;

                return true;
            }

            return false;
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
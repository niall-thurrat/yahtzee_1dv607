using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Yahtzee.model.strategy
{
    interface IScoreCard
    {
        IEnumerable<Category> GetCategories();
        Nullable<int>[] GetScores();
        void Update(List<Die> dice, Category.Type cat);
        void UpdateYahtzeeBonus(List<Die> dice, Category.Type chosenCat);
        bool IsThreeOfAKind(List<Die> dice);
        bool IsFourOfAKind(List<Die> dice);
        bool IsFullHouse(List<Die> dice);
        bool IsSequence(List<Die> dice, int sequenceAmount);
        bool IsYahtzee(List<Die> dice);
        bool IsBonusYahtzee(List<Die> dice);
        bool IsUsed(Category.Type cat);
    }
}
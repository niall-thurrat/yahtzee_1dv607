using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Yahtzee.model.rules
{
    interface IScoreCard
    {
        IEnumerable<Category> GetCategories();
        void Update(Category.Type c, List<Die> dice);
        void UpdateWithBonusYahtzee(Category.Type c, List<Die> dice);
        bool IsThreeOfAKind(List<Die> dice);
        bool IsFourOfAKind(List<Die> dice);
        bool IsFullHouse(List<Die> dice);
        bool IsSequence(List<Die> dice, int sequenceAmount);
        bool IsYahtzee(List<Die> dice);
        bool IsBonusYahtzee(List<Die> dice);
        bool IsUsed(Category.Type c);
        void Print();
    }
}
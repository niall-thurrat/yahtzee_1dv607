using System.Collections.Generic;

namespace Yahtzee.model.strategy
{
    interface IScoreCard
    {
        IEnumerable<Category> GetCategories();
        bool Update(List<Die> dice, Category.Type cat);
        bool UpdateYahtzeeBonus(List<Die> dice, Category.Type chosenCat);
        bool IsThreeOfAKind(List<Die> dice);
        bool IsFourOfAKind(List<Die> dice);
        bool IsFullHouse(List<Die> dice);
        bool IsSequence(List<Die> dice, int sequenceAmount);
        bool IsYahtzee(List<Die> dice);
        bool IsBonusYahtzee(List<Die> dice);
        bool IsUsed(Category.Type cat);
    }
}
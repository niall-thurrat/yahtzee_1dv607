using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Yahtzee.model.rules
{
    interface IScoreCard
    {
        void Update(Category.Type c, List<Die> dice);
        void UpdateWithBonusYahtzee(Category.Type c, List<Die> m_dice);
        bool IsThreeOfAKind(List<Die> dice);
        bool IsFourOfAKind(List<Die> dice);
        bool IsFullHouse(List<Die> dice);
        bool IsSequence(List<Die> dice, int sequenceAmount);
        bool IsYahtzee(List<Die> dice);
        bool IsBonusYahtzee(List<Die> dice);
        void Print();
    }
}
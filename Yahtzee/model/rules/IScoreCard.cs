using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Yahtzee.model.rules
{
    interface IScoreCard
    {
        void Update(Category.Type c, List<Die> dice);
        void UpdateOnes(List<Die> dice);
        bool IsYahtzee(List<Die> dice);
        void UpdateYahtzee(List<Die> dice);
        void Print();
    }
}
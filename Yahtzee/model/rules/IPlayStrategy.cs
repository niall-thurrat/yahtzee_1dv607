using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Yahtzee.model.rules
{
    interface IPlayStrategy
    {
        void Use(ScoreCard scoreCard, List<Die> dice, int rollsLeft);
        int CalcScore(List<Die> m_dice);
    }
}

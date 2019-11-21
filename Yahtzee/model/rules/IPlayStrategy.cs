using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Yahtzee.model.rules
{
    interface IPlayStrategy
    {
        Category.Type Use(Player player);
        int CalcScore(List<Die> m_dice);
    }
}

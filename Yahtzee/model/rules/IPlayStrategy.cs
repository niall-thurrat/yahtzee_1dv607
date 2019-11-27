using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Yahtzee.model.rules
{
    interface IPlayStrategy
    {
        Category.Type Use(Player player, int rollsLeft);
        Category.Type UseBonusYahtzee(Player player);
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Yahtzee.model.strategy
{
    interface IPlayStrategy
    {
        Category.Type Use(Player player, int rollsLeft);
        Category.Type UseYahtzeeBonus(Player player);
    }
}

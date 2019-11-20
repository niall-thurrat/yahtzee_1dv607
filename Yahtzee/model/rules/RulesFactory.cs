using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Yahtzee.model.rules
{
    class RulesFactory
    {
        public IPlayStrategy GetPlayStrategy()
        {
            return new OriginalPlayStrategy();
        }
    }
}

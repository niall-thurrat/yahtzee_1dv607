
namespace Yahtzee.model.strategy
{
    class StrategyFactory
    {
        public IPlayStrategy GetPlayStrategy()
        {
            return new OriginalPlayStrategy();
        }

        public IScoreCard GetScoreCard()
        {
            return new OriginalScoreCard();
        }
    }
}

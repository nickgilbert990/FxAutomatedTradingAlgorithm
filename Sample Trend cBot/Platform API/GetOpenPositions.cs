using cAlgo.Main;

namespace cAlgo.API
{
    ///<summary>
    /// Get open positions from the cAlgo API passing the instance of the main class into the constructor.
    /// The purpose of this helper class is to abstract the API calls from the main logic.
    /// </summary>
    public class GetOpenPositions
    {
        private readonly TradeAutomation _bot;

        public GetOpenPositions(TradeAutomation bot)
        {
            _bot = bot;
        }

        public Position LongPositions()
        {
            return _bot.Positions.Find(_bot.Label, _bot.Symbol, TradeType.Buy);
        }

        public Position ShortPositions()
        {
            return _bot.Positions.Find(_bot.Label, _bot.Symbol, TradeType.Sell);
        }

    }
}
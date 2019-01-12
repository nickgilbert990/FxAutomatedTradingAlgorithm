using System;
using cAlgo.Main;

namespace cAlgo.API
{
    public class CloseOrders
    {
        private readonly TradeAutomation _bot;

        public CloseOrders(TradeAutomation bot)
        {
            _bot = bot;
        }

        public bool ClosePosition(Position position)
        {
            try
            {
                _bot.ClosePosition(position);
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return false;
            }
        }

    }
}
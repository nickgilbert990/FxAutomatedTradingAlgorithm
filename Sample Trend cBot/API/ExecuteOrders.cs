using System;
using cAlgo.Main;

namespace cAlgo.API
{
    public class ExecuteOrders
    {
        private SampleTrendcBot _bot;

        public ExecuteOrders(SampleTrendcBot bot)
        {
            _bot = bot;
        }

        public bool ExecuteBuyOrder()
        {
            try
            {
                _bot.ExecuteMarketOrder(TradeType.Buy, _bot.Symbol, _bot.VolumeInUnits, _bot.Label, _bot.StopLoss, _bot.TakeProfit);
                return true;
            }
            catch (Exception e)
            {
                Console.Write(e);
                return false;
            }          
            
        }

        public bool ExecuteSellOrder()
        {
            try
            {
                _bot.ExecuteMarketOrder(TradeType.Sell, _bot.Symbol, _bot.VolumeInUnits, _bot.Label, _bot.StopLoss, _bot.TakeProfit);
                return true;
            }
            catch (Exception e)
            {
                Console.Write(e);
                return false;
            }
        }

    }
}
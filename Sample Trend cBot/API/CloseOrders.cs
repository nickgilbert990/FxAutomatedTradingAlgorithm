using System;
using cAlgo.Main;

namespace cAlgo.API
{
    public class CloseOrders
    {
        private SampleTrendcBot _bot;

        public CloseOrders(SampleTrendcBot bot)
        {
            _bot = bot;
        }

        public bool ClosePosition(Position position)
        {
            try
            {
                ClosePosition(position);
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
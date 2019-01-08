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
            return false;
        }

    }
}
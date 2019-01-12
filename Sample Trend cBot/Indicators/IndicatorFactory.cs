using System.Runtime.Remoting.Messaging;
using cAlgo.API;
using cAlgo.Indicators;
using cAlgo.Main;

namespace cAlgo
{
    public class IndicatorFactory
    {
        public IIndicators GetIndicator(TradeAutomation.FactoryParameters inputParameters)
        {
            switch (inputParameters.IndicatorType)
            {
                case "MA":
                    return new MovingAverageCrossOver(inputParameters);
                default:                 
                    return null;
            }
        }
    }
}
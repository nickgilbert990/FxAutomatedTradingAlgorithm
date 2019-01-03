using System.Runtime.Remoting.Messaging;
using cAlgo.API;

namespace cAlgo
{
    public class IndicatorFactory
    {
        public IIndicators GetIndicator(SampleTrendcBot.FactoryParameters inputParameters)
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
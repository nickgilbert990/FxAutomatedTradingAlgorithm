using cAlgo;
using cAlgo.API;
using cAlgo.API.Indicators;

namespace cAlgo
{
    public class MovingAverageCrossOver : IIndicators
    {
        private MovingAverage _slowMa;
        private MovingAverage _fastMa;
        private string _alert = null;

        public MovingAverageCrossOver(SampleTrendcBot.FactoryParameters inputParameters)
        {

            _fastMa = inputParameters.Bot.Indicators.MovingAverage(inputParameters.SourceSeries, inputParameters.FastPeriods, inputParameters.MAType);
            _slowMa = inputParameters.Bot.Indicators.MovingAverage(inputParameters.SourceSeries, inputParameters.SlowPeriods, inputParameters.MAType);
        }

        public string IndicatorAlert()
        {
            var currentSlowMa = _slowMa.Result.Last(0);
            var currentFastMa = _fastMa.Result.Last(0);
            var previousSlowMa = _slowMa.Result.Last(1);
            var previousFastMa = _fastMa.Result.Last(1);

            if (previousSlowMa > previousFastMa && currentSlowMa <= currentFastMa)
            {
                _alert = "AlertLong";
            }
            else if (previousSlowMa < previousFastMa && currentSlowMa >= currentFastMa)
            {
                _alert = "AlertShort";
            }

            return _alert;
        }

    }

}
using cAlgo;
using cAlgo.API;
using cAlgo.API.Indicators;

namespace Indicators
{
    public class MovingAverageCrossOver
    {
        private MovingAverage _slowMa;
        private MovingAverage _fastMa;
        private string _alert = null;

        public MovingAverageCrossOver(SampleTrendcBot bot, DataSeries sourceSeries, int fastPeriods, int slowPeriods, MovingAverageType mAType)
        {
            _fastMa = bot.Indicators.MovingAverage(sourceSeries, fastPeriods, mAType);
            _slowMa = bot.Indicators.MovingAverage(sourceSeries, slowPeriods, mAType);
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
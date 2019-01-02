// -------------------------------------------------------------------------------------------------
//
//    This code is a cAlgo API sample.
//
//    This cBot is intended to be used as a sample and does not guarantee any particular outcome or
//    profit of any kind. Use it at your own risk.
//
//    The "Sample Trend cBot" will buy when fast period moving average crosses the slow period moving average and sell when 
//    the fast period moving average crosses the slow period moving average. The orders are closed when an opposite signal 
//    is generated. There can only by one Buy or Sell order at any time.
//
// -------------------------------------------------------------------------------------------------

using System;
using System.Globalization;
using System.Linq;
using cAlgo;
using cAlgo.API;
using cAlgo.API.Indicators;
using cAlgo.API.Internals;
using cAlgo.Indicators;
using Indicators;

namespace cAlgo
{
    [Robot(TimeZone = TimeZones.UTC, AccessRights = AccessRights.None)]
    public class SampleTrendcBot : Robot
    {
        [Parameter("MA Type")]
        public MovingAverageType MAType { get; set; }

        [Parameter()]
        public DataSeries SourceSeries { get; set; }

        [Parameter("Slow Periods", DefaultValue = 10)]
        public int SlowPeriods { get; set; }

        [Parameter("Fast Periods", DefaultValue = 5)]
        public int FastPeriods { get; set; }

        [Parameter("Quantity (Lots)", DefaultValue = 1, MinValue = 0.01, Step = 0.01)]
        public double Quantity { get; set; }

        [Parameter("Take Profit", DefaultValue = 20)]
        public int TakeProfit { get; set; }

        [Parameter("Stop Loss", DefaultValue = 20)]
        public int StopLoss { get; set; }

        private const string label = "Sample Trend cBot";
        private  MovingAverageCrossOver maAlert;
        
        protected override void OnStart()
        {
            maAlert = new MovingAverageCrossOver(this, SourceSeries, FastPeriods, SlowPeriods, MAType);
        }


        protected override void OnTick()
        {
            var longPosition = Positions.Find(label, Symbol, TradeType.Buy);
            var shortPosition = Positions.Find(label, Symbol, TradeType.Sell);

            if (maAlert.IndicatorAlert() == "AlertLong" && longPosition == null)
            {
                if (shortPosition != null)
                    ClosePosition(shortPosition);
                ExecuteMarketOrder(TradeType.Buy, Symbol, VolumeInUnits, label, StopLoss, TakeProfit);
            }
            else if (maAlert.IndicatorAlert() == "AlertShort" && shortPosition == null)
            {
                if (longPosition != null)
                    ClosePosition(longPosition);
                ExecuteMarketOrder(TradeType.Sell, Symbol, VolumeInUnits, label, StopLoss, TakeProfit);
            }
        }

        private long VolumeInUnits
        {
            get { return Symbol.QuantityToVolume(Quantity); }
        }

    }
}

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

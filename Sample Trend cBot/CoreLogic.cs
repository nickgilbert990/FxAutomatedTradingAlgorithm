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

        private const string Label = "Sample Trend cBot";
        private  MovingAverageCrossOver _maAlert;
        
        protected override void OnStart()
        {
            _maAlert = new MovingAverageCrossOver(this, SourceSeries, FastPeriods, SlowPeriods, MAType);
        }


        protected override void OnTick()
        {
            var longPosition = Positions.Find(Label, Symbol, TradeType.Buy);
            var shortPosition = Positions.Find(Label, Symbol, TradeType.Sell);

            if (_maAlert.IndicatorAlert() == "AlertLong" && longPosition == null)
            {
                if (shortPosition != null)
                    ClosePosition(shortPosition);
                ExecuteMarketOrder(TradeType.Buy, Symbol, VolumeInUnits, Label, StopLoss, TakeProfit);
            }
            else if (_maAlert.IndicatorAlert() == "AlertShort" && shortPosition == null)
            {
                if (longPosition != null)
                    ClosePosition(longPosition);
                ExecuteMarketOrder(TradeType.Sell, Symbol, VolumeInUnits, Label, StopLoss, TakeProfit);
            }
        }

        private long VolumeInUnits
        {
            get { return Symbol.QuantityToVolume(Quantity); }
        }

    }
}


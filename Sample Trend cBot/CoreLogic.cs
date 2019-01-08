// -------------------------------------------------------------------------------------------------
using System;
using System.Globalization;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using cAlgo;
using cAlgo.API;
using cAlgo.API.Indicators;
using cAlgo.API.Internals;
using cAlgo.Indicators;

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

        [Parameter("Stop Loss", DefaultValue = 30)]
        public int StopLoss { get; set; }

        public string Label = "FxAutomation";
        private  IIndicators _indicator;
        private GetOpenPositions _getOpenPositions;

        ///<summary>
        /// Parameters to be passed into the indicator factory
        /// </summary>
        public struct FactoryParameters
        {
            public string IndicatorType;
            public MovingAverageType MAType;
            public DataSeries SourceSeries;
            public int SlowPeriods;
            public int FastPeriods;
            public SampleTrendcBot Bot;
        }
        
        protected override void OnStart()
        {
            ///<summary>
            /// Initialsise parameters from input data and pass to the factory which returns the object identified by IndicatorType
            /// </summary>
            var factoryParameters = new FactoryParameters {Bot = this, IndicatorType = "MA", MAType = MAType, FastPeriods = FastPeriods, SlowPeriods = SlowPeriods, SourceSeries = SourceSeries};
            _indicator = new IndicatorFactory().GetIndicator(factoryParameters);

            ///<summary>
            /// Instanciate positions object to abstract API functions from the main logic
            /// </summary>
            _getOpenPositions = new GetOpenPositions(this);
        }

        protected override void OnTick()
        {
            ///<summary>
            /// Get open positions
            /// </summary>
            var longPositions = _getOpenPositions.LongPositions();

            var longPosition = Positions.Find(Label, Symbol, TradeType.Buy);
            var shortPosition = Positions.Find(Label, Symbol, TradeType.Sell);

            if (_indicator.IndicatorAlert() == "AlertLong" && longPosition == null)
            {
                if (shortPosition != null)
                    ClosePosition(shortPosition);
                ExecuteMarketOrder(TradeType.Buy, Symbol, VolumeInUnits, Label, StopLoss, TakeProfit);
            }
            else if (_indicator.IndicatorAlert() == "AlertShort" && shortPosition == null)
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

    ///<summary>
    /// Get open positions from the cAlgo API passing the instance of the main class into the constructor.
    /// The purpose of this helper class is to abstract the API calls from the main logic.
    /// </summary>
    public class GetOpenPositions 
    {
        private SampleTrendcBot _bot;

        public GetOpenPositions(SampleTrendcBot bot)
        {
            _bot = bot;
        }

        public Position LongPositions()
        {
            return _bot.Positions.Find(_bot.Label, _bot.Symbol, TradeType.Buy);
        }

        public Position ShortPositions()
        {
            return _bot.Positions.Find(_bot.Label, _bot.Symbol, TradeType.Sell);
        }

    }
}


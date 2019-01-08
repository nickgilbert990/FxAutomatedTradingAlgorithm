// -------------------------------------------------------------------------------------------------
using System;
using System.Linq;
using cAlgo;
using cAlgo.API;
using cAlgo.API.Indicators;
using cAlgo.API.Internals;
using cAlgo.Indicators;

namespace cAlgo.Main
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
        private CloseOrders _closeOrders;
        private ExecuteOrders _executeOrders;

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
            /// Creaqte instances of API objects to decouple API functions from the main logic
            /// and support mocking
            /// </summary>
            _getOpenPositions = new GetOpenPositions(this);
            _closeOrders      = new CloseOrders(this);
            _executeOrders    = new ExecuteOrders(this);
        }

        ///<summary>
        /// Main processing logic executed on price tick
        /// </summary>
        protected override void OnTick()
        {
            ///<summary>
            /// Get open positions
            /// </summary>
            var longPositions  = _getOpenPositions.LongPositions();
            var shortPositions = _getOpenPositions.ShortPositions();
            ///<summary>
            /// Old direct API code - to be removed after testing
            /// var longPosition = Positions.Find(Label, Symbol, TradeType.Buy);
            /// var shortPosition = Positions.Find(Label, Symbol, TradeType.Sell);            ///
            /// </summary>

            if (_indicator.IndicatorAlert() == "AlertLong" && longPositions == null)
            {
                if (shortPositions != null)
                    _closeOrders.ClosePosition(shortPositions);
                    ///<summary>
                    /// Old direct API code - to be removed after testing
                    /// ClosePosition(shortPositions);
                    ///</summary>

                _executeOrders.ExecuteBuyOrder();
                ///<summary>
                /// Old direct API code - to be removed after testing
                /// ExecuteMarketOrder(TradeType.Buy, Symbol, VolumeInUnits, Label, StopLoss, TakeProfit);
                /// </summary>
            }
            else if (_indicator.IndicatorAlert() == "AlertShort" && shortPositions == null)
            {
                if (longPositions != null)
                    _closeOrders.ClosePosition(longPositions);

                _executeOrders.ExecuteSellOrder();
                ///<summary>
                /// Old direct API code - to be removed after testing
                /// ExecuteMarketOrder(TradeType.Sell, Symbol, VolumeInUnits, Label, StopLoss, TakeProfit);
                ///</summary>

            }
        }

        public long VolumeInUnits
        {
            #pragma warning disable 0618
            get { return Symbol.QuantityToVolume(Quantity); }
        }
    }

}


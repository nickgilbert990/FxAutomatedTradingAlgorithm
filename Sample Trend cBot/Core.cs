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
    public class TradeAutomation : Robot
    {
        [Parameter("MA Type")]
        public MovingAverageType MAType { get; set; }

        [Parameter()]
        public DataSeries SourceSeries { get; set; }

        [Parameter("Slow Periods", DefaultValue = 120)]
        public int SlowPeriods { get; set; }

        [Parameter("Fast Periods", DefaultValue = 40)]
        public int FastPeriods { get; set; }

        [Parameter("Quantity (Lots)", DefaultValue = 0.1, MinValue = 0.01, Step = 0.01)]
        public double Quantity { get; set; }

        [Parameter("Take Profit", DefaultValue = 10)]
        public int TakeProfit { get; set; }

        [Parameter("Stop Loss", DefaultValue = 20)]
        public int StopLoss { get; set; }

        public string Label = "FxAutomation";

        private  IIndicators _indicator;
        private GetOpenPositions _getOpenPositions;
        private CloseOrders _closeOrders;
        private ExecuteOrders _executeOrders;
        private ManageOrders _manageOrders;

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
            public TradeAutomation Bot;
        }
        
        protected override void OnStart()
        {
            ///<summary>
            /// Initialsise parameters from input data and pass to the factory which returns the object identified by IndicatorType
            /// </summary>
            var factoryParameters = new FactoryParameters {Bot = this, IndicatorType = "MA", MAType = MAType, FastPeriods = FastPeriods, SlowPeriods = SlowPeriods, SourceSeries = SourceSeries};
            _indicator = new IndicatorFactory().GetIndicator(factoryParameters);

            ///<summary>
            /// Create instances of API objects to decouple API functions from the main logic in order to 
            /// support unit testing and mocking
            /// </summary>
            _getOpenPositions = new GetOpenPositions(this);
            _closeOrders      = new CloseOrders(this);
            _executeOrders    = new ExecuteOrders(this);
            _manageOrders     = new ManageOrders();
        }

        ///<summary>
        /// Main processing logic executed on price tick
        /// </summary>
        protected override void OnTick()
        {
            _manageOrders.ExecuteBuyOrSellOrderOnSignal(_indicator, _getOpenPositions, _closeOrders, _executeOrders);           
        }

        public long VolumeInUnits
        {
            #pragma warning disable 0618
            get { return Symbol.QuantityToVolume(Quantity); }
        }
    }

    ///<summary>
    /// This class is called after every price tick. The purpose is to check for buy or sell alerts
    /// and call abstract API methods to close old orders and create new ones.  
    /// </summary>
    public class ManageOrders
    {
        ///<summary>
        /// This method receives initialised API objects and calls their methods buy, sell and close
        /// orders on the trading server. 
        /// </summary>
        public void ExecuteBuyOrSellOrderOnSignal(IIndicators indicators, GetOpenPositions getOpenPositions,
                                                  CloseOrders closeOrders, ExecuteOrders executeOrders)
        {
            if (indicators.IndicatorAlert() == "AlertLong" && getOpenPositions.LongPositions() == null)
            {
                if (getOpenPositions.ShortPositions() != null)
                    closeOrders.ClosePosition(getOpenPositions.ShortPositions());

                executeOrders.ExecuteBuyOrder();
            }
            else if (indicators.IndicatorAlert() == "AlertShort" && getOpenPositions.ShortPositions() == null)
            {
                if (getOpenPositions.LongPositions() != null)
                    closeOrders.ClosePosition(getOpenPositions.LongPositions());

                executeOrders.ExecuteSellOrder();
            }

        }
    }

}


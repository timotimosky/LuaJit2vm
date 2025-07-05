using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace MarshalEvent
{
    // 事件委托
    [ComVisible(false)]
    public delegate void StockPriceChangedHandler(
        double newPrice, double oldPrice);

    // 定义事件接收器的接口 (sink, outgoing interface)
    [InterfaceType(ComInterfaceType.InterfaceIsIDispatch)]
    public interface IStockPriceBeeper
    {
        // 这里的方法名*必须*与托管类中的事件名一致
        [DispId(17)]
        void OnPriceChanged(double newPrice, double oldPrice);
    }

    // 事件源的自定义接口 (source)
    [InterfaceType(ComInterfaceType.InterfaceIsIDispatch)]
    public interface IStockPriceControl
    {
        void ChangeStockPrice(double newPrice);
    }

    [ClassInterface(ClassInterfaceType.None)]
    [ComSourceInterfaces(typeof(IStockPriceBeeper))]
    public class StockPriceControlObj : IStockPriceControl
    {
        private double _oldPrice;

        // 声明事件
        public event StockPriceChangedHandler OnPriceChanged;

        public void ChangeStockPrice(double newPrice)
        {
            if (OnPriceChanged != null)
            {
                OnPriceChanged(newPrice, _oldPrice);
            }
            _oldPrice = newPrice;
        }
        
    }
}

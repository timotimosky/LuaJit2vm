using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace MarshalEvent
{
    // �¼�ί��
    [ComVisible(false)]
    public delegate void StockPriceChangedHandler(
        double newPrice, double oldPrice);

    // �����¼��������Ľӿ� (sink, outgoing interface)
    [InterfaceType(ComInterfaceType.InterfaceIsIDispatch)]
    public interface IStockPriceBeeper
    {
        // ����ķ�����*����*���й����е��¼���һ��
        [DispId(17)]
        void OnPriceChanged(double newPrice, double oldPrice);
    }

    // �¼�Դ���Զ���ӿ� (source)
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

        // �����¼�
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

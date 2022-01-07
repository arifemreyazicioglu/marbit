namespace SC_BtcTurk.Models
{
    public class Enums
    {
        public enum OrderType
        {
            /// <summary> 0'dan başlar. BtcTurk o şekilde kullanıyor. </summary>
            Buy = 0,
            Sell
        }
        public enum OrderMethod
        {
            /// <summary> 0'dan başlar. BtcTurk o şekilde kullanıyor. </summary>
            Limit = 0,
            Market,
            StopLimit,
            StopMarket
        }
        public enum OrderTransactionType
        {
            Updated = 0,
            Inserted = 1,
            Deleted = 3
        }
    }
}

namespace TradeAbstractions.Enums
{
    /// <summary>
    /// 0 en verimsizi
    /// 1'de Ticker verisi WebSocket'ten gelir. Order'lar WebSocket'ten dinlenir.
    /// 2'de Tüm veri ve Order sonuçları WebSocket'ten dinlenir. Sadece mecburi işlemler API'den yapılır.
    /// </summary>
    public enum EnumTradeEngineMode
    {
        OnlyApi = 0,
        ManyApisMinWebSockets,
        MinApiFullWebSockets
    }
}

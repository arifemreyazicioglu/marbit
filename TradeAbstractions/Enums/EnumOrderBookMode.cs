namespace TradeAbstractions.Enums
{
    public enum EnumOrderBookMode
    {
        /// <summary>
        /// OrderBook'taki 5 adet en iyi sonuç arasında en çok hacime sahip olan bulunur ve arbitraj yapılır mı diye bakılır?
        /// 0,0,0 olduğunda, tahtanın en üstündeki fiyatlardır ve amount değerine de bakılarak seçilmiştir.
        /// </summary>
        Mod1 = 1,
        /// <summary>
        /// OrderBook'taki 5,4,3,2,1. sıradakilere teker teker bakılır. İlk ratio > 1 olan alınır.
        /// Yani artık indexler: 1,5,2 olabilir.
        /// 5*5*5=125 adet kombinasyona bakılmalı.
        /// 0,0,0 olduğunda, tahtanın en üstündeki fiyatlardır ancak amount değerine bakılmadan seçilmiştir.
        /// </summary>
        Mod2 = 2
    }
}

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using absHelpers = TradeAbstractions.Helpers;
using mdlHlpr = TradeAbstractions.Models.TradeHelper;
using mdlOrder = TradeAbstractions.Models.TradeHelper.FromEngine.OrderModel;
namespace SC_BtcTurk.Operations
{
    public class Order
    {
        internal async Task<absHelpers.ReturnTask<mdlOrder.OrderResponseModel>> InsertOrder(mdlOrder.OrderRequestModel orderRequest)
        {
            var retTask = new absHelpers.ReturnTask<mdlOrder.OrderResponseModel> { Success = false };
            mdlOrder.OrderResponseModel orderResponse = new mdlOrder.OrderResponseModel();
            retTask.Data = orderResponse;

            if (orderRequest.ReqRespList == null) orderRequest.ReqRespList = new List<absHelpers.RequestResponseStats>();

            // Engine Request'in değerlerini buradan update edelim.
            orderRequest.Price = Math.Round(orderRequest.Price, orderRequest.PriceScale, MidpointRounding.ToZero);
            orderRequest.DenominatorTotalGross = Math.Round(orderRequest.DenominatorTotalGross, orderRequest.DenominatorScale, MidpointRounding.ToZero);
            orderRequest.NumeratorAmountGross = Math.Round(orderRequest.NumeratorAmountGross, orderRequest.NumeratorScale, MidpointRounding.ToZero);

            Models.Order.RequestModel req = new Models.Order.RequestModel
            {
                NewOrderClientId = orderRequest.OrderClientId,
                OrderMethod = Models.Enums.OrderMethod.Market,
                OrderType = orderRequest.IsBuyOrder ? Models.Enums.OrderType.Buy : Models.Enums.OrderType.Sell,
                PairSymbol = orderRequest.PairSymbol,
                // Market order için ihtiyaç yok ama yine de kayıtlarımızda bulunması açısından girelim
                // Ondalık değeri doğru girilmiş olmalı yoksa aşağıdaki hatayı alırız. 
                // Code: 1126, Message: FAILED_INVALID_PRICE_SCALE | Msg: CreateOrderService, 400 BadRequest
                Price = orderRequest.Price,
                // BtcTurk'te market emrinde satarken Sol Sembol, alırken sağ sembol girilir.
                // Örn: USDTTRY için satarken USDT miktarı, alırken TRY miktarı girilir.
                // SAT dedin USDT girdin eline TRY geçti, AL dedin TRY girdin eline USDT geçti. 
                Quantity = orderRequest.IsBuyOrder ? orderRequest.DenominatorTotalGross : orderRequest.NumeratorAmountGross
            };
            // Aşağıda sıralanan hataları verme sayısını minimuma indirmek için 100ms bekletiriz.
            // 429 response'unu alma sayısı da azalır. Fırsatlar devam ederken 65sn civarı bizi bekletmemiş olur.
            await Task.Delay(100);
            Models.ReturnModel<Models.Order.ResponseModel> orderRet = await CreateOrderService(req);
            orderRequest.ReqRespList.Add(orderRet.ReqResp);

            // Hata varsa mesajı yazalım ve işlemi sonlandıralım
            if (orderRet.NotSuccessOrCustomMessage)
            {
                // {"httpStatusCode":400,"data":null,"success":false,"message":"FAILED_ORDER_WITH_OPEN_ORDERS","code":1122}
                // {"httpStatusCode":400,"data":null,"success":false,"message":"FAILED_MARKET_ORDER","code":1118}
                // {"httpStatusCode":400,"data":null,"success":false,"message":"BALANCE_NOT_ENOUGH","code":1055}
                // {"success": false,"message": "Too many requests","code": 429}
                // Yukarıdakilerden biri gibi bir mesaj geldiğinde emrimizi yeniden verdiğimizde gerçekleşebilir.
                if (orderRet.Code == 1122 || orderRet.Code == 1118 || orderRet.Code == 1055 || orderRet.Code == 429)
                {
                    orderResponse.NeedReOrder = true;

                    // Bu durumda ne kadar milisaniye bekleteceğimiz bilgisini set edelim. Bir daha limite düşmemek için 1sn de ekstra beklesin bari..
                    if (orderRet.Code == 429) orderResponse.RetryAfterMs = orderRet.ReqResp.RetryAfter * 1000 + 1000;
                }

                retTask.Message = orderRet.GetApiOrCustomMessage;
                return retTask;
            }

            if (orderRet.Data.Id < 1)
            {
                retTask.Message = "CreateOrder(IOrderRequest orderRequest), Veri gelmedi veya Id < 1 !";
                return retTask;
            }

            orderResponse.Id = orderRet.Data.Id;
            orderResponse.OrderClientId = orderRet.Data.NewOrderClientId;
            orderResponse.PairSymbol = orderRet.Data.PairSymbol;

            if (orderRequest.IsBuyOrder) orderResponse.DenominatorTotalGross = orderRet.Data.Quantity;
            else orderResponse.NumeratorAmountGross = orderRet.Data.Quantity;

            retTask.Success = true;

            return retTask;
        }
        /// <summary> Sadece Market Order gönderilir. </summary>
        internal async Task<absHelpers.ReturnTask<mdlOrder.OrderResponseModel>> CreateOrder(mdlOrder.OrderRequestModel orderRequest)
        {
            absHelpers.ReturnTask<mdlOrder.OrderResponseModel> insertOrderTask = await InsertOrder(orderRequest);
            if (!insertOrderTask.Success)
            {
                return insertOrderTask;
            }

            var retTask = new absHelpers.ReturnTask<mdlOrder.OrderResponseModel> { Success = false };
            mdlOrder.OrderResponseModel orderResponse = insertOrderTask.Data;

            // Order servisinden sadece Quantity değeri gelir. O tek başına yetmediği için
            // $"api/v1/users/transactions/trade?orderId={orderId}" servis sonucuna göre işlem yaparız.

            bool getUserTradesOk = false;
            Models.ReturnModel<List<Models.UserTrade>> userTradeRet = null;
            // Veri hemen gelmiyor bazen, en fazla 50 defa deneyeceğiz ve gelmezse hata döneceğiz
            // 90 kere gelmediği ve hata verip zarar ettiğim oldu! En azından arbitrajı otomatik tamamlasın.
            for (int i = 1; i < 91; i++)
            {
                userTradeRet = await GetUserTradesWithOrderId(orderResponse.Id);
                orderRequest.ReqRespList.Add(userTradeRet.ReqResp);

                // Hata varsa mesajı yazalım ve işlemi sonlandıralım
                if (userTradeRet.NotSuccessOrCustomMessage)
                {
                    retTask.Message = userTradeRet.GetApiOrCustomMessage;
                    return retTask;
                }

                // Veri geldi mi?
                if (userTradeRet.Data.Count > 0)
                {
                    string errMsg = null;
                    // Tamam veri geldi ve doğru ise artık döngüden çıkalım
                    if (IsOrderResponseOk(orderRequest, userTradeRet.Data, orderResponse, ref errMsg))
                    {
                        getUserTradesOk = true;
                        break;
                    }
                    else
                    {
                        // Veri geldi ancak hata varsa programı kıralım
                        if (errMsg != null)
                        {
                            retTask.Message = errMsg;
                            return retTask;
                        }
                        // Veri geldi ancak hata yoksa doğru veriyi alana kadar döngüye devam edelim
                    }
                }

                // Belirli aralıklarla bekletelim. Yoksa 90 kontrol sonrası üçgen arbitraj yarıda kalıyor.
                // Bari pozisyondan çıkana kadar beklesin, yoksa zararı daha büyük olabilir.
                // i != 90 && i % 10 == 0 iken 250ms kullandığımda 67. denemesinde çekmişti.
                // Pozisyondan çıksın diye 500ms'e çıkardım.
                if (i != 90 && i % 10 == 0) await Task.Delay(500);
            }

            // Veri gelip hata vermediği her durumda response'a data'yı koyalım.
            // Yani IsOrderResponseOk'den uygun hesaplamayı yapmasa bile son hesaplamaya kolayca ulaşabilmek için bu Fix'i uyguluyoruz.
            // IsOrderResponseOk hata verdiği durumda zaten bu satıra ulaşamayacak ve return ile fonksiyon sonlanacak.
            retTask.Data = orderResponse;

            // Yukarıdaki for döngüsünde defalarca /api/v1/users/transactions/trade servisine gidildi ve halen veri yoksa hata verelim.
            if (!getUserTradesOk)
            {
                retTask.Message = $"CreateOrder, GetUserTradesWithOrderId deneme sayısı: {orderRequest.ReqRespList.Count - 1 }";
                return retTask;
            }

            retTask.Success = true;

            return retTask;
        }
        /// <param name="errMsg"> Sadece hata varsa set ederiz. return false olması hata olduğu anlamına gelmez bu fonksiyon için. </param>
        bool IsOrderResponseOk(mdlOrder.OrderRequestModel orderRequest, List<Models.UserTrade> userTrade, mdlOrder.OrderResponseModel orderResponse, ref string errMsg)
        {
            mdlOrder.OrderResponseModel tempResp = new mdlOrder.OrderResponseModel();

            // Bu fonksiyona girdiysek, count > 0 olduğunu biliyoruz.
            int userTradeCount = userTrade.Count;
            for (int i = 0; i < userTradeCount; i++)
            {
                // ÖNEMLİ!!! BtcTurk komisyonu hep pair'in sağ currency cinsinden gönderiyor..
                tempResp.Commission += Math.Abs(userTrade[i].Fee + userTrade[i].Tax);
            }

            // BUY with Denominator, BtcTurk Buy işlemini paritenin sağ kısmı ile yapıyor.
            if (orderRequest.IsBuyOrder)
            {
                // Order emrinin sonucu success ise döndüğü Quantity bilgisi kadar kısmı gerçekleşmiş demektir.
                // Henüz komisyon düşülmediği için gross kısmına kayıt ederek işlemlere başlarız.
                tempResp.DenominatorTotalGross = orderResponse.DenominatorTotalGross;
                tempResp.DenominatorTotalNet = tempResp.DenominatorTotalGross - tempResp.Commission;
                tempResp.CommissionRatio = tempResp.Commission / tempResp.DenominatorTotalGross;

                decimal itemCommission, itemCommissionRatio;
                for (int i = 0; i < userTradeCount; i++)
                {
                    itemCommission = Math.Abs(userTrade[i].Fee + userTrade[i].Tax);
                    if (itemCommission == 0)
                    {
                        errMsg = "commissionItem == 0, bu durumda hesaplama yapılamaz!";
                        return false;
                    }
                    itemCommissionRatio = itemCommission / tempResp.Commission;

                    tempResp.NumeratorAmountGross += (tempResp.DenominatorTotalGross * itemCommissionRatio) / userTrade[i].Price;
                    tempResp.NumeratorAmountNet += (tempResp.DenominatorTotalNet * itemCommissionRatio) / userTrade[i].Price;
                }
                tempResp.Price = tempResp.DenominatorTotalNet / tempResp.NumeratorAmountNet;
            }
            // Sell with Numerator, BtcTurk Sell işlemini paritenin sol kısmı ile yapıyor.
            else
            {
                // Order emrinin sonucu success ise döndüğü Quantity bilgisi kadar kısmı gerçekleşmiş demektir.
                // Henüz komisyon düşülmediği için gross kısmına kayıt ederek işlemlere başlarız.
                tempResp.NumeratorAmountGross = orderResponse.NumeratorAmountGross;

                decimal itemCommission, itemCommissionRatio, itemNumeratorAmountGross;
                for (int i = 0; i < userTradeCount; i++)
                {
                    itemCommission = Math.Abs(userTrade[i].Fee + userTrade[i].Tax);
                    if (itemCommission == 0)
                    {
                        errMsg = "commissionItem == 0, bu durumda hesaplama yapılamaz!";
                        return false;
                    }
                    itemCommissionRatio = itemCommission / tempResp.Commission;
                    itemNumeratorAmountGross = tempResp.NumeratorAmountGross * itemCommissionRatio;

                    tempResp.Price += userTrade[i].Price * itemCommissionRatio;
                    tempResp.DenominatorTotalGross += userTrade[i].Price * itemNumeratorAmountGross;
                }

                tempResp.DenominatorTotalNet = tempResp.DenominatorTotalGross - tempResp.Commission;
                tempResp.NumeratorAmountNet = tempResp.DenominatorTotalNet / tempResp.Price;
                tempResp.CommissionRatio = tempResp.Commission / tempResp.DenominatorTotalGross;
            }

            // Hesaplamaların sonucunu orderResponse'a aktaralım.
            orderResponse.Commission = tempResp.Commission;
            orderResponse.CommissionRatio = tempResp.CommissionRatio;
            orderResponse.DenominatorTotalGross = tempResp.DenominatorTotalGross;
            orderResponse.DenominatorTotalNet = tempResp.DenominatorTotalNet;
            orderResponse.NumeratorAmountGross = tempResp.NumeratorAmountGross;
            orderResponse.NumeratorAmountNet = tempResp.NumeratorAmountNet;
            orderResponse.Price = tempResp.Price;

            // Örn: 0.0018 için "0.00179 < orderResponse.CommissionRatio < 0.00181" şartı sağlanırsa true döneceğiz
            // 1) Komisyondan binde 5 çıkarırız ve servisten aldığımız komisyondan küçük mü diye bakarız.
            // 2) Komisyona binde 5 ekleriz ve servisten aldığımız komisyondan büyük mü diye bakarız.
            // Bu şekilde BtcTurk'ün gönderdiği verinin tutarlılığını sağlamış oluruz.
            decimal addRemoveRatio = orderRequest.CommissionRatio * (5m / 1000m);
            decimal smaller = orderRequest.CommissionRatio - addRemoveRatio;
            decimal bigger = orderRequest.CommissionRatio + addRemoveRatio;

            // Tüm şartlar sağlanıyorsa true dönelim.
            if (smaller < orderResponse.CommissionRatio && orderResponse.CommissionRatio < bigger)
            {
                return true;
            }

            return false;
        }
        /// <summary>
        /// Market, Limit, StopLimit veya StopMarket emri verelim.
        /// </summary>
        /// <remarks>
        /// https://github.com/BTCTrader/broker-api-docs/issues/271
        /// Cevap: Order uçnoktadı sadece emir iletmek için kullanılır. Sonucunu openOrders veya Trades uçnoktasından sorgulayabilirsiniz.
        /// Yani Market Order versek dahi sonuç beklemeyeceksin. Onun için "/api/v1/users/transactions/trade?orderId=" adresine yeni requestler
        /// çekmek zorunda kalıyoruz. Bu endpointten de hemen cevap gelecek diye birşey yok. Cevap gelene kadar sorgu çekmeye devam etmek gerekiyor.
        /// </remarks>
        async Task<Models.ReturnModel<Models.Order.ResponseModel>> CreateOrderService(Models.Order.RequestModel req)
        {
            return await Statics.ScHttp.SendRequest<Models.Order.ResponseModel>(
                "api/v1/order",
                ScHttp.Methods.Post,
                "CreateOrderService",
                needAuthentication: true,
                inputModel: req
                );
        }
        /// <summary>
        /// Requests per minute: 90
        /// </summary>
        public async Task<Models.ReturnModel<List<Models.UserTrade>>> GetUserTradesWithOrderId(long orderId)
        {
            // Bunu V2'ye yükseltme yanlış çalışıyor. OrderId'ye ait olan değil son 200 trade'i çekiyor.
            return await Statics.ScHttp.SendRequest<List<Models.UserTrade>>(
                $"api/v1/users/transactions/trade?orderId={orderId}",
                ScHttp.Methods.Get,
                "GetUserTradesWithOrderId",
                needAuthentication: true
                );
        }
    }
}

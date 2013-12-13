using MercadoBitcoinClient.API.Models;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace MercadoBitcoinClient.API.Controllers {
    /// <summary>
    /// Roda as queries para a API padrão do MercadoBitcoin.
    /// Como a informação é cacheada de 60 em 60 segundos, não deixamos nova query antes deste período.
    /// </summary>
    public static class Query {

        private enum QueryTypes { Ticker, OrderBook, Trades };

        private static int _cacheSeconds = 60;
        
        /// <summary>
        /// A última chamada da API por KEY
        /// </summary>
        private static Dictionary<QueryTypes, DateTime> _lastApiGet;

        /// <summary>
        /// Cache da última resposta da API por KEY
        /// </summary>
        private static Dictionary<QueryTypes, string> _lastApiGetResult;

        #region // Queries

        public static Ticker GetTicker() {
            Ticker ticker = new Ticker();

            JObject obj = JObject.Parse(GetJsonStringFromApi(QueryTypes.Ticker));
            ticker.High = Convert.ToDecimal(obj["ticker"]["high"].ToString());
            ticker.Low = Convert.ToDecimal(obj["ticker"]["low"].ToString());
            ticker.Last = Convert.ToDecimal(obj["ticker"]["last"].ToString());
            ticker.Buy = Convert.ToDecimal(obj["ticker"]["buy"].ToString());
            ticker.Sell = Convert.ToDecimal(obj["ticker"]["sell"].ToString());
            ticker.Date = FromUnixTimestamp(Convert.ToInt64(obj["ticker"]["date"].ToString()));
            
            return ticker;
        }

        public static OrderBook GetOrderBook() {
            OrderBook ob = new OrderBook();

            JObject obj = JObject.Parse(GetJsonStringFromApi(QueryTypes.OrderBook));
            
            foreach (JToken token in obj["asks"]) {
                ob.Asks.Add(new PriceVolPair(Convert.ToDecimal(token[0]), Convert.ToDecimal(token[1])));
            }

            foreach (JToken token in obj["bids"]) {
                ob.Bids.Add(new PriceVolPair(Convert.ToDecimal(token[0]), Convert.ToDecimal(token[1])));
            }

            return ob;
        }

        public static List<Trade> GetTrades() {
            List<Trade> trades = new List<Trade>();

            JArray array = JArray.Parse(GetJsonStringFromApi(QueryTypes.Trades));

            foreach (JToken token in array) {
                Trade trade = new Trade();
                trade.Id = Convert.ToInt32(token["tid"].ToString());
                trade.Date = FromUnixTimestamp(Convert.ToInt64(token["date"].ToString()));
                trade.Price = Convert.ToDecimal(token["price"].ToString());
                trade.Amount = decimal.Parse(token["amount"].ToString(), System.Globalization.NumberStyles.Float);
                trade.Type = token["type"].ToString();
                trades.Add(trade);
            }

            return trades;
        }

        #endregion

        #region // Helper Methods

        private static string GetJsonStringFromApi(QueryTypes type) {
            // Inicializa dicionários de controle
            if (_lastApiGet == null) _lastApiGet = new Dictionary<QueryTypes, DateTime>();
            if (_lastApiGetResult == null) _lastApiGetResult = new Dictionary<QueryTypes, string>();

            // Verifica se é possível fazer chamada da query por causa do cache
            if (_lastApiGet.ContainsKey(type) && _lastApiGet[type].AddSeconds(_cacheSeconds) >= DateTime.Now) {
                return _lastApiGetResult[type];
            }

            string result = "";

            using (WebClient wc = new WebClient()) {
                string url = "";

                switch (type) {
                    case QueryTypes.Ticker:
                        url = "https://www.mercadobitcoin.com.br/api/ticker/";
                        break;
                    case QueryTypes.OrderBook:
                        url = "https://www.mercadobitcoin.com.br/api/orderbook/";
                        break;
                    case QueryTypes.Trades:
                        // url = "https://www.mercadobitcoin.com.br/api/trades/{timestamp_min}/{timestamp_max}";
                        // url = "https://www.mercadobitcoin.com.br/api/trades/1386295211";
                        url = "https://www.mercadobitcoin.com.br/api/trades/";
                        break;
                }

                result = wc.DownloadString(url);
            }

            _lastApiGet[type] = DateTime.Now;
            _lastApiGetResult[type] = result;

            return result;
        }

        private static DateTime FromUnixTimestamp(long timestamp) {
            return new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Unspecified).AddSeconds(timestamp).ToLocalTime();
        }

        private static long ToUnixTimestamp(DateTime date) {
            return date.ToUniversalTime().Subtract(new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Unspecified)).Ticks / 10000000;
        }

        #endregion

    }
}

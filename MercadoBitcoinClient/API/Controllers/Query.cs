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

            return ticker;
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
                        url = "https://www.mercadobitcoin.com.br/api/trades/{timestamp_min}/{timestamp_max}";
                        break;
                }

                result = wc.DownloadString(url);
            }

            return result;
        }

        #endregion

    }
}

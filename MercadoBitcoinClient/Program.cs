using MercadoBitcoinClient.API.Controllers;
using MercadoBitcoinClient.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MercadoBitcoinClient {
    static class Program {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main() {

            List<Trade> trades = Query.GetTrades();
            using (MercadoBitcoinLocalDBTableAdapters.TradeTableAdapter ta = new MercadoBitcoinLocalDBTableAdapters.TradeTableAdapter()) {
                foreach (Trade trade in trades) {
                    ta.Insert(trade.Id, trade.Date, trade.Price, trade.Amount, trade.Type);
                }
            }

            //Ticker ticker = Query.GetTicker();
            //OrderBook ob = Query.GetOrderBook();
            //List<Trade> trades = Query.GetTrades();

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MercadoBitcoinClient.API.Models {
    public class OrderBook {
        public List<PriceVolPair> Bids { get; set; }
        public List<PriceVolPair> Asks { get; set; }

        public OrderBook() {
            this.Bids = new List<PriceVolPair>();
            this.Asks = new List<PriceVolPair>();
        }
    }
}

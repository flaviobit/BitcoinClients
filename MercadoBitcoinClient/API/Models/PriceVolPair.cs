using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MercadoBitcoinClient.API.Models {
    public class PriceVolPair {
        public decimal Price { get; set; }
        public decimal Vol { get; set; }

        public PriceVolPair(decimal price, decimal vol) {
            this.Price = price;
            this.Vol = vol;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MercadoBitcoinClient.API.Models {
    public class Ticker {
        public double High { get; set; }
        public double Low { get; set; }
        public double Vol { get; set; }
        public double Last { get; set; }
        public double Buy { get; set; }
        public double Sell { get; set; }
        public int Date { get; set; }
    }
}

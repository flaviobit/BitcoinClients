using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MercadoBitcoinClient.API.Models {
    public class Trade {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public decimal Price { get; set; }
        public decimal Amount { get; set; }
        public string Type { get; set; }
    }
}

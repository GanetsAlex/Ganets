using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ganets.Domain.Entities
{
    public class CartItem
    {
        public Gadget Item { get; set; }
        public int Qty { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenerateurDevis
{
    public class LineItem
    {
        public string description { get; set; }
        public int price { get; set; }
        public int quantity { get; set; }
        public int total { get; set; }

        public static List<LineItem> lines = new List<LineItem>();

        public int GetTotal()
        {
            int total = price * quantity;
            return total;
        }

        public int GetAbsoluteTotal()
        {
            int total = 0;
            foreach(LineItem line in lines)
            {
                total+=line.GetTotal();
            }

            return total;
        }

        public void AddItem(Produit prod, int qts)
        {
            lines.Add(new LineItem() { description=prod.name, price=prod.prix,quantity=qts, total=prod.prix*qts});
        }

        public List<LineItem> GetLineItems()
        {
            return lines;
        }

    }
}

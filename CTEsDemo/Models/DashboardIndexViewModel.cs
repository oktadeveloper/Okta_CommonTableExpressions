using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CTEsDemo.Models
{
    public class DashboardIndexViewModel
    {
        public List<DashboardLineItem> Items { get; set; }

        public DashboardIndexViewModel(System.Data.DataTable items)
        {
            Items = new List<DashboardLineItem>();

            foreach (System.Data.DataRow row in items.Rows)
            {
                Items.Add(
                    new DashboardLineItem(
                        row["CustomerName"].ToString(),
                        Convert.ToInt32(row["Quantity"]),
                        Convert.ToDecimal(row["LineProfit"]),
                        row["StockItemName"].ToString()
                        )
                    );
            }
        }
    }

    public class DashboardLineItem
    {
        public string CustomerName { get; set; }
        public string StockItemName { get; set; }
        public int Quantity { get; set; }
        public decimal LineProfit { get; set; }

        public DashboardLineItem(string customerName, int quantity, decimal lineProfit, string stockItemName)
        {
            CustomerName = customerName;
            Quantity = quantity;
            LineProfit = lineProfit;
            StockItemName = stockItemName;
        }
    }
}

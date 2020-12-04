using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using System.Data;
using System.Data.SqlClient;

using Microsoft.Extensions.Options;

namespace CTEsDemo.Controllers
{
    public class DashboardController : Controller
    {
        IOptions<Settings.SqlSettings> _sqlSettings;

        public DashboardController(IOptions<Settings.SqlSettings> sqlSettings)
        {
            _sqlSettings = sqlSettings;
        }

        [Authorize]
        public IActionResult Index()
        {
            return View(getDashboardIndexModel());
        }

        protected Models.DashboardIndexViewModel getDashboardIndexModel()
        {
            DataTable dt = new DataTable();

            string cmdText = @"with customer_items (CustomerID, StockItemId, Quantity, LineProfit) as (
	                            select
		                            CustomerID, 
		                            StockItemId,
		                            Sum(Quantity) as Quantity,
		                            sum(LineProfit) as LineProfit

	                                FROM [WideWorldImporters].[Sales].[Invoices]
			                            inner join [WideWorldImporters].[Sales].[InvoiceLines] on [Invoices].[InvoiceID] = [InvoiceLines].[InvoiceID]

		                            group by CustomerID, StockItemID
                            )

                            select CustomerName, 
	                               customer_items.Quantity,
	                               customer_items.LineProfit,
	                               [Warehouse].[StockItems].[StockItemName]
	                               from customer_items
	                            inner join [Sales].[Customers] on [Sales].[Customers].[CustomerId] = customer_items.CustomerID
	                            inner join [Warehouse].[StockItems] on customer_items.StockItemId = [Warehouse].[StockItems].[StockItemID]";

            SqlDataAdapter da = new SqlDataAdapter(cmdText, new SqlConnection(_sqlSettings.Value.ConnectionString));


            da.Fill(dt);

            return new Models.DashboardIndexViewModel(dt);
        }
    }
}

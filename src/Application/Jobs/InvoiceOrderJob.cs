using System.ComponentModel;
using Hangfire;
using Hangfire.Server;
using Microsoft.Extensions.Logging;
using TropicFeel.Application.Common.Interfaces;
using TropicFeel.Domain.Dtos.Netsuit;
using TropicFeel.Domain.Enums;

namespace TropicFeel.Application.Jobs;

public class InvoiceOrderJob(
    ILogger<InvoiceOrderJob> logger,
    INetsuitClientService netsuitClientService,
    IApplicationDbContext dbcontext)
{
    [Hangfire.Dashboard.Management.Support.Job]
    [DisplayName(nameof(InvoiceOrderJob))]
    [Description("Update Sales Order to Invoice Job")]
    [AutomaticRetry(Attempts = 0)]
    public async Task Execute(PerformContext? context)
    {
        logger.LogInformation($"Start job");

        var salesorders = await dbcontext.SalesOrder
            .Where(x => x.Status != nameof(StatusOrderSale.Invoiced) && x.OrderNetSuite != null).ToListAsync();
        
        foreach (var order in salesorders)
        {
            try
            {
                //7083870
                var request = new List<RequestSearchOrderDto>
                {
                    new RequestSearchOrderDto() { Action = "get", RecordType = "salesorder", Id = Convert.ToInt32(order.OrderNetSuite) }
                };
                var response = await netsuitClientService.GetSalesOrderById(request);
                var body = response.Body?.ToList();

                if (body == null)
                {
                    continue;
                }

                foreach (var sub in from sublist in body.Select(item => item.Sublists?.Links?.ToList())
                             .OfType<List<KeyValuePair<string, Link>>>()
                         from sub in sublist
                         where sub.Value?.Type == "Invoice"
                         select sub)
                {
                    order.Status = nameof(StatusOrderSale.Invoiced);
                    await dbcontext.SaveChangesAsync(CancellationToken.None);
                }
            }
            catch (Exception e)
            {
                logger.LogInformation(e.Message);
                throw;
            }


            logger.LogInformation($"End job");
        }
    }
}

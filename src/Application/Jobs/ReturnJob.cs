using System.ComponentModel;
using Hangfire;
using Hangfire.Dashboard.Management.Metadata;
using Hangfire.Server;
using Microsoft.Extensions.Logging;

namespace TropicFeel.Application.Jobs;

[ManagementPage("Jobs")]
public class ReturnJob(ILogger<ReturnJob> logger)
{

    [Hangfire.Dashboard.Management.Support.Job]
    [DisplayName(nameof(ReturnJob))]    
    [Description("Return Sales Order Job")]
    [AutomaticRetry(Attempts = 0)]
    public /*async Task*/ void Execute(PerformContext? context)
    {
        logger.LogInformation($"Start job");
        try
        {
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}

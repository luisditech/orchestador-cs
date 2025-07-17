using System.Linq.Expressions;
using Hangfire;
using TropicFeel.Application.Common.Interfaces;
using TropicFeel.Application.Jobs;

namespace TropicFeel.Infrastructure.Hangfire;

public class HangfireJobScheduler : IJobScheduler
{
    private const string DefaultQueue = "default";
    public string ScheduleJob<T>(Expression<Action<T>> methodCall)
    {
        return BackgroundJob.Enqueue(methodCall);
    }

    public string ScheduleJob<T>(Expression<Action<T>> methodCall, TimeSpan delay)
    {
        return BackgroundJob.Schedule(methodCall, delay);
    }

}

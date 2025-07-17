using System.Linq.Expressions;

namespace TropicFeel.Application.Common.Interfaces;

public interface IJobScheduler
{
    string ScheduleJob<T>(Expression<Action<T>> methodCall);
    string ScheduleJob<T>(Expression<Action<T>> methodCall, TimeSpan delay);
    

}

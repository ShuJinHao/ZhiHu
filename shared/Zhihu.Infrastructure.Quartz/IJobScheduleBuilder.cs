using Quartz;

namespace Zhihu.Infrastructure.Quartz;

public interface IJobScheduleBuilder
{
    IScheduler CreateJobSchedule(IScheduler scheduler);
}
// ISchedulerService.cs
using Application.DTOs;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Abstractions;
public interface ISchedulerService
{
    ScheduleResponse Schedule(List<ScheduleItem> items);
}

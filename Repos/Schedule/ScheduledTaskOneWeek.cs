using DAOs;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repos.Schedule
{
    public class ScheduledTaskOneWeek : BackgroundService
    {
        private readonly TimeSpan _interval = TimeSpan.FromDays(7);
        private IUnitOfWork _unitOfWork;

        public ScheduledTaskOneWeek(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            throw new NotImplementedException();
        }

        private void ReAssignShiftForUser()
        {

        }
    }
}

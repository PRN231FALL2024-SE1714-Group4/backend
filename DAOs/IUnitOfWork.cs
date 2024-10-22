using BOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAOs
{
    public interface IUnitOfWork : IDisposable
    {
        IGenericRepository<Animal> AnimalRepository { get; }
        IGenericRepository<Area> AreaRepository { get; }
        IGenericRepository<Cage> CageRepository { get; }
        IGenericRepository<History> HistoryRepository { get; }
        IGenericRepository<Report> ReportRepository { get; }
        IGenericRepository<Role> RoleRepository { get; }
        IGenericRepository<User> UserRepository { get; }
        IGenericRepository<Work> WorkRepository { get; }
        IGenericRepository<UserShift> UserShiftRepository { get; }
        IGenericRepository<HealthReport> HealthReportRepository { get; }


        void Save();

    }
}

using BOs;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAOs
{
    public class UnitOfWork : IUnitOfWork, IDisposable
    {
        private PiggeryManagementContext context;
        private GenericRepository<Animal> animalRepository;
        private GenericRepository<Area> areaRepository;
        private GenericRepository<Cage> cageRepository;
        private GenericRepository<History> historyRepository;
        private GenericRepository<Report> reportRepository;
        private GenericRepository<User> userRepository;
        private GenericRepository<Role> roleRepository;
        private GenericRepository<Work> workRepository;
        public UnitOfWork(PiggeryManagementContext _context)
        {
            context = _context;
        }

        public IGenericRepository<Animal> AnimalRepository
        {
            get
            {
                return animalRepository ??= new GenericRepository<Animal>(context);
            }
        }

        public IGenericRepository<Area> AreaRepository
        {
            get
            {
                return areaRepository ??= new GenericRepository<Area>(context);
            }
        }

        public IGenericRepository<Cage> CageRepository
        {
            get
            {
                return cageRepository ??= new GenericRepository<Cage>(context);
            }
        }

        public IGenericRepository<History> HistoryRepository
        {
            get
            {
                return historyRepository ??= new GenericRepository<History>(context);
            }
        }

        public IGenericRepository<Report> ReportRepository
        {
            get
            {
                return reportRepository ??= new GenericRepository<Report>(context);
            }
        }

        public IGenericRepository<Role> RoleRepository
        {
            get
            {
                return roleRepository ??= new GenericRepository<Role>(context);
            }
        }

        public IGenericRepository<User> UserRepository
        {
            get
            {
                return userRepository ??= new GenericRepository<User>(context);
            }
        }

        public IGenericRepository<Work> WorkRepository
        {
            get
            {
                return workRepository ??= new GenericRepository<Work>(context);
            }
        }

        public void Save()
        {
            var validationErrors = context.ChangeTracker.Entries<IValidatableObject>()
                .SelectMany(e => e.Entity.Validate(null))
                .Where(e => e != ValidationResult.Success)
                .ToArray();
            if (validationErrors.Any())
            {
                var exceptionMessage = string.Join(Environment.NewLine,
                    validationErrors.Select(error => $"Properties {error.MemberNames} Error: {error.ErrorMessage}"));
                throw new Exception(exceptionMessage);
            }
            context.SaveChanges();
        }

        private bool disposed = false;

        

        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    context.Dispose();
                }
                disposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}

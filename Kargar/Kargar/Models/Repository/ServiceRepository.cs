using Kargar.Data;
using Kargar.Models.Interface;

namespace Kargar.Models.Repository
{
    public class ServiceRepository : IServiceRepository
    {
            private readonly ApplicationDbContext _context;

            public ServiceRepository(ApplicationDbContext context)
            {
                _context = context;
            }

            public List<Service> GetAll()
            {
                return _context.Service.ToList();
            }

            public Service GetById(int id)
            {
                return _context.Service.Find(id);
            }
        }

    }


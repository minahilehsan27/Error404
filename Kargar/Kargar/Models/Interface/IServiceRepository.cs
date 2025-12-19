
namespace Kargar.Models.Interface

{
    public interface IServiceRepository
    {
        List<Service> GetAll();
        Service GetById(int id);
    }
}

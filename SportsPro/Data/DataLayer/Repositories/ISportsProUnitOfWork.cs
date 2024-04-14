using NuGet.Packaging.Core;
using SportsPro.Models;

namespace SportsPro.Data.DataLayer.Repositories
{
    public class ISportsProUnitOfWork
    {
        // Read-only properties for each repository
        IRepository<Customer>? Customers { get; }
        IRepository<Incident>? Incidents { get; }
        IRepository<Product>? Products { get; }
        IRepository<Registration>? Registrations { get; }
        IRepository<Technician>? Technicians { get; }
        IRepository<Country>? Countries { get; }
    }
}

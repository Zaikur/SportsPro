using SportsPro.Models;


namespace SportsPro.Data.DataLayer.Repositories
{
    public class SportsProUnitOfWork : ISportsProUnitOfWork
    {
        private readonly SportsProContext context;

        public IRepository<Customer> Customers { get; private set; }
        public IRepository<Incident> Incidents { get; private set; }
        public IRepository<Product> Products { get; private set; }
        public IRepository<Registration> Registrations { get; private set; }
        public IRepository<Technician> Technicians { get; private set; }
        public IRepository<Country> Countries { get; private set; }
  
        public SportsProUnitOfWork(SportsProContext context)
        {
            this.context = context;
            Customers = new Repository<Customer>(context);
            Incidents = new Repository<Incident>(context);
            Products = new Repository<Product>(context);
            Registrations = new Repository<Registration>(context);
            Technicians = new Repository<Technician>(context);
            Countries = new Repository<Country>(context);
        }

        public void Save() => context.SaveChanges();
    }
}

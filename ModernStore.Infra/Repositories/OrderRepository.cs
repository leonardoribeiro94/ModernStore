using ModernStore.Domain.Entities;
using ModernStore.Domain.Repositories;
using ModernStore.Infra.DataContexts;

namespace ModernStore.Infra.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly ModernStoreDataContext _context;

        public OrderRepository(ModernStoreDataContext modernStoreDataContext)
        {
            _context = modernStoreDataContext;
        }


        public void Save(Order order)
        {
            _context.Orders.Add(order);
        }
    }
}

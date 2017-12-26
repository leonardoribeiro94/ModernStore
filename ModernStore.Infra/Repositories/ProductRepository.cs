using ModernStore.Domain.Entities;
using ModernStore.Domain.Repositories;
using ModernStore.Infra.DataContexts;
using System;
using System.Linq;

namespace ModernStore.Infra.Repositories
{
    public class ProductRepositorycs : IProductRepository
    {
        private readonly ModernStoreDataContext _context;

        public ProductRepositorycs(ModernStoreDataContext context)
        {
            _context = context;
        }

        public Product Get(Guid id)
        {
            return _context
                .Products
                .AsNoTracking()
                .FirstOrDefault(x => x.Id == id);
        }
    }
}

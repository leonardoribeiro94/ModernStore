using Dapper;
using ModernStore.Domain.Commands.Results;
using ModernStore.Domain.Entities;
using ModernStore.Domain.Repositories;
using ModernStore.Infra.DataContexts;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace ModernStore.Infra.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly ModernStoreDataContext _context;

        public ProductRepository(ModernStoreDataContext context)
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

        public IEnumerable<GetListProductCommandResult> Get()
        {
            const string query = "SELECT [Id], [Title], [Price], [Image] FROM [Product]";

            using (var conn = new SqlConnection(@"Server=.\sqlexpress;Database=ModernStore;Trusted_Connection=True;"))
            {
                conn.Open();
                return conn.Query<GetListProductCommandResult>(query);
            }
        }
    }
}

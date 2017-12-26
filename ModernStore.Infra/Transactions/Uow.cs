using ModernStore.Infra.DataContexts;

namespace ModernStore.Infra.Transactions
{
    public class Uow : IUow
    {
        private readonly ModernStoreDataContext _context;

        public Uow(ModernStoreDataContext context)
        {
            _context = context;
        }


        public void Commit()
        {
            _context.SaveChanges();
        }

        public void RollBack()
        {
            // Im doing nothing :)
        }
    }
}

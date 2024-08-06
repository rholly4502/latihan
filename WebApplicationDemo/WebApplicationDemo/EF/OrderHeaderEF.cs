using Microsoft.EntityFrameworkCore;
using WebApplicationDemo.RapidModels;
using WebApplicationDemo.RapidViewModels;

namespace WebApplicationDemo.EF
{
    public class OrderHeaderEF : IOrderHeaders
    {
        private readonly AppDbContext _appDbContext;
        public OrderHeaderEF(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public OrderHeader Add(OrderHeader entity)
        {
            throw new NotImplementedException();
        }

        public void Delete(int id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<OrderHeader> GetAll()
        {
            var results = _appDbContext.OrderHeaders
                .Include(oh => oh.Wallet).ThenInclude(w => w.Customer)
                .Include(oh => oh.Wallet).ThenInclude(w => w.WalletType)
                .Include(oh => oh.OrderDetails).ThenInclude(od => od.Product).ThenInclude(p => p.Category).ToList();

            return results;
        }

        public IEnumerable<ViewOrderHeaderInfo> GetAllWithView()
        {
            throw new NotImplementedException();
        }

        public OrderHeader GetById(int id)
        {
            throw new NotImplementedException();
        }

        public string GetOrderLastHeaderId()
        {
            throw new NotImplementedException();
        }

        public OrderHeader Update(OrderHeader entity)
        {
            throw new NotImplementedException();
        }
    }
}

using WebApplicationDemo.RapidModels;
using WebApplicationDemo.RapidViewModels;

namespace WebApplicationDemo.EF
{
    public interface IOrderHeaders : ICrud<OrderHeader>
    {
        public IEnumerable<ViewOrderHeaderInfo> GetAllWithView();
        public string GetOrderLastHeaderId();
    }
}

using WebApplicationDemo.RapidModels;

namespace WebApplicationDemo.EF
{
    public interface IOrderDetail : ICrud<OrderDetail>
    {
        IEnumerable<OrderDetail> GetDetailsByHeaderId(string orderHeaderId);
        decimal GetTotalAmount(string orderHeaderId);

    }
}
